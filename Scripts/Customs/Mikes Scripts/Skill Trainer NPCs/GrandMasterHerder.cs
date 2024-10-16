using System;
using Server;
using Server.Items; 

namespace Server.Mobiles
{
    public class GrandMasterHerder : BaseCreature
    {
        [Constructable]
        public GrandMasterHerder() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Bo Peep";
            this.Title = "the GrandMaster Herder";

            this.Body = 0x191; // You can choose an appropriate body value for her
            this.Hue = Utility.RandomSkinHue();

            // Equip her with items, you can choose appropriate ones for a herder
            this.AddItem(new Robe());

            // Add other properties as needed.
        }

        public GrandMasterHerder(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Herding skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Herding.Base < 120)
                {
                    from.Skills.Herding.Base += 0.1;
                    from.SendMessage("Your Herding skill has improved under Bo Peep's guidance!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You've already mastered the art of Herding.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("Bo Peep requires gold for her training.");
                return false;
            }
        }
    }
}
