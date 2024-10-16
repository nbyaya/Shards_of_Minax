using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class CarvedTotem : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Carved Totem", "Car In Totus", 
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 30; } }

        public CarvedTotem(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CarvedTotem m_Owner;

            public InternalTarget(CarvedTotem owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                    m_Owner.Target((IPoint3D)targeted);
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);
                Point3D loc = new Point3D(p);

                // Play a sound and create visual effects when the totem is created
                Effects.PlaySound(loc, Caster.Map, 0x345);
                Effects.SendLocationEffect(loc, Caster.Map, 0x376A, 20, 10, 0, 0);

                // Create the totem item at the target location
                CarvedTotemItem totem = new CarvedTotemItem(Caster);
                totem.MoveToWorld(loc, Caster.Map);

                FinishSequence();
            }
        }

        public class CarvedTotemItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public CarvedTotemItem(Mobile caster) : base(0x009D)
            {
                Movable = false;
                Name = "Carved Totem";
                m_Caster = caster;

                // Start a timer for totem regeneration
                m_Timer = new RegenTimer(this, caster);
                m_Timer.Start();

                // Add a duration for the totem
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), Delete); // Totem lasts for 30 seconds
            }

            public CarvedTotemItem(Serial serial) : base(serial)
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

            private class RegenTimer : Timer
            {
                private CarvedTotemItem m_Totem;
                private Mobile m_Caster;

                public RegenTimer(CarvedTotemItem totem, Mobile caster) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
                {
                    m_Totem = totem;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    if (m_Totem.Deleted)
                    {
                        Stop();
                        return;
                    }

                    // Regenerate health for mobiles within 5 tiles
                    foreach (Mobile m in m_Totem.GetMobilesInRange(5))
                    {
                        if (m.Alive && m.CanBeBeneficial(m_Caster))
                        {
                            m.Heal(Utility.RandomMinMax(2, 5));
                            m.FixedEffect(0x376A, 1, 16, 1109, 3); // Healing effect on mobiles
                        }
                    }
                }
            }
        }
    }
}
