using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ThaddeusQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Headless Horde";

        public override object Description =>
            "The winds carry whispers of the Hollow... I am Thaddeus, last Sentinel of the forgotten crypts.\n\n" +
            "The HeadlessOnes multiply beyond the veil, their headless laughter chilling the marrow of the earth.\n\n" +
            "Bring me the silence of 500 HeadlessOnes. Cleanse their wickedness from this realm.\n\n" +
            "Only then will I trust you with the secrets and curiosities I’ve gathered from the crypts.";

        public override object Refuse => "Then you are not the one whispered of in the tombs. Begone.";

        public override object Uncomplete => "Their laughter still echoes. You haven’t slain enough.";

        public override object Complete =>
            "The crypts grow quiet... You've done it. The headless tide is broken.\n\n" +
            "As promised, my wares are now yours to peruse. Dark treasures, unearthed from forgotten worlds.";

        public ThaddeusQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(HeadlessOne), "HeadlessOnes", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.HeadlessSlayerQuest))
                profile.Talents[TalentID.HeadlessSlayerQuest] = new Talent(TalentID.HeadlessSlayerQuest);

            profile.Talents[TalentID.HeadlessSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 HeadlessOnes for Thaddeus!");
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

    public class Thaddeus : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThaddeus());
        }

        [Constructable]
        public Thaddeus() : base("Thaddeus", "The Hollow Watcher")
        {
        }

        public Thaddeus(Serial serial) : base(serial)
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
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x497));
            AddItem(new Sandals(0));
            AddItem(new Cloak(0x455));
            AddItem(new WizardsHat(0x497));
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.HeadlessSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "The crypts welcome you, slayer. Choose your reward.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Return only when 500 HeadlessOnes lie slain at your hand.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(ThaddeusQuest)
        };

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

    public class SBThaddeus : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Adjust prices and types accordingly
                Add(new AnimalBuyInfo(1, typeof(HeadlessOne), 5000, 10, 17, 0));
                Add(new GenericBuyInfo(typeof(GargoyleLamp), 5000, 20, 0xA29, 0));
                Add(new GenericBuyInfo(typeof(AnimalTopiary), 5000, 20, 0x45AD, 0));
                Add(new GenericBuyInfo(typeof(TinCowbell), 5000, 20, 0x1457, 0));
                Add(new GenericBuyInfo(typeof(WoodAlchohol), 5000, 20, 0x99F, 0));
                Add(new GenericBuyInfo(typeof(ChocolateFountain), 5000, 20, 0x2A1A, 0));
                Add(new GenericBuyInfo(typeof(PowerGem), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(AtomicRegulator), 5000, 20, 0x2A33, 0));
                Add(new GenericBuyInfo(typeof(JesterSkull), 5000, 20, 0x1AE8, 0));
                Add(new GenericBuyInfo(typeof(GamerGirlFeet), 5000, 20, 0x1D11, 0));
                Add(new GenericBuyInfo(typeof(HildebrandtTapestry), 5000, 20, 0xF91, 0));
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
