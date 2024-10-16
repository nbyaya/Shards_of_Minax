using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AshokaTreasureChest : WoodenChest
    {
        [Constructable]
        public AshokaTreasureChest()
        {
            Name = "Ashoka’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Karshapana"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Indian Soma Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Ashoka’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Ashoka"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Mauryans", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Khamr Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Dinar"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Compassionate", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Buddha"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Ashoka’s Visionary Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Buddhist Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreatePlateChest(), 0.30);
            AddItem(CreatePlateGloves(), 0.30);
            AddItem(CreateLongsword(), 0.30);
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
            note.NoteString = "I am Ashoka, beloved of the gods, king of kings, emperor of India, upholder of the Dharma";
            note.TitleString = "Ashoka’s Edict";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Ashoka’s Stupa";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Ashoka’s Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Ashoka’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Chestplate of Peace";
            plateChest.Hue = Utility.RandomMinMax(600, 1600);
            plateChest.Attributes.DefendChance = 10;
            plateChest.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            plateChest.PhysicalBonus = 20;
            return plateChest;
        }

        private Item CreatePlateGloves()
        {
            PlateGloves plateGloves = new PlateGloves();
            plateGloves.Name = "Gauntlets of Non-Violence";
            plateGloves.Hue = Utility.RandomMinMax(1, 1000);
            plateGloves.BaseArmorRating = Utility.Random(60, 90);
            plateGloves.AbsorptionAttributes.EaterPoison = 30;
            plateGloves.ArmorAttributes.ReactiveParalyze = 1;
            plateGloves.Attributes.BonusDex = 20;
            plateGloves.Attributes.AttackChance = 10;
            plateGloves.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            plateGloves.ColdBonus = 20;
            plateGloves.EnergyBonus = 20;
            plateGloves.FireBonus = 20;
            plateGloves.PhysicalBonus = 25;
            plateGloves.PoisonBonus = 25;
            return plateGloves;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Samrat’s Blade";
            longsword.Hue = Utility.RandomMinMax(50, 250);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.Slayer = SlayerName.DaemonDismissal;
            longsword.WeaponAttributes.HitFireball = 30;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 75.0);
            return longsword;
        }

        public AshokaTreasureChest(Serial serial) : base(serial)
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
