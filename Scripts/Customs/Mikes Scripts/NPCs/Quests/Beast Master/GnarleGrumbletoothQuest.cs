using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GnarleGrumbletoothQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Gnarle Grumbletooth's Earthly Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Gnarle Grumbletooth, keeper of the earth's fury. " +
                    "To prove your worth and access my exclusive shop, you must subdue these mighty beasts:\n\n" +
                    "1. Slay a Crystal Warden.\n" +
                    "2. Defeat a Fossil Elemental.\n" +
                    "3. Conquer a Granite Colossus.\n" +
                    "4. Overcome a Lava Fiend.\n" +
                    "5. Destroy a Magma Golem.\n" +
                    "6. Vanquish a Mud Golem.\n" +
                    "7. Subdue a Quake Bringer.\n" +
                    "8. Eradicate a Sandstorm Elemental.\n" +
                    "9. Defeat a Stone Guardian.\n" +
                    "10. Exterminate a Terra Wisp.\n\n" +
                    "Complete these tasks, and I will grant you access to my shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the earth’s fury."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep proving your might against these beasts!"; } }

        public override object Complete { get { return "Impressive! You have conquered the earth's fury. My shop is now open to you!"; } }

        public GnarleGrumbletoothQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CrystalWarden), "Crystal Wardens", 1));
            AddObjective(new SlayObjective(typeof(FossilElemental), "Fossil Elementals", 1));
            AddObjective(new SlayObjective(typeof(GraniteColossus), "Granite Colossi", 1));
            AddObjective(new SlayObjective(typeof(LavaFiend), "Lava Fiends", 1));
            AddObjective(new SlayObjective(typeof(MagmaGolem), "Magma Golems", 1));
            AddObjective(new SlayObjective(typeof(MudGolem), "Mud Golems", 1));
            AddObjective(new SlayObjective(typeof(QuakeBringer), "Quake Bringers", 1));
            AddObjective(new SlayObjective(typeof(SandstormElemental), "Sandstorm Elementals", 1));
            AddObjective(new SlayObjective(typeof(StoneGuardian), "Stone Guardians", 1));
            AddObjective(new SlayObjective(typeof(TerraWisp), "Terra Wisps", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(EarthToken), 1, "Earth Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Gnarle Grumbletooth's challenge!");
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
	
    public class GnarleGrumbletooth : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGnarleGrumbletooth());
        }

        [Constructable]
        public GnarleGrumbletooth()
            : base("Gnarle Grumbletooth", "Master of Earth's Fury")
        {
        }

        public GnarleGrumbletooth(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(EarthToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my earthy beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have an Earth Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GnarleGrumbletoothQuest)
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


    public class SBGnarleGrumbletooth : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBGnarleGrumbletooth()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the earthy-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(CrystalWarden), 1000, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(FossilElemental), 1200, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(GraniteColossus), 1100, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(LavaFiend), 1300, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(MagmaGolem), 1400, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(MudGolem), 950, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(QuakeBringer), 1500, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(SandstormElemental), 1250, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(StoneGuardian), 1350, 10, 14, 0));
                Add(new AnimalBuyInfo(1, typeof(TerraWisp), 1000, 10, 14, 0));
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



