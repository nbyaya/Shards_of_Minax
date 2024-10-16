using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ToadstoneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Hopper Toadstone's Toad Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Hopper Toadstone, the greatest toad enthusiast you'll ever meet. " +
                    "To prove your worth and gain access to my exclusive shop, you must hunt down the following toads: \n\n" +
                    "1. Blighted Toad\n" +
                    "2. Corrosive Toad\n" +
                    "3. Cursed Toad\n" +
                    "4. Eldritch Toad\n" +
                    "5. Fungal Toad\n" +
                    "6. Infernal Toad\n" +
                    "7. Shadow Toad\n" +
                    "8. Spectral Toad\n" +
                    "9. Venomous Toad\n" +
                    "10. Vile Toad\n\n" +
                    "Complete these tasks, and I will grant you access to my special toad shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to hunt some toads!"; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those toads!"; } }

        public override object Complete { get { return "Fantastic! You've proven your toad-hunting skills. My shop is now open to you!"; } }

        public ToadstoneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlightedToad), "Blighted Toads", 1));
            AddObjective(new SlayObjective(typeof(CorrosiveToad), "Corrosive Toads", 1));
            AddObjective(new SlayObjective(typeof(CursedToad), "Cursed Toads", 1));
            AddObjective(new SlayObjective(typeof(EldritchToad), "Eldritch Toads", 1));
            AddObjective(new SlayObjective(typeof(FungalToad), "Fungal Toads", 1));
            AddObjective(new SlayObjective(typeof(InfernalToad), "Infernal Toads", 1));
            AddObjective(new SlayObjective(typeof(ShadowToad), "Shadow Toads", 1));
            AddObjective(new SlayObjective(typeof(SpectralToad), "Spectral Toads", 1));
            AddObjective(new SlayObjective(typeof(VenomousToad), "Venomous Toads", 1));
            AddObjective(new SlayObjective(typeof(VileToad), "Vile Toads", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(ToadstoneToken), 1, "Toadstone Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Hopper Toadstone's challenge!");
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


    public class HopperToadstone : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHopperToadstone());
        }

        [Constructable]
        public HopperToadstone()
            : base("Hopper Toadstone", "Master of Toads")
        {
        }

        public HopperToadstone(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(ToadstoneToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my toad shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Toadstone Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ToadstoneQuest)
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


    public class SBHopperToadstone : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBHopperToadstone()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the toad-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BlightedToad), 1000, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(CorrosiveToad), 1200, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(CursedToad), 950, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(EldritchToad), 1100, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(FungalToad), 900, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernalToad), 1300, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowToad), 1150, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(SpectralToad), 1250, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomousToad), 1050, 10, 80, 0));
                Add(new AnimalBuyInfo(1, typeof(VileToad), 1200, 10, 80, 0));
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
