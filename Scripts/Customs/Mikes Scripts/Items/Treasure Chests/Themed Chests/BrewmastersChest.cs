using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BrewmastersChest : WoodenChest
    {
        [Constructable]
        public BrewmastersChest()
        {
            Name = "Brewmaster's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateDiamondItem("Diamond of the Draught"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Brewmaster’s Special", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Brewer’s Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Hops Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Red Ale", 2117), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Old Stout", 1109), 0.08);
            AddItem(CreateNamedItem<BarrelTap>("Golden Pint"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Sipper", 1153), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Barley Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<BarrelTap>("Brewmaster’s Tap"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Brew"), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateShoes(), 0.20);
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
            note.NoteString = "The secret to a good beer is in the yeast.";
            note.TitleString = "Brewmaster’s Notes";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Hidden Brewery";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Brewer’s Best";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Spellweaver's Enchanted Shoes";
            shoes.Hue = Utility.RandomMinMax(100, 1000);
            shoes.Attributes.LowerManaCost = 10;
            shoes.Attributes.SpellChanneling = 1;
            shoes.SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
            return shoes;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Gauntlets of Precision";
            gloves.Hue = Utility.RandomMinMax(100, 600);
            gloves.BaseArmorRating = Utility.Random(25, 55);
            gloves.Attributes.BonusDex = 15;
            gloves.Attributes.EnhancePotions = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Cooking, 10.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 10;
            gloves.FireBonus = 5;
            gloves.PhysicalBonus = 15;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateBlackStaff()
        {
            BlackStaff staff = new BlackStaff();
            staff.Name = "Staff of the Elements";
            staff.Hue = Utility.RandomMinMax(350, 550);
            staff.MinDamage = Utility.Random(15, 50);
            staff.MaxDamage = Utility.Random(50, 85);
            staff.Attributes.SpellChanneling = 1;
            staff.Attributes.BonusMana = 20;
            staff.Slayer = SlayerName.ElementalBan;
            staff.Slayer2 = SlayerName.ElementalHealth;
            staff.WeaponAttributes.HitFireArea = 20;
            staff.WeaponAttributes.HitColdArea = 20;
            return staff;
        }

        public BrewmastersChest(Serial serial) : base(serial)
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
