using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MechanoGrimQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dr. MechanoGrim's Robotic Roundup"; } }

        public override object Description
        {
            get
            {
                return
                    "Greetings, brave adventurer! I am Dr. MechanoGrim, the eccentric " +
                    "collector of all things mechanical. To gain access to my exclusive shop, " +
                    "you must prove your prowess against some of the most advanced robotic creatures " +
                    "I have ever encountered. Complete these tasks:\n\n" +
                    "1. Destroy a Dreadnaught.\n" +
                    "2. Defeat an ElectroWraith.\n" +
                    "3. Overcome a FrostDroid.\n" +
                    "4. Vanquish a GrapplerDrone.\n" +
                    "5. Conquer an InfernoSentinel.\n" +
                    "6. Crush a NanoSwarm.\n" +
                    "7. Annihilate a PlasmaJuggernaut.\n" +
                    "8. Obliterate a SpectralAutomaton.\n" +
                    "9. Neutralize a TacticalEnforcer.\n" +
                    "10. Destroy a VortexConstruct.\n\n" +
                    "Complete these tasks, and you will gain access to my exclusive robotic beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to take on these mechanical marvels."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep battling the robots!"; } }

        public override object Complete { get { return "Remarkable! You have proven yourself worthy against my mechanical creations. My shop is now open to you!"; } }

        public MechanoGrimQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Dreadnaught), "Dreadnaughts", 1));
            AddObjective(new SlayObjective(typeof(ElectroWraith), "ElectroWraiths", 1));
            AddObjective(new SlayObjective(typeof(FrostDroid), "FrostDroids", 1));
            AddObjective(new SlayObjective(typeof(GrapplerDrone), "GrapplerDrones", 1));
            AddObjective(new SlayObjective(typeof(InfernoSentinel), "InfernoSentinels", 1));
            AddObjective(new SlayObjective(typeof(NanoSwarm), "NanoSwarms", 1));
            AddObjective(new SlayObjective(typeof(PlasmaJuggernaut), "PlasmaJuggernauts", 1));
            AddObjective(new SlayObjective(typeof(SpectralAutomaton), "SpectralAutomatons", 1));
            AddObjective(new SlayObjective(typeof(TacticalEnforcer), "TacticalEnforcers", 1));
            AddObjective(new SlayObjective(typeof(VortexConstruct), "VortexConstructs", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(MechanoToken), 1, "Mechano Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed Dr. MechanoGrim's robotic challenge!");
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

    public class DrMechanoGrim : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBDrMechanoGrim());
        }

        [Constructable]
        public DrMechanoGrim()
            : base("Dr. MechanoGrim", "Master of Mechanical Beasts")
        {
        }

        public DrMechanoGrim(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(MechanoToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my mechanical beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Mechano Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MechanoGrimQuest)
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

    public class SBDrMechanoGrim : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBDrMechanoGrim()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the robotic-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(Dreadnaught), 1000, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(ElectroWraith), 1200, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostDroid), 900, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(GrapplerDrone), 950, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernoSentinel), 1100, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(NanoSwarm), 1300, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(PlasmaJuggernaut), 1400, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(SpectralAutomaton), 1250, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(TacticalEnforcer), 1500, 10, 0x2F4, 0));
                Add(new AnimalBuyInfo(1, typeof(VortexConstruct), 1350, 10, 0x2F4, 0));
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
