using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PorkiusTruffleQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Porkius Truffle's Swine Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Ah, greetings, adventurer! I am Porkius Truffle, the swine whisperer. " +
                    "To gain access to my exclusive collection of swine-themed beasts, " +
                    "you must hunt and defeat various rare pigs and hogs. Complete these tasks:\n\n" +
                    "1. Slay a Babirusa Beast.\n" +
                    "2. Defeat a Borneo Pig.\n" +
                    "3. Conquer a Bush Pig.\n" +
                    "4. Overcome a Domestic Swine.\n" +
                    "5. Destroy a Giant Forest Hog.\n" +
                    "6. Vanquish a Hog Wild.\n" +
                    "7. Subdue a Javelina Jinx.\n" +
                    "8. Eradicate a Peccary Protector.\n" +
                    "9. Defeat a Vietnamese Pig.\n" +
                    "10. Exterminate a Warthog Warrior.\n\n" +
                    "Complete these tasks, and I shall grant you access to my swine-themed shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to prove yourself against the swine!"; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Continue your hunt for swine!"; } }

        public override object Complete { get { return "Impressive! You have proven yourself in the art of swine hunting. My shop is now open to you!"; } }

        public PorkiusTruffleQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BabirusaBeast), "Babirusa Beasts", 1));
            AddObjective(new SlayObjective(typeof(BorneoPig), "Borneo Pigs", 1));
            AddObjective(new SlayObjective(typeof(BushPig), "Bush Pigs", 1));
            AddObjective(new SlayObjective(typeof(DomesticSwine), "Domestic Swine", 1));
            AddObjective(new SlayObjective(typeof(GiantForestHog), "Giant Forest Hogs", 1));
            AddObjective(new SlayObjective(typeof(HogWild), "Hog Wilds", 1));
            AddObjective(new SlayObjective(typeof(JavelinaJinx), "Javelina Jinxes", 1));
            AddObjective(new SlayObjective(typeof(PeccaryProtector), "Peccary Protectors", 1));
            AddObjective(new SlayObjective(typeof(VietnamesePig), "Vietnamese Pigs", 1));
            AddObjective(new SlayObjective(typeof(WarthogWarrior), "Warthog Warriors", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(SwineToken), 1, "Swine Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Porkius Truffle's swine hunt!");
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

    public class PorkiusTruffle : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBPorkiusTruffle());
        }

        [Constructable]
        public PorkiusTruffle()
            : base("Porkius Truffle", "Master of Swine")
        {
        }

        public PorkiusTruffle(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(SwineToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my swine-themed shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Swine Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PorkiusTruffleQuest)
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

    public class SBPorkiusTruffle : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBPorkiusTruffle()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the swine-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BabirusaBeast), 1000, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(BorneoPig), 1200, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(BushPig), 900, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(DomesticSwine), 950, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(GiantForestHog), 1100, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(HogWild), 1300, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(JavelinaJinx), 1400, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(PeccaryProtector), 1250, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(VietnamesePig), 1350, 10, 290, 0));
                Add(new AnimalBuyInfo(1, typeof(WarthogWarrior), 1500, 10, 290, 0));
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
