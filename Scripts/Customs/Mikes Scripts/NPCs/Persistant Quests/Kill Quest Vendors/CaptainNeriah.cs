using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class DolphinSlayerQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Culling the Tides";
        public override object Description =>
            "I am Captain Neriah, once of the High Seas Fleet.\n\n" +
            "The dolphins, once graceful companions to sailors, have turned ravenous—twisted by strange magicks beneath the waves.\n\n" +
            "I need someone brave enough to cull their numbers. Slay 500 of them, and I shall reward you—not with coin, but with secrets and rare goods for the bold.\n\n" +
            "Return to me when the seas run calm again.";
        public override object Refuse => "The ocean will claim more lives until someone acts. Return if you find your courage.";
        public override object Uncomplete => "You haven't slain enough dolphins. The waters are still unsafe.";
        public override object Complete =>
            "You've returned... and the tides feel calmer already.\n\n" +
            "As promised, you now have access to my secret stock. These goods are yours, should you afford them.\n\n" +
            "May the seas be kind to you, hunter.";

        public DolphinSlayerQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Dolphin), "Dolphins", 500));
            AddReward(new BaseReward(typeof(Gold), 5000, "5,000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.DolphinSlayerQuest))
                profile.Talents[TalentID.DolphinSlayerQuest] = new Talent(TalentID.DolphinSlayerQuest);
            
            profile.Talents[TalentID.DolphinSlayerQuest].Points = 1;

            Owner.SendMessage(0x59, "You have calmed the seas. Captain Neriah now welcomes your trade.");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class CaptainNeriah : MondainQuester
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        public override bool IsActiveVendor => true;
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCaptainNeriah());
        }

        [Constructable]
        public CaptainNeriah() : base("Captain Neriah", "Watcher of the Waves")
        {
        }

        public CaptainNeriah(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new ThighBoots(Utility.RandomBlueHue()));
            AddItem(new FancyShirt(Utility.RandomBlueHue()));
            AddItem(new Skirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomBlueHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.DolphinSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "You've earned the right. Browse my wares.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must first slay 500 dolphins to gain my trust.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(DolphinSlayerQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class SBCaptainNeriah : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Dolphin), 5000, 10, 0x20DD, 0));

                Add(new GenericBuyInfo(typeof(AlchemyAugmentCrystal), 5000, 10, 0x2BFD, 0));
                Add(new GenericBuyInfo(typeof(AnatomyAugmentCrystal), 5000, 10, 0x2BFD, 0));
                Add(new GenericBuyInfo(typeof(AnimalLoreAugmentCrystal), 5000, 10, 0x2BFD, 0));
                Add(new GenericBuyInfo(typeof(AnimalTamingAugmentCrystal), 5000, 10, 0x2BFD, 0));
                Add(new GenericBuyInfo(typeof(ArcheryAugmentCrystal), 5000, 10, 0x2BFD, 0));
                Add(new GenericBuyInfo(typeof(ArmsLoreAugmentCrystal), 5000, 10, 0x2BFD, 0));

                Add(new GenericBuyInfo(typeof(ArmSlotChangeDeed), 5000, 5, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(BagOfBombs), 5000, 5, 0x2256, 0));
                Add(new GenericBuyInfo(typeof(BagOfHealth), 5000, 5, 0x2256, 0));
                Add(new GenericBuyInfo(typeof(BagOfJuice), 5000, 5, 0x2256, 0));
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
