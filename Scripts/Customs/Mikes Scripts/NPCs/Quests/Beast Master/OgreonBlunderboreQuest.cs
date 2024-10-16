using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class OgreonBlunderboreQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ogreon Blunderbore's Ogre Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Ogreon Blunderbore, and I need your help. " +
                    "I’m on a quest to find out what makes these ogres so troublesome. " +
                    "To gain access to my exclusive ogre shop, you must help me by slaying the following ogres:\n\n" +
                    "1. Bonecrusher Ogre\n" +
                    "2. Chromatic Ogre\n" +
                    "3. Flamebringer Ogre\n" +
                    "4. FleshEater Ogre\n" +
                    "5. Frost Ogre\n" +
                    "6. Gloom Ogre\n" +
                    "7. Ironclad Ogre\n" +
                    "8. Necrotic Ogre\n" +
                    "9. Shadow Ogre\n" +
                    "10. Storm Ogre\n" +
                    "11. Toxic Ogre\n\n" +
                    "Complete these tasks, and I'll reward you with access to my ogre-themed shop!";
            }
        }

        public override object Refuse { get { return "Alright, if you're not ready to handle these ogres, come back when you are."; } }

        public override object Uncomplete { get { return "You haven’t defeated all the ogres yet. Keep hunting!"; } }

        public override object Complete { get { return "Well done! You’ve proven yourself worthy. My ogre shop is now open to you!"; } }

        public OgreonBlunderboreQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BonecrusherOgre), "Bonecrusher Ogres", 1));
            AddObjective(new SlayObjective(typeof(ChromaticOgre), "Chromatic Ogres", 1));
            AddObjective(new SlayObjective(typeof(FlamebringerOgre), "Flamebringer Ogres", 1));
            AddObjective(new SlayObjective(typeof(FleshEaterOgre), "FleshEater Ogres", 1));
            AddObjective(new SlayObjective(typeof(FrostOgre), "Frost Ogres", 1));
            AddObjective(new SlayObjective(typeof(GloomOgre), "Gloom Ogres", 1));
            AddObjective(new SlayObjective(typeof(IroncladOgre), "Ironclad Ogres", 1));
            AddObjective(new SlayObjective(typeof(NecroticOgre), "Necrotic Ogres", 1));
            AddObjective(new SlayObjective(typeof(ShadowOgre), "Shadow Ogres", 1));
            AddObjective(new SlayObjective(typeof(StormOgre), "Storm Ogres", 1));
            AddObjective(new SlayObjective(typeof(ToxicOgre), "Toxic Ogres", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(OgreToken), 1, "Ogre Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed Ogreon Blunderbore's quest!");
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

    public class OgreonBlunderbore : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBOgreonBlunderbore());
        }

        [Constructable]
        public OgreonBlunderbore()
            : base("Ogreon Blunderbore", "Master of the Ogres")
        {
        }

        public OgreonBlunderbore(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(OgreToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my ogre-themed shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have an Ogre Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(OgreonBlunderboreQuest)
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
	
    public class SBOgreonBlunderbore : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBOgreonBlunderbore()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the ogre-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BonecrusherOgre), 1000, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(ChromaticOgre), 1200, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(FlamebringerOgre), 1100, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(FleshEaterOgre), 1300, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostOgre), 1150, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(GloomOgre), 1400, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(IroncladOgre), 1250, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(NecroticOgre), 1350, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowOgre), 1450, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(StormOgre), 1500, 10, 1, 0));
                Add(new AnimalBuyInfo(1, typeof(ToxicOgre), 1550, 10, 1, 0));
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
