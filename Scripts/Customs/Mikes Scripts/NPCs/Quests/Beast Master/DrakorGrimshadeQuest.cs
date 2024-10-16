using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DrakorGrimshadeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Drakor Grimshade's Grim Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Drakor Grimshade, apprentice to the dark arts. " +
                    "To prove your prowess and gain entry to my unique shop, you must vanquish these notorious fiends. " +
                    "Complete these tasks:\n\n" +
                    "1. Slay Acererak.\n" +
                    "2. Defeat Azalin Rex.\n" +
                    "3. Conquer Kas The Bloody Handed.\n" +
                    "4. Overcome Kel Thuzad.\n" +
                    "5. Destroy Larloch The Shadow King.\n" +
                    "6. Vanquish Nagash.\n" +
                    "7. Subdue Raistlin Majere.\n" +
                    "8. Eradicate Soth The Death Knight.\n" +
                    "9. Defeat Szass Tam.\n" +
                    "10. Exterminate Vecna.\n\n" +
                    "Complete these tasks, and I will grant you access to my shop of dark beasts!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face these dark entities."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting these dark entities!"; } }

        public override object Complete { get { return "Excellent work! You have proven yourself worthy of my dark collection. My shop is now open to you!"; } }

        public DrakorGrimshadeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Acererak), "Acereraks", 1));
            AddObjective(new SlayObjective(typeof(AzalinRex), "Azalin Rexes", 1));
            AddObjective(new SlayObjective(typeof(KasTheBloodyHanded), "Kas The Bloody Handeds", 1));
            AddObjective(new SlayObjective(typeof(KelThuzad), "Kel Thuzads", 1));
            AddObjective(new SlayObjective(typeof(LarlochTheShadowKing), "Larloch The Shadow Kings", 1));
            AddObjective(new SlayObjective(typeof(Nagash), "Nagashes", 1));
            AddObjective(new SlayObjective(typeof(RaistlinMajere), "Raistlin Majeres", 1));
            AddObjective(new SlayObjective(typeof(SothTheDeathKnight), "Soth The Death Knights", 1));
            AddObjective(new SlayObjective(typeof(SzassTam), "Szass Tams", 1));
            AddObjective(new SlayObjective(typeof(Vecna), "Vecnas", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(GrimshadeToken), 1, "Grimshade Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Drakor Grimshade's challenge!");
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


    public class DrakorGrimshade : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBDrakorGrimshade());
        }

        [Constructable]
        public DrakorGrimshade()
            : base("Drakor Grimshade", "The Necromancer's Apprentice")
        {
        }

        public DrakorGrimshade(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(GrimshadeToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my dark beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Grimshade Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DrakorGrimshadeQuest)
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


    public class SBDrakorGrimshade : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBDrakorGrimshade()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the dark-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(Acererak), 1000, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(AzalinRex), 1200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(KasTheBloodyHanded), 1100, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(KelThuzad), 1300, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(LarlochTheShadowKing), 1400, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(Nagash), 1250, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(RaistlinMajere), 1500, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(SothTheDeathKnight), 1350, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(SzassTam), 1600, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(Vecna), 1450, 10, 13, 0));
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
