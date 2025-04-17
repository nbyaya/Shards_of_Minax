using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class DerrickQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Hind Cull";
        public override object Description =>
            "I am Derrick, once a ranger of Yew, now a quiet observer of imbalance.\n\n" +
            "The Hind, once graceful guardians of our woods, have multiplied far beyond nature's law. " +
            "Their unchecked numbers are warping the forest's harmony.\n\n" +
            "Cull *five hundred* of these beasts, and I shall grant you access to tools and trinkets " +
            "preserved from a different age. This is not cruelty—it is balance.";

        public override object Refuse => "Then leave, and may the woods remain blind to your presence.";
        public override object Uncomplete => "The Hind still graze unchecked. You must hunt further.";
        public override object Complete =>
            "You have done what few could stomach. The glades will remember your mercy through steel.\n\n" +
            "My wares are now yours to claim—oddities and remnants gathered through the wild paths.\n\n" +
            "Spend your gold, Wildwalker.";

        public DerrickQuest()
        {
            AddObjective(new SlayObjective(typeof(Hind), "Hinds", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.HindSlayerQuest))
                profile.Talents[TalentID.HindSlayerQuest] = new Talent(TalentID.HindSlayerQuest);

            profile.Talents[TalentID.HindSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Hinds for Derrick!");
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
            reader.ReadInt();
        }
    }

    public class Derrick : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBDerrick());
        }

        [Constructable]
        public Derrick() : base("Derrick", "the Wildwalker")
        {
        }

        public Derrick(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x2040);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new LeatherLegs());
            AddItem(new LeatherChest());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
            AddItem(new Bandana());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.HindSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You walk with the wilds now. Take what you need.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Nature is not yet balanced. Return when the Hind are culled.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(DerrickQuest)
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

    public class SBDerrick : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Hind), 5000, 10, 0xEA, 0));
                Add(new GenericBuyInfo(typeof(AnimalBox), 5000, 10, 0x9A9, 0));
                Add(new GenericBuyInfo(typeof(BakingBoard), 5000, 10, 0x1034, 0));
                Add(new GenericBuyInfo(typeof(SatanicTable), 5000, 10, 0x2810, 0));
                Add(new GenericBuyInfo(typeof(WaterFountain), 5000, 10, 0x1B75, 0));
                Add(new GenericBuyInfo(typeof(FountainWall), 5000, 10, 0x1B76, 0));
                Add(new GenericBuyInfo(typeof(HildebrandtFlag), 5000, 10, 0x1ED0, 0));
                Add(new GenericBuyInfo(typeof(MysteryOrb), 5000, 10, 0xE2D, 0));
                Add(new GenericBuyInfo(typeof(BlueberryPie), 5000, 10, 0x9EC, 0));
                Add(new GenericBuyInfo(typeof(BioSamples), 5000, 10, 0xE2D, 0));
                Add(new GenericBuyInfo(typeof(OldEmbroideryTool), 5000, 10, 0x13B2, 0));
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
