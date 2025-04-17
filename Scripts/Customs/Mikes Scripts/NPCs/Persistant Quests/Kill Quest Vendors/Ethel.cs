using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class EthelQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Acid Elementals";

        public override object Description => "The acidic scourge must be contained! The wilds are brimming with unstable Acid Elementals, corrupting everything they touch.\n\nSlay 500 of these volatile beasts, and I shall reward you with access to my experimental wares.";

        public override object Refuse => "A wise decision... or a coward's choice? Either way, the elementals persist.";

        public override object Uncomplete => "The acid still bubbles in the deep... You haven't slain enough.";

        public override object Complete => "You've done it. The acidic plague has lessened—if only for a time.\n\nAs promised, my arcane collection is now available to you. Handle it with care.";

        public EthelQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AcidElemental), "Acid Elementals", 500));
            AddReward(new BaseReward(typeof(Gold), 2500, "2500 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.AcidElementalQuest))
                profile.Talents[TalentID.AcidElementalQuest] = new Talent(TalentID.AcidElementalQuest);

            profile.Talents[TalentID.AcidElementalQuest].Points = 1;

            Owner.SendMessage(0x23, "You have purged 500 Acid Elementals for Ethel the Alchemist!");
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

    public class Ethel : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBEthel());
        }

        [Constructable]
        public Ethel() : base("Ethel", "the Alchemist")
        {
        }

        public Ethel(Serial serial) : base(serial)
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
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals());
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.AcidElementalQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "Ah, my fellow acid slayer. Have a look at my finest curiosities.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Come back when you've dealt with those Acid Elementals. I only trade with proven hands.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(EthelQuest) };

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

    public class SBEthel : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(AcidElemental), 5000, 5, 0x23B, 0));
                Add(new GenericBuyInfo(typeof(ExoticFish), 5000, 10, 0x9CC, 0));
                Add(new GenericBuyInfo(typeof(HildebrandtShield), 5000, 5, 0x1B74, 0));
                Add(new GenericBuyInfo(typeof(GarbageBag), 5000, 10, 0xE76, 0));
                Add(new GenericBuyInfo(typeof(GlassOfBubbly), 5000, 10, 0x99B, 0));
                Add(new GenericBuyInfo(typeof(CivilWarCache), 5000, 3, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(CyrusTreasure), 5000, 3, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(DroidWorkshopChest), 5000, 3, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(EmperorLegacyChest), 5000, 3, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(WisdomsCirclet), 5000, 3, 0x1F09, 0));
                Add(new GenericBuyInfo(typeof(TheThinkingCap), 5000, 3, 0x1F0C, 0));
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
