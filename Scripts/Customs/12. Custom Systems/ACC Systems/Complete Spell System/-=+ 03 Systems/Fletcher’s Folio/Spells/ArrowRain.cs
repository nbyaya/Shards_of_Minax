using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class ArrowRain : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arrow Rain", "Incendio Ex Pluviam",
            // SpellCircle.Sixth,
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public ArrowRain(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ArrowRain m_Owner;

            public InternalTarget(ArrowRain owner) : base(12, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
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
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                // Create a visual effect of arrows raining down
                Effects.PlaySound(loc, Caster.Map, 0x145); // Arrow volley sound
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052, 0, 0, 0);

                // Deal damage to enemies within a 5-tile radius
                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (Caster != m && Caster.CanBeHarmful(m, false))
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    Caster.DoHarmful(m);

                    double damage = Utility.RandomMinMax(5, 15); // Base damage for a single arrow

                    // Additional damage calculation for multiple arrows hitting the same target
                    int arrowCount = Utility.RandomMinMax(3, 6); // Random number of arrows hitting the target
                    double totalDamage = damage * arrowCount;

                    // Display damage effect on the target
                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    m.PlaySound(0x208);

                    // Apply damage to the target
                    AOS.Damage(m, Caster, (int)totalDamage, 0, 100, 0, 0, 0);
                }
            }

            FinishSequence();
        }
    }
}
