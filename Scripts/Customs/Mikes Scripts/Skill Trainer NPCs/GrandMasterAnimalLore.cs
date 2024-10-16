using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterAnimalLore : BaseCreature
    {
        [Constructable]
        public GrandMasterAnimalLore() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "David Attenborough";
            this.Title = "the GrandMaster of Animal Lore";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equip the NPC with appropriate items, for example:
            this.AddItem(new Robe());
            this.AddItem(new Sandals());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterAnimalLore(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        // Handle the interaction logic for training Animal Lore skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check, you can customize this as needed
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming training costs 1000 gold
            {
                if (from.Skills.AnimalLore.Base < 120)
                {
                    from.Skills.AnimalLore.Base += 0.1; // Increase skill by 0.1, adjust as needed
                    from.SendMessage("Your Animal Lore skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already have extensive knowledge of Animal Lore.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my teachings.");
                return false;
            }
        }
    }
}
