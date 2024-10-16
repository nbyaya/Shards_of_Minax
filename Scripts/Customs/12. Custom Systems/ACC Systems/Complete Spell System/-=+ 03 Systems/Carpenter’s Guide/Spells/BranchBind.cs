using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class BranchBind : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Branch Bind", "In Box Por Yam",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public BranchBind(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BranchBind m_Owner;

            public InternalTarget(BranchBind owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        m_Owner.Effect(target);
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        private void Effect(Mobile target)
        {
            // Play effects and sounds
            target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            target.PlaySound(0x1FD);

            // Paralyze target
            target.Paralyze(TimeSpan.FromSeconds(5.0));

            // Paralyze all mobiles in 1-tile radius
            foreach (Mobile m in target.GetMobilesInRange(1))
            {
                if (m != target && m != Caster && Caster.CanBeHarmful(m))
                {
                    Caster.DoHarmful(m);
                    m.Paralyze(TimeSpan.FromSeconds(5.0));
                    m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    m.PlaySound(0x1FD);
                }
            }
        }
    }
}
