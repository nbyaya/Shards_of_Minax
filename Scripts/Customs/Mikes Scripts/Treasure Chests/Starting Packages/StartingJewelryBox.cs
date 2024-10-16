using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Multis.Deeds;
using System.Collections;
using Server.Multis;
using Server.Targeting;
using Server.Regions;

namespace Server.Custom
{
    public class StartingJewelryBox : LockableContainer
    {
        [Constructable]
        public StartingJewelryBox() : base(0x9AA) // Jewelry Box item ID
        {
            Name = "Jewelry Box";
            Hue = Utility.RandomMinMax(1, 1600);

            // Add random jewelry items
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.25);
            AddItemWithProbability(CreateRandomJewelry(), 0.15);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
			AddItemWithProbability(new SmallBoatDeed(), 0.01);
			AddItemWithProbability(new SmallDragonBoatDeed(), 0.01);
			AddItemWithProbability(new MediumBoatDeed(), 0.01);
			AddItemWithProbability(new MediumDragonBoatDeed(), 0.01);
			AddItemWithProbability(new LargeBoatDeed(), 0.01);
			AddItemWithProbability(new LargeDragonBoatDeed(), 0.01);
			AddItemWithProbability(new WoodHouseDeed(), 0.01);
			AddItemWithProbability(new LogCabinDeed(), 0.01);
            AddItemWithProbability(new Gold(Utility.RandomMinMax(1, 5000)), 0.20);
			AddItemWithProbability(new RandomSkillJewelryA(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryB(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryC(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryD(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryE(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryF(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryG(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryH(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryI(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryJ(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryK(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryL(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryM(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryN(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryO(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryP(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryQ(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryR(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryS(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryT(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryU(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryV(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryW(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryY(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryZ(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAA(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAB(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAC(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAD(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAE(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAF(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAG(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAH(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAI(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAJ(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAK(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAL(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAM(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAN(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAO(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAP(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }

        private Item CreateRandomJewelry()
        {
            Item item;
            int random = Utility.Random(10);

            switch (random)
            {
                case 0:
                    item = new GoldRing { Name = "Golden Ring", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 1:
                    item = new SilverRing { Name = "Silver Ring", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 2:
                    item = new GoldBracelet { Name = "Golden Bracelet", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 3:
                    item = new SilverBracelet { Name = "Silver Bracelet", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 4:
                    item = new RandomMagicJewelry { Name = "Family Jewels", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 5:
                    item = new Necklace { Name = "Fancy Necklace", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 6:
                    item = new GoldEarrings { Name = "Golden Earrings", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 7:
                    item = new SilverEarrings { Name = "Silver Earrings", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 8:
                    item = new RandomSkillJewelryX { Name = "Gemmed Ring", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                case 9:
                    item = new GoldNecklace { Name = "Golden Necklace", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
                default:
                    item = new GoldRing { Name = "Common Ring", Hue = Utility.RandomMinMax(1, 2000) };
                    break;
            }

            return item;
        }

        public StartingJewelryBox(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
