using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class FreezeTrap : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Freeze Trap", "In Vas Ylem",
            //SpellCircle.Fifth,
            21001,
            9200
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public FreezeTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FreezeTrap m_Owner;

            public InternalTarget(FreezeTrap owner) : base(12, true, TargetFlags.None)
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
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023);
                Effects.PlaySound(loc, Caster.Map, 0x208);

                // Create the trap item and move it to the world
                FreezeTrapItem trap = new FreezeTrapItem(Caster, loc, Caster.Map);
                trap.MoveToWorld(loc, Caster.Map); // Move the item to the world at the specified location

                FinishSequence();
            }
        }

        private class FreezeTrapItem : Item
        {
            private Mobile m_Caster;
            private Timer m_Timer;

            public FreezeTrapItem(Mobile caster, Point3D loc, Map map) : base(0x11F4) // Trap ID
            {
                Movable = false;
                Light = LightType.Circle300;
                Hue = 1152; // Ice Blue Hue

                m_Caster = caster;
                m_Timer = new InternalTimer(this, m_Caster);
                m_Timer.Start();
            }

            public FreezeTrapItem(Serial serial) : base(serial)
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
            }

            public override bool OnMoveOver(Mobile m)
            {
                if (m.Alive && m is BaseCreature && m != m_Caster)
                {
                    m.SendMessage("You feel a chilling sensation as you step on the trap!");
                    m.PlaySound(0x204); // Freezing sound effect
                    m.FixedParticles(0x376A, 9, 32, 5023, EffectLayer.Waist);

                    if (Utility.RandomDouble() < 0.5) // 50% chance to freeze
                    {
                        m.Freeze(TimeSpan.FromSeconds(3.0));
                        m.SendMessage("You are frozen in place!");
                    }
                    else
                    {
                        m.Paralyze(TimeSpan.FromSeconds(5.0));
                        m.SendMessage("Your movements are slowed!");
                    }

                    Delete();
                }

                return base.OnMoveOver(m);
            }

            private class InternalTimer : Timer
            {
                private FreezeTrapItem m_Item;
                private Mobile m_Caster;

                public InternalTimer(FreezeTrapItem item, Mobile caster) : base(TimeSpan.FromSeconds(30.0))
                {
                    m_Item = item;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }
        }
    }
}
