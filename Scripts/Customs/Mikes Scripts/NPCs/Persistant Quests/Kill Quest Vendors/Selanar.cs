using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SelanarQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Wrath of the Kirin";

        public override object Description =>
            "Greetings, adventurer. I am Selanar, once a Warden of the Silver Canopy.\n\n" +
            "Long ago, the Kirin protected these lands. But something has changed—they have become violent, corrupted.\n\n" +
            "Slay *five hundred* of these fallen beasts, and I will reward your efforts with access to rare relics and creatures.\n\n" +
            "Bring balance to the wilds, and return when your blade has sung five hundred times.";

        public override object Refuse =>
            "Then the balance shall remain broken. Return when your resolve is clear.";

        public override object Uncomplete =>
            "You have not yet fulfilled your oath. The wilds still rage with corrupted Kirin.";

        public override object Complete =>
            "You have returned, and with you the scent of thunder and blood.\n\n" +
            "The Kirin plague has been culled—my gratitude is boundless.\n\n" +
            "As promised, my wares are now yours to purchase. May their power guide your next hunt.";

        public SelanarQuest()
        {
            AddObjective(new SlayObjective(typeof(Kirin), "Kirin", 500));

            // Optional reward - symbolic
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.KirinSlayerQuest))
                profile.Talents[TalentID.KirinSlayerQuest] = new Talent(TalentID.KirinSlayerQuest);

            profile.Talents[TalentID.KirinSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Kirin for Selanar!");
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

    public class Selanar : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSelanar());
        }

        [Constructable]
        public Selanar() : base("Selanar", "Warden of the Silver Canopy")
        {
        }

        public Selanar(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Elf;
            Hue = 0x83EA;

            HairItemID = 0x203C;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new ElvenBoots());
            AddItem(new Kilt(1150));
            AddItem(new ElvenShirt(1175));
            AddItem(new Cloak(1175));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.KirinSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "You have proven yourself. Browse my wares freely.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Balance must be restored. Return after slaying 500 Kirin.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(SelanarQuest) };

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

    public class SBSelanar : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Kirin), 5000, 5, 0x25A0, 0));
                Add(new GenericBuyInfo(typeof(CartographyDesk), 5000, 5, 0x1EB8, 0));
                Add(new GenericBuyInfo(typeof(BirdStatue), 5000, 5, 0x2101, 0));
                Add(new GenericBuyInfo(typeof(Hamburger), 5000, 10, 0x97F, 0));
                Add(new GenericBuyInfo(typeof(SerpantCrest), 5000, 3, 0x1B76, 0));
                Add(new GenericBuyInfo(typeof(AstroLabe), 5000, 3, 0x1F2F, 0));
                Add(new GenericBuyInfo(typeof(OrganicHeart), 5000, 2, 0x1C0F, 0));
                Add(new GenericBuyInfo(typeof(Birdhouse), 5000, 5, 0x1BD1, 0));
                Add(new GenericBuyInfo(typeof(SabertoothSkull), 5000, 3, 0x1B0B, 0));
                Add(new GenericBuyInfo(typeof(ZebulinVase), 5000, 3, 0x241C, 0));
                Add(new GenericBuyInfo(typeof(PineResin), 5000, 10, 0x97B, 0));
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
