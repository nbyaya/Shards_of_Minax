using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using Server;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class DimensionDoor : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Dimension Door", "Fold Space!",
            21004, // Icon ID
            9300,  // Sound ID
            false  // Not a combat spell
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 50; } }

        private const int PortalDuration = 120; // Duration in seconds
        private const int CooldownMinutes = 60; // Cooldown in minutes

        public DimensionDoor(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.SendMessage("Select the first location for the portal.");
            Caster.Target = new InternalTarget(this, PortalSelection.FirstLocation);
        }

        private void TargetSelected(IPoint3D p, PortalSelection selection)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D location = new Point3D(p);
                Map map = Caster.Map;

                if (selection == PortalSelection.FirstLocation)
                {
                    Caster.SendMessage("Select the second location for the portal.");
                    Caster.Target = new InternalTarget(this, PortalSelection.SecondLocation, location, map);
                }
                else if (selection == PortalSelection.SecondLocation)
                {
                    CreatePortals(location, map, p);
                }
            }

            FinishSequence();
        }

        private void CreatePortals(Point3D firstLocation, Map firstMap, IPoint3D secondLocation)
        {
            Map map = Caster.Map;
            Point3D loc = new Point3D(secondLocation);

            Portal firstPortal = new Portal(Caster, firstLocation, map, loc, firstMap);
            Portal secondPortal = new Portal(Caster, loc, map, firstLocation, firstMap);

            firstPortal.MoveToWorld(firstLocation, map);
            secondPortal.MoveToWorld(loc, map);

            Effects.PlaySound(firstLocation, map, 0x1FE); // Sound for portal creation
            Effects.SendLocationParticles(EffectItem.Create(firstLocation, map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052);
            Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052);

            firstPortal.StartTimer(PortalDuration);
            secondPortal.StartTimer(PortalDuration);
        }

        private enum PortalSelection
        {
            FirstLocation,
            SecondLocation
        }

        private class InternalTarget : Target
        {
            private readonly DimensionDoor m_Owner;
            private readonly PortalSelection m_Selection;
            private readonly Point3D m_FirstLocation;
            private readonly Map m_FirstMap;

            public InternalTarget(DimensionDoor owner, PortalSelection selection, Point3D firstLocation = default(Point3D), Map firstMap = null) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
                m_Selection = selection;
                m_FirstLocation = firstLocation;
                m_FirstMap = firstMap;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D point)
                {
                    m_Owner.TargetSelected(point, m_Selection);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class Portal : Item
        {
            private Timer m_Timer;
            private Point3D m_Destination;
            private Map m_DestMap;

            public Portal(Mobile caster, Point3D location, Map map, Point3D dest, Map destMap) : base(0x1FD4)
            {
                Movable = false;
                Light = LightType.Circle300;
                Hue = 0x48D;

                m_Destination = dest;
                m_DestMap = destMap;

                MoveToWorld(location, map);
            }

            public void StartTimer(int durationInSeconds)
            {
                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(durationInSeconds));
                m_Timer.Start();
            }

            public override void OnDoubleClick(Mobile from)
            {
                if (from.InRange(this, 1))
                {
                    Effects.PlaySound(Location, Map, 0x1FE); // Sound for teleport
                    from.MoveToWorld(m_Destination, m_DestMap);
                    Effects.SendLocationParticles(EffectItem.Create(m_Destination, m_DestMap, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052);
                }
                else
                {
                    from.SendMessage("You are too far away to use the portal.");
                }
            }

            public Portal(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
                writer.Write(m_Destination);
                writer.Write(m_DestMap);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                m_Destination = reader.ReadPoint3D();
                m_DestMap = reader.ReadMap();
            }

            private class InternalTimer : Timer
            {
                private Portal m_Portal;

                public InternalTimer(Portal portal, TimeSpan duration) : base(duration)
                {
                    m_Portal = portal;
                }

                protected override void OnTick()
                {
                    m_Portal.Delete();
                }
            }
        }
    }
}
