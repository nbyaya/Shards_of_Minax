using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SkillOrb : Item
    {
        [Constructable]
        public SkillOrb() : base(0x1869) // Use the appropriate item ID for your orb
        {
            Name = "Skill Orb";
            Hue = 1153; // Adjust color as needed
            Weight = 1.0;
        }
		
		public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Raise Skillcap by 1");
        }

        public SkillOrb(Serial serial) : base(serial)
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

            player.SkillsCap += 10; // Skill cap is measured in tenths of points
            from.SendMessage("Your skill cap has been increased.");
            this.Consume(); // Consume the orb
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
