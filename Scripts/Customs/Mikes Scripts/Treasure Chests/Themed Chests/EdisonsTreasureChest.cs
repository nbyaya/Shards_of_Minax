using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EdisonsTreasureChest : WoodenChest
    {
        [Constructable]
        public EdisonsTreasureChest()
        {
            Name = "Edison's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Dollar"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("American Whiskey", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Edison's Inventions"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of Edison"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Industrial Revolution", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("French Champagne"), 0.08);
            AddItem(CreateGoldItem("Golden Eagle"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Inventor", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Electricity"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Edison's Magnifying Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Edison's Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateGlasses(), 0.30);
            AddItem(CreateLeatherGloves(), 0.30);
            AddItem(CreateWarFork(), 0.30);
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
            note.NoteString = "I am Thomas Edison, inventor of the light bulb, the phonograph, and many other devices that changed the world";
            note.TitleString = "Edison's Diary";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Edison's Laboratory";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new WarFork(); // Assuming WarFork or a similar weapon class exists
            weapon.Name = "Edison's Light Saber";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Edison's Suit";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateGlasses()
        {
            JesterHat glasses = new JesterHat();
            glasses.Name = "Hat of Insight";
            glasses.Hue = Utility.RandomMinMax(600, 1600);
            glasses.ClothingAttributes.DurabilityBonus = 5;
            glasses.Attributes.DefendChance = 10;
            glasses.SkillBonuses.SetValues(0, SkillName.Tinkering, 20.0);
            return glasses;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Gloves of Dexterity";
            gloves.Hue = Utility.RandomMinMax(1, 1000);
            gloves.BaseArmorRating = Utility.Random(60, 90);
            gloves.AbsorptionAttributes.EaterEnergy = 30;
            gloves.ArmorAttributes.ReactiveParalyze = 1;
            gloves.Attributes.BonusDex = 20;
            gloves.SkillBonuses.SetValues(0, SkillName.Tinkering, 20.0);
            return gloves;
        }

        private Item CreateWarFork()
        {
            WarFork fork = new WarFork();
            fork.Name = "Tesla’s Fork";
            fork.Hue = Utility.RandomMinMax(50, 250);
            fork.MinDamage = Utility.Random(30, 80);
            fork.MaxDamage = Utility.Random(80, 120);
            fork.Attributes.BonusStr = 10;
            fork.Attributes.SpellDamage = 5;
            fork.AosElementDamages.Energy = Utility.Random(50, 100);
            fork.SkillBonuses.SetValues(0, SkillName.Tinkering, 25.0);
            return fork;
        }

        public EdisonsTreasureChest(Serial serial) : base(serial)
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
