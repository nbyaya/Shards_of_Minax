using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class FurTradersChest : WoodenChest
    {
        [Constructable]
        public FurTradersChest()
        {
            Name = "Fur Trader’s Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Sapphire>("Sapphire of the Hudson"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("French Brandy", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Fur Trader’s Bounty"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Beaver Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the North", 1775), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Aged Maple Syrup", 1486), 0.08);
            AddItem(CreateGoldItem("French Louis d’or"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Explorer", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Fleur-de-lis Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Radisson’s Trusted Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Brew"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreatePlateHelm(), 0.20);
            AddItem(CreateBow(), 0.20);
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
            note.NoteString = "We have made a good deal with the natives today!";
            note.TitleString = "Pierre Radisson’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Radisson’s Secret Cache";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Bow());
            weapon.Name = "Coureur de Bois";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Fur Trader’s Best";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Hippie Peace Bandana";
            bandana.Hue = Utility.RandomMinMax(300, 1300);
            bandana.Attributes.BonusMana = 10;
            bandana.Attributes.LowerRegCost = 5;
            bandana.SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
            return bandana;
        }

        private Item CreatePlateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Courtesan's Graceful Helm";
            helm.Hue = Utility.RandomMinMax(100, 500);
            helm.BaseArmorRating = Utility.Random(25, 55);
            helm.AbsorptionAttributes.EaterFire = 10;
            helm.ArmorAttributes.SelfRepair = 3;
            helm.Attributes.BonusInt = 15;
            helm.SkillBonuses.SetValues(0, SkillName.MagicResist, 10.0);
            helm.ColdBonus = 5;
            helm.EnergyBonus = 5;
            helm.FireBonus = 10;
            helm.PhysicalBonus = 5;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "David's Sling";
            bow.Hue = Utility.RandomMinMax(400, 600);
            bow.MinDamage = Utility.Random(20, 50);
            bow.MaxDamage = Utility.Random(80, 120);
            bow.Attributes.Luck = 200;
            bow.Attributes.BonusDex = 10;
            bow.WeaponAttributes.HitMagicArrow = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return bow;
        }

        public FurTradersChest(Serial serial) : base(serial)
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
