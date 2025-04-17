using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GorlokQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Ogre Lord Purge";

        public override object Description => 
            "Hah! You look like you can handle yourself in a brawl!\n\n" +
            "I’m Gorlok, beast tamer of the north. These cursed Ogre Lords have stomped across my lands for too long, scaring off my pets and ruining my brew.\n\n" +
            "Bring me the heads of **five hundred** Ogre Lords, and I’ll share my prized possessions with you. Trophies, pets, relics... things of power and delight.\n\n" +
            "Return only when your hands are stained with their filth.";

        public override object Refuse => "Tch! Not everyone has the stomach for real work. Off with ya!";

        public override object Uncomplete => "Still too many of those brutes out there. Keep smashing their skulls in, friend.";

        public override object Complete => 
            "By the gods... You did it?! HaHA! You’ve earned your reward and my trust. " +
            "From now on, my collection is open to you. Pick something out—every piece has a story.";

        public GorlokQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(OgreLord), "Ogre Lords", 500));
            AddReward(new BaseReward(typeof(Gold), 2000, "2000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.OgreLordSlayerQuest))
                profile.Talents[TalentID.OgreLordSlayerQuest] = new Talent(TalentID.OgreLordSlayerQuest);

            profile.Talents[TalentID.OgreLordSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Ogre Lords for Gorlok!");
            Owner.PlaySound(0x20F); // Completion sound
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

    public class Gorlok : MondainQuester
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        public override bool IsActiveVendor => true;

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGorlok());
        }

        [Constructable]
        public Gorlok() : base("Gorlok", "Beast Tamer of the North")
        {
        }

        public Gorlok(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(150, 100, 100);

            Female = false;
            Race = Race.Human;

            Hue = 33770;
            HairItemID = 0x2047;
            HairHue = 1109;
            FacialHairItemID = 0x204B;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new BearMask());
            AddItem(new BoneArms());
            AddItem(new BoneChest());
            AddItem(new BoneGloves());
            AddItem(new BoneLegs());
            AddItem(new Boots());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();

                if (profile.Talents.TryGetValue(TalentID.OgreLordSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(player, "You’ve earned your right, hero. Take a look.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(player, "Slay 500 Ogre Lords, then we’ll talk shop.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(GorlokQuest) };

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

    public class SBGorlok : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(OgreLord), 5000, 10, 0x20, 0));
                Add(new GenericBuyInfo(typeof(BeeHive), 5000, 10, 0x9EC, 0));
                Add(new GenericBuyInfo(typeof(FestiveChampagne), 5000, 10, 0x9F1, 0));
                Add(new GenericBuyInfo(typeof(DecorativeFishTank), 5000, 10, 0x1E2A, 0));
                Add(new GenericBuyInfo(typeof(SkullsStack), 5000, 10, 0x1B17, 0));
                Add(new GenericBuyInfo(typeof(VenusFlytrap), 5000, 10, 0x18E5, 0));
                Add(new GenericBuyInfo(typeof(HorribleMask), 5000, 10, 0x154B, 0));
                Add(new GenericBuyInfo(typeof(Meteorite), 5000, 10, 0x1F1C, 0));
                Add(new GenericBuyInfo(typeof(ExcitingTome), 5000, 10, 0x1F4D, 0));
                Add(new GenericBuyInfo(typeof(WindRelic), 5000, 10, 0x2B6F, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
