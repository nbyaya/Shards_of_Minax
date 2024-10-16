using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BeastlyWobblepawsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bertram Wobblepaws' Beastly Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Bertram 'Beastly' Wobblepaws, the most eccentric beast master you'll ever meet. " +
                    "To prove your worth and gain access to my exclusive shop, you must hunt down the most unusual beasts. Complete these tasks:\n\n" +
                    "1. Slay an Azure Moose.\n" +
                    "2. Defeat a Crimson Mule.\n" +
                    "3. Conquer a Cursed White Tail.\n" +
                    "4. Overcome an Eclipse Reindeer.\n" +
                    "5. Destroy an Ember Axis.\n" +
                    "6. Vanquish a Frost Wapiti.\n" +
                    "7. Subdue a Mystic Fallow.\n" +
                    "8. Eradicate a Shadow Muntjac.\n" +
                    "9. Defeat a Storm Sika.\n" +
                    "10. Exterminate a Venomous Roe.\n\n" +
                    "Complete these tasks, and my quirky beast shop will be open to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you're ready to hunt these peculiar creatures."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those unusual beasts!"; } }

        public override object Complete { get { return "Amazing! You have proven yourself worthy of my peculiar beast collection. My shop is now open to you!"; } }

        public BeastlyWobblepawsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AzureMoose), "Azure Mooses", 1));
            AddObjective(new SlayObjective(typeof(CrimsonMule), "Crimson Mules", 1));
            AddObjective(new SlayObjective(typeof(CursedWhiteTail), "Cursed White Tails", 1));
            AddObjective(new SlayObjective(typeof(EclipseReindeer), "Eclipse Reindeers", 1));
            AddObjective(new SlayObjective(typeof(EmberAxis), "Ember Axis", 1));
            AddObjective(new SlayObjective(typeof(FrostWapiti), "Frost Wapitis", 1));
            AddObjective(new SlayObjective(typeof(MysticFallow), "Mystic Fallows", 1));
            AddObjective(new SlayObjective(typeof(ShadowMuntjac), "Shadow Muntjacs", 1));
            AddObjective(new SlayObjective(typeof(StormSika), "Storm Sikas", 1));
            AddObjective(new SlayObjective(typeof(VenomousRoe), "Venomous Roes", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(WobbleToken), 1, "Beastly Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Bertram Wobblepaws' challenge!");
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

    public class BertramWobblepaws : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBertramWobblepaws());
        }

        [Constructable]
        public BertramWobblepaws()
            : base("Bertram 'Beastly' Wobblepaws", "Master of Peculiar Beasts")
        {
        }

        public BertramWobblepaws(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(WobbleToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my beastly shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beastly Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BeastlyWobblepawsQuest)
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

    public class SBBertramWobblepaws : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBertramWobblepaws()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the peculiar creatures here
                Add(new AnimalBuyInfo(1, typeof(AzureMoose), 1000, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(CrimsonMule), 1200, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(CursedWhiteTail), 950, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(EclipseReindeer), 1100, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(EmberAxis), 1300, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostWapiti), 1400, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(MysticFallow), 1250, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowMuntjac), 1150, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(StormSika), 1350, 10, 234, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomousRoe), 1500, 10, 234, 0));
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
