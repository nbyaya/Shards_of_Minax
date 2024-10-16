using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MucusMaximusQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mucus Maximus' Slime Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Mucus Maximus, master of slimes and all things gooey. " +
                    "To prove your worth and gain access to my exclusive shop, you must show your prowess " +
                    "against the squishiest of foes. Complete these tasks:\n\n" +
                    "1. Slay an Acidic Slime.\n" +
                    "2. Defeat a Crystal Ooze.\n" +
                    "3. Conquer an Electric Slime.\n" +
                    "4. Overcome a Frozen Ooze.\n" +
                    "5. Destroy a Glistening Ooze.\n" +
                    "6. Vanquish a Molten Slime.\n" +
                    "7. Subdue a Radiant Slime.\n" +
                    "8. Eradicate a Shadow Sludge.\n" +
                    "9. Defeat a Toxic Sludge.\n" +
                    "10. Exterminate a Void Slime.\n\n" +
                    "Complete these tasks, and I will grant you access to my slimy beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the slimes."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep battling the slimes!"; } }

        public override object Complete { get { return "Well done! You have proven yourself against the slimiest of foes. My shop is now open to you!"; } }

        public MucusMaximusQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AcidicSlime), "Acidic Slimes", 1));
            AddObjective(new SlayObjective(typeof(CrystalOoze), "Crystal Oozes", 1));
            AddObjective(new SlayObjective(typeof(ElectricSlime), "Electric Slimes", 1));
            AddObjective(new SlayObjective(typeof(FrozenOoze), "Frozen Oozes", 1));
            AddObjective(new SlayObjective(typeof(GlisteningOoze), "Glistening Oozes", 1));
            AddObjective(new SlayObjective(typeof(MoltenSlime), "Molten Slimes", 1));
            AddObjective(new SlayObjective(typeof(RadiantSlime), "Radiant Slimes", 1));
            AddObjective(new SlayObjective(typeof(ShadowSludge), "Shadow Sludges", 1));
            AddObjective(new SlayObjective(typeof(ToxicSludge), "Toxic Sludges", 1));
            AddObjective(new SlayObjective(typeof(VoidSlime), "Void Slimes", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(SlimeToken), 1, "Slime Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed Mucus Maximus' slime challenge!");
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

    public class MucusMaximus : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMucusMaximus());
        }

        [Constructable]
        public MucusMaximus()
            : base("Mucus Maximus", "Master of the Slimes")
        {
        }

        public MucusMaximus(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(SlimeToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my slime shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Slime Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MucusMaximusQuest)
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

    public class SBMucusMaximus : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBMucusMaximus()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the slime-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AcidicSlime), 1000, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(CrystalOoze), 1200, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(ElectricSlime), 900, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(FrozenOoze), 950, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(GlisteningOoze), 1100, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(MoltenSlime), 1300, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(RadiantSlime), 1400, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowSludge), 1250, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(ToxicSludge), 1500, 10, 51, 0));
                Add(new AnimalBuyInfo(1, typeof(VoidSlime), 1350, 10, 51, 0));
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
