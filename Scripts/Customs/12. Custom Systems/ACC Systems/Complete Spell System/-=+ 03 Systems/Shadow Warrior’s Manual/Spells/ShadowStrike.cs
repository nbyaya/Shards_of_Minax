using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class ShadowStrike : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shadow Strike", "In Vas Kal An Flam",
                                                        //SpellCircle.Second,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ShadowStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShadowStrike m_Owner;

            public InternalTarget(ShadowStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.CanBeHarmful(target) && m_Owner.CheckSequence())
                {
                    SpellHelper.Turn(from, target);

                    from.PlaySound(0x3E9); // Play a stealthy attack sound
                    from.FixedParticles(0x3728, 1, 13, 9912, 1108, 0, EffectLayer.Waist); // Shadowy effect

                    double damage = Utility.RandomMinMax(15, 30); // Base damage

                    if (from.Hidden) // If caster is in stealth
                    {
                        damage *= 1.5; // 50% bonus damage
                        from.SendMessage("You strike with the power of the shadows!");
                    }

                    // Bleed effect chance
                    if (Utility.RandomDouble() < 0.3) // 30% chance to bleed
                    {
                        BleedEffect.BeginBleed(target, from);
                        target.SendMessage("You feel a sharp pain as you start to bleed!");
                        target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    }

                    from.RevealingAction(); // Reveals the caster if they were hidden
                    from.Mana -= m_Owner.RequiredMana; // Use m_Owner.RequiredMana instead of RequiredMana

                    AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0); // Apply damage

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }

    // Bleed effect implementation for completeness
    public static class BleedEffect
    {
        public static void BeginBleed(Mobile target, Mobile from)
        {
            // Custom logic for bleed effect
            Timer timer = new BleedTimer(target, from);
            timer.Start();
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_From;
            private int m_Ticks;

            public BleedTimer(Mobile target, Mobile from) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_From = from;
                m_Ticks = 5; // Bleed duration, total of 5 ticks
            }

            protected override void OnTick()
            {
                if (m_Target.Alive && m_Ticks > 0)
                {
                    int damage = Utility.RandomMinMax(2, 4); // Bleed damage per tick
                    AOS.Damage(m_Target, m_From, damage, 0, 100, 0, 0, 0); // Apply bleed damage
                    m_Target.PlaySound(0x133); // Sound for bleeding
                    m_Ticks--;
                }
                else
                {
                    Stop(); // Stop the timer when done
                }
            }
        }
    }
}
