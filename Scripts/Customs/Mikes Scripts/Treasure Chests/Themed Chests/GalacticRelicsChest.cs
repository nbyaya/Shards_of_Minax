using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GalacticRelicsChest : WoodenChest
    {
        [Constructable]
        public GalacticRelicsChest()
        {
            Name = "Galactic Relics";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Galactic Credits"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Endorian Ale", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Jedi's Cache"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of Alderaan"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<Spellbook>("Holocron of Master Yoda", 2119), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Blue Milk", 1159), 0.08);
            AddItem(CreateGoldItem("Hutt's Gold Coin"), 0.16);
            AddItem(CreateColoredItem<MaxxiaScroll>("Grove Disc of Obi-Wan", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Princess Leia's Ring"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Rebel Scout's Spyglass"), 0.13);
            AddItem(CreatePotion(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateFlyingCarpet(), 0.30);
            AddItem(CreateRobe(), 0.30);
            AddItem(CreateRod(), 0.30);
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
            note.NoteString = "May the Force be with you always";
            note.TitleString = "From the Jedi Archives";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Death Star";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Elixir of the Force";
            return potion;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Lightsaber";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Mandalorian Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFlyingCarpet()
        {
            FancyShirt carpet = new FancyShirt();
            carpet.Name = "Imperial Shirt";
            carpet.Hue = Utility.RandomMinMax(600, 1600);
            carpet.ClothingAttributes.DurabilityBonus = 5;
            carpet.Attributes.DefendChance = 10;
            carpet.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return carpet;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Robe of the Sith";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.Attributes.BonusMana = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            return robe;
        }

        private Item CreateRod()
        {
            RodOfOrcControl rod = new RodOfOrcControl();
            rod.Name = "Blaster Rifle";
            rod.Hue = Utility.RandomMinMax(50, 250);
            rod.MinDamage = Utility.Random(30, 80);
            rod.MaxDamage = Utility.Random(80, 120);
            rod.Attributes.BonusDex = 10;
            rod.Attributes.SpellDamage = 5;
            rod.WeaponAttributes.HitEnergyArea = 30;
            rod.WeaponAttributes.SelfRepair = 5;
            rod.SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
            return rod;
        }

        public GalacticRelicsChest(Serial serial) : base(serial)
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
