using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MalrikLichQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "Purge of the Undead";
        public override object Description =>
            "Ah, the stench of undeath thickens with each passing night...\n\n" +
            "I am Malrik, once a necromancer—now their bane. The Liches have gone too far, corrupting life, perverting magic. " +
            "Destroy 500 of them, and I shall reward you not with coin alone, but with rare items salvaged from their very bones.";

        public override object Refuse =>
            "Hmph. If you lack the will to face death itself, then begone.";
        public override object Uncomplete =>
            "500 Liches must fall, and your tally is not yet enough. Keep going.";
        public override object Complete =>
            "You... reek of old crypts and scorched bone. Glorious.\n\n" +
            "You have cleansed these lands, and as promised, my cache of forbidden relics is now yours to access.";

        public MalrikLichQuest()
        {
            AddObjective(new SlayObjective(typeof(Lich), "Liches", 500));
            AddReward(new BaseReward(typeof(Gold), 2000, "2000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.LichSlayerQuest))
                profile.Talents[TalentID.LichSlayerQuest] = new Talent(TalentID.LichSlayerQuest);

            profile.Talents[TalentID.LichSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have vanquished 500 Liches for Malrik!");
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

    public class Malrik : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMalrik());
        }

        [Constructable]
        public Malrik() : base("Malrik", "Hunter of the Damned")
        {
        }

        public Malrik(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;

            Hue = 33770;
            HairItemID = 0x2047;
            HairHue = 1150;
            FacialHairItemID = 0x204B;
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 1109 });
            AddItem(new Sandals { Hue = 0 });
            AddItem(new Cloak { Hue = 1150 });
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.LichSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "My wares are yours, Lichbane.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Return when you've slain 500 Liches. Then, we speak of trade.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(MalrikLichQuest) };

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

    public class SBMalrik : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Lich), 5000, 5, 17, 0));
                Add(new GenericBuyInfo(typeof(RareMinerals), 5000, 10, 0x1BEF, 0));
                Add(new GenericBuyInfo(typeof(QuestWineRack), 5000, 5, 0x2DDA, 0));
                Add(new GenericBuyInfo(typeof(FancyCrystalSkull), 5000, 5, 0x1AE0, 0));
                Add(new GenericBuyInfo(typeof(StoneHead), 5000, 5, 0x2202, 0));
                Add(new GenericBuyInfo(typeof(FluxCompound), 5000, 5, 0x1BF2, 0));
                Add(new GenericBuyInfo(typeof(ArtisanHolidayTree), 5000, 3, 0x2376, 0));
                Add(new GenericBuyInfo(typeof(TinyWizard), 5000, 5, 0x12B8, 0));
                Add(new GenericBuyInfo(typeof(MiniKeg), 5000, 5, 0x1940, 0));
                Add(new GenericBuyInfo(typeof(DemonPlatter), 5000, 5, 0x9E9, 0));
                Add(new GenericBuyInfo(typeof(Hotdogs), 5000, 20, 0x97B, 0));
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
