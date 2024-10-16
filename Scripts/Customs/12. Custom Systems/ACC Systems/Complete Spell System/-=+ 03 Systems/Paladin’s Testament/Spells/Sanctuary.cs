using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class Sanctuary : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Sanctuary", "Sanctus Locus",
                                                        21011,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 40; } }

        public Sanctuary(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Effects.PlaySound(loc, Caster.Map, 0x20E); // Play protective sound
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x375A, 10, 30, 1153, 4, 9962, 0); // Holy light effect

                // Create sanctuary effect area
                InternalItem sanctuaryArea = new InternalItem(loc, Caster.Map, Caster);
                sanctuaryArea.MoveToWorld(loc, Caster.Map);

                FinishSequence();
            }
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Timer m_DestructionTimer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1ECD) // Light green area ID
            {
                Movable = false;
                Visible = true;
                MoveToWorld(loc, map);
                m_Caster = caster;

                m_Timer = new InternalTimer(this, caster);
                m_Timer.Start();

                // Start destruction timer (e.g., 30 seconds duration)
                m_DestructionTimer = Timer.DelayCall(TimeSpan.FromSeconds(30), () => Delete());
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

                public InternalTimer(InternalItem item, Mobile caster) : base(TimeSpan.Zero, TimeSpan.FromSeconds(2.0))
                {
                    m_Item = item;
                    m_Caster = caster;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted)
                    {
                        Stop();
                        return;
                    }

                    foreach (Mobile m in m_Item.GetMobilesInRange(5))
                    {
                        if (m != null && m.Alive && m.Player && m != m_Caster && m.Criminal == false)
                        {
                            // Healing over time effect
                            m.Hits += 5; // Adjust healing amount as needed
                            m.SendMessage("You feel a soothing warmth from the sanctuary.");

                            // Immune to negative effects
                            m.Blessed = true; // Temporarily make them immune to negative status effects
                            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () => m.Blessed = false); // Reset after 2 seconds

                            // Visual and sound effects for allies in range
                            m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Holy light effect on player
                            m.PlaySound(0x1F2); // Healing sound
                        }
                    }

                    m_Item.PublicOverheadMessage(Network.MessageType.Regular, 0x44, false, "The sanctuary is healing!"); // Overhead message
                }
            }
        }

        private class InternalTarget : Target
        {
            private Sanctuary m_Owner;

            public InternalTarget(Sanctuary owner) : base(12, true, TargetFlags.None)
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
