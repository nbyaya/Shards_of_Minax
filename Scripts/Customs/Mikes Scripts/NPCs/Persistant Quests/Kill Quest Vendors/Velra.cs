using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    // === Quest Definition ===
    public class VelraGhoulQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Hunt of 500 Ghouls";

        public override object Description => 
            "Darkness festers beneath the earth... and from it crawl the Ghouls.\n\n" +
            "I am Velra, Necrotracker of the Pale Moon Circle. My brethren have fallen prey to these soulless fiends.\n\n" +
            "Prove to me that you are more than a blade in the dark. Slay *five hundred* of these creatures, and I shall open my trove to you.\n\n" +
            "Among my wares: bound Ghouls, and relic chests pulled from the hands of ancient kings and cursed empires.";

        public override object Refuse => "A wise choice, perhaps. These ghouls are no mere pests.";

        public override object Uncomplete => "You’ve not yet sent 500 Ghouls to their graves. Return when the stench of rot no longer makes you flinch.";

        public override object Complete =>
            "The earth grows quieter. I feel it. You have done what I asked, and more.\n\n" +
            "My respect is yours... and so are my wares. May your coin be heavy and your enemies few.";

        public VelraGhoulQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Ghoul), "Ghouls", 500));
            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GhoulSlayerQuest))
                profile.Talents[TalentID.GhoulSlayerQuest] = new Talent(TalentID.GhoulSlayerQuest);

            profile.Talents[TalentID.GhoulSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Ghouls for Velra.");
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

    // === The NPC Itself ===
    public class Velra : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVelra());
        }

        [Constructable]
        public Velra() : base("Velra", "Necrotracker")
        {
        }

        public Velra(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2047;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x497));
            AddItem(new Sandals(0x497));
            AddItem(new HoodedShroudOfShadows());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GhoulSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "My treasures are yours to peruse.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You are not yet worthy. Slay the 500 Ghouls I asked of you.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(VelraGhoulQuest) };

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

    // === Velra's Shop ===
    public class SBVelra : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBVelra() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Bound Ghoul pet for sale
                Add(new AnimalBuyInfo(1, typeof(Ghoul), 5000, 10, 17, 0));

                // Chests for 5000gp
                Add(new GenericBuyInfo("Arcade King's Treasure", typeof(ArcadeKingsTreasure), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Arcade Master's Vault", typeof(ArcadeMastersVault), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Arcane Treasure Chest", typeof(ArcaneTreasureChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Arcanum Chest", typeof(ArcanumChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Archery Bonus Chest", typeof(ArcheryBonusChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Ashoka's Treasure Chest", typeof(AshokasTreasureChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Ashoka Treasure Chest", typeof(AshokaTreasureChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Assassin's Coffer", typeof(AssassinsCoffer), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Athenian Treasure Chest", typeof(AthenianTreasureChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo("Babylonian Chest", typeof(BabylonianChest), 5000, 10, 0xE43, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                // No selling back to Velra
            }
        }
    }
}
