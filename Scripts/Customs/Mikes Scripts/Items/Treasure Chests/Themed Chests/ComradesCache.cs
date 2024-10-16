using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ComradesCache : WoodenChest
    {
        [Constructable]
        public ComradesCache()
        {
            Name = "Comrade's Cache";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateGoldItem("People's Currency"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Workers' Brew", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Revolutionary Relics"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of Solidarity"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("Proletariat Punch"), 0.08);
            AddItem(CreateGoldItem("Lenin's Gold Token"), 0.16);
            AddItem(CreateColoredItem<RedRose>("Rose of the Revolution", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Ring of the Workers"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Comrade's Visionary Tool"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Drink of the Masses"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateBooks(), 0.30);
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
            note.NoteString = "From each according to his ability, to each according to his needs";
            note.TitleString = "Marx's Maxim";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Kremlin";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Hammer and Sickle Blade";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            LeatherChest armor = new LeatherChest();
            armor.Name = "Red Army Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBooks()
        {
            HammerPick books = new HammerPick();
            books.Name = "Manifesto of the Communist Party";
            books.Hue = Utility.RandomMinMax(600, 1600);
            books.SkillBonuses.SetValues(0, SkillName.Macing, 60.0);
            return books;
        }

        private Item CreateRobe()
        {
            LeatherLegs robe = new LeatherLegs();
            robe.Name = "Legs of the Commissar";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.BaseArmorRating = Utility.Random(60, 90);
            robe.AbsorptionAttributes.EaterPoison = 30;
            robe.ArmorAttributes.ReactiveParalyze = 1;
            robe.Attributes.BonusInt = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
            robe.ColdBonus = 20;
            robe.EnergyBonus = 20;
            robe.FireBonus = 20;
            robe.PhysicalBonus = 25;
            robe.PoisonBonus = 25;
            return robe;
        }

        private Item CreateRod()
        {
            RodOfOrcControl rod = new RodOfOrcControl();
            rod.Name = "Red Baton of Leadership";
            rod.Hue = Utility.RandomMinMax(50, 250);
            rod.MinDamage = Utility.Random(30, 80);
            rod.MaxDamage = Utility.Random(80, 120);
            rod.Attributes.BonusStr = 10;
            rod.Attributes.SpellDamage = 5;
            rod.WeaponAttributes.HitFireArea = 30;
            rod.WeaponAttributes.SelfRepair = 5;
            rod.SkillBonuses.SetValues(0, SkillName.Peacemaking, 25.0);
            return rod;
        }

        public ComradesCache(Serial serial) : base(serial)
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
