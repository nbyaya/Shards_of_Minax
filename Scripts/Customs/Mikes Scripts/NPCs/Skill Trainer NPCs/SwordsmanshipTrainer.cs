using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class SwordsmanshipTrainer : BaseCreature
    {
        [Constructable]
        public SwordsmanshipTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Hanzo";
            this.Title = "the Swordsmanship Trainer";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue();

            this.AddItem(new Kama()); // Swordsmanship trainers often have swords
            this.AddItem(new LeatherJingasa());
            this.AddItem(new ShortPants());
            this.AddItem(new Sandals());

            // Add other properties or equipment as needed.
        }

        public SwordsmanshipTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Swordsmanship skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Swords.Base < 120)
                {
                    from.Skills.Swords.Base += 0.1;
                    from.SendMessage("Your Swordsmanship skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You are already a master of Swordsmanship.");
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
