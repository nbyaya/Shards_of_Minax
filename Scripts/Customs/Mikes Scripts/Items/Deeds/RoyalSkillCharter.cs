using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RoyalSkillCharter : Item
    {
        [Constructable]
        public RoyalSkillCharter() : base( 5360 ) // Use the appropriate item ID for your charter
        {
            Name = "Royal Skill Charter";
            Hue = 1266; // Adjust color as needed
            Weight = 1.0;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Raise Skillcap by 5");
        }

        public RoyalSkillCharter(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;

            if (player == null)
            {
                from.SendMessage("Only players can use this.");
                return;
            }

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }

            if (player.SkillsCap >= 5000) // Skill cap is measured in tenths of points (500 * 10)
            {
                from.SendMessage("You have reached the maximum skill cap.");
                return;
            }

            player.SkillsCap += 50; // Increase skill cap by 5 (5 * 10)
            from.SendMessage("Your skill cap has been increased.");
            this.Consume(); // Consume the charter
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
    }
}
