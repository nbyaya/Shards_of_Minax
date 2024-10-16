using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TravelerChest : WoodenChest
    {
        [Constructable]
        public TravelerChest()
        {
            Name = "Traveler's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Sky", 235), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Traveler's Brew", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Traveler's Loot"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Compass Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Forest", 1775), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Old Traveler's Wine", 2448), 0.08);
            AddItem(CreateGoldItem("Lucky Coin"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Wanderer", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Feather Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Traveler's Trusted Spyglass"), 0.13);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Bottle of Mysterious Brew"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateWideBrimHat(), 0.20);
            AddItem(CreateCloseHelm(), 0.20);
            AddItem(CreateDagger(), 0.20);
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
            note.NoteString = "I have seen many wonders on my journey.";
            note.TitleString = "Traveler's Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to a Hidden Oasis";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger());
            weapon.Name = "Wayfarer";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Traveler's Best";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateWideBrimHat()
        {
            WideBrimHat hat = new WideBrimHat();
            hat.Name = "Scout's Wide Brim Hat";
            hat.Hue = Utility.RandomMinMax(350, 1200);
            hat.Attributes.NightSight = 1;
            hat.SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);
            hat.SkillBonuses.SetValues(1, SkillName.Camping, 15.0);
            return hat;
        }

        private Item CreateCloseHelm()
        {
            CloseHelm helm = new CloseHelm();
            helm.Name = "Blade Dancer's CloseHelm";
            helm.Hue = Utility.RandomMinMax(600, 900);
            helm.BaseArmorRating = Utility.Random(40, 75);
            helm.AbsorptionAttributes.EaterEnergy = 10;
            helm.ArmorAttributes.LowerStatReq = 10;
            helm.Attributes.BonusInt = 5;
            helm.Attributes.ReflectPhysical = 10;
            helm.SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 10;
            helm.FireBonus = 10;
            helm.PhysicalBonus = 20;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Fang of Storms";
            dagger.Hue = Utility.RandomMinMax(100, 250);
            dagger.MinDamage = Utility.Random(20, 45);
            dagger.MaxDamage = Utility.Random(45, 85);
            dagger.Attributes.BonusInt = 10;
            dagger.Attributes.SpellChanneling = 1;
            dagger.Slayer = SlayerName.WaterDissipation;
            dagger.WeaponAttributes.HitLightning = 30;
            dagger.SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            return dagger;
        }

        public TravelerChest(Serial serial) : base(serial)
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
