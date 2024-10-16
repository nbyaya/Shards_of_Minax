using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TempestusGaleQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Tempestus Gale's Elemental Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Tempestus Gale, master of the elements. " +
                    "To prove your worth and gain access to my exclusive shop, you must show " +
                    "your might against the very essence of the storm. Complete these tasks:\n\n" +
                    "1. Slay a Breeze Phantom.\n" +
                    "2. Defeat a Cyclone Demon.\n" +
                    "3. Conquer a Gale Wisp.\n" +
                    "4. Overcome a Mystic Wisp.\n" +
                    "5. Destroy a Shadow Drifter.\n" +
                    "6. Vanquish a Sky Seraph.\n" +
                    "7. Subdue a Storm Herald.\n" +
                    "8. Eradicate a Tempest Spirit.\n" +
                    "9. Defeat a Tempest Wyrm.\n" +
                    "10. Exterminate a Vortex Guardian.\n" +
                    "11. Destroy a Whirlwind Fiend.\n" +
                    "12. Conquer a Zephyr Warden.\n\n" +
                    "Complete these tasks, and I will grant you access to my elemental beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the elements."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep fighting the storm!"; } }

        public override object Complete { get { return "Impressive! You have proven yourself against the forces of nature. My shop is now open to you!"; } }

        public TempestusGaleQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BreezePhantom), "Breeze Phantoms", 1));
            AddObjective(new SlayObjective(typeof(CycloneDemon), "Cyclone Demons", 1));
            AddObjective(new SlayObjective(typeof(GaleWisp), "Gale Wisps", 1));
            AddObjective(new SlayObjective(typeof(MysticWisp), "Mystic Wisps", 1));
            AddObjective(new SlayObjective(typeof(ShadowDrifter), "Shadow Drifters", 1));
            AddObjective(new SlayObjective(typeof(SkySeraph), "Sky Seraphs", 1));
            AddObjective(new SlayObjective(typeof(StormHerald), "Storm Heralds", 1));
            AddObjective(new SlayObjective(typeof(TempestSpirit), "Tempest Spirits", 1));
            AddObjective(new SlayObjective(typeof(TempestWyrm), "Tempest Wyrms", 1));
            AddObjective(new SlayObjective(typeof(VortexGuardian), "Vortex Guardians", 1));
            AddObjective(new SlayObjective(typeof(WhirlwindFiend), "Whirlwind Fiends", 1));
            AddObjective(new SlayObjective(typeof(ZephyrWarden), "Zephyr Wardens", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(GaleToken), 1, "Beast Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Tempestus Gale's challenge!");
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


    public class TempestusGale : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTempestusGale());
        }

        [Constructable]
        public TempestusGale()
            : base("Tempestus Gale", "Master of the Elements")
        {
        }

        public TempestusGale(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(GaleToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my elemental beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Gale Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(TempestusGaleQuest)
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


    public class SBTempestusGale : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBTempestusGale()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the elemental-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BreezePhantom), 1000, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(CycloneDemon), 1200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(GaleWisp), 900, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(MysticWisp), 950, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowDrifter), 1100, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(SkySeraph), 1300, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(StormHerald), 1400, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(TempestSpirit), 1250, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(TempestWyrm), 1500, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(VortexGuardian), 1350, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(WhirlwindFiend), 1600, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ZephyrWarden), 1450, 10, 13, 0));
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
