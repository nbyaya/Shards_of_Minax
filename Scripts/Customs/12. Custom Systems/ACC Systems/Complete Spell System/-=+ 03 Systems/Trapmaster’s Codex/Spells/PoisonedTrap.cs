using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class PoisonedTrap : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Poisoned Trap", "Creatio Venenum!",
            21001,
            9200
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public PoisonedTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Point3D loc = new Point3D(p);

                // Create the trap item
                InternalItem trap = new InternalItem(Caster, loc, Caster.Map);
                trap.MoveToWorld(loc, Caster.Map);

                Caster.PlaySound(0x2A); // Sound effect for placing the trap
                Caster.SendMessage("You have created a poisoned trap!");
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Mobile caster, Point3D loc, Map map) : base(0x11B9) // ItemID for trap
            {
                Movable = false;
                Hue = 1267; // Poison trap color
                MoveToWorld(loc, map);
                m_Caster = caster;

                // Start the timer for the trap's lifespan
                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0)); // Trap lasts for 30 seconds
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

            public override bool OnMoveOver(Mobile m)
            {
                if (m != null && m != m_Caster && m is PlayerMobile)
                {
                    m.SendMessage("You triggered a poisoned trap!");
                    m.FixedParticles(0x374A, 10, 15, 5031, 61, 2, EffectLayer.Waist); // Poison cloud effect
                    m.PlaySound(0x205); // Poison sound effect
                    m.ApplyPoison(m_Caster, Poison.Regular); // Apply poison effect to the player

                    if (m.Alive)
                    {
                        m.SendMessage("You feel your movements slow as the poison takes hold.");
                        m.SendMessage(0x22, "Your movements are slowed!");
                        m.Delta(MobileDelta.Flags); // Refresh the player's status to apply effects
                        m.Send(SpeedControl.WalkSpeed); // Slow down the player's movement speed
                    }

                    Delete(); // Remove the trap after triggering
                }

                return base.OnMoveOver(m);
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;

                public InternalTimer(InternalItem item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }
        }

        private class InternalTarget : Target
        {
            private PoisonedTrap m_Owner;

            public InternalTarget(PoisonedTrap owner) : base(12, true, TargetFlags.None)
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
