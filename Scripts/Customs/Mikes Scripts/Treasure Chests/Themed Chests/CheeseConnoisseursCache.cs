using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CheeseConnoisseursCache : WoodenChest
    {
        [Constructable]
        public CheeseConnoisseursCache()
        {
            Name = "Cheese Connoisseur's Cache";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Cheese Tokens"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Fine Wine Pairing", 1162), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Dairy Delights"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of Brie"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<GreaterHealPotion>("Aged Apple Cider Vinegar", 1162), 0.08);
            AddItem(CreateGoldItem("Golden Cheese Wheel"), 0.16);
            AddItem(CreateColoredItem<CheeseSlice>("Slice of Vintage Cheddar", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Ring of Roquefort"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Cheese Hunter's Lens"), 0.13);
            AddItem(CreatePotion(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateCookedBird(), 0.30);
            AddItem(CreateMalletAndChisel(), 0.30);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "To the true cheese lover, every nibble is an adventure.";
            note.TitleString = "Cheese Chronicles";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Legendary Cheese Cave";
            map.Bounds = new Rectangle2D(2500, 2500, 700, 700);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Elixir of Lactose Tolerance";
            return potion;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Cheese Cutter Blade";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            FancyShirt armor = new FancyShirt();
            armor.Name = "Smock of the Cheese Maker";
            armor.Hue = Utility.RandomList(1, 1788);
			armor.SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
            return armor;
        }

        private Item CreateCookedBird()
        {
            PlateLegs bird = new PlateLegs();
            bird.Name = "Cheese-Stuffed Legs";
            bird.Hue = Utility.RandomMinMax(1, 2900);
            bird.BaseArmorRating = Utility.Random(60, 90);
            bird.AbsorptionAttributes.EaterEnergy = 30;
            bird.ArmorAttributes.ReactiveParalyze = 1;
            bird.Attributes.BonusDex = 20;
            bird.Attributes.AttackChance = 10;
            bird.SkillBonuses.SetValues(0, SkillName.Cooking, 40.0);
            bird.ColdBonus = 20;
            bird.EnergyBonus = 20;
            bird.FireBonus = 20;
            bird.PhysicalBonus = 25;
            bird.PoisonBonus = 25;
            return bird;
        }

        private Item CreateMalletAndChisel()
        {
            Mace tool = new Mace();
            tool.Name = "Tool for Cheese Artistry";
            tool.Hue = Utility.RandomMinMax(50, 250);
            tool.MinDamage = Utility.Random(30, 80);
            tool.MaxDamage = Utility.Random(80, 120);
            tool.Attributes.BonusStr = 10;
            tool.Attributes.SpellDamage = 5;
            tool.WeaponAttributes.HitPoisonArea = 30;
            tool.WeaponAttributes.SelfRepair = 5;
            tool.SkillBonuses.SetValues(0, SkillName.Cooking, 25.0);
            return tool;
        }

        public CheeseConnoisseursCache(Serial serial) : base(serial)
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
