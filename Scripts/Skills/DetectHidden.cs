using System;
using System.Collections.Generic;
using System.Linq;

using Server.Factions;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;
using Server.Engines.VvV;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Items
{
    public interface IRevealableItem
    {
        bool CheckReveal(Mobile m);
        bool CheckPassiveDetect(Mobile m);
        void OnRevealed(Mobile m);

        bool CheckWhenHidden { get; }
    }
}

namespace Server.SkillHandlers
{
    public class DetectHidden
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.DetectHidden].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile src)
        {
            src.SendLocalizedMessage(500819); // Where will you search?
            src.Target = new InternalTarget();

            return TimeSpan.FromSeconds(10.0);
        }

        public class InternalTarget : Target
        {
            public InternalTarget() : base(12, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile src, object targ)
            {
                bool foundAnyone = false;
                Point3D p;
                if (targ is Mobile)
                    p = ((Mobile)targ).Location;
                else if (targ is Item)
                    p = ((Item)targ).Location;
                else if (targ is IPoint3D)
                    p = new Point3D((IPoint3D)targ);
                else
                    p = src.Location;

                double srcSkill = src.Skills[SkillName.DetectHidden].Value;
                int baseRange = Math.Max(2, (int)(srcSkill / 10.0));

                // If the skill check fails, halve the range.
                if (!src.CheckSkill(SkillName.DetectHidden, 0.0, 100.0))
                    baseRange /= 2;

                // Check for passive bonus to range from the Detect Hidden tree.
                if (src is PlayerMobile player)
                {
                    var profile = player.AcquireTalents();
                    int bonusRange = profile.Talents[TalentID.DetectHiddenRange].Points;
                    baseRange += bonusRange;
                }

                // House bonus as before.
                BaseHouse house = BaseHouse.FindHouseAt(p, src.Map, 16);
                bool inHouse = house != null && house.IsFriend(src);
                if (inHouse)
                    baseRange = 22;

                if (baseRange > 0)
                {
                    var inRange = src.Map.GetMobilesInRange(p, baseRange);
                    foreach (Mobile trg in inRange)
                    {
                        if (trg.Hidden && src != trg)
                        {
                            double ss = srcSkill + Utility.Random(21) - 10;
                            // Apply bonus chance and reduce target's effective hiding.
                            int bonusChance = 0, stealthReduction = 0;
                            if (src is PlayerMobile psrc)
                            {
                                var profile = psrc.AcquireTalents();
                                bonusChance = profile.Talents[TalentID.DetectHiddenChance].Points;
                                stealthReduction = profile.Talents[TalentID.DetectHiddenStealthReduction].Points;
                            }
                            ss += bonusChance;
                            double ts = trg.Skills[SkillName.Hiding].Value + Utility.Random(21) - 10 - stealthReduction;
                            double shadow = Server.Spells.SkillMasteries.ShadowSpell.GetDifficultyFactor(trg);
                            bool houseCheck = inHouse && house.IsInside(trg);

                            if (src.AccessLevel >= trg.AccessLevel && (ss >= ts || houseCheck) && Utility.RandomDouble() > shadow)
                            {
                                if ((trg is ShadowKnight && (trg.X != p.X || trg.Y != p.Y)) ||
                                    (!houseCheck && !CanDetect(src, trg)))
                                    continue;

                                trg.RevealingAction();
                                trg.SendLocalizedMessage(500814); // You have been revealed!
                                trg.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500814, trg.NetState);
                                foundAnyone = true;
                            }
                        }
                    }

                    var itemsInRange = src.Map.GetItemsInRange(p, baseRange);
                    foreach (Item item in itemsInRange)
                    {
                        if (item is LibraryBookcase && Server.Engines.Khaldun.GoingGumshoeQuest3.CheckBookcase(src, item))
                        {
                            foundAnyone = true;
                        }
                        else if (item is IRevealableItem dItem)
                        {
                            if (!item.Visible && !dItem.CheckWhenHidden && dItem.CheckReveal(src))
                            {
                                dItem.OnRevealed(src);
                                foundAnyone = true;
                            }
                        }
                    }
                }

                if (!foundAnyone)
                    src.SendLocalizedMessage(500817); // You can see nothing hidden there.
            }
        }

        public static void DoPassiveDetect(Mobile src)
        {
            if (src == null || src.Map == null || src.Location == Point3D.Zero || src.IsStaff())
                return;

            double ss = src.Skills[SkillName.DetectHidden].Value;
            if (ss <= 0)
                return;

            int baseRange = 4;
            if (src is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                baseRange += profile.Talents[TalentID.DetectHiddenRange].Points;
                ss += profile.Talents[TalentID.DetectHiddenChance].Points;
            }

            var nearby = src.Map.GetMobilesInRange(src.Location, baseRange);
            foreach (Mobile m in nearby)
            {
                if (m == null || m == src || m is ShadowKnight || !CanDetect(src, m))
                    continue;

                double ts = (m.Skills[SkillName.Hiding].Value + m.Skills[SkillName.Stealth].Value) / 2;
                if (src.Race == Race.Elf)
                    ss += 20;

                // Apply stealth reduction bonus
                if (src is PlayerMobile psrc)
                {
                    var profile = psrc.AcquireTalents();
                    ts -= profile.Talents[TalentID.DetectHiddenStealthReduction].Points;
                }

                if (src.AccessLevel >= m.AccessLevel && Utility.Random(1000) < (ss - ts) + 1)
                {
                    m.RevealingAction();
                    m.SendLocalizedMessage(500814);
                }
            }

            var items = src.Map.GetItemsInRange(src.Location, baseRange + 4);
            foreach (Item item in items)
            {
                if (!item.Visible && item is IRevealableItem dItem && dItem.CheckPassiveDetect(src))
                    src.SendLocalizedMessage(1153493); // Your keen senses detect something hidden...
            }
        }

        public static bool CanDetect(Mobile src, Mobile target)
        {
            if (src.Map == null || target.Map == null || !src.CanBeHarmful(target, false))
                return false;
            if (src.Blessed || (src is BaseCreature && ((BaseCreature)src).IsInvulnerable))
                return false;
            if (target.Blessed || (target is BaseCreature && ((BaseCreature)target).IsInvulnerable))
                return false;
            if (!Server.Spells.SpellHelper.ValidIndirectTarget(target, src))
                return false;
            if (src.Aggressed.Any(x => x.Defender == target) || src.Aggressors.Any(x => x.Attacker == target))
                return true;
            return src.Map.Rules == MapRules.FeluccaRules;
        }
    }
}
