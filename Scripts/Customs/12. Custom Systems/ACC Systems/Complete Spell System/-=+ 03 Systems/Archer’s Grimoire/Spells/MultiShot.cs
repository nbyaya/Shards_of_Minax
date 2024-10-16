using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class MultiShot : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Multi-Shot", "Saggitae Multiplicatae",
                                                        //SpellCircle.Fifth,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 35; } }

        public MultiShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MultiShot m_Owner;

            public InternalTarget(MultiShot owner) : base(12, false, TargetFlags.Harmful)
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
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                List<Mobile> targets = new List<Mobile>();

                Map map = Caster.Map;

                if (map != null)
                {
                    foreach (Mobile m in Caster.GetMobilesInRange(10))
                    {
                        if (m != Caster && Caster.InLOS(m) && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                        {
                            targets.Add(m);
                        }
                    }

                    Effects.PlaySound(Caster.Location, Caster.Map, 0x145); // Arrow sound

                    foreach (Mobile m in targets)
                    {
                        double dist = Caster.GetDistanceToSqrt(m);

                        if (dist <= 5) // within cone range
                        {
                            Caster.DoHarmful(m);
                            m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Arrow hit effect
                            m.PlaySound(0x145); // Arrow hit sound
                            AOS.Damage(m, Caster, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0); // Damage with physical type
                        }
                    }
                }

                FinishSequence();
            }
        }
    }
}
