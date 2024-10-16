using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BalthazarBeastyQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Balthazar Beasty's Bizarre Beasts"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Balthazar Beasty, connoisseur of curious creatures. " +
                    "To prove your worth and gain access to my peculiar shop, you must capture these " +
                    "extraordinary beasts:\n\n" +
                    "1. Slay an AbbadonTheAbyssal.\n" +
                    "2. Defeat a Chimereon.\n" +
                    "3. Conquer a Drolatic.\n" +
                    "4. Overcome a Grimorie.\n" +
                    "5. Destroy a GrotesqueOfRouen.\n" +
                    "6. Vanquish a GrymalkinTheWatcher.\n" +
                    "7. Subdue a Mordrake.\n" +
                    "8. Eradicate a Strix.\n" +
                    "9. Defeat a Vespa.\n" +
                    "10. Exterminate a VitrailTheMosaic.\n\n" +
                    "Complete these tasks, and I will open my shop of bizarre beasts to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face these bizarre beasts."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting the bizarre beasts!"; } }

        public override object Complete { get { return "Remarkable! You have proven yourself against the bizarre creatures of the realm. My shop is now open to you!"; } }

        public BalthazarBeastyQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AbbadonTheAbyssal), "Abbadon The Abyssal", 1));
            AddObjective(new SlayObjective(typeof(Chimereon), "Chimereon", 1));
            AddObjective(new SlayObjective(typeof(Drolatic), "Drolatic", 1));
            AddObjective(new SlayObjective(typeof(Grimorie), "Grimorie", 1));
            AddObjective(new SlayObjective(typeof(GrotesqueOfRouen), "Grotesque Of Rouen", 1));
            AddObjective(new SlayObjective(typeof(GrymalkinTheWatcher), "Grymalkin The Watcher", 1));
            AddObjective(new SlayObjective(typeof(Mordrake), "Mordrake", 1));
            AddObjective(new SlayObjective(typeof(Strix), "Strix", 1));
            AddObjective(new SlayObjective(typeof(Vespa), "Vespa", 1));
            AddObjective(new SlayObjective(typeof(VitrailTheMosaic), "Vitrail The Mosaic", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BeastyToken), 1, "Beasty Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Balthazar Beasty's bizarre challenge!");
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

    public class BalthazarBeasty : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBalthazarBeasty());
        }

        [Constructable]
        public BalthazarBeasty()
            : base("Balthazar Beasty", "Connoisseur of Curious Creatures")
        {
        }

        public BalthazarBeasty(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BeastyToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my shop of bizarre beasts!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beasty Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BalthazarBeastyQuest)
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

    public class SBBalthazarBeasty : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBalthazarBeasty()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the bizarre beasts here
                Add(new AnimalBuyInfo(1, typeof(AbbadonTheAbyssal), 1000, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(Chimereon), 1200, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(Drolatic), 900, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(Grimorie), 950, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(GrotesqueOfRouen), 1100, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(GrymalkinTheWatcher), 1300, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(Mordrake), 1400, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(Strix), 1250, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(Vespa), 1350, 10, 4, 0));
                Add(new AnimalBuyInfo(1, typeof(VitrailTheMosaic), 1500, 10, 4, 0));
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
