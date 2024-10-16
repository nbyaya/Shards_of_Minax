using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AlliedForcesTreasureChest : WoodenChest
    {
        [Constructable]
        public AlliedForcesTreasureChest()
        {
            Name = "Allied Forces’ Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Eagle"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("French Champagne", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Allied Forces’ Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Churchill"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Resistance", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Bordeaux Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Sovereign"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Liberator", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Roosevelt"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Allied Forces’ Reconnaissance Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Penicillin"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateFedora(), 0.30);
            AddItem(CreateLeatherGloves(), 0.30);
            AddItem(CreateFireballWand(), 0.30);
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
            note.NoteString = "We shall fight on the beaches, we shall fight on the landing grounds, we shall fight in the fields and in the streets, we shall fight in the hills; we shall never surrender";
            note.TitleString = "Churchill’s Speech";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Allied Forces’ Base";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Crossbow()); // Using Longsword as placeholder
            weapon.Name = "Allied Forces’ Rifle";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest()); // Using PlateChest as placeholder
            armor.Name = "Allied Forces’ Uniform";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFedora()
        {
            WideBrimHat fedora = new WideBrimHat();
            fedora.Name = "Fedora of the Detective";
            fedora.Hue = Utility.RandomMinMax(600, 1600);
            fedora.ClothingAttributes.DurabilityBonus = 5;
            fedora.SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);
            return fedora;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Gloves of the Spy";
            gloves.Hue = Utility.RandomMinMax(1, 1000);
            gloves.BaseArmorRating = Utility.Random(60, 90);
            gloves.AbsorptionAttributes.EaterFire = 30;
            gloves.Attributes.BonusDex = 20;
            gloves.SkillBonuses.SetValues(0, SkillName.Snooping, 20.0);
            return gloves;
        }

        private Item CreateFireballWand()
        {
            Crossbow wand = new Crossbow();
            wand.Name = "Bond’s Revolver";
            wand.Hue = Utility.RandomMinMax(50, 250);
            wand.MinDamage = Utility.Random(30, 80);
            wand.MaxDamage = Utility.Random(80, 120);
            wand.Attributes.BonusStr = 10;
            wand.Attributes.SpellDamage = 5;
            wand.WeaponAttributes.HitLightning = 30;
            wand.SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
            return wand;
        }

        public AlliedForcesTreasureChest(Serial serial) : base(serial)
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
