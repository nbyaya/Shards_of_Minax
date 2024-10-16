using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SatyrMasterQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Satyr Master's Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Satyr Master Thistletoes, " +
                    "the keeper of the mystical satyrs. To earn the right to access " +
                    "my exclusive shop and obtain a rare Satyr Token, you must prove " +
                    "your valor by defeating the following satyrs:\n\n" +
                    "1. Arcane Satyr\n" +
                    "2. Celestial Satyr\n" +
                    "3. Enigmatic Satyr\n" +
                    "4. Frenzied Satyr\n" +
                    "5. Gentle Satyr\n" +
                    "6. Melodic Satyr\n" +
                    "7. Mystic Satyr\n" +
                    "8. Rhythmic Satyr\n" +
                    "9. Tempest Satyr\n" +
                    "10. Wicked Satyr\n\n" +
                    "Complete these tasks, and you will gain access to my satyr shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the satyrs."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting the satyrs!"; } }

        public override object Complete { get { return "Excellent work! You have proven yourself worthy of my satyr shop. Enjoy your rewards!"; } }

        public SatyrMasterQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ArcaneSatyr), "Arcane Satyrs", 1));
            AddObjective(new SlayObjective(typeof(CelestialSatyr), "Celestial Satyrs", 1));
            AddObjective(new SlayObjective(typeof(EnigmaticSatyr), "Enigmatic Satyrs", 1));
            AddObjective(new SlayObjective(typeof(FrenziedSatyr), "Frenzied Satyrs", 1));
            AddObjective(new SlayObjective(typeof(GentleSatyr), "Gentle Satyrs", 1));
            AddObjective(new SlayObjective(typeof(MelodicSatyr), "Melodic Satyrs", 1));
            AddObjective(new SlayObjective(typeof(MysticSatyr), "Mystic Satyrs", 1));
            AddObjective(new SlayObjective(typeof(RhythmicSatyr), "Rhythmic Satyrs", 1));
            AddObjective(new SlayObjective(typeof(TempestSatyr), "Tempest Satyrs", 1));
            AddObjective(new SlayObjective(typeof(WickedSatyr), "Wicked Satyrs", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(SatyrToken), 1, "Satyr Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Satyr Master's challenge!");
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

    public class SatyrMasterThistletoes : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSatyrMasterThistletoes());
        }

        [Constructable]
        public SatyrMasterThistletoes()
            : base("Satyr Master Thistletoes", "Keeper of the Satyrs")
        {
        }

        public SatyrMasterThistletoes(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(SatyrToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my satyr shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Satyr Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SatyrMasterQuest)
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

    public class SBSatyrMasterThistletoes : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBSatyrMasterThistletoes()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the satyr-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(ArcaneSatyr), 1000, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(CelestialSatyr), 1200, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(EnigmaticSatyr), 900, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(FrenziedSatyr), 950, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(GentleSatyr), 1100, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(MelodicSatyr), 1300, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(MysticSatyr), 1400, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(RhythmicSatyr), 1250, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(TempestSatyr), 1500, 10, 271, 0));
                Add(new AnimalBuyInfo(1, typeof(WickedSatyr), 1350, 10, 271, 0));
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
