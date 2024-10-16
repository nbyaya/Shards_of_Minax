using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class FairyDustChest : WoodenChest
    {
        [Constructable]
        public FairyDustChest()
        {
            Name = "Fairy Dust Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateDiamond(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Fairy Nectar", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Fairy Treasure"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Butterfly Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Sun", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Fairy Wine"), 0.08);
            AddItem(CreateGoldItem("Fairy Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Shoes of the Sprite", 1153), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Flower Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateMagicWand(), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Dew"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreateHelm(), 0.20);
            AddItem(CreateButcherKnife(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateDiamond()
        {
            Diamond diamond = new Diamond();
            diamond.Name = "Diamond of the Stars";
            return diamond;
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
            note.NoteString = "To the one who finds this chest, may you be blessed with fairy magic!";
            note.TitleString = "A note from the fairies";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Fairy Realm";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1150);
            map.Protected = true;
            return map;
        }

        private Item CreateMagicWand()
        {
            MagicWand wand = new MagicWand();
            wand.Name = "Fairy Wand";
            wand.Hue = Utility.RandomList(1, 1788);
            return wand;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new ShortSpear(), new Longsword());
            weapon.Name = "Fairy Blade";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseClothing armor = Utility.RandomList<BaseClothing>(new Robe(), new Shirt());
            armor.Name = "Fairy Garment";
            armor.Hue = Utility.RandomList(1, 1788);
            return armor;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Mender's Divine Robe";
            robe.Hue = Utility.RandomMinMax(200, 1200);
            robe.Attributes.BonusInt = 15;
            robe.Attributes.RegenMana = 5;
            robe.SkillBonuses.SetValues(0, SkillName.Healing, 25.0);
            robe.SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
            return robe;
        }

        private Item CreateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Fortune's Helm";
            helm.Hue = Utility.RandomMinMax(700, 950);
            helm.BaseArmorRating = Utility.Random(30, 65);
            helm.AbsorptionAttributes.EaterFire = 10;
            helm.ArmorAttributes.SelfRepair = 4;
            helm.Attributes.Luck = 150;
            helm.SkillBonuses.SetValues(0, SkillName.ItemID, 15.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 5;
            helm.FireBonus = 15;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateButcherKnife()
        {
            ButcherKnife knife = new ButcherKnife();
            knife.Name = "Valiant Thrower";
            knife.Hue = Utility.RandomMinMax(200, 400);
            knife.MinDamage = Utility.Random(20, 50);
            knife.MaxDamage = Utility.Random(50, 90);
            knife.Attributes.BonusDex = 10;
            knife.Attributes.WeaponSpeed = 10;
            knife.Slayer = SlayerName.ReptilianDeath;
            knife.WeaponAttributes.HitLeechHits = 20;
            knife.WeaponAttributes.BloodDrinker = 15;
            knife.SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
            return knife;
        }

        public FairyDustChest(Serial serial) : base(serial)
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
