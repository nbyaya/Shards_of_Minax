using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class MeteorShower : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Meteor Shower", "In Vas Flam Grav",
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.SpidersSilk
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 40; } }

        public MeteorShower(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                // Play initial spell effects
                Effects.PlaySound(loc, Caster.Map, 0x15E);
                Caster.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                
                Timer.DelayCall(TimeSpan.FromSeconds(1.5), () => CreateMeteor(loc, Caster.Map, Caster));
            }

            FinishSequence();
        }

        private void CreateMeteor(Point3D loc, Map map, Mobile caster)
        {
            int meteors = Utility.RandomMinMax(3, 6); // Random number of meteors
            for (int i = 0; i < meteors; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 * i), () => DropMeteor(loc, map, caster));
            }
        }

        private void DropMeteor(Point3D loc, Map map, Mobile caster)
        {
            // Randomize location slightly for each meteor
            int xOffset = Utility.RandomMinMax(-2, 2);
            int yOffset = Utility.RandomMinMax(-2, 2);
            Point3D meteorLoc = new Point3D(loc.X + xOffset, loc.Y + yOffset, loc.Z);

            Effects.SendLocationEffect(meteorLoc, map, 0x36D4, 30, 10, 1161, 0);
            Effects.PlaySound(meteorLoc, map, 0x5C3);

            IPooledEnumerable eable = map.GetMobilesInRange(meteorLoc, 2);
            foreach (Mobile m in eable)
            {
                if (m != caster && caster.CanBeHarmful(m, false))
                {
                    caster.DoHarmful(m);
                    AOS.Damage(m, caster, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0); // Pure fire damage
                }
            }
            eable.Free();
        }

        private class InternalTarget : Target
        {
            private MeteorShower m_Owner;

            public InternalTarget(MeteorShower owner) : base(12, true, TargetFlags.Harmful)
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
