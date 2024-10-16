using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RailwayWorkersChest : WoodenChest
    {
        [Constructable]
        public RailwayWorkersChest()
        {
            Name = "Railway Worker’s Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Rockies", 2710), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Canadian Whiskey", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Railway Worker’s Reward"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Spike Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the West", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Maple Syrup"), 0.10);
            AddItem(CreateGoldItem("British Sovereign"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Engineer", 1618), 0.20);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Maple Leaf Earring"), 0.20);
            AddItem(CreateMap(), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Macdonald’s Trusted Spyglass"), 0.10);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Antidote Brew"), 0.15);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateFancyShirt(), 0.20);
            AddItem(CreatePlateGorget(), 0.20);
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
            note.NoteString = "We have finally completed the transcontinental railway today!";
            note.TitleString = "Sir John A. Macdonald’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Macdonald’s Secret Stash";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "The Last Spike";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Railway Worker’s Best";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFancyShirt()
        {
            FancyShirt shirt = new FancyShirt();
            shirt.Name = "Psychedelic Tie-Dye Shirt";
            shirt.Hue = Utility.RandomMinMax(100, 1500);
            shirt.Attributes.BonusInt = 10;
            shirt.Attributes.RegenMana = 3;
            shirt.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            return shirt;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget gorget = new PlateGorget();
            gorget.Name = "Courtier's Enchanted Amulet";
            gorget.Hue = Utility.RandomMinMax(700, 950);
            gorget.BaseArmorRating = Utility.Random(35, 62);
            gorget.AbsorptionAttributes.EaterFire = 10;
            gorget.ArmorAttributes.MageArmor = 1;
            gorget.Attributes.BonusHits = 10;
            gorget.Attributes.SpellDamage = 5;
            gorget.SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            gorget.ColdBonus = 5;
            gorget.EnergyBonus = 10;
            gorget.FireBonus = 10;
            gorget.PhysicalBonus = 5;
            gorget.PoisonBonus = 5;
            return gorget;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Ra's Searing Dagger";
            dagger.Hue = Utility.RandomMinMax(600, 800);
            dagger.MinDamage = Utility.Random(10, 40);
            dagger.MaxDamage = Utility.Random(40, 70);
            dagger.Attributes.SpellDamage = 5;
            dagger.Attributes.NightSight = 1;
            dagger.Slayer = SlayerName.FlameDousing;
            dagger.WeaponAttributes.HitFireArea = 30;
            dagger.WeaponAttributes.HitFireball = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            return dagger;
        }

        public RailwayWorkersChest(Serial serial) : base(serial)
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
