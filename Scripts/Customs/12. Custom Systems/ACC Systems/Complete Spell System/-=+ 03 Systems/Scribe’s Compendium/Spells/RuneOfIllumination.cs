using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class RuneOfIllumination : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Rune of Illumination", "Lux Fortis",
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public RuneOfIllumination(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                // Play sound effect for rune activation
                Effects.PlaySound(p, Caster.Map, 0x1E3);

                // Create a bright light effect
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x376A, 20, 10, 1153, 0);

                // Illuminate the area and dispel darkness
                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                InternalItem lightSource = new InternalItem(loc, Caster.Map, Caster);
                lightSource.MoveToWorld(loc, Caster.Map);

                FinishSequence();
            }
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1ECD) // Bright light item ID
            {
                Movable = false;
                MoveToWorld(loc, map);
                Light = LightType.Circle300; // Illuminate a large area
                m_Caster = caster;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0)); // Duration of illumination
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
            private RuneOfIllumination m_Owner;

            public InternalTarget(RuneOfIllumination owner) : base(12, true, TargetFlags.None)
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
