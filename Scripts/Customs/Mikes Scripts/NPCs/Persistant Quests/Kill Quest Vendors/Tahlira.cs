using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class TahliraQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Tomb";

        public override object Description => "The restless dead stir once more. I am Tahlira, Binder of the Sands.\n\n" +
            "The Mummies claw at the veil between life and death, and their corruption spreads like a plague. " +
            "Bring me their silence. Slay *five hundred* Mummies.\n\n" +
            "Only then shall I entrust you with rare wares drawn from across the desert and sea.";

        public override object Refuse => "Then be gone. Let others brave the tombs if you cannot.";

        public override object Uncomplete => "Your hands are clean... too clean. The Mummies still shamble. Keep fighting.";

        public override object Complete => "The tombs fall quiet, and the sand sighs with relief.\n\n" +
            "You have done what few dared to attempt, and fewer still survived. My wares are now yours to browse—" +
            "each one drawn from rare finds and forbidden trades.\n\n" +
            "May your path remain unbroken, warrior of silence.";

        public TahliraQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Mummy), "Mummies", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.MummySlayerQuest))
                profile.Talents[TalentID.MummySlayerQuest] = new Talent(TalentID.MummySlayerQuest);

            profile.Talents[TalentID.MummySlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have completed Tahlira's task and silenced 500 Mummies!");
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

    public class Tahlira : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTahlira());
        }

        [Constructable]
        public Tahlira() : base("Tahlira", "the Binding")
        {
        }

        public Tahlira(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049);
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new GoldRing());
            AddItem(new GoldBracelet());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.MummySlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "The tomb welcomes you, champion. Browse what I offer.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only those who have silenced the Mummies may earn my trust.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(TahliraQuest) };

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

    public class SBTahlira : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBTahlira()
        {
        }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Mummy), 5000, 10, 0x2DDF, 0)); // Adjust graphic if needed
                Add(new GenericBuyInfo("Grandma's Knitting", typeof(GrandmasKnitting), 5000, 10, 0x1E2B, 0));
                Add(new GenericBuyInfo("Fancy Ship Wheel", typeof(FancyShipWheel), 5000, 10, 0x0E89, 0));
                Add(new GenericBuyInfo("Exotic Boots", typeof(ExoticBoots), 5000, 10, 0x170B, 0));
                Add(new GenericBuyInfo("Fine Iron Wire", typeof(FineIronWire), 5000, 10, 0x1EB1, 0));
                Add(new GenericBuyInfo("Sea Serpent Steak", typeof(SeaSerpantSteak), 5000, 10, 0x097B, 0));
                Add(new GenericBuyInfo("Rare Oil", typeof(RareOil), 5000, 10, 0x1FDC, 0));
                Add(new GenericBuyInfo("Spiked Chair", typeof(SpikedChair), 5000, 10, 0x2847, 0));
                Add(new GenericBuyInfo("Kraken Trophy", typeof(KrakenTrophy), 5000, 10, 0x1C14, 0));
                Add(new GenericBuyInfo("Ztty Crystal", typeof(ZttyCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Fine Copper Wire", typeof(FineCopperWire), 5000, 10, 0x1EB1, 0));
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
