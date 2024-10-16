using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class HealingWave : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Wave", "Sanctus Unda",
            21004,
            9300,
            false,
            Reagent.Ginseng,
            Reagent.Garlic,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 30; } }

        public HealingWave(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Define the target area for the healing effect
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private HealingWave m_Owner;

            public InternalTarget(HealingWave owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D point)
                {
                    m_Owner.Target(point);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                // Play visual and sound effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F2); // Sound of a magical wave
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x376A, 20, 10, 1153, 0); // Visual effect

                // Heal allies in a small radius around the caster
                ArrayList allies = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(3)) // 3 tile radius
                {
                    if (m != Caster && m.Alive && !m.IsDeadBondedPet && m.AccessLevel == AccessLevel.Player && m.Karma >= 0)
                    {
                        allies.Add(m);
                    }
                }

                foreach (Mobile ally in allies)
                {
                    int healAmount = Utility.RandomMinMax(20, 40); // Random healing amount
                    ally.Heal(healAmount);

                    // Additional visual effect on healed ally
                    ally.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    ally.PlaySound(0x1F2); // Healing sound
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
