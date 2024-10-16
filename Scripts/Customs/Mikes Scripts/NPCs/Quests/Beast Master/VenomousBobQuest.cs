using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class VenomousBobQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Venomous Bob's Beastly Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Venomous Bob, a connoisseur of serpents and all things slithery. " +
                    "To prove your worth and gain access to my exclusive beast shop, you must take on my beastly challenge. " +
                    "Your tasks are as follows:\n\n" +
                    "1. Slay a Blood Serpent.\n" +
                    "2. Defeat a Celestial Python.\n" +
                    "3. Conquer an Emperor Cobra.\n" +
                    "4. Overcome a Frost Serpent.\n" +
                    "5. Destroy a Gorgon Viper.\n" +
                    "6. Vanquish an Inferno Python.\n" +
                    "7. Subdue a Shadow Anaconda.\n" +
                    "8. Eradicate a Thunder Serpent.\n" +
                    "9. Defeat a Titan Boa.\n" +
                    "10. Exterminate a Vengeful Pit Viper.\n\n" +
                    "Complete these tasks, and my beastly shop will be open to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face my serpentine challenge."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those serpents!"; } }

        public override object Complete { get { return "Well done! You have proven your worth. My shop is now open to you!"; } }

        public VenomousBobQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BloodSerpent), "Blood Serpents", 1));
            AddObjective(new SlayObjective(typeof(CelestialPython), "Celestial Pythons", 1));
            AddObjective(new SlayObjective(typeof(EmperorCobra), "Emperor Cobras", 1));
            AddObjective(new SlayObjective(typeof(FrostSerpent), "Frost Serpents", 1));
            AddObjective(new SlayObjective(typeof(GorgonViper), "Gorgon Vipers", 1));
            AddObjective(new SlayObjective(typeof(InfernoPython), "Inferno Pythons", 1));
            AddObjective(new SlayObjective(typeof(ShadowAnaconda), "Shadow Anacondas", 1));
            AddObjective(new SlayObjective(typeof(ThunderSerpent), "Thunder Serpents", 1));
            AddObjective(new SlayObjective(typeof(TitanBoa), "Titan Boas", 1));
            AddObjective(new SlayObjective(typeof(VengefulPitViper), "Vengeful Pit Vipers", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(VenomToken), 1, "Beast Master Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Venomous Bob's challenge!");
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

    public class VenomousBob : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVenomousBob());
        }

        [Constructable]
        public VenomousBob()
            : base("Venomous Bob", "Master of Serpents")
        {
        }

        public VenomousBob(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(VenomToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my beastly shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beast Master Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(VenomousBobQuest)
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

    public class SBVenomousBob : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBVenomousBob()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the beast-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BloodSerpent), 1000, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(CelestialPython), 1200, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(EmperorCobra), 1100, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostSerpent), 1050, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(GorgonViper), 1150, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernoPython), 1250, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowAnaconda), 1300, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(ThunderSerpent), 1400, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(TitanBoa), 1350, 10, 21, 0));
                Add(new AnimalBuyInfo(1, typeof(VengefulPitViper), 1450, 10, 21, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    // Define the VenomToken item
    public class VenomToken : Item
    {
        [Constructable]
        public VenomToken() : base(0x1B74)
        {
            Name = "Beast Master Token";
            Weight = 1.0;
        }

        public VenomToken(Serial serial) : base(serial)
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
