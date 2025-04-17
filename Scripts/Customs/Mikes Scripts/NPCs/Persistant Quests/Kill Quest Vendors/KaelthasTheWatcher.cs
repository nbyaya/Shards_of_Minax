using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class KaelthasGazerQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Sight Unending";

        public override object Description =>
            "The Gazers... they are not what they seem. Their minds reach into the void and whisper secrets we cannot grasp.\n\n" +
            "I am Kael'thas, a watcher, a seeker. I must understand them... and to do that, I need *specimens*. " +
            "Bring me the corpses of *five hundred* Gazers. In return, I shall open my vault of... curiosities.\n\n" +
            "Will you do this for me, hunter of eyes?";

        public override object Refuse => "Ah, fear not the unknown... It fears you.";

        public override object Uncomplete => "You have not yet harvested enough eyes. The gaze persists... unbroken.";

        public override object Complete =>
            "Five hundred Gazers have fallen beneath your blade... and I feel the whispers grow silent.\n\n" +
            "As promised, my shop is open. Within, relics beyond reason await the brave. Choose... but choose wisely.";

        public KaelthasGazerQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Gazer), "Gazers", 500));

            // Optional reward on completion
            AddReward(new BaseReward(typeof(Gold), 2500, "2500 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GazerSlayerQuest))
                profile.Talents[TalentID.GazerSlayerQuest] = new Talent(TalentID.GazerSlayerQuest);

            profile.Talents[TalentID.GazerSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Gazers for Kael'thas!");
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

    public class KaelthasTheWatcher : MondainQuester
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override bool IsActiveVendor => true;

        public override Type[] Quests => new Type[] { typeof(KaelthasGazerQuest) };

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKaelthas());
        }

        [Constructable]
        public KaelthasTheWatcher() : base("Kael'thas", "Watcher of the Gaze")
        {
        }

        public KaelthasTheWatcher(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 100);
            Race = Race.Human;
            Female = false;

            Hue = 0x83EA; // Pale mage-like
            HairItemID = 0x203C;
            HairHue = 1150;
            FacialHairItemID = 0x2041;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x497)); // Deep purple
            AddItem(new Sandals());
            AddItem(new HoodedShroudOfShadows());
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GazerSlayerQuest, out var t) && t.Points > 0)
                {
                    SayTo(from, "The vault is open, hunter. Choose your relic.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You are not yet ready. I require five hundred Gazers.");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBKaelthas : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Gazer), 5000, 5, 17, 0));

                Add(new GenericBuyInfo(typeof(AbbasidsTreasureChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AbyssalPlaneChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AlehouseChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AlienArtifactChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AlienArtifaxChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AlliedForcesTreasureChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AnarchistsCache), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AncientRelicChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AngelBlessingChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(AnglersBounty), 5000, 5, 0xE43, 0));
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
