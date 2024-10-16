using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class SplinterShot : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Splinter Shot", "Splinter",
            21004,
            9300,
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public SplinterShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                // Play sound and visual effects
                Effects.PlaySound(m.Location, m.Map, 0x1F5);
                Effects.SendTargetParticles(m, 0x36BD, 20, 10, 1153, 0, 5052, EffectLayer.Head, 0);  // Added EffectLayer and unknown

                // Deal initial damage to the target
                AOS.Damage(m, Caster, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0);

                // Area of effect damage
                foreach (Mobile target in m.GetMobilesInRange(3))
                {
                    if (target != m && Caster.CanBeHarmful(target, false) && Caster.InLOS(target))
                    {
                        Caster.DoHarmful(target);
                        Effects.SendTargetParticles(target, 0x36BD, 20, 10, 1153, 0, 5052, EffectLayer.Head, 0);  // Added EffectLayer and unknown
                        target.PlaySound(0x1F5);

                        AOS.Damage(target, Caster, Utility.RandomMinMax(15, 25), 0, 0, 0, 100, 0);
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SplinterShot m_Owner;

            public InternalTarget(SplinterShot owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    m_Owner.Target((Mobile)targeted);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
