using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BrughorsOgreQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Brughor’s Wrath: The Ogre Reckoning";

        public override object Description => 
            "I am Brughor... once slave-master of the Ogre Clans.\n\n" +
            "Now, cast out and mocked. My kin grow soft. You, outsider—bring ruin to 500 of them.\n" +
            "Let their blood spill for my glory, and I will share with you treasures of the old Ogre forges.\n\n" +
            "Prove yourself, and I shall trade with you as kin.";

        public override object Refuse => "Cowardice smells like goblin sweat. Come back when you stink less.";

        public override object Uncomplete => "Not enough ogres dead. Return when 500 have fallen.";

        public override object Complete =>
            "Hah! I can hear their death cries echoing still.\n" +
            "You fight like a beast… I like you.\n\n" +
            "My goods are yours—for coin, of course. Let the others tremble.";

        public BrughorsOgreQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Ogre), "Ogres", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.OgreSlayerQuest))
                profile.Talents[TalentID.OgreSlayerQuest] = new Talent(TalentID.OgreSlayerQuest);

            profile.Talents[TalentID.OgreSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Ogres for Brughor!");
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
            reader.ReadInt();
        }
    }

    public class BrughorTheTamer : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBrughor());
        }

        [Constructable]
        public BrughorTheTamer() : base("Brughor the Tamer", "Ogre Outcast")
        {
        }

        public BrughorTheTamer(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(150, 75, 100);
            Female = false;
            Race = Race.Human;
            Hue = 33770; // Grayish-blue skin
            Body = 0x190;

            HairItemID = 0x2048;
            HairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new BearMask());
            AddItem(new RingmailChest());
            AddItem(new RingmailLegs());
            AddItem(new Boots(0x455)); // Ogreish brown
            AddItem(new Cloak(0x497));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.OgreSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "You’ve earned the right. Choose your trinkets, slayer.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Hmph. Kill 500 ogres first, then we talk business.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(BrughorsOgreQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SBBrughor : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Ogre), 5000, 10, 845, 0));

                Add(new GenericBuyInfo(typeof(AncientRunes), 5000, 20, 0x1F1C, 0));
                Add(new GenericBuyInfo(typeof(WeddingChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(ExoticShipInABottle), 5000, 5, 0x14F8, 0));
                Add(new GenericBuyInfo(typeof(FancySewingMachine), 5000, 5, 0x1010, 0));
                Add(new GenericBuyInfo(typeof(CowPoo), 5000, 50, 0x09B7, 0));
                Add(new GenericBuyInfo(typeof(FeedingTrough), 5000, 10, 0x097F, 0));
                Add(new GenericBuyInfo(typeof(SkullBottle), 5000, 10, 0x0F0D, 0));
                Add(new GenericBuyInfo(typeof(AncientDrum), 5000, 5, 0x0E9C, 0));
                Add(new GenericBuyInfo(typeof(BrassBell), 5000, 5, 0x14ED, 0));
                Add(new GenericBuyInfo(typeof(PlagueBanner), 5000, 3, 0x2A58, 0));
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
