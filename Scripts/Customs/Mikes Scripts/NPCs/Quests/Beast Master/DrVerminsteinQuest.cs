using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DrVerminsteinQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dr. Verminstein's Pest Problem"; } }

        public override object Description
        {
            get
            {
                return 
                    "Hello there, brave adventurer! I am Dr. Verminstein, a renowned expert in pest control. " +
                    "Unfortunately, I've had a rather unfortunate rat infestation in my laboratory. " +
                    "To help me deal with this rodent crisis and earn access to my exclusive shop, " +
                    "you must hunt down these nasty rats. Here's what you need to do:\n\n" +
                    "1. Slay an Anthrax Rat.\n" +
                    "2. Defeat a Black Death Rat.\n" +
                    "3. Conquer a Cholera Rat.\n" +
                    "4. Eliminate a Death Rat.\n" +
                    "5. Destroy a Fever Rat.\n" +
                    "6. Vanquish a Leprosy Rat.\n" +
                    "7. Overcome a Malaria Rat.\n" +
                    "8. Defeat a Rabid Rat.\n" +
                    "9. Annihilate a Smallpox Rat.\n" +
                    "10. Exterminate a Typhus Rat.\n\n" +
                    "Complete these tasks, and I'll grant you access to my specialized pest shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to tackle these troublesome rodents."; } }

        public override object Uncomplete { get { return "You haven't managed to clear out all the rats yet. Keep hunting!"; } }

        public override object Complete { get { return "Fantastic job! You've cleared out the vermin and proven your worth. My shop is now open to you!"; } }

        public DrVerminsteinQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AnthraxRat), "Anthrax Rats", 1));
            AddObjective(new SlayObjective(typeof(BlackDeathRat), "Black Death Rats", 1));
            AddObjective(new SlayObjective(typeof(CholeraRat), "Cholera Rats", 1));
            AddObjective(new SlayObjective(typeof(DeathRat), "Death Rats", 1));
            AddObjective(new SlayObjective(typeof(FeverRat), "Fever Rats", 1));
            AddObjective(new SlayObjective(typeof(LeprosyRat), "Leprosy Rats", 1));
            AddObjective(new SlayObjective(typeof(MalariaRat), "Malaria Rats", 1));
            AddObjective(new SlayObjective(typeof(RabidRat), "Rabid Rats", 1));
            AddObjective(new SlayObjective(typeof(SmallpoxRat), "Smallpox Rats", 1));
            AddObjective(new SlayObjective(typeof(TyphusRat), "Typhus Rats", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(VerminToken), 1, "Vermin Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have successfully dealt with the vermin crisis!");
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

    public class DrVerminstein : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBDrVerminstein());
        }

        [Constructable]
        public DrVerminstein()
            : base("Dr. Verminstein", "Master of Pests")
        {
        }

        public DrVerminstein(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(VerminToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my pest control shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Vermin Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DrVerminsteinQuest)
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

    public class SBDrVerminstein : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBDrVerminstein()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the disease-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AnthraxRat), 1000, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(BlackDeathRat), 1200, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(CholeraRat), 900, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(DeathRat), 950, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(FeverRat), 1100, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(LeprosyRat), 1300, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(MalariaRat), 1400, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(RabidRat), 1250, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(SmallpoxRat), 1500, 10, 238, 0));
                Add(new AnimalBuyInfo(1, typeof(TyphusRat), 1350, 10, 238, 0));
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
