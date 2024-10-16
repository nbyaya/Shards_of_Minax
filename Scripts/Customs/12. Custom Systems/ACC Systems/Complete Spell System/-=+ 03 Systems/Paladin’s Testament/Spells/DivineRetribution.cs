using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class DivineRetribution : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Divine Retribution", "Re Vengeance",
            21010,
            9300,
            false,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle => SpellCircle.Fourth;
        public override double CastDelay => 1.5;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 20;

        private static readonly TimeSpan Duration = TimeSpan.FromSeconds(30.0); // Duration of the aura
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5.0); // Interval between damage pulses
        private const int DamageAmount = 10; // Damage amount per pulse

        public DivineRetribution(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You invoke divine retribution, creating a damaging aura around you!");

                // Start the aura effect
                var auraEffect = new DivineRetributionEffect(Caster, Duration, Interval, DamageAmount);
                auraEffect.Start(); // Start the periodic damage effect

                // Visual and sound effects
                Caster.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);
                Caster.PlaySound(0x20F); // Divine sound effect

                Timer.DelayCall(Duration, () =>
                {
                    auraEffect.Stop(); // Stop the effect when duration ends
                    Caster.SendMessage("The divine aura effect has worn off.");
                });
            }

            FinishSequence();
        }

        private class DivineRetributionEffect
        {
            private Mobile m_Caster;
            private TimeSpan m_Interval;
            private int m_DamageAmount;
            private Timer m_Timer;

            public DivineRetributionEffect(Mobile caster, TimeSpan duration, TimeSpan interval, int damageAmount)
            {
                m_Caster = caster;
                m_Interval = interval;
                m_DamageAmount = damageAmount;
            }

            public void Start()
            {
                // Start a repeating timer that deals damage every interval
                m_Timer = Timer.DelayCall(m_Interval, m_Interval, OnTick);
            }

            public void Stop()
            {
                // Stop the timer to end the effect
                m_Timer?.Stop();
            }

            private void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted)
                {
                    Stop();
                    return;
                }

                // Get all mobiles (characters) in a certain radius around the caster
                IPooledEnumerable eable = m_Caster.GetMobilesInRange(2); // Adjust range as needed

                foreach (Mobile mobile in eable)
                {
                    if (mobile != m_Caster && !(mobile is BaseCreature creature && creature.Controlled && creature.ControlMaster == m_Caster)) // Exclude the caster and their controlled pets
                    {
                        // Apply damage to the mobile
                        mobile.Damage(m_DamageAmount, m_Caster);

                        // Visual effect on the affected mobile
                        mobile.FixedEffect(0x374A, 10, 30);
                        mobile.PlaySound(0x1F2); // Sound effect for taking damage
                    }
                }

                eable.Free(); // Free the pooled enumerable to avoid memory leaks
            }
        }
    }
}
