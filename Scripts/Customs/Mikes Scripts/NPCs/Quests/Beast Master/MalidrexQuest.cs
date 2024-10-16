using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MalidrexQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Malidrex the Twisted Witch's Beast Master Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Welcome, brave adventurer. I am Malidrex, the Twisted Witch, master of all beasts. " +
                    "To prove your worth and gain access to my ultimate beast shop, you must present me with the following tokens:\n\n" +
                    "1. Arachnid Token\n" +
                    "2. Astro Token\n" +
                    "3. Bear Token\n" +
                    "4. BeastBane Token\n" +
                    "5. Beastiarius Token\n" +
                    "6. Beastly Token\n" +
                    "7. BeastMaster Token\n" +
                    "8. Beast Token\n" +
                    "9. Beasty Token\n" +
                    "10. Ben Token\n" +
                    "11. Bongo Token\n" +
                    "12. Botanicus Token\n" +
                    "13. Bramble Token\n" +
                    "14. Cluck Token\n" +
                    "15. Crab Token\n" +
                    "16. Draconis Token\n" +
                    "17. Earth Token\n" +
                    "18. Feline Token\n" +
                    "19. Ferret Token\n" +
                    "20. Fizzlewick Token\n" +
                    "21. Flame Token\n" +
                    "22. Gale Token\n" +
                    "23. Gator Token\n" +
                    "24. Goat Token\n" +
                    "25. Golem Token\n" +
                    "26. Grimshade Token\n" +
                    "27. Lich Token\n" +
                    "28. Llama Token\n" +
                    "29. Lupine Token\n" +
                    "30. Mechanical Token\n" +
                    "31. Mechano Token\n" +
                    "32. Moolin Token\n" +
                    "33. Nefertina Token\n" +
                    "34. Ogre Token\n" +
                    "35. Satyr Token\n" +
                    "36. Shadowthorn Token\n" +
                    "37. Slime Token\n" +
                    "38. Squirrel Token\n" +
                    "39. Sweetsworth Token\n" +
                    "40. Swine Token\n" +
                    "41. Toadstone Token\n" +
                    "42. Venom Token\n" +
                    "43. Vermin Token\n" +
                    "44. Wobble Token\n" +
                    "45. Zodiac Token\n\n" +
                    "Bring me these tokens, and I will grant you the Master Token, which will unlock access to my shop.";
            }
        }

        public override object Refuse { get { return "Very well. Return when you have collected all the Beast Tokens."; } }

        public override object Uncomplete { get { return "You haven't collected all the tokens yet. Keep searching!"; } }

        public override object Complete { get { return "Impressive! You have proven yourself as the ultimate Beast Master. My shop is now open to you!"; } }

        public MalidrexQuest() : base()
        {
            // Adding objectives to collect each Beast Token
            AddObjective(new ObtainObjective(typeof(ArachnidToken), "Arachnid Token", 1));
            AddObjective(new ObtainObjective(typeof(AstroToken), "Astro Token", 1));
            AddObjective(new ObtainObjective(typeof(BearToken), "Bear Token", 1));
            AddObjective(new ObtainObjective(typeof(BeastBaneToken), "BeastBane Token", 1));
            AddObjective(new ObtainObjective(typeof(BeastiariusToken), "Beastiarius Token", 1));
            AddObjective(new ObtainObjective(typeof(BeastlyToken), "Beastly Token", 1));
            AddObjective(new ObtainObjective(typeof(BeastMasterToken), "BeastMaster Token", 1));
            AddObjective(new ObtainObjective(typeof(BeastToken), "Beast Token", 1));
            AddObjective(new ObtainObjective(typeof(BeastyToken), "Beasty Token", 1));
            AddObjective(new ObtainObjective(typeof(BenToken), "Ben Token", 1));
            AddObjective(new ObtainObjective(typeof(BongoToken), "Bongo Token", 1));
            AddObjective(new ObtainObjective(typeof(BotanicusToken), "Botanicus Token", 1));
            AddObjective(new ObtainObjective(typeof(BrambleToken), "Bramble Token", 1));
            AddObjective(new ObtainObjective(typeof(CluckToken), "Cluck Token", 1));
            AddObjective(new ObtainObjective(typeof(CrabToken), "Crab Token", 1));
            AddObjective(new ObtainObjective(typeof(DraconisToken), "Draconis Token", 1));
            AddObjective(new ObtainObjective(typeof(EarthToken), "Earth Token", 1));
            AddObjective(new ObtainObjective(typeof(FelineToken), "Feline Token", 1));
            AddObjective(new ObtainObjective(typeof(FerretToken), "Ferret Token", 1));
            AddObjective(new ObtainObjective(typeof(FizzlewickToken), "Fizzlewick Token", 1));
            AddObjective(new ObtainObjective(typeof(FlameToken), "Flame Token", 1));
            AddObjective(new ObtainObjective(typeof(GaleToken), "Gale Token", 1));
            AddObjective(new ObtainObjective(typeof(GatorToken), "Gator Token", 1));
            AddObjective(new ObtainObjective(typeof(GoatToken), "Goat Token", 1));
            AddObjective(new ObtainObjective(typeof(GolemToken), "Golem Token", 1));
            AddObjective(new ObtainObjective(typeof(GrimshadeToken), "Grimshade Token", 1));
            AddObjective(new ObtainObjective(typeof(LichToken), "Lich Token", 1));
            AddObjective(new ObtainObjective(typeof(LlamaToken), "Llama Token", 1));
            AddObjective(new ObtainObjective(typeof(LupineToken), "Lupine Token", 1));
            AddObjective(new ObtainObjective(typeof(MechanicalToken), "Mechanical Token", 1));
            AddObjective(new ObtainObjective(typeof(MechanoToken), "Mechano Token", 1));
            AddObjective(new ObtainObjective(typeof(MoolinToken), "Moolin Token", 1));
            AddObjective(new ObtainObjective(typeof(NefertinaToken), "Nefertina Token", 1));
            AddObjective(new ObtainObjective(typeof(OgreToken), "Ogre Token", 1));
            AddObjective(new ObtainObjective(typeof(SatyrToken), "Satyr Token", 1));
            AddObjective(new ObtainObjective(typeof(ShadowthornToken), "Shadowthorn Token", 1));
            AddObjective(new ObtainObjective(typeof(SlimeToken), "Slime Token", 1));
            AddObjective(new ObtainObjective(typeof(SquirrelToken), "Squirrel Token", 1));
            AddObjective(new ObtainObjective(typeof(SweetsworthToken), "Sweetsworth Token", 1));
            AddObjective(new ObtainObjective(typeof(SwineToken), "Swine Token", 1));
            AddObjective(new ObtainObjective(typeof(ToadstoneToken), "Toadstone Token", 1));
            AddObjective(new ObtainObjective(typeof(VenomToken), "Venom Token", 1));
            AddObjective(new ObtainObjective(typeof(VerminToken), "Vermin Token", 1));
            AddObjective(new ObtainObjective(typeof(WobbleToken), "Wobble Token", 1));
            AddObjective(new ObtainObjective(typeof(ZodiacToken), "Zodiac Token", 1));

            AddReward(new BaseReward(typeof(MasterToken), 1, "Master Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Malidrex the Twisted Witch's challenge!");
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

    public class Malidrex : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMalidrex());
        }

        [Constructable]
        public Malidrex()
            : base("Malidrex the Twisted Witch", "Master of Beasts")
        {
        }

        public Malidrex(Serial serial)
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
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new WizardsHat(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null)
                {
                    SayTo(from, "Welcome to my ultimate beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Master Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MalidrexQuest)
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

    public class SBMalidrex : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBMalidrex()
        {
        }

        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(RandomAngelScroll), 10000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomAnimatedPlantScroll), 9000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomApeScroll), 8000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomArachnidScroll), 7500, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomBearScroll), 8500, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomBeholderScroll), 9500, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomBovineScroll), 8200, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomCanineScroll), 7800, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomCatScroll), 7000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomDemonScroll), 12000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomDragonScroll), 15000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomElementalScroll), 11000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomFeyScroll), 9800, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomGargoyleScroll), 10500, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomGiantScroll), 0x1F5B000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomInsectScroll), 7200, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomKnightLordScroll), 20000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomLizardScroll), 8500, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomMinotaurScroll), 14000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomMonsterScroll), 11000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomOrcScroll), 9000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomPigScroll), 6500, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomRidableScroll), 15000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomRobotScroll), 18000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomRodentScroll), 7200, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomRuminantScroll), 7600, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomSlimeScroll), 5000, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomSnakeScroll), 6800, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomUndeadScroll), 12500, 10, 0x1F5B, 0));
                Add(new GenericBuyInfo(typeof(RandomWizardWitchScroll), 15000, 10, 0x1F5B, 0));
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
