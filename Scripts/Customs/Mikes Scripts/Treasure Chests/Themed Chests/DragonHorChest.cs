using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DragonHorChest : WoodenChest
    {
        [Constructable]
        public DragonHorChest()
        {
            Name = "Dragon’s Hoard";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<FireRuby>("Fire Ruby of the Dragon"), 1.0);
            AddItem(CreateColoredItem<GreaterHealPotion>("Dragon’s Breath", 1161), 1.0);
            AddItem(CreateNamedItem<TreasureLevel3>("Dragon’s Treasure"), 1.0);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Dragon Necklace"), 1.0);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 15000)), 1.0);
            AddItem(CreateColoredItem<Amber>("Amber of the Dragon", 1266), 1.0);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Dragon’s Blood"), 1.0);
            AddItem(CreateGoldItem("Golden Dragon Scale"), 1.0);
            AddItem(CreateColoredItem<Boots>("Boots of the Dragon", 1266), 1.0);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Dragon Earring"), 1.0);
            AddItem(CreateMap(), 1.0);
            AddItem(CreateNamedItem<Spyglass>("Dragon’s Eye Spyglass"), 1.0);
            AddItem(CreateNamedItem<GreaterStrengthPotion>("Bottle of Mysterious Potion"), 1.0);
            AddItem(CreateWeapon(), 1.0);
            AddItem(CreateArmor(), 1.0);
            AddItem(CreateMuffler(), 0.2);
            AddItem(CreateStuddedLegs(), 0.2);
            AddItem(CreateDagger(), 0.2);
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

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "I am the king of all dragons and this is my hoard.";
            note.TitleString = "Dragon’s Roar";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Dragon’s Lair";
            map.Bounds = new Rectangle2D(6000, 6000, 400, 400);
            map.NewPin = new Point2D(6100, 6150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Dragon Slayer";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(40, 80);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherArms(), new LeatherLegs(), new LeatherCap());
            armor.Name = "Dragon Skin Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(40, 80);
            return armor;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Timberland Muffler";
            muffler.Hue = Utility.RandomMinMax(520, 1520);
            muffler.Attributes.RegenStam = 3;
            muffler.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
            return muffler;
        }

        private Item CreateStuddedLegs()
        {
            StuddedLegs legs = new StuddedLegs();
            legs.Name = "Bard's Nimble Step";
            legs.Hue = Utility.RandomMinMax(100, 600);
            legs.BaseArmorRating = Utility.Random(25, 60);
            legs.AbsorptionAttributes.EaterKinetic = 15;
            legs.ArmorAttributes.LowerStatReq = 20;
            legs.Attributes.BonusDex = 20;
            legs.Attributes.RegenStam = 7;
            legs.SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
            legs.ColdBonus = 5;
            legs.EnergyBonus = 10;
            legs.FireBonus = 5;
            legs.PhysicalBonus = 15;
            legs.PoisonBonus = 5;
            return legs;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Manajuma's Knife";
            dagger.Hue = Utility.RandomMinMax(650, 850);
            dagger.MinDamage = Utility.Random(10, 50);
            dagger.MaxDamage = Utility.Random(50, 90);
            dagger.Attributes.BonusInt = 15;
            dagger.Attributes.SpellDamage = 5;
            dagger.Slayer = SlayerName.Ophidian;
            dagger.WeaponAttributes.HitLeechMana = 25;
            dagger.WeaponAttributes.HitPoisonArea = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            return dagger;
        }

        public DragonHorChest(Serial serial) : base(serial)
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
