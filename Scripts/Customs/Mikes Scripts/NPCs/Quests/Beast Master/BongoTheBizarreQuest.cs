using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BongoTheBizarreQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bongo the Bizarre's Beastly Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Bongo the Bizarre, a collector of the most " +
                    "curious creatures. To gain access to my exclusive beast shop, you must " +
                    "prove your mettle by defeating these peculiar primates and arachnids. Complete the following tasks:\n\n" +
                    "1. Slay a Baboon Alpha.\n" +
                    "2. Defeat a Capuchin Trickster.\n" +
                    "3. Conquer a Chimpanzee Berserker.\n" +
                    "4. Overcome a Gibbon Mystic.\n" +
                    "5. Destroy a Howler Monkey.\n" +
                    "6. Vanquish a Mandrill Shaman.\n" +
                    "7. Subdue a Mountain Gorilla.\n" +
                    "8. Eradicate an Orangutan Sage.\n" +
                    "9. Defeat a Sifaka Warrior.\n" +
                    "10. Exterminate a Spider Monkey.\n\n" +
                    "Complete these tasks, and my unique shop will be open to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you're ready to tackle these creatures."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep fighting!"; } }

        public override object Complete { get { return "Well done! You've proven yourself worthy. My shop is now open to you!"; } }

        public BongoTheBizarreQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BaboonsAlpha), "Baboon Alphas", 1));
            AddObjective(new SlayObjective(typeof(CapuchinTrickster), "Capuchin Tricksters", 1));
            AddObjective(new SlayObjective(typeof(ChimpanzeeBerserker), "Chimpanzee Berserkers", 1));
            AddObjective(new SlayObjective(typeof(GibbonMystic), "Gibbon Mystics", 1));
            AddObjective(new SlayObjective(typeof(HowlerMonkey), "Howler Monkeys", 1));
            AddObjective(new SlayObjective(typeof(MandrillShaman), "Mandrill Shamans", 1));
            AddObjective(new SlayObjective(typeof(MountainGorilla), "Mountain Gorillas", 1));
            AddObjective(new SlayObjective(typeof(OrangutanSage), "Orangutan Sages", 1));
            AddObjective(new SlayObjective(typeof(SifakaWarrior), "Sifaka Warriors", 1));
            AddObjective(new SlayObjective(typeof(SpiderMonkey), "Spider Monkeys", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BongoToken), 1, "Bongo Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Bongo the Bizarre's challenge!");
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

    public class BongoTheBizarre : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBongoTheBizarre());
        }

        [Constructable]
        public BongoTheBizarre()
            : base("Bongo the Bizarre", "Collector of Curious Creatures")
        {
        }

        public BongoTheBizarre(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BongoToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my collection of curious creatures!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Bongo Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BongoTheBizarreQuest)
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

    public class SBBongoTheBizarre : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBongoTheBizarre()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the beast-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BaboonsAlpha), 1000, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(CapuchinTrickster), 1200, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(ChimpanzeeBerserker), 1100, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(GibbonMystic), 1300, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(HowlerMonkey), 1400, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(MandrillShaman), 1250, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(MountainGorilla), 1500, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(OrangutanSage), 1600, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(SifakaWarrior), 1350, 10, 29, 0));
                Add(new AnimalBuyInfo(1, typeof(SpiderMonkey), 1550, 10, 29, 0));
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
