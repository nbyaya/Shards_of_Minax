using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TsarsLegacyChest : WoodenChest
    {
        [Constructable]
        public TsarsLegacyChest()
        {
            Name = "Tsar's Legacy";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Russian Rubles"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Vodka of the Czars", 1158), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Imperial Cache"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of the Romanovs"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<GreaterHealPotion>("Classic Kvass", 1158), 0.08);
            AddItem(CreateGoldItem("Imperial Gold Coin"), 0.16);
            AddItem(CreateNamedItem<GoldRing>("Signet Ring of Catherine"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Tsar's Expeditionary Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Russian Courage"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateElegantArmoire(), 0.30);
            AddItem(CreateRobe(), 0.30);
            AddItem(CreateScorp(), 0.30);
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
            note.NoteString = "In memory of the great Tsars of Russia";
            note.TitleString = "Memories of Russia";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Winter Palace";
            map.Bounds = new Rectangle2D(2000, 2000, 600, 600);
            map.NewPin = new Point2D(2200, 2200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Tsar's Saber";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Imperial Guard's Plate";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateElegantArmoire()
        {
            Robe armoire = new Robe();
            armoire.Name = "Imperial Wardrobe";
            armoire.Hue = Utility.RandomMinMax(600, 1600);
            armoire.ClothingAttributes.DurabilityBonus = 5;
            armoire.Attributes.DefendChance = 10;
            armoire.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            return armoire;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Robe of Rasputin";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.Attributes.BonusInt = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            return robe;
        }

        private Item CreateScorp()
        {
            Longsword scorp = new Longsword();
            scorp.Name = "Kulich Crafting Tool";
            scorp.Hue = Utility.RandomMinMax(50, 250);
            scorp.MinDamage = Utility.Random(30, 80);
            scorp.MaxDamage = Utility.Random(80, 120);
            scorp.Attributes.BonusStr = 10;
            scorp.Attributes.SpellDamage = 5;
            scorp.Slayer = SlayerName.OrcSlaying;
            scorp.WeaponAttributes.HitColdArea = 30;
            scorp.WeaponAttributes.SelfRepair = 5;
            scorp.SkillBonuses.SetValues(0, SkillName.Cooking, 25.0);
            return scorp;
        }

        public TsarsLegacyChest(Serial serial) : base(serial)
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
