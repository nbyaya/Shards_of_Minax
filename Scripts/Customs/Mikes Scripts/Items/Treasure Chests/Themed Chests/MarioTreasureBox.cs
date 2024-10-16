using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MarioTreasureBox : WoodenChest
    {
        [Constructable]
        public MarioTreasureBox()
        {
            Name = "Mario's Treasure Box";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Mario Coins"), 0.20);
            AddItem(CreateColoredItem<ApplePie>("Super Mushroom Pie", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Pipe to Another World"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of the Koopa"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<RedRose>("Fire Flower", 2119), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Starman Elixir"), 0.08);
            AddItem(CreateGoldItem("Princess Peach's Gold Coin"), 0.16);
            AddItem(CreateColoredItem<Cloak>("Mario's Cape", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Ring of Luigi"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Luigi's Adventure Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Invincibility"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateGreenGourd(), 0.30);
            AddItem(CreateRobeOfTheExplorer(), 0.30);
            AddItem(CreateFork(), 0.30);
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
            note.NoteString = "It's-a me, Mario!";
            note.TitleString = "Mario's Message";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Bowser's Castle";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Mace weapon = new Mace();
            weapon.Name = "Mario's Hammer";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Yoshi's Shell";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateGreenGourd()
        {
            CloseHelm gourd = new CloseHelm();
            gourd.Name = "1-Up Mushroom";
            gourd.Hue = Utility.RandomMinMax(600, 1600);
            gourd.Attributes.DefendChance = 10;
            gourd.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            gourd.FireBonus = 20;
            return gourd;
        }

        private Item CreateRobeOfTheExplorer()
        {
            ChainChest robe = new ChainChest();
            robe.Name = "Chest of Toad";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.BaseArmorRating = Utility.Random(60, 90);
            robe.AbsorptionAttributes.EaterPoison = 30;
            robe.ArmorAttributes.ReactiveParalyze = 1;
            robe.Attributes.BonusInt = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            robe.ColdBonus = 20;
            robe.EnergyBonus = 20;
            robe.FireBonus = 20;
            robe.PhysicalBonus = 25;
            robe.PoisonBonus = 25;
            return robe;
        }

        private Item CreateFork()
        {
            WarFork fork = new WarFork();
            fork.Name = "Bowser's Trident";
            fork.Hue = Utility.RandomMinMax(50, 250);
            fork.MinDamage = Utility.Random(30, 80);
            fork.MaxDamage = Utility.Random(80, 120);
            fork.Attributes.BonusStr = 10;
            fork.Attributes.SpellDamage = 5;
            fork.WeaponAttributes.HitFireArea = 30;
            fork.WeaponAttributes.SelfRepair = 5;
            fork.SkillBonuses.SetValues(0, SkillName.Wrestling, 25.0);
            return fork;
        }

        public MarioTreasureBox(Serial serial) : base(serial)
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
