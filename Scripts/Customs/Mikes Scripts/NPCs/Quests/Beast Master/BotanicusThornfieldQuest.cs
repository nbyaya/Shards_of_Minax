using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BotanicusThornfieldQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Botanicus Thornfield's Verdant Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Botanicus Thornfield, master of the botanical beasts. " +
                    "To prove your worth and gain access to my exclusive shop, you must show your prowess " +
                    "against the most formidable of plant-based creatures. Complete these tasks:\n\n" +
                    "1. Slay a Bloodthirsty Vine.\n" +
                    "2. Defeat a Corrupting Creeper.\n" +
                    "3. Conquer a Dreaded Creeper.\n" +
                    "4. Overcome an Elder Tendril.\n" +
                    "5. Destroy a Nightshade Bramble.\n" +
                    "6. Vanquish a Phantom Vine.\n" +
                    "7. Subdue a Sinister Root.\n" +
                    "8. Eradicate a Thorned Horror.\n" +
                    "9. Defeat a Venomous Ivy.\n" +
                    "10. Exterminate a Vile Blossom.\n\n" +
                    "Complete these tasks, and I will grant you access to my botanical beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the botanical beasts."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep fighting the plants!"; } }

        public override object Complete { get { return "Impressive! You have proven yourself against the botanical horrors. My shop is now open to you!"; } }

        public BotanicusThornfieldQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BloodthirstyVines), "Bloodthirsty Vines", 1));
            AddObjective(new SlayObjective(typeof(CorruptingCreeper), "Corrupting Creepers", 1));
            AddObjective(new SlayObjective(typeof(DreadedCreeper), "Dreaded Creepers", 1));
            AddObjective(new SlayObjective(typeof(ElderTendril), "Elder Tendrils", 1));
            AddObjective(new SlayObjective(typeof(NightshadeBramble), "Nightshade Brambles", 1));
            AddObjective(new SlayObjective(typeof(PhantomVines), "Phantom Vines", 1));
            AddObjective(new SlayObjective(typeof(SinisterRoot), "Sinister Roots", 1));
            AddObjective(new SlayObjective(typeof(ThornedHorror), "Thorned Horrors", 1));
            AddObjective(new SlayObjective(typeof(VenomousIvy), "Venomous Ivy", 1));
            AddObjective(new SlayObjective(typeof(VileBlossom), "Vile Blossoms", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BotanicusToken), 1, "Botanicus Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Botanicus Thornfield's challenge!");
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

    public class BotanicusThornfield : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBotanicusThornfield());
        }

        [Constructable]
        public BotanicusThornfield()
            : base("Botanicus Thornfield", "Master of Botanical Beasts")
        {
        }

        public BotanicusThornfield(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BotanicusToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my botanical beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Botanicus Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BotanicusThornfieldQuest)
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

    public class SBBotanicusThornfield : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBotanicusThornfield()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the plant-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BloodthirstyVines), 1000, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(CorruptingCreeper), 1200, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(DreadedCreeper), 1100, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(ElderTendril), 1300, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(NightshadeBramble), 1400, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(PhantomVines), 1250, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(SinisterRoot), 1050, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(ThornedHorror), 1500, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomousIvy), 1600, 10, 8, 0));
                Add(new AnimalBuyInfo(1, typeof(VileBlossom), 1550, 10, 8, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class BotanicusToken : Item
    {
        [Constructable]
        public BotanicusToken() : base(0x1D00)
        {
            Name = "Botanicus Token";
            Hue = 0x497;
        }

        public BotanicusToken(Serial serial) : base(serial)
        {
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
}
