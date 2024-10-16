using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CrabbertCrustaceanQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Crabbert's Crustacean Conquest"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Crabbert the Crustacean Collector, " +
                    "and I have an obsession with crabs of all kinds. If you wish to earn " +
                    "access to my exclusive shop, you'll need to prove your skill by " +
                    "collecting a unique token from each type of crab I've set my eyes on:\n\n" +
                    "1. Slay a BansheeCrab.\n" +
                    "2. Defeat an EtherealCrab.\n" +
                    "3. Conquer an IceCrab.\n" +
                    "4. Overcome a LavaCrab.\n" +
                    "5. Destroy a MagneticCrab.\n" +
                    "6. Vanquish a PoisonousCrab.\n" +
                    "7. Subdue a RiptideCrab.\n" +
                    "8. Eradicate a ShadowCrab.\n" +
                    "9. Defeat a StormCrab.\n" +
                    "10. Exterminate a VortexCrab.\n\n" +
                    "Complete these tasks, and I'll gladly open my shop to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you're ready to face the crabs!"; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those crabs!"; } }

        public override object Complete { get { return "Fantastic! You've proven your prowess with crabs. My shop is now open to you!"; } }

        public CrabbertCrustaceanQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BansheeCrab), "BansheeCrabs", 1));
            AddObjective(new SlayObjective(typeof(EtherealCrab), "EtherealCrabs", 1));
            AddObjective(new SlayObjective(typeof(IceCrab), "IceCrabs", 1));
            AddObjective(new SlayObjective(typeof(LavaCrab), "LavaCrabs", 1));
            AddObjective(new SlayObjective(typeof(MagneticCrab), "MagneticCrabs", 1));
            AddObjective(new SlayObjective(typeof(PoisonousCrab), "PoisonousCrabs", 1));
            AddObjective(new SlayObjective(typeof(RiptideCrab), "RiptideCrabs", 1));
            AddObjective(new SlayObjective(typeof(ShadowCrab), "ShadowCrabs", 1));
            AddObjective(new SlayObjective(typeof(StormCrab), "StormCrabs", 1));
            AddObjective(new SlayObjective(typeof(VortexCrab), "VortexCrabs", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(CrabToken), 1, "Crab Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You've completed Crabbert's challenge!");
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

    public class CrabbertCrustacean : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCrabbertCrustacean());
        }

        [Constructable]
        public CrabbertCrustacean()
            : base("Crabbert the Crustacean Collector", "Master of Crabs")
        {
        }

        public CrabbertCrustacean(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(CrabToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my crab shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You need a Crab Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CrabbertCrustaceanQuest)
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

    public class SBCrabbertCrustacean : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBCrabbertCrustacean()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the crab-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BansheeCrab), 1000, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(EtherealCrab), 1200, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(IceCrab), 900, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(LavaCrab), 950, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(MagneticCrab), 1100, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(PoisonousCrab), 1300, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(RiptideCrab), 1400, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowCrab), 1250, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(StormCrab), 1350, 10, 1510, 0));
                Add(new AnimalBuyInfo(1, typeof(VortexCrab), 1500, 10, 1510, 0));
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
