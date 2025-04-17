using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ThargosQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Wrath of Thargos";

        public override object Description => 
            "I am Thargos, once a warrior of the Storm Peaks.\n\n" +
            "Cyclops overrun the old caverns where my people lived. I seek vengeance.\n\n" +
            "Prove your might. Slay 500 Cyclops and I shall reward you with access to my hidden vault—" +
            "where great beasts and rare stones await.\n\n" +
            "Return only when your hands are bloodied and your will, iron.";

        public override object Refuse =>
            "Vengeance waits for no coward. Return when you find your spine.";

        public override object Uncomplete =>
            "You have not yet felled 500 Cyclops. Return when your hunt is complete.";

        public override object Complete =>
            "I see fury in your eyes and blood on your blade.\n" +
            "The Cyclops are broken, and my honor is restored through your steel.\n\n" +
            "My vault is open to you now. Use it wisely.";

        public ThargosQuest()
        {
            AddObjective(new SlayObjective(typeof(Cyclops), "Cyclops", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.CyclopsSlayerQuest))
                profile.Talents[TalentID.CyclopsSlayerQuest] = new Talent(TalentID.CyclopsSlayerQuest);

            profile.Talents[TalentID.CyclopsSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Cyclops for Thargos!");
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

    public class Thargos : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThargos());
        }

        [Constructable]
        public Thargos() : base("Thargos", "The Cyclops Slayer")
        {
        }

        public Thargos(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 100);
            Female = false;
            Race = Race.Human;

            Hue = 33770; // Some greyish tone
            HairItemID = 0x203C; // Short Hair
            HairHue = 1150;
            FacialHairItemID = 0x204B;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new BearMask());
            AddItem(new PlateChest());
            AddItem(new PlateArms());
            AddItem(new PlateGloves());
            AddItem(new PlateLegs());
            AddItem(new Boots(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.CyclopsSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "You have earned the right. Browse my vault.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Return when you've crushed 500 Cyclops beneath your boot.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(ThargosQuest) };

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

    public class SBThargos : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBThargos()
        {
        }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Tamed Cyclops
                Add(new AnimalBuyInfo(1, typeof(Cyclops), 5000, 5, 0x20F8, 0));

                // Radiant Crystals
                Add(new GenericBuyInfo("Radiant Xen Crystal", typeof(RadiantXenCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Radiant Pol Crystal", typeof(RadiantPolCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Radiant Wol Crystal", typeof(RadiantWolCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Radiant Bal Crystal", typeof(RadiantBalCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Radiant Tal Crystal", typeof(RadiantTalCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Radiant Jal Crystal", typeof(RadiantJalCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Radiant Ral Crystal", typeof(RadiantRalCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Radiant Kal Crystal", typeof(RadiantKalCrystal), 5000, 10, 0x1F19, 0));

                // Gems
                Add(new GenericBuyInfo("Mythic Ruby", typeof(MythicRuby), 5000, 10, 0xF15, 0));
                Add(new GenericBuyInfo("Legendary Ruby", typeof(LegendaryRuby), 5000, 10, 0xF15, 0));
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
