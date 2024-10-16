using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class WoodlandAmbush : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Woodland Ambush", "In Vas Ylem",
                                                        21014,
                                                        9314,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 22; } }

        public WoodlandAmbush(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Effects.PlaySound(p, Caster.Map, 0x22C); // Sound of branches snapping
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x36B0, 30, 10, 0, 0); // Trap visual effect

                InternalItem trap = new InternalItem(new Point3D(p), Caster.Map, Caster);
                trap.MoveToWorld(new Point3D(p), Caster.Map);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1B72) // Spiked trap graphic
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
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
                if (m != m_Caster && m.Player && !m.IsDeadBondedPet && m.Alive)
                {
                    m.Damage(Utility.RandomMinMax(15, 25), m_Caster); // Damage effect
                    m.SendMessage("You have stepped on a hidden trap!"); // Message to player

                    Effects.PlaySound(m.Location, m.Map, 0x22C); // Sound of being impaled
                    Effects.SendTargetParticles(m, 0x36B0, 1, 30, 1153, 0, 0, EffectLayer.Waist, 0); // Blood splatter effect

                    m.Paralyze(TimeSpan.FromSeconds(2.0)); // Slow effect
                }
                return base.OnMoveOver(m);
            }

            private class InternalTimer : Timer
            {
                private Item m_Item;

                public InternalTimer(Item item, TimeSpan duration) : base(duration)
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
            private WoodlandAmbush m_Owner;

            public InternalTarget(WoodlandAmbush owner) : base(12, true, TargetFlags.None)
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
