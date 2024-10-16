using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class KingKamehamehaTreasure : WoodenChest
    {
        [Constructable]
        public KingKamehamehaTreasure()
        {
            Name = "King Kamehameha’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateDiamondItem("Diamond of the King"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Royal Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("King’s Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Unity"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Fire Goddess", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Royal Wine"), 0.08);
            AddItem(CreateColoredItem<Gold>("Golden Feather", 2213), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Warrior King", 1153), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Power"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("King’s Navigational Tool"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Nectar"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateLeatherGloves(), 0.20);
            AddItem(CreateBow(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateDiamondItem(string name)
        {
            Diamond diamond = new Diamond();
            diamond.Name = name;
            return diamond;
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
            note.NoteString = "I have conquered all the islands and unified them under my rule.";
            note.TitleString = "King Kamehameha’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to King Kamehameha’s Secret Vault";
            map.Bounds = new Rectangle2D(5000, 5000, 400, 400);
            map.NewPin = new Point2D(5100, 5150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Kaʻahumanu";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Kamehameha’s Pride";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Mod-Style Tunic";
            tunic.Hue = Utility.RandomMinMax(250, 750);
            tunic.Attributes.BonusDex = 10;
            tunic.Attributes.CastSpeed = 1;
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            return tunic;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Alchemist's Precise Gloves";
            gloves.Hue = Utility.RandomMinMax(500, 750);
            gloves.BaseArmorRating = Utility.Random(20, 55);
            gloves.AbsorptionAttributes.ResonanceEnergy = 10;
            gloves.ArmorAttributes.DurabilityBonus = 25;
            gloves.Attributes.BonusDex = 15;
            gloves.Attributes.LowerManaCost = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            gloves.ColdBonus = 10;
            gloves.EnergyBonus = 15;
            gloves.FireBonus = 5;
            gloves.PhysicalBonus = 5;
            gloves.PoisonBonus = 10;
            return gloves;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Bowsprit of Bluenose";
            bow.Hue = Utility.RandomMinMax(350, 550);
            bow.MinDamage = Utility.Random(20, 60);
            bow.MaxDamage = Utility.Random(60, 85);
            bow.Attributes.Luck = 100;
            bow.Attributes.AttackChance = 5;
            bow.Slayer = SlayerName.WaterDissipation;
            bow.WeaponAttributes.HitColdArea = 20;
            bow.WeaponAttributes.HitLightning = 15;
            bow.SkillBonuses.SetValues(0, SkillName.Cartography, 20.0);
            return bow;
        }

        public KingKamehamehaTreasure(Serial serial) : base(serial)
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
