using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class HikariQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Trial of the Fox Flame";
        public override object Description =>
            "I am Hikari, Keeper of the Celestial Grove.\n\n" +
            "Lately, the lands tremble beneath the paws of Bake Kitsune—fox spirits twisted by dark intent. " +
            "They once were protectors... now, they burn all they touch.\n\n" +
            "Bring down 500 of these corrupted flames. End their suffering, and prove yourself worthy of sacred knowledge and rare relics.";

        public override object Refuse => "Then go, but beware the flickering eyes in the mist. They watch.";
        public override object Uncomplete => "The flames still dance. You have not yet silenced enough Bake Kitsune.";
        public override object Complete =>
            "The grove sighs in relief. You have calmed the chaos with your blade.\n\n" +
            "Take these wares, bound by spirit and stone. Spend wisely, friend of the foxfire.";

        public HikariQuest()
        {
            AddObjective(new SlayObjective(typeof(BakeKitsune), "Bake Kitsune", 500));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.BakeKitsuneSlayerQuest))
                profile.Talents[TalentID.BakeKitsuneSlayerQuest] = new Talent(TalentID.BakeKitsuneSlayerQuest);

            profile.Talents[TalentID.BakeKitsuneSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have cleansed the lands of 500 Bake Kitsune.");
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

    public class Hikari : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHikari());
        }

        [Constructable]
        public Hikari() : base("Hikari", "Spirit Warden")
        {
        }

        public Hikari(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2044, 0x2047, 0x2048);
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Robe(1153)); // Light blue hue
            AddItem(new Sandals(0x901));
            AddItem(new HoodedShroudOfShadows()); // Mystical feel
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.BakeKitsuneSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned the right to trade with me, Flamewalker.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Your spirit is not yet proven. Return after silencing 500 Bake Kitsune.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(HikariQuest) };

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

    public class SBHikari : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBHikari() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(BakeKitsune), 5000, 5, 0x25D1, 0));

                Add(new GenericBuyInfo(typeof(RookStone), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo(typeof(AlchemistBookcase), 5000, 10, 0x2DDD, 0));
                Add(new GenericBuyInfo(typeof(ElementRegular), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(Satchel), 5000, 10, 0x9AA, 0));
                Add(new GenericBuyInfo(typeof(LightningAugmentCrystal), 5000, 10, 0x186E, 0));
                Add(new GenericBuyInfo(typeof(PhysicalResistAugmentCrystal), 5000, 10, 0x186E, 0));
                Add(new GenericBuyInfo(typeof(SwordsmanshipAugmentCrystal), 5000, 10, 0x186E, 0));
                Add(new GenericBuyInfo(typeof(BloodSwarmGem), 5000, 10, 0xF19, 0));
                Add(new GenericBuyInfo(typeof(BullFrogSummoningMateria), 5000, 10, 0x26BD, 0));
                Add(new GenericBuyInfo(typeof(AstartesShoulderGuard), 5000, 10, 0x2B74, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
