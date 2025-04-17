using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SsiltharQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Slaying of 500 Giant Serpents";

        public override object Description => 
            "Sss... You, warm-blood, have come at good time. I am Ssilthar, last of Coilbinders. " +
            "The wild serpentssss—GiantSerpentsss—have grown too many... too hungriesss...\n\n" +
            "Bring me 500 fangless corpsesss... and I give you reward worthy of ssstinger's bite.\n\n" +
            "Yesss... the wild must be tamed again.";

        public override object Refuse => 
            "Then begone, soft-skin. The jungle cares not for cowardsss.";

        public override object Uncomplete => 
            "Sss... not enough blood... not yet. You must ssslither deeper, and kill more.";

        public override object Complete =>
            "Yesss... I sssmell it. Your blade has danced with the serpents'sss fury...\n\n" +
            "As promisssed, the Coilbinder's hoard is open to you. Buy what you wisssh... you have earned it.";

        public SsiltharQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GiantSerpent), "Giant Serpents", 500));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GiantSerpentSlayerQuest))
                profile.Talents[TalentID.GiantSerpentSlayerQuest] = new Talent(TalentID.GiantSerpentSlayerQuest);

            profile.Talents[TalentID.GiantSerpentSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Giant Serpents for Ssilthar!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Ssilthar : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSerpentVendor());
        }

        [Constructable]
        public Ssilthar() : base("Ssilthar", "The Coilbinder")
        {
        }

        public Ssilthar(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = 0x455;

            HairItemID = 0;
            HairHue = 0;
        }

        public override void InitOutfit()
        {
            AddItem(new BoneArms());
            AddItem(new BoneChest());
            AddItem(new BoneLegs());
            AddItem(new LeatherGloves());
            AddItem(new Cloak(0x7DA));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GiantSerpentSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "Sss... you have earned thisss. Buy what you wish.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You have not completed the Coilbinder's hunt. Return with 500 ssslain.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(SsiltharQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBSerpentVendor : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(GiantSerpent), 5000, 10, 8495, 0)); // tamed serpent

                Add(new GenericBuyInfo(typeof(BakersDelightChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BakersDolightChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BavarianFestChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BismarcksTreasureChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BolsheviksLoot), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BountyHuntersCache), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BoyBandBox), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BrewmastersChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BritainsRoyalTreasuryChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(BuccaneersChest), 5000, 5, 0xE43, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
