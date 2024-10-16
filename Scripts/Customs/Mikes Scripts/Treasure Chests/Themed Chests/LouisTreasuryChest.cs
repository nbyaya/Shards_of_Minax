using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class LouisTreasuryChest : WoodenChest
    {
        [Constructable]
        public LouisTreasuryChest()
        {
            Name = "Louis' Treasury";
            Hue = Utility.Random(1, 1890);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("French Francs"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Bottle of Fine Bordeaux", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Napoleon's Stash"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Marie Antoinette's Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("Classic Champagne"), 0.08);
            AddItem(CreateGoldItem("Royal Louis D'or"), 0.16);
            AddItem(CreateColoredItem<ElegantArmoire>("Versailles Armoire", 1180), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Fleur-de-lis Signet Ring"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Explorer's Telescope of Cartier"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of French Valor"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateBasket(), 0.30);
            AddItem(CreateRobe(), 0.30);
            AddItem(CreateLongsword(), 0.30);
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
            note.NoteString = "In honor of the French Monarchy and the spirit of the revolution";
            note.TitleString = "Memories of France";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Louvre";
            map.Bounds = new Rectangle2D(2300, 2300, 700, 700);
            map.NewPin = new Point2D(2500, 2500);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Musketeer's Rapier";
            weapon.Hue = Utility.RandomList(1, 1890);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "French Royal Cuirass";
            armor.Hue = Utility.RandomList(1, 1890);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBasket()
        {
            StrawHat basket = new StrawHat();
            basket.Name = "Parisian Bread Basket";
            basket.Hue = Utility.RandomMinMax(620, 1680);
            basket.ClothingAttributes.DurabilityBonus = 5;
            basket.Attributes.DefendChance = 10;
            basket.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            return basket;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Robe of Joan of Arc";
            robe.Hue = Utility.RandomMinMax(1, 1100);
            robe.Attributes.BonusDex = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return robe;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Blade of Charlemagne";
            longsword.Hue = Utility.RandomMinMax(60, 270);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitLightning = 30;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return longsword;
        }

        public LouisTreasuryChest(Serial serial) : base(serial)
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
