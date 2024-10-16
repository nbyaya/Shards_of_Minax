using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class FirestartersEmber : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Firestarters Ember", "In Flam Grav",
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Arbitrary choice; adjust as needed
        }

        public override double CastDelay { get { return 0.2; } } // 2 seconds cast delay
        public override double RequiredSkill { get { return 50.0; } } // Skill required to cast
        public override int RequiredMana { get { return 30; } } // Mana required to cast

        public FirestartersEmber(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Map map = Caster.Map;

                // Create the fire effect in a 3x3 area centered on the target location
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        Point3D fireLocation = new Point3D(loc.X + x, loc.Y + y, loc.Z);
                        if (map.CanFit(fireLocation, 0, true, false) && Caster.InLOS(fireLocation))
                        {
                            Effects.SendLocationEffect(fireLocation, map, 0x3709, 30, 10, 1161, 0); // Fire effect
                            Effects.PlaySound(fireLocation, map, 0x208); // Fire crackling sound

                            FireItem fire = new FireItem(fireLocation, map);
                            fire.MoveToWorld(fireLocation, map);
                        }
                    }
                }
            }

            FinishSequence();
        }

        private class FireItem : Item
        {
            private Timer m_Timer;

            public FireItem(Point3D loc, Map map) : base(0x398C) // Fire item graphic
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Timer = new InternalTimer(this);
                m_Timer.Start();
            }

            public FireItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();

                m_Timer = new InternalTimer(this);
                m_Timer.Start();
            }

            private class InternalTimer : Timer
            {
                private FireItem m_Item;

                public InternalTimer(FireItem item) : base(TimeSpan.FromSeconds(10.0)) // Fire duration 10 seconds
                {
                    m_Item = item;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }

            public override bool OnMoveOver(Mobile m)
            {
                if (m.CanBeDamaged() && !m.Blessed)
                {
                    m.PlaySound(0x208); // Fire burning sound
                    m.Damage(Utility.RandomMinMax(5, 10)); // Deal 5-10 fire damage
                }

                return true;
            }
        }

        private class InternalTarget : Target
        {
            private FirestartersEmber m_Owner;

            public InternalTarget(FirestartersEmber owner) : base(12, true, TargetFlags.Harmful)
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

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds(7.0); // Cooldown of 7 minutes (420 seconds)
            }
        }
    }
}
