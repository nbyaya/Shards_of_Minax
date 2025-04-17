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

            // Store the player's old location so we can come back.
            Point3D oldLocation = from.Location;
            Map oldMap = from.Map;

            // Define the new location to flee to.
            Point3D newLocation = new Point3D(1325, 1624, 55);
            Map newMap = Map.Trammel;

            // Gather the player's pets (controlled or summoned).
            List<BaseCreature> pets = new List<BaseCreature>();
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is BaseCreature bc)
                {
                    if ((bc.Controlled && bc.ControlMaster == from) ||
                        (bc.Summoned  && bc.SummonMaster  == from))
                    {
                        pets.Add(bc);
                    }
                }
            }

            // Teleport the player to the "runaway" location.
            from.MoveToWorld(newLocation, newMap);

            // Teleport each pet as well, dismounting them first if necessary.
            foreach (BaseCreature pet in pets)
            {
                if (pet is IMount mount && mount.Rider != null)
                {
                    mount.Rider = null; // forcibly dismount
                }
                pet.MoveToWorld(newLocation, newMap);
            }

            // Display a message to the player.
            from.SendMessage("You bravely run away, taking your pets with you!");

            // Create a portal at your new location that will send you back.
            // It will last for 2 minutes (customize as needed).
            TemporaryReturnPortal portal = new TemporaryReturnPortal(oldLocation, oldMap, TimeSpan.FromMinutes(2.0));
            portal.MoveToWorld(newLocation, newMap);

            // Notify the player that the portal exists.
            from.SendMessage("A magical portal opens nearby! Double-click it within 2 minutes to return.");
        }
    }

    // ------------------------------------------------------------
    // TemporaryReturnPortal
    // ------------------------------------------------------------
    public class TemporaryReturnPortal : Item
    {
        private Point3D m_ReturnLocation;
        private Map m_ReturnMap;
        private DateTime m_ExpireTime;
        private Timer m_Timer;

        [Constructable]
        public TemporaryReturnPortal(Point3D returnLocation, Map returnMap, TimeSpan duration)
            : base(0xF6C)  // 0xF6C is the item ID for a moongate graphic
        {
            Movable = false;
            Hue = 0x490;  // Optional: Change hue if desired
            m_ReturnLocation = returnLocation;
            m_ReturnMap = returnMap;
            m_ExpireTime = DateTime.UtcNow + duration;

            // Check every second if the portal should expire
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), CheckExpire);
        }

        public TemporaryReturnPortal(Serial serial) : base(serial)
        {
        }

        // When a player double-clicks the portal, if they're close enough, teleport them back.
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
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
                Delete(); // Portal expires
            }
        }

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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ReturnLocation = reader.ReadPoint3D();
            m_ReturnMap = reader.ReadMap();
            m_ExpireTime = reader.ReadDeltaTime();

            // Restart the expiration timer if server restarts while portal still active.
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), CheckExpire);
        }
    }
}
