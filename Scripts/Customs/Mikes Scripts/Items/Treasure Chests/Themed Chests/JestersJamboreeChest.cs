using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JestersJamboreeChest : WoodenChest
    {
        [Constructable]
        public JestersJamboreeChest()
        {
            Name = "Jester's Jamboree";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateGoldItem("Comedy Coins"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Bubbly Seltzer Water", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Circus Coffer"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Clown Bangle"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.16);
            AddItem(CreateColoredItem<Bag>("Bag of Tricks", 2120), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Juggling Pins Potion"), 0.08);
            AddItem(CreateGoldItem("Laughing Loot"), 0.14);
            AddItem(CreateNamedItem<GoldRing>("Ring of Giggles"), 0.15);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Jester's Viewfinder"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Chuckles"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateMuffins(), 0.30);
            AddItem(CreateRobe(), 0.30);
            AddItem(CreateSkillet(), 0.30);
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
            note.NoteString = "Laughter is the best medicine!";
            note.TitleString = "Joker's Proverb";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Big Top";
            map.Bounds = new Rectangle2D(3000, 3000, 700, 700);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Crossbow weapon = new Crossbow();
            weapon.Name = "Pie Launcher";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(20, 60);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new ChainLegs(), new LeatherChest(), new ChainCoif(), new PlateChest());
            armor.Name = "Jester's Outfit";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(10, 40);
            return armor;
        }

        private Item CreateMuffins()
        {
            LeatherCap muffins = new LeatherCap();
            muffins.Name = "Funny Face Cap";
            muffins.Hue = Utility.RandomMinMax(600, 1600);
            muffins.Attributes.DefendChance = 5;
            muffins.SkillBonuses.SetValues(0, SkillName.Begging, 20.0);
            muffins.EnergyBonus = 20;
            return muffins;
        }

        private Item CreateRobe()
        {
            LeatherGloves robe = new LeatherGloves();
            robe.Name = "Gloves of Gags";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.BaseArmorRating = Utility.Random(30, 60);
            robe.AbsorptionAttributes.EaterFire = 30;
            robe.ArmorAttributes.LowerStatReq = 1;
            robe.Attributes.BonusDex = 10;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            robe.ColdBonus = 10;
            robe.EnergyBonus = 20;
            robe.FireBonus = 10;
            robe.PhysicalBonus = 15;
            robe.PoisonBonus = 15;
            return robe;
        }

        private Item CreateSkillet()
        {
            Hatchet skillet = new Hatchet();
            skillet.Name = "Comedy Prop Axe";
            skillet.Hue = Utility.RandomMinMax(50, 250);
            skillet.MinDamage = Utility.Random(10, 40);
            skillet.MaxDamage = Utility.Random(40, 80);
            skillet.Attributes.BonusStr = 5;
            skillet.Attributes.SpellDamage = 2;
            skillet.Slayer = SlayerName.OrcSlaying;
            skillet.WeaponAttributes.HitFireArea = 20;
            skillet.WeaponAttributes.SelfRepair = 3;
            skillet.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            return skillet;
        }

        public JestersJamboreeChest(Serial serial) : base(serial)
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
