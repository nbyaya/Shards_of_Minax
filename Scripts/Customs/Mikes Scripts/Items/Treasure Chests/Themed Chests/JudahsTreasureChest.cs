using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JudahsTreasureChest : WoodenChest
    {
        [Constructable]
        public JudahsTreasureChest()
        {
            Name = "Judah’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Shekel"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Kosher Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Judah’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of Judah"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Levites", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Kiddush Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Maccabee Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Patriarch", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of the Covenant"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Judah’s Prophetic Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Manna from Heaven"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateHat(), 0.30);
            AddItem(CreateGloves(), 0.30);
            AddItem(CreateBroadsword(), 0.30);
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
            note.NoteString = "I am Judah, son of Jacob and Leah, leader of the tribe of Judah, ancestor of King David and the Messiah";
            note.TitleString = "Judah’s Testament";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Judah’s Tomb";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Judah’s Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Judah’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateHat()
        {
            TallStrawHat hat = new TallStrawHat();
            hat.Name = "Hat of the Rabbi";
            hat.Hue = Utility.RandomMinMax(600, 1600);
            hat.ClothingAttributes.DurabilityBonus = 5;
            hat.Attributes.DefendChance = 10;
            hat.SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);
            return hat;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Gloves of the Scribe";
            gloves.Hue = Utility.RandomMinMax(1, 1000);
            gloves.BaseArmorRating = Utility.Random(60, 90);
            gloves.AbsorptionAttributes.EaterFire = 30;
            gloves.ArmorAttributes.LowerStatReq = 100;
            gloves.Attributes.BonusInt = 20;
            gloves.SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            return gloves;
        }

        private Item CreateBroadsword()
        {
            Broadsword broadsword = new Broadsword();
            broadsword.Name = "Sword of the Maccabees";
            broadsword.Hue = Utility.RandomMinMax(50, 250);
            broadsword.MinDamage = Utility.Random(30, 80);
            broadsword.MaxDamage = Utility.Random(80, 120);
            broadsword.Attributes.BonusStr = 10;
            return broadsword;
        }

        public JudahsTreasureChest(Serial serial) : base(serial)
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
