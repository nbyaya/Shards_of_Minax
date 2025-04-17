using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class MarionHorseQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "The Hooves of Vengeance";

        public override object Description =>
            "The wild horses roam freely, and their growing numbers trample crops and spook travelers.\n\n" +
            "I am Marion, once a tamer, now a guardian of order.\n\n" +
            "Culling 500 of them may seem cruel, but balance must be restored.\n\n" +
            "When your hands are stained with dust and sweat from the hunt, return to me.";

        public override object Refuse => "Then go. Let the herds overrun us if you lack the will.";

        public override object Uncomplete => "The herd still runs. You haven’t culled enough horses.";

        public override object Complete =>
            "I see the resolve in your eyes—and the dust on your boots.\n\n" +
            "Balance returns to these lands, and now I offer you my wares:\n" +
            "Tamed steeds and rare relics. You've earned this, rider.";

        public MarionHorseQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Horse), "Horses", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.HorseSlayerQuest))
                profile.Talents[TalentID.HorseSlayerQuest] = new Talent(TalentID.HorseSlayerQuest);

            profile.Talents[TalentID.HorseSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have culled 500 Horses for Marion.");
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

    public class Marion : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMarion());
        }

        [Constructable]
        public Marion() : base("Marion", "the Stablemaster")
        {
        }

        public Marion(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2049;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Skirt(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.HorseSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "Welcome, rider. Browse the wares of balance.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Earn your way by culling 500 wild horses.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(MarionHorseQuest) };

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

    public class SBMarion : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBMarion() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Horse), 5000, 10, 204, 0));

                Add(new GenericBuyInfo(typeof(StarMap), 5000, 10, 0x0FBC, 0));
                Add(new GenericBuyInfo(typeof(SkullRing), 5000, 10, 0x1F09, 0));
                Add(new GenericBuyInfo(typeof(BrandingIron), 5000, 10, 0x13F6, 0));
                Add(new GenericBuyInfo(typeof(OldBones), 5000, 10, 0x1BFB, 0));
                Add(new GenericBuyInfo(typeof(MillStones), 5000, 10, 0x1920, 0));
                Add(new GenericBuyInfo(typeof(Steroids), 5000, 10, 0x1009, 0));
                Add(new GenericBuyInfo(typeof(HildebrandtBunting), 5000, 10, 0x1F03, 0));
                Add(new GenericBuyInfo(typeof(LexxVase), 5000, 10, 0x241A, 0));
                Add(new GenericBuyInfo(typeof(OrnateHarp), 5000, 10, 0xEB2, 0));
                Add(new GenericBuyInfo(typeof(FletchingTalisman), 5000, 10, 0x2F58, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
