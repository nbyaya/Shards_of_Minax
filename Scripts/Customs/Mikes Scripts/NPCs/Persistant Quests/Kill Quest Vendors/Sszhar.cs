using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SszharsHunt : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Lizardman's Reckoning";

        public override object Description => 
            "Sss... you come bearing blade, but can you bear the burden of bloodshed?\n\n" +
            "Lizardmen swarm our swamps, twisted kin of mine... traitors to our kind.\n\n" +
            "Bring me the corpses of 500 Lizardman. Not in flesh, but in deed.\n\n" +
            "Then... and only then... shall I reward you. I will share with you my tamed beasts and relics of forgotten times.";

        public override object Refuse => 
            "Weaknessssss. You are not ready to hunt the betrayers.";

        public override object Uncomplete => 
            "Still their kind slither, still they draw breath. Return to the hunt.";

        public override object Complete => 
            "Yesss... the air smells of ash and blood. You have done well.\n\n" +
            "The betrayers are broken. My trust is earned.\n\n" +
            "I now open my collection to you. Choose wisely, hunter.";

        public SszharsHunt() : base()
        {
            AddObjective(new SlayObjective(typeof(Lizardman), "Lizardmen", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.LizardmanSlayerQuest))
                profile.Talents[TalentID.LizardmanSlayerQuest] = new Talent(TalentID.LizardmanSlayerQuest);
            
            profile.Talents[TalentID.LizardmanSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Lizardmen for Sszhar!");
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

    public class Sszhar : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSszhar());
        }

        [Constructable]
        public Sszhar() : base("Sszhar", "The Exiled Beastmaster")
        {
        }

        public Sszhar(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Body = 0x33; // Lizardman body
            Hue = 0x3D; // Optional lizardy hue
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new BoneChest());
            AddItem(new BoneLegs());
            AddItem(new BearMask());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.LizardmanSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned this, hunter. My treasures await.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You have not yet spilled enough betrayer blood.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(SszharsHunt) };

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

    public class SBSszhar : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        private class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Lizardman), 5000, 10, 17, 0));

                Add(new GenericBuyInfo("Nexus Shard", typeof(NexusShard), 5000, 10, 0x2AAA, 0));
                Add(new GenericBuyInfo("Rare Sausage", typeof(RareSausage), 5000, 10, 0x9C9, 0));
                Add(new GenericBuyInfo("Watermelon Fruit", typeof(WatermelonFruit), 5000, 10, 0xC5E, 0));
                Add(new GenericBuyInfo("Tool Box", typeof(ToolBox), 5000, 10, 0x1EB0, 0));
                Add(new GenericBuyInfo("Horrodrick Cube", typeof(HorrodrickCube), 5000, 10, 0x1F13, 0));
                Add(new GenericBuyInfo("Trophie Award", typeof(TrophieAward), 5000, 10, 0x9B0, 0));
                Add(new GenericBuyInfo("Cluttered Desk", typeof(ClutteredDesk), 5000, 10, 0x2810, 0));
                Add(new GenericBuyInfo("Tinkering Talisman", typeof(TinkeringTalisman), 5000, 10, 0x2F58, 0));
                Add(new GenericBuyInfo("Death Blow Item", typeof(DeathBlowItem), 5000, 10, 0x1B76, 0));
                Add(new GenericBuyInfo("Dead Body", typeof(DeadBody), 5000, 10, 0x2006, 0));
            }
        }

        private class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
