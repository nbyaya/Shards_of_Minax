using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class BongoQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Gorilla Justice";

        public override object Description => 
            "Greetings, stranger. I am Bongo, Guardian of the Wilds.\n\n" +
            "An unnatural surge of Gorillas has upset the balance of our sacred jungle. " +
            "These beasts, once noble, now ravage the land with fury born of corruption.\n\n" +
            "Prove your strength and your honor—bring down *five hundred* Gorillas, and I shall trust you with the care of one of their own. " +
            "In return, I will open my cache of mystical treasures to you.\n\n" +
            "Let the rhythm of the hunt guide your heart.";

        public override object Refuse => "Very well. Perhaps the jungle's rhythm has not yet stirred within you.";

        public override object Uncomplete => "The jungle still trembles beneath Gorilla feet. Keep hunting.";

        public override object Complete =>
            "You have returned, and the jungle sings a quieter song.\n" +
            "Five hundred Gorillas have fallen, and balance has been restored—for now.\n\n" +
            "You have earned my trust. You may now trade with me for rare artifacts, and even take in a tamed Gorilla of your own.\n\n" +
            "Go with honor.";

        public BongoQuest()
        {
            AddObjective(new SlayObjective(typeof(Gorilla), "Gorillas", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GorillaSlayerQuest))
                profile.Talents[TalentID.GorillaSlayerQuest] = new Talent(TalentID.GorillaSlayerQuest);

            profile.Talents[TalentID.GorillaSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Gorillas for Bongo!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Bongo : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBongo());
        }

        [Constructable]
        public Bongo() : base("Bongo", "Keeper of the Jungle")
        {
        }

        public Bongo(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2047, 0x2048, 0x2049);
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals());
            AddItem(new LeafTonlet(Utility.RandomNeutralHue()));
            AddItem(new LeafChest(Utility.RandomNeutralHue()));
            AddItem(new BearMask());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GorillaSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "The jungle welcomes you. Browse what I have to offer.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You are not yet worthy. Return after slaying 500 Gorillas.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(BongoQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBBongo : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Gorilla), 5000, 10, 0x271D, 0));
                Add(new GenericBuyInfo(typeof(EnchantedForestChest), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(EtherealPlaneChest), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(EuropeanRelicsChest), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(FairyDustChest), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(FirstNationsHeritageChest), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(FlowerPowerChest), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(FocusBonusChest), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(ForbiddenAlchemistsCache), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(FrontierExplorersStash), 5000, 10, 0x9AB, 0));
                Add(new GenericBuyInfo(typeof(FunkyFashionChest), 5000, 10, 0x9AB, 0));
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
