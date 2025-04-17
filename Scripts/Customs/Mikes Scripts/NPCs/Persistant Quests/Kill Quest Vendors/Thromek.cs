using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class JukaMageQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Juka Mages";

        public override object Description => 
            "I am Thromek the Mystic, once a battlemage of Moonglow.\n\n" +
            "The Juka Mages have corrupted the fabric of magical ley lines with their twisted incantations. " +
            "Their influence must be broken.\n\n" +
            "Slay five hundred of these sorcerers, and I shall offer you rare and powerful curiosities in return.";

        public override object Refuse => "Then begone. I have no patience for cowards.";
        public override object Uncomplete => "The ley lines still tremble with their corruption. You have not finished your task.";
        public override object Complete => 
            "You have done well, and the arcane pulses quieter now. As promised, my artifacts are now yours to browse. Choose with care.";

        public JukaMageQuest()
        {
            AddObjective(new SlayObjective(typeof(JukaMage), "Juka Mages", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.JukaMageSlayerQuest))
                profile.Talents[TalentID.JukaMageSlayerQuest] = new Talent(TalentID.JukaMageSlayerQuest);

            profile.Talents[TalentID.JukaMageSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Juka Mages for Thromek!");
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

    public class Thromek : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThromek());
        }

        [Constructable]
        public Thromek() : base("Thromek", "the Mystic")
        {
        }

        public Thromek(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 100);
            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2044, 0x2047, 0x203B);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x204B;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x487));
            AddItem(new Sandals());
            AddItem(new WizardsHat(0x487));
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.JukaMageSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned my respect. View my wares.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You are not yet worthy. Slay 500 Juka Mages, and return to me.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(JukaMageQuest)
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

    public class SBThromek : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Tamed Juka Mage (if custom mount/creature, replace with correct type)
                Add(new AnimalBuyInfo(1, typeof(JukaMage), 5000, 10, 0x2103, 0)); // Use correct itemID if not mount

                Add(new GenericBuyInfo("Meat Hooks", typeof(MeatHooks), 5000, 10, 0x13B5, 0));
                Add(new GenericBuyInfo("Gamer Jelly", typeof(GamerJelly), 5000, 10, 0x103D, 0));
                Add(new GenericBuyInfo("Shard Crest", typeof(ShardCrest), 5000, 10, 0x1F13, 0));
                Add(new GenericBuyInfo("Evil Candle", typeof(EvilCandle), 5000, 10, 0x1854, 0));
                Add(new GenericBuyInfo("Hot Flaming Scarecrow", typeof(HotFlamingScarecrow), 5000, 10, 0x1F95, 0));
                Add(new GenericBuyInfo("Amateur Telescope", typeof(AmatureTelescope), 5000, 10, 0x223B, 0));
                Add(new GenericBuyInfo("Anniversary Painting", typeof(AnniversaryPainting), 5000, 10, 0x241E, 0));
                Add(new GenericBuyInfo("Magic Book Stand", typeof(MagicBookStand), 5000, 10, 0x1A9D, 0));
                Add(new GenericBuyInfo("Smuggler's Crate", typeof(SmugglersCrate), 5000, 10, 0x1DB1, 0));
                Add(new GenericBuyInfo("Holiday Pillow", typeof(HolidayPillow), 5000, 10, 0x13A4, 0));
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
