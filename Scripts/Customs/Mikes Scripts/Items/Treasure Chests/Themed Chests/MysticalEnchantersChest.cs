using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MysticalEnchantersChest : WoodenChest
    {
        [Constructable]
        public MysticalEnchantersChest()
        {
            Name = "Mystical Enchanter's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Diamond>("Diamond of the Archmage"), 0.20);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Elixir of Wisdom", 1489), 0.16);
            AddItem(CreateNamedItem<TreasureLevel2>("Enchanter's Trove"), 0.19);
            AddItem(CreateNamedItem<Ruby>("Ruby of the Sorcerer"), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.14);
            AddItem(CreateNamedItem<Spellbook>("Ancient Book of Magic"), 0.10);
            AddItem(CreateNamedItem<Spellbook>("Tome of the Dead"), 0.08);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Enchanter", 1772), 0.18);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Crescent Moon Earring"), 0.18);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Bottle of Eldritch Brew"), 0.15);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateDarkLordsRobe(), 1.0);
            AddItem(CreateGauntlets(), 0.20);
            AddItem(CreateKryss(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "The secrets of the ancients shall be revealed to the worthy.";
            note.TitleString = "Mystical Enchanter's Journal";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Enchanter's Robe";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateDarkLordsRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Dark Lords Robe";
            robe.Hue = Utility.RandomList(1, 1908);
            robe.ClothingAttributes.SelfRepair = 5;
            robe.Attributes.BonusDex = 50;
            robe.Attributes.BonusInt = 10;
            robe.SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
            robe.SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
            return robe;
        }

        private Item CreateGauntlets()
        {
            LeatherGloves gauntlets = new LeatherGloves();
            gauntlets.Name = "Artisan's Crafted Gauntlets";
            gauntlets.Hue = Utility.RandomMinMax(1, 1000);
            gauntlets.BaseArmorRating = Utility.Random(25, 70);
            gauntlets.ArmorAttributes.LowerStatReq = 15;
            gauntlets.Attributes.LowerRegCost = 20;
            gauntlets.Attributes.Luck = 50;
            gauntlets.SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
            gauntlets.SkillBonuses.SetValues(1, SkillName.Carpentry, 15.0);
            gauntlets.SkillBonuses.SetValues(2, SkillName.Tailoring, 10.0);
            gauntlets.ColdBonus = 10;
            gauntlets.EnergyBonus = 10;
            gauntlets.FireBonus = 10;
            gauntlets.PhysicalBonus = 10;
            gauntlets.PoisonBonus = 10;
            return gauntlets;
        }

        private Item CreateKryss()
        {
            Kryss kryss = new Kryss();
            kryss.Name = "Assassin's Kryss";
            kryss.Hue = Utility.RandomMinMax(1, 1000);
            kryss.MinDamage = Utility.Random(15, 65);
            kryss.MaxDamage = Utility.Random(65, 120);
            kryss.Attributes.AttackChance = 15;
            kryss.Attributes.LowerRegCost = 10;
            kryss.Slayer = SlayerName.OrcSlaying;
            kryss.Slayer2 = SlayerName.TrollSlaughter;
            kryss.WeaponAttributes.HitPoisonArea = 30;
            kryss.WeaponAttributes.HitLeechStam = 25;
            kryss.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            kryss.SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);
            return kryss;
        }

        public MysticalEnchantersChest(Serial serial) : base(serial)
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
