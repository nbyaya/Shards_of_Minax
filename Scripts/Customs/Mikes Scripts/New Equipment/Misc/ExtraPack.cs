using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class ExtraPack : Item // Inherit from Backpack or BaseContainer depending on your needs
    {
        private DateTime m_LastUsed;

        [Constructable]
        public ExtraPack() : base(0x09B2)
        {
            Name = "Extra Pack";
            m_LastUsed = DateTime.MinValue; // Initialize to ensure it's usable at least once
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack)) // Ensure the item is in the player's backpack
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }

            PlayerMobile player = from as PlayerMobile;

            if (player != null)
            {
                TimeSpan timeSinceLastUse = DateTime.Now - m_LastUsed;
                if (timeSinceLastUse < TimeSpan.FromMinutes(5))
                {
                    // Inform the player of the remaining cooldown time
                    player.SendMessage("You must wait for {0} more minutes before using this again.", (TimeSpan.FromMinutes(8) - timeSinceLastUse).Minutes);
                    return;
                }

                if (player.Skills.Camping.Value >= 100)
                {
                    // Apply the buff
                    player.Backpack.MaxItems += 100; // Increase item count limit

                    player.SendMessage("You feel invigorated, able to carry more for a short time!");

                    m_LastUsed = DateTime.Now; // Update the last used time

                    Timer.DelayCall(TimeSpan.FromMinutes(5), delegate
                    {
                        // Revert the buff after 10 seconds
                        player.Backpack.MaxItems -= 100;

                        player.SendMessage("The invigorating effect wears off.");
                    });
                }
                else
                {
                    player.SendMessage("You must have at least 100 skill in Camping to use this.");
                }
            }
        }

        public ExtraPack(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Increases Item Capacity");
        }
		
		public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write(m_LastUsed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                m_LastUsed = reader.ReadDateTime();
            }
        }
    }
}
