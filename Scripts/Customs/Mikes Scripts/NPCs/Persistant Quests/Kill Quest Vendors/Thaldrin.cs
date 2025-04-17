using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ThaldrinCuSidheQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "Verdant Wrath: The Cu Sidhe Hunt";

        public override object Description => 
            "I am Thaldrin the Verdant, protector of harmony between nature and mortals.\n\n" +
            "The Cu Sidhe, once guardians of the Emerald Dream, have turned feral and rampant, threatening balance across the realm.\n\n" +
            "Hunt down *five hundred* of these majestic but dangerous beasts, and I shall reward you with access to my finest creatures and magical crystals.\n\n" +
            "Let your resolve guide your blade.";

        public override object Refuse => "Then go in peace. But know that the wilds remember cowardice.";

        public override object Uncomplete => "You are not yet finished. The Cu Sidhe still prowl unchecked.";

        public override object Complete =>
            "The storm of your hunt has calmed, and the Cu Sidhe howl no more.\n\n" +
            "You have earned more than thanks—you now have access to rare companions and radiant crystals of ancient power.\n\n" +
            "Speak to me should you wish to trade.";

        public ThaldrinCuSidheQuest()
        {
            AddObjective(new SlayObjective(typeof(CuSidhe), "Cu Sidhe", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.CuSidheHunterQuest))
                profile.Talents[TalentID.CuSidheHunterQuest] = new Talent(TalentID.CuSidheHunterQuest);

            profile.Talents[TalentID.CuSidheHunterQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Cu Sidhe for Thaldrin!");
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
            int version = reader.ReadInt();
        }
    }

    public class Thaldrin : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThaldrin());
        }

        [Constructable]
        public Thaldrin() : base("Thaldrin", "the Verdant")
        {
        }

        public Thaldrin(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Elf;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x59B));
            AddItem(new Sandals());
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.CuSidheHunterQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You are welcome here, beastmaster. Browse my goods.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Return only when you’ve ended the threat of the Cu Sidhe.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(ThaldrinCuSidheQuest)
        };

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

    public class SBThaldrin : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(CuSidhe), 10000, 10, 0x00D6, 0)); // Cu Sidhe for 10k gold

                // Each crystal for 5000gp
                Add(new GenericBuyInfo(typeof(AncientDiamond), 5000, 20, 0xF26, 0));
                Add(new GenericBuyInfo(typeof(MythicEmerald), 5000, 20, 0xF10, 0));
                Add(new GenericBuyInfo(typeof(LegendaryEmerald), 5000, 20, 0xF10, 0));
                Add(new GenericBuyInfo(typeof(AncientEmerald), 5000, 20, 0xF10, 0));
                Add(new GenericBuyInfo(typeof(RadiantRhoCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(RadiantRysCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(RadiantWyrCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(RadiantFreCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(RadiantTorCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(RadiantVelCrystal), 5000, 20, 0x1F19, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
