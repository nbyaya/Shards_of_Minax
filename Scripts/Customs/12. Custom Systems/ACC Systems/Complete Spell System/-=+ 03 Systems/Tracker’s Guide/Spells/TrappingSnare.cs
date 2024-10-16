using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class TrappingSnare : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trapping Snare", "Entangle",
            21004, // Spell Icon ID
            9300 // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public TrappingSnare(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);

                InternalItem snareTrap = new InternalItem(loc, Caster.Map, Caster);
                snareTrap.MoveToWorld(loc, Caster.Map);

                Effects.SendLocationEffect(loc, Caster.Map, 0x36BD, 20, 10, 0, 0);
                Effects.PlaySound(loc, Caster.Map, 0x1FB);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1B72) // Trap ID
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;
                Visible = true;

                m_Timer = new InternalTimer(this, caster);
                m_Timer.Start();
            }

            public InternalItem(Serial serial) : base(serial)
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

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;
                private Mobile m_Caster;

                public InternalTimer(InternalItem item, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
                {
                    m_Item = item;
                    m_Caster = caster;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    if (m_Item == null || m_Item.Deleted)
                        return;

                    foreach (Mobile m in m_Item.GetMobilesInRange(1)) // Check for mobiles within range
                    {
                        if (m != m_Caster && m.Alive && m.AccessLevel == AccessLevel.Player)
                        {
                            m.SendMessage("You have triggered a snare trap!");
                            m.Damage(Utility.RandomMinMax(10, 20), m_Caster);
                            m.Freeze(TimeSpan.FromSeconds(2.0)); // Freeze effect for 2 seconds
                            Effects.SendLocationEffect(m_Item.Location, m_Item.Map, 0x36BD, 20, 10, 0, 0);
                            Effects.PlaySound(m_Item.Location, m_Item.Map, 0x208);
                            m_Item.Delete();
                            break; // Only one mobile should trigger the trap
                        }
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private TrappingSnare m_Owner;

            public InternalTarget(TrappingSnare owner) : base(12, true, TargetFlags.None)
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
