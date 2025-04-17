using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ThavikQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Binding the Darkness";

        public override object Description =>
            "I am Thavik, Binder of Terrors. My soul was nearly consumed by an Abysmal Horror in the Pit of Shrieking Shadows.\n\n" +
            "I survived—but just barely. Now I seek vengeance. If you are brave—or foolish—enough to face them, slay *five hundred* Abysmal Horrors.\n\n" +
            "Do this, and I will reward you with the secrets I’ve gathered from their twisted essence.";

        public override object Refuse =>
            "Then may the darkness find you first.";

        public override object Uncomplete =>
            "No... I still sense their vile presence. You have not yet slain enough Abysmal Horrors.";

        public override object Complete =>
            "Yes... Yes! The Abysmal Horrors wail from beyond the veil. You have done it.\n\n" +
            "As promised, my shop is now open to you. May these items aid you in binding your own demons.";

        public ThavikQuest()
        {
            AddObjective(new SlayObjective(typeof(AbysmalHorror), "Abysmal Horrors", 500));
            AddReward(new BaseReward(typeof(Gold), 2500, "2,500 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.AbysmalHorrorQuest))
                profile.Talents[TalentID.AbysmalHorrorQuest] = new Talent(TalentID.AbysmalHorrorQuest);

            profile.Talents[TalentID.AbysmalHorrorQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Abysmal Horrors!");
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

    public class Thavik : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThavik());
        }

        [Constructable]
        public Thavik() : base("Thavik", "The Binder")
        {
        }

        public Thavik(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2049;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 1109 });
            AddItem(new Sandals { Hue = 1150 });
            AddItem(new GnarledStaff());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.AbysmalHorrorQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You may browse the tools of the binder.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "The horrors still roam. Return when 500 of them lie dead.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(ThavikQuest)
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

    public class SBThavik : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(AbysmalHorror), 5000, 10, 0x117, 0)); // Adjust body/item ID if needed

                Add(new GenericBuyInfo(typeof(MahJongTile), 5000, 10, 0x1F03, 0));
                Add(new GenericBuyInfo(typeof(AlchemyTalisman), 5000, 10, 0x2F58, 0));
                Add(new GenericBuyInfo(typeof(CartographyTable), 5000, 10, 0x0B4F, 0));
                Add(new GenericBuyInfo(typeof(StrappedBooks), 5000, 10, 0x1F4D, 0));
                Add(new GenericBuyInfo(typeof(CarpentryTalisman), 5000, 10, 0x2F58, 0));
                Add(new GenericBuyInfo(typeof(CharcuterieBoard), 5000, 10, 0x1608, 0));
                Add(new GenericBuyInfo(typeof(BottledPlague), 5000, 10, 0x1005, 0));
                Add(new GenericBuyInfo(typeof(NixieStatue), 5000, 10, 0x2826, 0));
                Add(new GenericBuyInfo(typeof(GrandmasSpecialRolls), 5000, 10, 0x160A, 0));
                Add(new GenericBuyInfo(typeof(WorkersRevolutionChest), 5000, 10, 0x1AE0, 0));
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
