using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Commands
{
    public class RunawayCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("runaway", AccessLevel.Player, new CommandEventHandler(Runaway_OnCommand));
        }

        [Usage("runaway")]
        [Description("Instantly teleports you and your pets to a predefined location, and creates a limited-time portal to return.")]
        public static void Runaway_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            Point3D oldLocation = from.Location;
            Map oldMap = from.Map;

            Point3D newLocation = new Point3D(1325, 1624, 55);
            Map newMap = Map.Trammel;

            List<BaseCreature> pets = new List<BaseCreature>();
            BaseCreature playersMount = from.Mount as BaseCreature; // Get current mount

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is BaseCreature bc)
                {
                    // Skip the player's current mount
                    if (bc == playersMount)
                        continue;

                    if ((bc.Controlled && bc.ControlMaster == from) ||
                        (bc.Summoned && bc.SummonMaster == from))
                    {
                        pets.Add(bc);
                    }
                }
            }

            // Teleport pets first (excluding current mount)
            foreach (BaseCreature pet in pets)
            {
                if (pet is IMount mount && mount.Rider != null)
                {
                    mount.Rider = null;
                }
                pet.MoveToWorld(newLocation, newMap);
            }

            // Teleport player and their mount together
            from.MoveToWorld(newLocation, newMap);

            from.SendMessage("You bravely run away, taking your pets with you!");

            TemporaryReturnPortal portal = new TemporaryReturnPortal(oldLocation, oldMap, TimeSpan.FromMinutes(2.0), pets);
            portal.MoveToWorld(newLocation, newMap);

            from.SendMessage("A magical portal opens nearby! Double-click it within 2 minutes to return.");
        }
    }

    public class TemporaryReturnPortal : Item
    {
        private Point3D m_ReturnLocation;
        private Map m_ReturnMap;
        private DateTime m_ExpireTime;
        private Timer m_Timer;
        private List<BaseCreature> m_Pets;

        [Constructable]
        public TemporaryReturnPortal(Point3D returnLocation, Map returnMap, TimeSpan duration, List<BaseCreature> pets)
            : base(0xF6C)
        {
            Movable = false;
            Hue = 0x490;
            m_ReturnLocation = returnLocation;
            m_ReturnMap = returnMap;
            m_ExpireTime = DateTime.UtcNow + duration;
            m_Pets = pets;

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), CheckExpire);
        }

        public TemporaryReturnPortal(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                // Teleport pets back
                foreach (BaseCreature pet in m_Pets)
                {
                    if (pet.Map != Map.Internal)
                    {
                        pet.MoveToWorld(m_ReturnLocation, m_ReturnMap);
                    }
                }

                // Teleport player and their mount together
                from.MoveToWorld(m_ReturnLocation, m_ReturnMap);
                from.SendMessage("You step through the portal and return to where you fled from!");
                Delete();
            }
            else
            {
                from.SendMessage("You are too far away to use the portal.");
            }
        }

        private void CheckExpire()
        {
            if (DateTime.UtcNow >= m_ExpireTime)
            {
                Delete();
            }
        }

        // Remaining methods unchanged for brevity
        // (Serialization/Deserialization same as previous version)
        public override void OnDelete()
        {
            base.OnDelete();
            if (m_Timer != null)
                m_Timer.Stop();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_ReturnLocation);
            writer.Write(m_ReturnMap);
            writer.WriteDeltaTime(m_ExpireTime);
            writer.WriteMobileList<BaseCreature>(m_Pets);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ReturnLocation = reader.ReadPoint3D();
            m_ReturnMap = reader.ReadMap();
            m_ExpireTime = reader.ReadDeltaTime();
            m_Pets = reader.ReadStrongMobileList<BaseCreature>();

            // Restart the expiration timer if server restarts while portal still active.
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), CheckExpire);
        }		
    }
}