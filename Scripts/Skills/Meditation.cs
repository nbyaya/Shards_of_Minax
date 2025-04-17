using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.SkillHandlers
{
    class Meditation
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Meditation].Callback = new SkillUseCallback(OnUse);
        }

        public static bool CheckOkayHolding(Item item)
        {
            if (item == null)
                return true;

            if (item is Spellbook || item is Runebook)
                return true;

            if (Core.AOS && item is BaseWeapon && ((BaseWeapon)item).Attributes.SpellChanneling != 0)
                return true;

            if (Core.AOS && item is BaseArmor && ((BaseArmor)item).Attributes.SpellChanneling != 0)
                return true;

            return false;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.RevealingAction();

            if (m.Target != null)
            {
                m.SendLocalizedMessage(501845); // You are busy doing something else and cannot focus.
                return TimeSpan.FromSeconds(5.0);
            }
            else if (!Core.AOS && m.Hits < (m.HitsMax / 10)) // Less than 10% health
            {
                m.SendLocalizedMessage(501849); // The mind is strong but the body is weak.
                return TimeSpan.FromSeconds(5.0);
            }
            else if (m.Mana >= m.ManaMax)
            {
                m.SendLocalizedMessage(501846); // You are at peace.
                return TimeSpan.FromSeconds(Core.AOS ? 10.0 : 5.0);
            }
            else if (Core.AOS && Server.Misc.RegenRates.GetArmorOffset(m) > 0)
            {
                m.SendLocalizedMessage(500135); // Regenative forces cannot penetrate your armor!
                return TimeSpan.FromSeconds(10.0);
            }
            else
            {
                Item oneHanded = m.FindItemOnLayer(Layer.OneHanded);
                Item twoHanded = m.FindItemOnLayer(Layer.TwoHanded);

                if (Core.AOS && m.Player)
                {
                    if (!CheckOkayHolding(oneHanded))
                        m.AddToBackpack(oneHanded);

                    if (!CheckOkayHolding(twoHanded))
                        m.AddToBackpack(twoHanded);
                }
                else if (!CheckOkayHolding(oneHanded) || !CheckOkayHolding(twoHanded))
                {
                    m.SendLocalizedMessage(502626); // Your hands must be free to cast spells or meditate.
                    return TimeSpan.FromSeconds(2.5);
                }

                double skillVal = m.Skills[SkillName.Meditation].Value;
                double chance = (50.0 + ((skillVal - (m.ManaMax - m.Mana)) * 2)) / 100;

                // 🔥 APPLY PASSIVE BONUSES FROM SKILL TREE 🔥
                if (m is PlayerMobile player)
                {
                    var profile = player.AcquireTalents();

                    // MeditationFocus increases success rate by 5% per point
                    int focusBonus = profile.Talents.ContainsKey(TalentID.MeditationFocus) ? profile.Talents[TalentID.MeditationFocus].Points : 0;
                    double focusBoost = focusBonus * 0.05; // 5% per MeditationFocus point
                    chance += focusBoost * 100;

                    // TemporalStillness reduces cooldown (reducing by 1s per point, up to max of 5s)
                    int stillnessBonus = profile.Talents.ContainsKey(TalentID.TemporalStillness) ? profile.Talents[TalentID.TemporalStillness].Points : 0;
                    double cooldownReduction = Math.Min(5.0, stillnessBonus * 1.0); // Max reduction = 5s

                    // Debugging & Logging
                    player.SendMessage($"[DEBUG] Meditation Chance: {chance:F2}% (+{focusBoost * 100}% from Focus)");
                    player.SendMessage($"[DEBUG] Cooldown Reduction: {cooldownReduction}s from TemporalStillness");

                    // Apply skill check
                    if (chance > Utility.RandomDouble() * 100)
                    {
                        m.CheckSkill(SkillName.Meditation, 0.0, 100.0);

                        m.SendLocalizedMessage(501851); // You enter a meditative trance.
                        m.Meditating = true;
                        BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.ActiveMeditation, 1075657));

                        if (m.Player || m.Body.IsHuman)
                            m.PlaySound(0xF9);

                        m.ResetStatTimers();
                    }
                    else
                    {
                        m.SendLocalizedMessage(501850); // You cannot focus your concentration.
                    }

                    return TimeSpan.FromSeconds(10.0 - cooldownReduction);
                }

                return TimeSpan.FromSeconds(10.0);
            }
        }

        // 💡 APPLY PASSIVE MANA REGENERATION BONUS 💡
        public static void ApplyPassiveManaRegen(PlayerMobile player)
        {
            if (player == null) return;

            var profile = player.AcquireTalents();

            // MeditationRecovery increases passive mana regen by +1 per point
            int recoveryBonus = profile.Talents.ContainsKey(TalentID.MeditationRecovery) ? profile.Talents[TalentID.MeditationRecovery].Points : 0;
            int manaBoost = recoveryBonus * 1; // Each point adds 1 passive mana regen

            // Apply bonus to player's mana regeneration rate
            player.Mana += manaBoost;

            // Debugging & Logging
            player.SendMessage($"[DEBUG] Passive Mana Regen Bonus: +{manaBoost} (from MeditationRecovery)");
        }
    }
}
