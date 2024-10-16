using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RiverPiratesChest : WoodenChest
    {
        [Constructable]
        public RiverPiratesChest()
        {
            Name = "River Pirates Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateDiamond(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Pirate’s Grog", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Pirates Bounty"), 0.10);
            AddItem(CreateNamedItem<SilverRing>("Silver Skull Ring"), 0.10);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Sea", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Pirate’s Rum"), 0.08);
            AddItem(CreateGoldItem("Cursed Doubloon"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Buccaneer", 1618), 0.20);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Skull Earring"), 0.18);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Cutlass>("Captain’s Cutlass"), 0.15);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Bottle of Deadly Poison"), 0.20);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreatePlateLegs(), 0.20);
            AddItem(CreateBow(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateDiamond()
        {
            Diamond diamond = new Diamond();
            diamond.Name = "Diamond of the Delta";
            return diamond;
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Yo ho ho and a bottle of rum!";
            note.TitleString = "Captain Hook’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Pirate’s Cove";
            map.Bounds = new Rectangle2D(2000, 2000, 400, 400);
            map.NewPin = new Point2D(2100, 2100);
            map.Protected = true;
            return map;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Adventurer's Boots";
            boots.Hue = Utility.RandomMinMax(250, 1150);
            boots.ClothingAttributes.LowerStatReq = 2;
            boots.Attributes.BonusStam = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Camping, 20.0);
            return boots;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs plateLegs = new PlateLegs();
            plateLegs.Name = "Blade Dancer's Plate Legs";
            plateLegs.Hue = Utility.RandomMinMax(600, 900);
            plateLegs.BaseArmorRating = Utility.Random(45, 80);
            plateLegs.AbsorptionAttributes.EaterFire = 10;
            plateLegs.ArmorAttributes.DurabilityBonus = 20;
            plateLegs.Attributes.BonusDex = 10;
            plateLegs.Attributes.AttackChance = 8;
            plateLegs.SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
            plateLegs.ColdBonus = 10;
            plateLegs.EnergyBonus = 10;
            plateLegs.FireBonus = 15;
            plateLegs.PhysicalBonus = 20;
            plateLegs.PoisonBonus = 10;
            return plateLegs;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Shadowstride Bow";
            bow.Hue = Utility.RandomMinMax(600, 700);
            bow.MinDamage = Utility.Random(20, 60);
            bow.MaxDamage = Utility.Random(60, 100);
            bow.Attributes.LowerManaCost = 10;
            bow.Attributes.BonusDex = 10;
            bow.Slayer = SlayerName.ArachnidDoom;
            bow.WeaponAttributes.HitLowerDefend = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
            bow.SkillBonuses.SetValues(1, SkillName.Archery, 15.0);
            return bow;
        }

        public RiverPiratesChest(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
