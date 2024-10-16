using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WorkersRevolutionChest : WoodenChest
    {
        [Constructable]
        public WorkersRevolutionChest()
        {
            Name = "Workers’ Revolution";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Solidus"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("French Champagne", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("The Communist Manifesto"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Spartacus"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Paris Commune", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Russian Vodka"), 0.08);
            AddItem(CreateGoldItem("Golden Sovereign"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Gracchi", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of the Levellers"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Lenin’s Revolutionary Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Cuban Medicine"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateBoots(), 0.30);
            AddItem(CreateGauntlets(), 0.30);
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
            note.NoteString = "Workers of the world, unite! You have nothing to lose but your chains!";
            note.TitleString = "The International";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Haymarket Square";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Spartacus’s Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Spartacus’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Boots of the Wobblies";
            boots.Hue = Utility.RandomMinMax(600, 1600);
            boots.ClothingAttributes.DurabilityBonus = 5;
            boots.Attributes.DefendChance = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return boots;
        }

        private Item CreateGauntlets()
        {
            PlateGloves gauntlets = new PlateGloves();
            gauntlets.Name = "Gauntlets of the Diggers";
            gauntlets.Hue = Utility.RandomMinMax(1, 1000);
            gauntlets.BaseArmorRating = Utility.Random(60, 90);
            gauntlets.AbsorptionAttributes.EaterPoison = 30;
            gauntlets.ArmorAttributes.ReactiveParalyze = 1;
            gauntlets.Attributes.BonusDex = 20;
            gauntlets.Attributes.AttackChance = 10;
            gauntlets.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            gauntlets.ColdBonus = 20;
            gauntlets.EnergyBonus = 20;
            gauntlets.FireBonus = 20;
            gauntlets.PhysicalBonus = 25;
            gauntlets.PoisonBonus = 25;
            return gauntlets;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Zapata’s Blade";
            longsword.Hue = Utility.RandomMinMax(50, 250);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return longsword;
        }

        public WorkersRevolutionChest(Serial serial) : base(serial)
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
