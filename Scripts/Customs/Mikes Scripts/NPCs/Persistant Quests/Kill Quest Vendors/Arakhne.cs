using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Engines.Quests
{
    public class ArakhneSpiderQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Web of Retribution";

        public override object Description => 
            "Greetings, wanderer. I am Arakhne, guardian of the ancient silk paths.\n\n" +
            "A monstrous swarm of Giant Spiders has overrun the lower forests, threatening balance and beauty alike.\n\n" +
            "I seek a soul both fearless and relentless. Destroy 500 of these twisted beasts, and I shall grant you access to rare treasures—and a loyal spider of your own.";

        public override object Refuse => "Then the forest shall continue to suffer...";

        public override object Uncomplete => "You have not yet slain enough of the foul spiders. Return when your work is done.";

        public override object Complete => 
            "The tremors of your victory echo through the webways. The Giant Spiders have been culled, and balance begins to return.\n\n" +
            "As promised, my wares are now yours to peruse. May they serve you well, silk-hunter.";

        public ArakhneSpiderQuest()
        {
            AddObjective(new SlayObjective(typeof(GiantSpider), "Giant Spiders", 500));
            AddReward(new BaseReward(typeof(Gold), 2500, "2500 Gold"));
            AddReward(new BaseReward(typeof(SpidersSilk), 100, "100 Spider Silk"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GiantSpiderSlayerQuest))
                profile.Talents[TalentID.GiantSpiderSlayerQuest] = new Talent(TalentID.GiantSpiderSlayerQuest);

            profile.Talents[TalentID.GiantSpiderSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Giant Spiders for Arakhne!");
            Owner.PlaySound(CompleteSound);
        }

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

    public class Arakhne : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArakhne());
        }

        [Constructable]
        public Arakhne() : base("Arakhne", "The Silkweaver")
        {
        }

        public Arakhne(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;
            Hue = 33770; // Pale tone

            HairItemID = 0x203B;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Robe(1109));
            AddItem(new Sandals(1108));
            AddItem(new Cloak(1107));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GiantSpiderSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "Welcome, silk-hunter. Choose your reward.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must first slay 500 Giant Spiders to earn my trust.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(ArakhneSpiderQuest)
        };

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

    public class SBArakhne : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBArakhne() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(GiantSpider), 5000, 10, 0x25CE, 0));
                Add(new GenericBuyInfo(typeof(CandyCarnivalCoffer), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(CaptainCooksTreasure), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(CelticLegendsChest), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(ChamplainTreasureChest), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(CheeseConnoisseursCache), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(ChocolatierTreasureChest), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(CivilRightsStrongbox), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(CivilWarChest), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(ClownsWhimsicalChest), 5000, 10, 0xE75, 0));
                Add(new GenericBuyInfo(typeof(ComradesCache), 5000, 10, 0xE75, 0));
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
