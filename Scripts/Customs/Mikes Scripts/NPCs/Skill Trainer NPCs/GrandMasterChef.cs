using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterChef : BaseCreature
    {
        [Constructable]
        public GrandMasterChef() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Guy Fieri";
            this.Title = "the GrandMaster Chef";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equip the NPC with chef-related items
            this.AddItem(new Skillet());
            this.AddItem(new FancyShirt());
            this.AddItem(new LongPants());
            this.AddItem(new Sandals());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterChef(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Cooking skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // This is a basic check; you can customize it further
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Cooking.Base < 120)
                {
                    from.Skills.Cooking.Base += 0.1; // Increase by 0.1; adjust as needed
                    from.SendMessage("Your Cooking skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You're already a master chef.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my culinary expertise.");
                return false;
            }
        }
    }
}
