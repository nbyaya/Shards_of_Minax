using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class XerxesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "The Purge of 500 Juka Lords";

        public override object Description => 
            "I am Xerxes the Beastcaller. The Juka Lords threaten the balance of nature.\n\n" +
            "These unnatural warriors must be driven back. Prove your might: slay *five hundred* of their kind.\n\n" +
            "Return to me when their pride is broken. I shall reward you with my rarest creatures and items.";

        public override object Refuse => 
            "Perhaps your courage needs sharpening. Return when you're ready.";

        public override object Uncomplete => 
            "You have not slain enough Juka Lords. Do not return until their numbers are truly broken.";

        public override object Complete =>
            "You have done well, hunter. The Juka Lords fall silent in your wake.\n\n" +
            "My shop is now open to you. Within, rare beasts and strange relics await.\n\n" +
            "Tread wisely, and may your journey continue in strength.";

        public XerxesQuest()
        {
            AddObjective(new SlayObjective(typeof(JukaLord), "Juka Lords", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.JukaLordSlayerQuest))
                profile.Talents[TalentID.JukaLordSlayerQuest] = new Talent(TalentID.JukaLordSlayerQuest);

            profile.Talents[TalentID.JukaLordSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Juka Lords for Xerxes!");
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

    public class Xerxes : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBXerxes());
        }

        [Constructable]
        public Xerxes() : base("Xerxes", "The Beastcaller")
        {
        }

        public Xerxes(Serial serial) : base(serial)
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
            FacialHairItemID = Utility.RandomList(0x203E, 0x2040, 0x204B);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals());
            AddItem(new Robe(0x497));
            AddItem(new WizardsHat(0x497));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.JukaLordSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned my trust. My rarest beasts and relics are yours to purchase.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "I only deal with those who have proven their worth against the Juka Lords.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(XerxesQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBXerxes : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Price: 5000gp for each
                Add(new AnimalBuyInfo(1, typeof(JukaLord), 5000, 10, 0x2101, 0));
                Add(new GenericBuyInfo("Tin Tub", typeof(TinTub), 5000, 5, 0x1B36, 0));
                Add(new GenericBuyInfo("Fishing Bear", typeof(FishingBear), 5000, 5, 0x20D6, 0));
                Add(new GenericBuyInfo("World Shard", typeof(WorldShard), 5000, 5, 0x2B6F, 0));
                Add(new GenericBuyInfo("Sheep Carcass", typeof(SheepCarcass), 5000, 5, 0x1609, 0));
                Add(new GenericBuyInfo("Tailoring Talisman", typeof(TailoringTalisman), 5000, 5, 0x2F58, 0));
                Add(new GenericBuyInfo("Decorative Orchid", typeof(DecorativeOrchid), 5000, 5, 0xC97, 0));
                Add(new GenericBuyInfo("Sub Oil", typeof(SubOil), 5000, 5, 0x1F5B, 0));
                Add(new GenericBuyInfo("Fancy Painting", typeof(FancyPainting), 5000, 5, 0x9A3, 0));
                Add(new GenericBuyInfo("Medusa Head", typeof(MedusaHead), 5000, 5, 0x23A0, 0));
                Add(new GenericBuyInfo("Pet Rock", typeof(PetRock), 5000, 5, 0x1363, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
