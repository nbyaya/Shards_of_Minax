using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Gumps;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class RegenerationElixir : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Regeneration Elixir", "Curatio",
            // Assuming it's a high-level spell
            266, 9040
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 20;

        public RegenerationElixir(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private RegenerationElixir m_Owner;

            public InternalTarget(RegenerationElixir owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (target == null || !target.Alive)
                    {
                        from.SendLocalizedMessage(501942); // Target is invalid
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        Effects.PlaySound(target.Location, target.Map, 0x1F5); // Magical sound effect

                        // Create a special visual effect with correct parameters
                        Effects.SendTargetParticles(target, 0x376A, 9, 32, 5020, 3, 9502, EffectLayer.Waist, 0); // Shimmering effect

                        // Apply the regeneration effect
                        new RegenerationTimer(target, TimeSpan.FromSeconds(10.0)).Start();

                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501942); // Target is invalid
                }
            }
        }

        private class RegenerationTimer : Timer
        {
            private Mobile m_Target;
            private DateTime m_EndTime;

            public RegenerationTimer(Mobile target, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_EndTime = DateTime.Now + duration;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || DateTime.Now > m_EndTime)
                {
                    Stop();
                    return;
                }

                // Regenerate health
                m_Target.Heal(2); // Heal 2 HP per second
                m_Target.FixedEffect(0x376A, 1, 9); // Heal effect
            }
        }
    }
}
