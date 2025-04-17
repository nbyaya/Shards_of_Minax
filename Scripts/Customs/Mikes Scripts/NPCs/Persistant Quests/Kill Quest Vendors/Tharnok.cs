using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TharnokQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Grove: 500 Corpsers";

        public override object Description => 
            "I am Tharnok, Grovekeeper and Warden of the Marshlight Roots.\n\n" +
            "A blight grows unchecked—Corpsers, writhing, twisted things defiling nature itself.\n\n" +
            "Cleanse the swamps of five hundred of these vile husks. When the bogs run quiet, I will offer you the blessings of the Grove.\n\n" +
            "Only then may you claim what lies beneath: ancient creatures and refined crystals, sacred and rare.";

        public override object Refuse => 
            "The Grove endures, for now. But it cannot wait forever.";

        public override object Uncomplete => 
            "Their vines still grasp and their roots still crawl. Keep killing—nature demands it.";

        public override object Complete => 
            "The rot recoils from your presence. You have done what few would dare.\n\n" +
            "As promised, the Grove opens to you. I now offer creatures nurtured in secrecy and crystals formed in purity.\n\n" +
            "Spend your gold with care. These are not mere baubles, but ancient gifts.";

        public TharnokQuest()
        {
            AddObjective(new SlayObjective(typeof(Corpser), "Corpser", 500));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.CorpserSlayerQuest))
                profile.Talents[TalentID.CorpserSlayerQuest] = new Talent(TalentID.CorpserSlayerQuest);

            profile.Talents[TalentID.CorpserSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have cleansed 500 Corpsers from the Grove.");
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

    public class Tharnok : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTharnok());
        }

        [Constructable]
        public Tharnok() : base("Tharnok", "Grovekeeper")
        {
        }

        public Tharnok(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Elf;
            Hue = 33770;
            HairItemID = 0x203B;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new LeafChest());
            AddItem(new LeafArms());
            AddItem(new LeafLegs());
            AddItem(new Sandals());
            AddItem(new Cloak(1372));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.CorpserSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned the Grove’s trust. Browse its gifts.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must first rid the Grove of 500 Corpsers before I open my wares to you.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(TharnokQuest)
        };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SBTharnok : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Corpser), 5000, 10, 17, 0)); // Tamed Corpser for 5000gp

                Add(new GenericBuyInfo("BalCrystal", typeof(BalCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("TalCrystal", typeof(TalCrystal), 5000, 10, 0x1F1A, 0));
                Add(new GenericBuyInfo("JalCrystal", typeof(JalCrystal), 5000, 10, 0x1F1B, 0));
                Add(new GenericBuyInfo("RalCrystal", typeof(RalCrystal), 5000, 10, 0x1F1C, 0));
                Add(new GenericBuyInfo("KalCrystal", typeof(KalCrystal), 5000, 10, 0x1F1D, 0));
                Add(new GenericBuyInfo("MythicDiamond", typeof(MythicDiamond), 5000, 10, 0x1F1E, 0));
                Add(new GenericBuyInfo("MythicAmethyst", typeof(MythicAmethyst), 5000, 10, 0x1F1F, 0));
                Add(new GenericBuyInfo("LegendaryAmethyst", typeof(LegendaryAmethyst), 5000, 10, 0x1F20, 0));
                Add(new GenericBuyInfo("AncientAmethyst", typeof(AncientAmethyst), 5000, 10, 0x1F21, 0));
                Add(new GenericBuyInfo("LegendaryDiamond", typeof(LegendaryDiamond), 5000, 10, 0x1F22, 0));
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
