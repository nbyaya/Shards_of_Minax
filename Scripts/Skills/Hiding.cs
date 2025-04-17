using System;
using Server.Multis;
using Server.Network;
using Server.Mobiles;

namespace Server.SkillHandlers
{
    public class Hiding
    {
        private static bool m_CombatOverride;
        public static bool CombatOverride
        {
            get { return m_CombatOverride; }
            set { m_CombatOverride = value; }
        }

        public static void Initialize()
        {
            SkillInfo.Table[21].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            if (m.Spell != null)
            {
                m.SendLocalizedMessage(501238); // You are busy doing something else and cannot hide.
                return TimeSpan.FromSeconds(1.0);
            }

            if (Server.Engines.VvV.ManaSpike.UnderEffects(m))
            {
                return TimeSpan.FromSeconds(1.0);
            }

            if (Core.ML && m.Target != null)
            {
                Targeting.Target.Cancel(m);
            }

            // Default Values
            double baseSkill = m.Skills[SkillName.Hiding].Value;
            double hidingBonus = 0.0;
            double detectionReduction = 0.0;
            double stealthDurationBonus = 0.0;

            if (m is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.HidingEfficiency, out var hidingTalent))
                {
                    hidingBonus = hidingTalent.Points * 1.5;      // Each point increases hiding success by 1.5%
                    detectionReduction = hidingTalent.Points * 0.2; // Each point decreases detection range by 0.2 tiles
                    stealthDurationBonus = hidingTalent.Points * 0.5; // Each point increases stealth duration by 0.5 sec
                }
            }

            // Apply Hiding Bonus
            double totalSkill = Math.Min(100, baseSkill + hidingBonus);
            int skill = (int)totalSkill;

            // Detection range formula: Lower skill = easier to detect, higher skill = harder to detect
            int range = Math.Min((100 - skill) / 2 + 8, 18);

            // Apply passive detection reduction
            range = Math.Max(1, (int)(range - detectionReduction)); 

            // House hiding bonus
            double houseBonus = 0.0;
            BaseHouse house = BaseHouse.FindHouseAt(m);
            if (house != null && house.IsFriend(m))
            {
                houseBonus = 100.0;
            }

            bool badCombat = (!m_CombatOverride && m.Combatant is Mobile && m.InRange(m.Combatant.Location, range) && ((Mobile)m.Combatant).InLOS(m.Combatant));
            bool success = (!badCombat && m.CheckSkill(SkillName.Hiding, 0.0 - houseBonus, 100.0 - houseBonus));

            // Combat check: enemies in range will prevent hiding
            if (success)
            {
                if (!m_CombatOverride)
                {
                    foreach (Mobile check in m.GetMobilesInRange(range))
                    {
                        if (check.InLOS(m) && check.Combatant == m)
                        {
                            badCombat = true;
                            success = false;
                            break;
                        }
                    }
                }

                success = (!badCombat && m.CheckSkill(SkillName.Hiding, 0.0 - houseBonus, 100.0 - houseBonus));
            }

            // If combat is preventing hiding
            if (badCombat)
            {
                m.RevealingAction();
                m.LocalOverheadMessage(MessageType.Regular, 0x22, 501237); // You can't seem to hide right now.
                return TimeSpan.Zero;
            }

            if (success)
            {
                m.Hidden = true;
                m.Warmode = false;

                Server.Spells.Sixth.InvisibilitySpell.RemoveTimer(m);
                Server.Items.InvisibilityPotion.RemoveTimer(m);

                // Apply passive stealth bonuses
                if (m is PlayerMobile pm)
                {
                    pm.SendMessage($"Your advanced training grants +{hidingBonus:N1}% hiding success, reduces detection by {detectionReduction:N1} tiles, and increases stealth duration by {stealthDurationBonus:N1} sec.");
                }

                m.LocalOverheadMessage(MessageType.Regular, 0x1F4, 501240); // You have hidden yourself well.
            }
            else
            {
                m.RevealingAction();
                m.LocalOverheadMessage(MessageType.Regular, 0x22, 501241); // You can't seem to hide here.
            }

            return TimeSpan.FromSeconds(10.0 + stealthDurationBonus);
        }
    }
}
