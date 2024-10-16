using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WorldWarIIChest : WoodenChest
    {
        [Constructable]
        public WorldWarIIChest()
        {
            Name = "World War II";
            Hue = Utility.Random(1, 1950);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.09);
            AddItem(CreateNamedItem<SilverNecklace>("War Medal"), 0.32);
            AddItem(CreateNamedItem<Emerald>("Codebreaker's Gem"), 0.31);
            AddItem(CreateNamedItem<TreasureLevel4>("Allied Relic"), 0.29);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5300)), 0.14);
            AddItem(CreateWeapon("Commando Dagger"), 0.16);
            AddItem(CreateArmor("Airman's Uniform"), 0.16);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateDagger(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "Letter from the front lines.";
            note.TitleString = "Soldier's Note";
            return note;
        }

        private Item CreateWeapon(string name)
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger());
            weapon.Name = name;
            weapon.Hue = Utility.RandomList(1, 1950);
            return weapon;
        }

        private Item CreateArmor(string name)
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = name;
            armor.Hue = Utility.RandomList(1, 1950);
            return armor;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Whisperer's Boots";
            boots.Hue = Utility.RandomMinMax(750, 1750);
            boots.ClothingAttributes.LowerStatReq = 3;
            boots.Attributes.BonusStr = 5;
            boots.Attributes.BonusDex = 15;
            boots.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            boots.SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
            return boots;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Demonspike Guard";
            plateChest.Hue = Utility.RandomMinMax(100, 500);
            plateChest.BaseArmorRating = Utility.Random(60, 100);
            plateChest.ArmorAttributes.DurabilityBonus = 25;
            plateChest.Attributes.DefendChance = 20;
            plateChest.Attributes.ReflectPhysical = 15;
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 5;
            plateChest.FireBonus = 25;
            plateChest.PhysicalBonus = 25;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Dragon's Scale Dagger";
            dagger.Hue = Utility.RandomMinMax(250, 450);
            dagger.MinDamage = Utility.Random(10, 50);
            dagger.MaxDamage = Utility.Random(50, 90);
            dagger.Attributes.ReflectPhysical = 5;
            dagger.Attributes.DefendChance = 10;
            dagger.Slayer = SlayerName.DragonSlaying;
            dagger.WeaponAttributes.HitLeechHits = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            return dagger;
        }

        public WorldWarIIChest(Serial serial) : base(serial)
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
