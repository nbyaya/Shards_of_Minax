using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class OrenShadowthornQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Oren Shadowthorn's Beastly Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer. I am Oren Shadowthorn, master of the monstrous. " +
                    "To prove your worth and gain access to my exclusive shop of beastly wonders, " +
                    "you must hunt down the very beasts I once sought to control. Complete these tasks:\n\n" +
                    "1. Slay an Abyssal Warden.\n" +
                    "2. Defeat a Blight Demon.\n" +
                    "3. Conquer a Cursed Harbinger.\n" +
                    "4. Overcome a Deadlord.\n" +
                    "5. Destroy a Frostbound Behemoth.\n" +
                    "6. Vanquish a Hellfire Juggernaut.\n" +
                    "7. Subdue an Infernal Incinerator.\n" +
                    "8. Eradicate a Storm Deamon.\n" +
                    "9. Defeat a Toxic Reaver.\n" +
                    "10. Exterminate a Void Stalker.\n\n" +
                    "Complete these tasks, and I will grant you access to my shop of monstrous delights!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the monstrous challenges."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. The beasts await!"; } }

        public override object Complete { get { return "Remarkable! You have proven your prowess against these monstrous creatures. My shop is now open to you!"; } }

        public OrenShadowthornQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AbyssalWarden), "Abyssal Wardens", 1));
            AddObjective(new SlayObjective(typeof(BlightDemon), "Blight Demons", 1));
            AddObjective(new SlayObjective(typeof(CursedHarbinger), "Cursed Harbingers", 1));
            AddObjective(new SlayObjective(typeof(Deadlord), "Deadlords", 1));
            AddObjective(new SlayObjective(typeof(FrostboundBehemoth), "Frostbound Behemoths", 1));
            AddObjective(new SlayObjective(typeof(HellfireJuggernaut), "Hellfire Juggernauts", 1));
            AddObjective(new SlayObjective(typeof(InfernalIncinerator), "Infernal Incinerators", 1));
            AddObjective(new SlayObjective(typeof(StormDaemon), "Storm Deamons", 1));
            AddObjective(new SlayObjective(typeof(ToxicReaver), "Toxic Reavers", 1));
            AddObjective(new SlayObjective(typeof(VoidStalker), "Void Stalkers", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(ShadowthornToken), 1, "Shadowthorn Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Oren Shadowthorn's challenge!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }


    public class OrenShadowthorn : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBOrenShadowthorn());
        }

        [Constructable]
        public OrenShadowthorn()
            : base("Oren Shadowthorn", "Master of the Monstrous Menagerie")
        {
        }

        public OrenShadowthorn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(ShadowthornToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my menagerie of monstrous beasts!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Shadowthorn Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(OrenShadowthornQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }


    public class SBOrenShadowthorn : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBOrenShadowthorn()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the monstrous-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AbyssalWarden), 1000, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(BlightDemon), 1200, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(CursedHarbinger), 900, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(Deadlord), 950, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostboundBehemoth), 1100, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(HellfireJuggernaut), 1300, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernalIncinerator), 1400, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(StormDaemon), 1250, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(ToxicReaver), 1350, 10, 9, 0));
                Add(new AnimalBuyInfo(1, typeof(VoidStalker), 1500, 10, 9, 0));
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
