using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FlareonBlazeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Flareon Blaze's Fiery Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Flareon Blaze, the master of fire. " +
                    "To prove your worth and gain access to my exclusive shop, you must " +
                    "show your valor by defeating some of the fiercest fire beasts. Complete these tasks:\n\n" +
                    "1. Slay a Cinder Wraith.\n" +
                    "2. Defeat an Ember Serpent.\n" +
                    "3. Conquer an Ember Spirit.\n" +
                    "4. Overcome a Flare Imp.\n" +
                    "5. Destroy an Infernal Duke.\n" +
                    "6. Vanquish an Inferno Warden.\n" +
                    "7. Subdue a Molten Golem.\n" +
                    "8. Eradicate a Pyroclastic Golem.\n" +
                    "9. Defeat a Solar Elemental.\n" +
                    "10. Exterminate a Volcanic Titan.\n\n" +
                    "Complete these tasks, and I will grant you access to my fiery beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the flames."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep battling the inferno!"; } }

        public override object Complete { get { return "Outstanding! You have proven yourself against the fires of the world. My shop is now open to you!"; } }

        public FlareonBlazeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CinderWraith), "Cinder Wraiths", 1));
            AddObjective(new SlayObjective(typeof(EmberSerpent), "Ember Serpents", 1));
            AddObjective(new SlayObjective(typeof(EmberSpirit), "Ember Spirits", 1));
            AddObjective(new SlayObjective(typeof(FlareImp), "Flare Imps", 1));
            AddObjective(new SlayObjective(typeof(InfernalDuke), "Infernal Dukes", 1));
            AddObjective(new SlayObjective(typeof(InfernoWarden), "Inferno Wardens", 1));
            AddObjective(new SlayObjective(typeof(MoltenGolem), "Molten Golems", 1));
            AddObjective(new SlayObjective(typeof(PyroclasticGolem), "Pyroclastic Golems", 1));
            AddObjective(new SlayObjective(typeof(SolarElemental), "Solar Elementals", 1));
            AddObjective(new SlayObjective(typeof(VolcanicTitan), "Volcanic Titans", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(FlameToken), 1, "Flame Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Flareon Blaze's challenge!");
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

    public class FlareonBlaze : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFlareonBlaze());
        }

        [Constructable]
        public FlareonBlaze()
            : base("Flareon Blaze", "Master of the Fiery Beasts")
        {
        }

        public FlareonBlaze(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(FlameToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my fiery beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Flame Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FlareonBlazeQuest)
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
	
    public class SBFlareonBlaze : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBFlareonBlaze()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the fiery-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(CinderWraith), 1000, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(EmberSerpent), 1200, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(EmberSpirit), 950, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(FlareImp), 1100, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernalDuke), 1400, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernoWarden), 1300, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(MoltenGolem), 1500, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(PyroclasticGolem), 1600, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(SolarElemental), 1250, 10, 15, 0));
                Add(new AnimalBuyInfo(1, typeof(VolcanicTitan), 1700, 10, 15, 0));
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
