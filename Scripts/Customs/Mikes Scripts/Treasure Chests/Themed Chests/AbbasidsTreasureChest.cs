using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AbbasidsTreasureChest : WoodenChest
    {
        [Constructable]
        public AbbasidsTreasureChest()
        {
            Name = "Abbasid’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Dinar"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Arabian Coffee", 1172), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Abbasid’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of Harun al-Rashid"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Abbasids", 2126), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Mint Tea"), 0.08);
            AddItem(CreateGoldItem("Golden Dirham"), 0.16);
            AddItem(CreateColoredItem<Robe>("Robe of the Caliph", Utility.RandomMinMax(600, 1600)), 0.19);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Necklace of al-Ma’mun"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Abbasid’s Astronomical Sextant"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Islamic Medicine"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTurban(), 0.20);
            AddItem(CreateLeatherGloves(), 0.20);
            AddItem(CreateBroadsword(), 0.20);
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
            note.NoteString = "There is no god but Allah and Muhammad is his messenger";
            note.TitleString = "Shahada";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the House of Wisdom";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Scimitar());
            weapon.Name = "Abbasid’s Scimitar";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateLegs(), new PlateArms(), new PlateGloves());
            armor.Name = "Abbasid’s Plate";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTurban()
        {
            Cap turban = new Cap();
            turban.Name = "Turban of the Scholar";
            turban.Hue = Utility.RandomMinMax(1, 1000);
            turban.ClothingAttributes.DurabilityBonus = 5;
            turban.SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            return turban;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Gloves of the Translator";
            gloves.Hue = Utility.RandomMinMax(600, 1600);
            gloves.BaseArmorRating = Utility.Random(60, 90);
            gloves.AbsorptionAttributes.EaterEnergy = 30;
            gloves.SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
            return gloves;
        }

        private Item CreateBroadsword()
        {
            VikingSword broadsword = new VikingSword();
            broadsword.Name = "Sword of Saladin";
            broadsword.Hue = Utility.RandomMinMax(50, 250);
            broadsword.MinDamage = Utility.Random(30, 80);
            broadsword.MaxDamage = Utility.Random(80, 120);
            broadsword.Attributes.BonusDex = 10;
            broadsword.Attributes.SpellDamage = 5;
            return broadsword;
        }

        public AbbasidsTreasureChest(Serial serial) : base(serial)
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
