using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class NavyCaptainsChest : WoodenChest
    {
        [Constructable]
        public NavyCaptainsChest()
        {
            Name = "Navy Captain’s Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Fleet", 2551), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Navy Captain’s Rum", 1109), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Navy Captain’s Prize"), 0.10);
            AddItem(CreateNamedItem<SilverRing>("Silver Anchor Ring"), 0.10);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Sky", 1102), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Navy Captain’s Grog", 1109), 0.08);
            AddItem(CreateColoredItem<Gold>("Navy Medal", 1109), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Fleet", 1109), 0.15);
            AddItem(CreateColoredItem<GoldNecklace>("Golden Compass Necklace", 1109), 0.20);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateColoredItem<Sextant>("Navy Captain’s Sextant", 1109), 0.15);
            AddItem(CreateColoredItem<GreaterCurePotion>("Bottle of Antidote", 1109), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateLeatherGloves(), 0.20);
            AddItem(CreateBlackStaff(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "I have been promoted to Admiral today!";
            note.TitleString = "Navy Captain’s Log";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Navy Captain’s Secret Base";
            map.Bounds = new Rectangle2D(2000, 2200, 300, 300);
            map.NewPin = new Point2D(2100, 2150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Cutlass();
            weapon.Name = "Navy Captain’s Cutlass";
            weapon.Hue = Utility.RandomList(1, 1788, 1109);
            weapon.MaxDamage = Utility.Random(30, 70);
            weapon.MinDamage = Utility.Random(20, 50);
            weapon.SkillBonuses.SetValues(0, SkillName.Swords, 50.0);
            weapon.Attributes.SpellDamage = 10;
            weapon.WeaponAttributes.HitFireArea = 10;
            weapon.WeaponAttributes.HitFireball = 10;
            weapon.WeaponAttributes.HitLowerAttack = 10;
            weapon.WeaponAttributes.HitLowerDefend = 10;
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = new LeatherChest();
            armor.Name = "Navy Captain’s Uniform";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Ambassador's Cloak";
            cloak.Hue = Utility.RandomMinMax(50, 950);
            cloak.Attributes.BonusStr = 10;
            cloak.Attributes.CastSpeed = 1;
            cloak.SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
            cloak.SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
            return cloak;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Witch's Binding Gloves";
            gloves.Hue = Utility.RandomMinMax(500, 750);
            gloves.BaseArmorRating = Utility.Random(20, 50);
            gloves.AbsorptionAttributes.ResonanceCold = 15;
            gloves.Attributes.EnhancePotions = 20;
            gloves.Attributes.LowerRegCost = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            gloves.ColdBonus = 15;
            gloves.EnergyBonus = 5;
            gloves.FireBonus = 5;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 10;
            return gloves;
        }

        private Item CreateBlackStaff()
        {
            BlackStaff staff = new BlackStaff();
            staff.Name = "Power's Beacon";
            staff.Hue = Utility.RandomMinMax(10, 150);
            staff.MinDamage = Utility.Random(15, 55);
            staff.MaxDamage = Utility.Random(55, 85);
            staff.Attributes.BonusMana = 20;
            staff.Attributes.SpellDamage = 10;
            staff.Slayer = SlayerName.ElementalBan;
            staff.WeaponAttributes.MageWeapon = 1;
            staff.WeaponAttributes.HitDispel = 20;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            staff.SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
            return staff;
        }

        public NavyCaptainsChest(Serial serial) : base(serial)
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
