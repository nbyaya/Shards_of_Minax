using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SweetsworthLollypopQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Sweetsworth Lollypop's Sweet Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Sweetsworth Lollypop, master of sweet beasts. " +
                    "To prove your worth and gain access to my exclusive candy-themed shop, you must " +
                    "show your bravery by dealing with the candy creatures that have escaped from my factory. " +
                    "Complete these tasks:\n\n" +
                    "1. Slay a Bubblegum Blaster.\n" +
                    "2. Defeat a Candy Corn Creep.\n" +
                    "3. Conquer a Caramel Conjurer.\n" +
                    "4. Overcome a Chocolate Truffle.\n" +
                    "5. Destroy a Gummy Sheep.\n" +
                    "6. Vanquish a Jellybean Jester.\n" +
                    "7. Subdue a Licorice Sheep.\n" +
                    "8. Eradicate a Lollipop Lord.\n" +
                    "9. Defeat a Peppermint Puff.\n" +
                    "10. Exterminate a Taffy Titan.\n\n" +
                    "Complete these tasks, and I will grant you access to my sweet beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the sweet challenge."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep dealing with those sweet beasts!"; } }

        public override object Complete { get { return "Sweet victory! You have proven yourself against my confectionary creations. My shop is now open to you!"; } }

        public SweetsworthLollypopQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BubblegumBlaster), "Bubblegum Blasters", 1));
            AddObjective(new SlayObjective(typeof(CandyCornCreep), "Candy Corn Creeps", 1));
            AddObjective(new SlayObjective(typeof(CaramelConjurer), "Caramel Conjurers", 1));
            AddObjective(new SlayObjective(typeof(ChocolateTruffle), "Chocolate Truffles", 1));
            AddObjective(new SlayObjective(typeof(GummySheep), "Gummy Sheep", 1));
            AddObjective(new SlayObjective(typeof(JellybeanJester), "Jellybean Jesters", 1));
            AddObjective(new SlayObjective(typeof(LicoriceSheep), "Licorice Sheep", 1));
            AddObjective(new SlayObjective(typeof(LollipopLord), "Lollipop Lords", 1));
            AddObjective(new SlayObjective(typeof(PeppermintPuff), "Peppermint Puffs", 1));
            AddObjective(new SlayObjective(typeof(TaffyTitan), "Taffy Titans", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(SweetsworthToken), 1, "Sweetsworth Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Sweetsworth Lollypop's challenge!");
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

    public class SweetsworthLollypop : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSweetsworthLollypop());
        }

        [Constructable]
        public SweetsworthLollypop()
            : base("Sweetsworth Lollypop", "Master of Sweet Beasts")
        {
        }

        public SweetsworthLollypop(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(SweetsworthToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my sweet beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Sweetsworth Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SweetsworthLollypopQuest)
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

    public class SBSweetsworthLollypop : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBSweetsworthLollypop()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the candy-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BubblegumBlaster), 1000, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(CandyCornCreep), 1200, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(CaramelConjurer), 900, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(ChocolateTruffle), 950, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(GummySheep), 1100, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(JellybeanJester), 1300, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(LicoriceSheep), 1400, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(LollipopLord), 1250, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(PeppermintPuff), 1500, 10, 207, 0));
                Add(new AnimalBuyInfo(1, typeof(TaffyTitan), 1350, 10, 207, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class SweetsworthToken : Item
    {
        [Constructable]
        public SweetsworthToken() : base(0x2D5B)
        {
            Name = "Sweetsworth Token";
            Hue = 1150; // Give it a sweet color
        }

        public SweetsworthToken(Serial serial) : base(serial)
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
    }
}
