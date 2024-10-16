using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SqueakusFuzzlestoneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Squeakus Fuzzlestone's Squirrel Safari"; } }

        public override object Description
        {
            get
            {
                return
                    "Greetings, brave adventurer! I am Squeakus Fuzzlestone, the world's leading squirrel connoisseur. " +
                    "I have a passion for collecting every kind of squirrel there is! To prove your worth and gain access " +
                    "to my exclusive squirrel shop, you must hunt down and defeat the following types of squirrels:\n\n" +
                    "1. Alberts Squirrel\n" +
                    "2. Beldings Ground Squirrel\n" +
                    "3. Douglas Squirrel\n" +
                    "4. Eastern Gray Squirrel\n" +
                    "5. Flying Squirrel\n" +
                    "6. Fox Squirrel\n" +
                    "7. Indian Palm Squirrel\n" +
                    "8. Red Squirrel\n" +
                    "9. Red-Tailed Squirrel\n" +
                    "10. Rock Squirrel\n\n" +
                    "Bring me a unique token from each type, and my squirrel shop shall be yours to explore!";
            }
        }

        public override object Refuse { get { return "Ah, I see you're not up for the challenge. Come back when you're ready to embrace the squirrel adventure!"; } }

        public override object Uncomplete { get { return "You haven't collected all the tokens yet. Keep searching and hunting!"; } }

        public override object Complete { get { return "Remarkable! You have collected tokens from every type of squirrel. My shop is now open to you!"; } }

        public SqueakusFuzzlestoneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AlbertsSquirrel), "Alberts Squirrels", 1));
            AddObjective(new SlayObjective(typeof(BeldingsGroundSquirrel), "Beldings Ground Squirrels", 1));
            AddObjective(new SlayObjective(typeof(DouglasSquirrel), "Douglas Squirrels", 1));
            AddObjective(new SlayObjective(typeof(EasternGraySquirrel), "Eastern Gray Squirrels", 1));
            AddObjective(new SlayObjective(typeof(FlyingSquirrel), "Flying Squirrels", 1));
            AddObjective(new SlayObjective(typeof(FoxSquirrel), "Fox Squirrels", 1));
            AddObjective(new SlayObjective(typeof(IndianPalmSquirrel), "Indian Palm Squirrels", 1));
            AddObjective(new SlayObjective(typeof(RedSquirrel), "Red Squirrels", 1));
            AddObjective(new SlayObjective(typeof(RedTailedSquirrel), "Red-Tailed Squirrels", 1));
            AddObjective(new SlayObjective(typeof(RockSquirrel), "Rock Squirrels", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(SquirrelToken), 1, "Squirrel Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have successfully completed Squeakus Fuzzlestone's Squirrel Safari!");
            Owner.PlaySound(CompleteSound);
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

    public class SqueakusFuzzlestone : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSqueakusFuzzlestone());
        }

        [Constructable]
        public SqueakusFuzzlestone()
            : base("Squeakus Fuzzlestone", "Master of Squirrels")
        {
        }

        public SqueakusFuzzlestone(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(SquirrelToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my squirrel shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Squirrel Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SqueakusFuzzlestoneQuest)
                };
            }
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

    public class SBSqueakusFuzzlestone : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBSqueakusFuzzlestone()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the squirrel-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AlbertsSquirrel), 1000, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(BeldingsGroundSquirrel), 1200, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(DouglasSquirrel), 900, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(EasternGraySquirrel), 950, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(FlyingSquirrel), 1100, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(FoxSquirrel), 1300, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(IndianPalmSquirrel), 1400, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(RedSquirrel), 1250, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(RedTailedSquirrel), 1500, 10, 278, 0));
                Add(new AnimalBuyInfo(1, typeof(RockSquirrel), 1350, 10, 278, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }
}
