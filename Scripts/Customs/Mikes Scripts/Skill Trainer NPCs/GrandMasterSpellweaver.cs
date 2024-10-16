using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterSpellweaver : BaseCreature
    {
        [Constructable]
        public GrandMasterSpellweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Elminster";
            this.Title = "the GrandMaster Spellweaver";

            this.Body = 0x190;
            this.Hue = Utility.RandomSkinHue();

            this.AddItem(new Robe(Utility.RandomNeutralHue())); // He wears a robe

            // Add other properties or equipment as needed.
        }

        public GrandMasterSpellweaver(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Spellweaving skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Customize the cost as needed
            {
                if (from.Skills.Spellweaving.Base < 120)
                {
                    from.Skills.Spellweaving.Base += 0.1; // Customize the skill gain amount
                    from.SendMessage("Your Spellweaving skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered the art of Spellweaving.");
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
