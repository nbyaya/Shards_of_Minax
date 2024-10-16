using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterArmsLoreTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterArmsLoreTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Clint Eastwood";
            this.Title = "the Arms Lore Grandmaster";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            this.AddItem(new SmithHammer()); // Smith's hammer or any suitable item
            this.AddItem(new FullApron()); // Full apron or any suitable clothing
            this.AddItem(new LongPants()); // Long pants or any suitable clothing
            this.AddItem(new Boots()); // Boots or any suitable footwear

            // Add other properties or equipment as needed
        }

        public GrandMasterArmsLoreTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Arms Lore skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Assume training costs 1000 gold
            {
                if (from.Skills.ArmsLore.Base < 120)
                {
                    from.Skills.ArmsLore.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Arms Lore skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Arms Lore.");
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
