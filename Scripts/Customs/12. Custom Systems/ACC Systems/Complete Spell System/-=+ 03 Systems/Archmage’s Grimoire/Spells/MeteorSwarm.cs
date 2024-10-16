using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class MeteorSwarm : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Meteor Swarm", "In Flam Grav",
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.SpidersSilk
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public MeteorSwarm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);

                Effects.PlaySound(loc, Caster.Map, 0x5C3); // Meteor sound

                for (int i = 0; i < 5; i++)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), () =>
                    {
                        Point3D targetLoc = new Point3D(loc.X + Utility.RandomMinMax(-2, 2), loc.Y + Utility.RandomMinMax(-2, 2), loc.Z);
                        Effects.SendLocationEffect(targetLoc, Caster.Map, 0x36BD, 20, 10, 1160, 0); // Meteor effect
                        Effects.PlaySound(targetLoc, Caster.Map, 0x208); // Explosion sound

                        foreach (Mobile m in Caster.Map.GetMobilesInRange(targetLoc, 1))
                        {
                            if (m != null && Caster.CanBeHarmful(m))
                            {
                                Caster.DoHarmful(m);
                                AOS.Damage(m, Caster, Utility.RandomMinMax(20, 40), 0, 100, 0, 0, 0); // Fire damage
                            }
                        }
                    });
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private MeteorSwarm m_Owner;

            public InternalTarget(MeteorSwarm owner) : base(12, true, TargetFlags.Harmful)
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
    }
}
