using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class NefertinaQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Nefertina the Unfathomable's Ancient Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Nefertina the Unfathomable, keeper of ancient secrets. " +
                    "To prove your worth and gain access to my unique shop, you must embark on a quest " +
                    "to defeat the most enigmatic creatures of our ancient past. Complete these tasks:\n\n" +
                    "1. Slay Akhenaten The Heretic.\n" +
                    "2. Defeat Cleopatra The Enigmatic.\n" +
                    "3. Conquer Hatshepsut The Queen.\n" +
                    "4. Overcome Khufu The Great Builder.\n" +
                    "5. Destroy Mentuhotep The Wise.\n" +
                    "6. Vanquish Nefertiti.\n" +
                    "7. Subdue Ramses The Immortal.\n" +
                    "8. Eradicate Seti The Avenger.\n" +
                    "9. Defeat Thutmose The Conqueror.\n" +
                    "10. Exterminate Tutankhamun The Cursed.\n\n" +
                    "Complete these tasks, and I will grant you access to my exclusive beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to unlock the mysteries of the past."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Continue your journey through ancient enigmas!"; } }

        public override object Complete { get { return "Excellent! You have conquered the ancient beasts. My shop is now open to you!"; } }

        public NefertinaQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AkhenatenTheHeretic), "Akhenaten The Heretics", 1));
            AddObjective(new SlayObjective(typeof(CleopatraTheEnigmatic), "Cleopatra The Enigmatics", 1));
            AddObjective(new SlayObjective(typeof(HatshepsutTheQueen), "Hatshepsut The Queens", 1));
            AddObjective(new SlayObjective(typeof(KhufuTheGreatBuilder), "Khufu The Great Builders", 1));
            AddObjective(new SlayObjective(typeof(MentuhotepTheWise), "Mentuhotep The Wises", 1));
            AddObjective(new SlayObjective(typeof(Nefertiti), "Nefertitis", 1));
            AddObjective(new SlayObjective(typeof(RamsesTheImmortal), "Ramses The Immortals", 1));
            AddObjective(new SlayObjective(typeof(SetiTheAvenger), "Seti The Avengers", 1));
            AddObjective(new SlayObjective(typeof(ThutmoseTheConqueror), "Thutmose The Conquerors", 1));
            AddObjective(new SlayObjective(typeof(TutankhamunTheCursed), "Tutankhamun The Cursed", 1));

            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(NefertinaToken), 1, "Nefertina Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Nefertina's challenge!");
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


    public class Nefertina : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNefertina());
        }

        [Constructable]
        public Nefertina()
            : base("Nefertina the Unfathomable", "Keeper of Ancient Secrets")
        {
        }

        public Nefertina(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = -1;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Robe(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(NefertinaToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my ancient beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Nefertina Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(NefertinaQuest)
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


    public class SBNefertina : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBNefertina()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the ancient-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AkhenatenTheHeretic), 1000, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(CleopatraTheEnigmatic), 1200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(HatshepsutTheQueen), 1100, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(KhufuTheGreatBuilder), 1300, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(MentuhotepTheWise), 1400, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(Nefertiti), 1500, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(RamsesTheImmortal), 1600, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(SetiTheAvenger), 1700, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ThutmoseTheConqueror), 1800, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(TutankhamunTheCursed), 1900, 10, 13, 0));
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
