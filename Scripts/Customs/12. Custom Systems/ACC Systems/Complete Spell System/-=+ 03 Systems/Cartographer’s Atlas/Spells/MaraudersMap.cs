using System;
using System.Collections.Generic; // Add this line
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class MaraudersMap : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Marauder’s Map", "Find Thy Way",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 30; } }

        private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan CooldownDuration = TimeSpan.FromMinutes(30);

        public MaraudersMap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                PlayerMobile pm = Caster as PlayerMobile; // Assign pm here

                // Check if ability is on cooldown
                if (pm != null && pm.IsOnCooldown("MaraudersMap"))
                {
                    Caster.SendMessage("This ability is on cooldown.");
                    return;
                }

                // Apply the tracking skill bonus
                Caster.SendMessage("You feel your tracking abilities heighten as the map reveals paths unseen.");
                Caster.PlaySound(0x1F5); // Play a magical sound effect
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 20, 10, 0, 0); // Display a flashy visual effect

                Caster.Skills[SkillName.Tracking].Base += 20; // Temporarily increase tracking skill

                Timer buffTimer = new BuffTimer(Caster, BuffDuration);
                buffTimer.Start();

                // Start cooldown timer
                if (pm != null)
                {
                    pm.StartCooldown("MaraudersMap", CooldownDuration);
                }
            }

            FinishSequence();
        }

        private class BuffTimer : Timer
        {
            private Mobile m_Caster;

            public BuffTimer(Mobile caster, TimeSpan duration) : base(duration)
            {
                m_Caster = caster;
            }

            protected override void OnTick()
            {
                m_Caster.SendMessage("The effects of the Marauder’s Map fade away.");
                m_Caster.Skills[SkillName.Tracking].Base -= 20; // Revert tracking skill increase
            }
        }
    }

    public static class MobileExtensions
    {
        private static readonly Dictionary<Mobile, Dictionary<string, DateTime>> cooldowns = new Dictionary<Mobile, Dictionary<string, DateTime>>();

        public static bool IsOnCooldown(this Mobile mob, string abilityName)
        {
            if (!cooldowns.ContainsKey(mob) || !cooldowns[mob].ContainsKey(abilityName))
                return false;

            return cooldowns[mob][abilityName] > DateTime.UtcNow;
        }

        public static void StartCooldown(this Mobile mob, string abilityName, TimeSpan duration)
        {
            if (!cooldowns.ContainsKey(mob))
                cooldowns[mob] = new Dictionary<string, DateTime>();

            cooldowns[mob][abilityName] = DateTime.UtcNow.Add(duration);
            mob.SendMessage($"{abilityName} will be ready to use in {duration.TotalMinutes} minutes.");
        }
    }
}
