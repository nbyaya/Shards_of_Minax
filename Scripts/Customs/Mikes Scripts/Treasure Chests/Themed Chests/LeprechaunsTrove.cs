using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class LeprechaunsTrove : WoodenChest
    {
        [Constructable]
        public LeprechaunsTrove()
        {
            Name = "Leprechaun's Trove";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Celtic Gold"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Irish Whiskey", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Druid's Cache"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Tara"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<GreaterHealPotion>("Ancient Mead", 1160), 0.09);
            AddItem(CreateGoldItem("Gaelic Coin"), 0.16);
            AddItem(CreateNamedItem<GoldRing>("Ring of the Claddagh"), 0.18);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Celtic Seafarer's Spyglass"), 0.14);
            AddItem(CreateNamedItem<GreenGourd>("Gourd of the Banshee"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Irish Vigor"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateIrishRose(), 0.30);
            AddItem(CreateShroudOfBale(), 0.30);
            AddItem(CreateRodOfOrcControl(), 0.30);
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
            note.NoteString = "May the luck of the Irish be with you";
            note.TitleString = "Blessings of Eire";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Blarney Stone";
            map.Bounds = new Rectangle2D(2500, 2500, 700, 700);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Mace weapon = new Mace();
            weapon.Name = "Shillelagh of the Fae";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of the Emerald Isle";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateIrishRose()
        {
            Boots rose = new Boots();
            rose.Name = "Rose of Dublin";
            rose.Hue = Utility.RandomMinMax(600, 1600);
            rose.ClothingAttributes.DurabilityBonus = 5;
            rose.Attributes.DefendChance = 10;
            rose.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            return rose;
        }

        private Item CreateShroudOfBale()
        {
            Robe shroud = new Robe();
            shroud.Name = "Cloak of the Celtic Druid";
            shroud.Hue = Utility.RandomMinMax(1, 1000);
            shroud.Attributes.BonusMana = 20;
            shroud.Attributes.AttackChance = 10;
            shroud.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return shroud;
        }

        private Item CreateRodOfOrcControl()
        {
            RodOfOrcControl rod = new RodOfOrcControl();
            rod.Name = "Celtic Bard's Harp";
            rod.Hue = Utility.RandomMinMax(50, 250);
            rod.MinDamage = Utility.Random(30, 80);
            rod.MaxDamage = Utility.Random(80, 120);
            rod.Attributes.BonusDex = 10;
            rod.Attributes.SpellDamage = 5;
            rod.WeaponAttributes.HitEnergyArea = 30;
            rod.WeaponAttributes.SelfRepair = 5;
            rod.SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
            return rod;
        }

        public LeprechaunsTrove(Serial serial) : base(serial)
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
