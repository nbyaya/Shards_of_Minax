using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    // Quest Definition
    public class BraggukQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Bragguk's Ettin Slaughter";

        public override object Description => "You stand before Bragguk, once a chieftain of stone-skinned warriors.\n\n" +
                                              "He seeks vengeance for betrayals long past and will reward anyone who fells 500 Ettins.\n\n" +
                                              "\"Break their bones. Smash their clubs. Show me their blood, and I show you my wares.\"\n\n" +
                                              "Return only when the number five-hundred is etched in broken corpses.";

        public override object Refuse => "Bragguk snorts. \"Come back when spine stronger.\"";

        public override object Uncomplete => "Bragguk growls. \"Not enough Ettin blood. Keep killing.\"";

        public override object Complete =>
            "Bragguk grins, teeth like stone. \"You did it. Ettins scream no more. You earn my trust... and goods.\"\n\n" +
            "Buy what you want. I make fair trade for fair blood spilled.";

        public BraggukQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Ettin), "Ettins", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Access to Bragguk's Wares"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.EttinSlayerQuest))
                profile.Talents[TalentID.EttinSlayerQuest] = new Talent(TalentID.EttinSlayerQuest);

            profile.Talents[TalentID.EttinSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Ettins for Bragguk!");
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
            int version = reader.ReadInt();
        }
    }

    // NPC Definition
    public class Bragguk : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBragguk());
        }

        [Constructable]
        public Bragguk() : base("Bragguk", "Ettin Hunter")
        {
        }

        public Bragguk(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(150, 100, 50);
            Female = false;
            Race = Race.Human;

            Hue = 33770; // Stone-like skin hue
            HairItemID = 0x2048;
            HairHue = 1150;
            FacialHairItemID = 0x204B;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new BearMask());
            AddItem(new StuddedGorget());
            AddItem(new BoneChest());
            AddItem(new BoneArms());
            AddItem(new BoneLegs());
            AddItem(new Boots());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.EttinSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "You earned right to buy. Choose.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You not ready. Kill more Ettins!");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(BraggukQuest) };

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

    // Vendor Shop
    public class SBBragguk : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Ettin), 5000, 10, 17, 0)); // Tamed Ettin

                Add(new GenericBuyInfo(typeof(LumberjackingAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(MaceFightingAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(MageryAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(ManaDrainAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(ManaLeechAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(MeditationAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(MiningAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(MirrorOfKalandra), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(MusicianshipAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(NeckSlotChangeDeed), 5000, 20, 0x2FB7, 0));
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
