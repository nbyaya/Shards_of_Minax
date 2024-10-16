using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DragonHoChest : WoodenChest
    {
        [Constructable]
        public DragonHoChest()
        {
            Name = "Dragon's Hoard";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Dragon", 0), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Dragon's Breath", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Dragon's Treasure"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Dragon Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Dragon's Eye", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Dragon's Ale"), 0.08);
            AddItem(CreateGoldItem("Dragon's Gold"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Dragon Slayer", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Dragon Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Dragon's Eye Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Dragon's Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateBascinet(), 0.20);
            AddItem(CreateShield(), 0.20);
            AddItem(CreateBlackStaff(), 0.20);
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
            note.NoteString = "The dragon's hoard is vast and endless.";
            note.TitleString = "Dragon's Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Dragon's Lair";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Dragon Slayer";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Dragon Scale Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBascinet()
        {
            Bascinet bascinet = new Bascinet();
            bascinet.Name = "Helmet of the Ore Whisperer";
            bascinet.Hue = Utility.RandomMinMax(400, 1400);
            bascinet.Attributes.NightSight = 1;
            bascinet.SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
            bascinet.SkillBonuses.SetValues(1, SkillName.ItemID, 15.0);
            bascinet.FireBonus = 10;
            bascinet.PhysicalBonus = 20;
            return bascinet;
        }

        private Item CreateShield()
        {
            WoodenKiteShield shield = new WoodenKiteShield();
            shield.Name = "Witchfire Shield";
            shield.Hue = Utility.RandomMinMax(1, 1000);
            shield.BaseArmorRating = Utility.Random(25, 75);
            shield.AbsorptionAttributes.EaterFire = 30;
            shield.ArmorAttributes.ReactiveParalyze = 1;
            shield.Attributes.ReflectPhysical = 10;
            shield.Attributes.LowerManaCost = 10;
            shield.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            shield.ColdBonus = 10;
            shield.EnergyBonus = 15;
            shield.FireBonus = 15;
            shield.PhysicalBonus = 10;
            shield.PoisonBonus = 10;
            return shield;
        }

        private Item CreateBlackStaff()
        {
            BlackStaff blackStaff = new BlackStaff();
            blackStaff.Name = "Searing Touch";
            blackStaff.Hue = Utility.RandomMinMax(150, 350);
            blackStaff.MinDamage = Utility.Random(25, 70);
            blackStaff.MaxDamage = Utility.Random(70, 105);
            blackStaff.Attributes.SpellDamage = 15;
            blackStaff.Attributes.RegenStam = 5;
            blackStaff.Slayer = SlayerName.FlameDousing;
            blackStaff.WeaponAttributes.HitFireArea = 40;
            blackStaff.WeaponAttributes.MageWeapon = 1;
            blackStaff.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            blackStaff.SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
            return blackStaff;
        }

        public DragonHoChest(Serial serial) : base(serial)
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
