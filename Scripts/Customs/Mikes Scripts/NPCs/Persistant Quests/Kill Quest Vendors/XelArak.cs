using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DaemonologistQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Infernal Culling";

        public override object Description =>
            "Ah, greetings. I am Xel’Arak, once a scholar of the arcane, now a hunter of Daemons.\n\n" +
            "These infernal beasts pour from the rifts unchecked. Their presence disrupts the balance of magic and corrupts the land.\n\n" +
            "Slay five hundred of them. Show me you can cleanse the world of their filth.\n\n" +
            "Succeed, and I shall offer you a reward most rare—companions and relics drawn from the very essence of the Abyss.";

        public override object Refuse => "Another coward who fears the fire. Go then, and leave the world to burn.";

        public override object Uncomplete => "The stench of Daemon blood is not yet thick upon you. Keep hunting.";

        public override object Complete =>
            "Yes... I see it in your eyes. The fury, the fire. You have done it.\n\n" +
            "The Daemons recoil from your name alone.\n\n" +
            "As promised, my store is now open to you. Choose wisely, for the gifts of Hell are never freely given.";

        public DaemonologistQuest()
        {
            AddObjective(new SlayObjective(typeof(Daemon), "Daemons", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold Coins"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.KillDaemonQuest))
                profile.Talents[TalentID.KillDaemonQuest] = new Talent(TalentID.KillDaemonQuest);

            profile.Talents[TalentID.KillDaemonQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Daemons for Xel’Arak!");
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

    public class XelArak : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBXelArak());
        }

        [Constructable]
        public XelArak() : base("Xel’Arak", "Daemonologist")
        {
        }

        public XelArak(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;

            Hue = 0x83EA;
            HairItemID = 0x2048;
            HairHue = 0x497;
            FacialHairItemID = 0x204C;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 0x497 });
            AddItem(new Sandals { Hue = 0x497 });
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;
            if (pm != null)
            {
                var profile = pm.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.KillDaemonQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "Step forward, slayer of hellspawn. My offerings are yours to claim.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You have not yet earned the trust of the Abyss. Return after 500 Daemons lie dead.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(DaemonologistQuest)
        };

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

    public class SBXelArak : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Daemon), 5000, 10, 8401, 0));
                Add(new GenericBuyInfo("Ancient Ruby", typeof(AncientRuby), 5000, 10, 0xF10, 0));
                Add(new GenericBuyInfo("Tyr Rune", typeof(TyrRune), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo("Ahm Rune", typeof(AhmRune), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo("Mor Rune", typeof(MorRune), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo("Mef Rune", typeof(MefRune), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo("Ylm Rune", typeof(YlmRune), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo("Kot Rune", typeof(KotRune), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo("Jor Rune", typeof(JorRune), 5000, 10, 0x1F18, 0));
                Add(new GenericBuyInfo("Mythic Sapphire", typeof(MythicSapphire), 5000, 10, 0xF21, 0));
                Add(new GenericBuyInfo("Legendary Sapphire", typeof(LegendarySapphire), 5000, 10, 0xF21, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
