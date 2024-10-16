using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AnglersBounty : WoodenChest
    {
        [Constructable]
        public AnglersBounty()
        {
            Name = "Angler's Bounty";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateGoldItem("Fisherman's Gold"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Ancient Brine", 1159), 0.14);
            AddItem(CreateNamedItem<TreasureLevel2>("Deep Sea Hoard"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of the Mariner"), 0.52);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.17);
            AddItem(CreateColoredItem<Painting4NorthArtifact>("Painting of the Mystic Sea", 2119), 0.11);
            AddItem(CreateNamedItem<GreaterHealPotion>("Fisherman's Rum"), 0.09);
            AddItem(CreateGoldItem("Oceanic Coin"), 0.15);
            AddItem(CreateColoredItem<Spyglass>("Angler's Insight", 1177), 0.18);
            AddItem(CreateNamedItem<GoldRing>("Ring of the Deep"), 0.16);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Fisherman's Farsight"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of the Tides"), 0.21);
            AddItem(CreateWeapon(), 0.19);
            AddItem(CreateArmor(), 0.29);
            AddItem(CreateFishingPole(), 0.31);
            AddItem(CreateSextant(), 0.28);
            AddItem(CreateFroe(), 0.28);
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
            note.NoteString = "Cast your line, embrace the sea, and find treasures that are meant to be!";
            note.TitleString = "Sea Legends";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Legendary Fishing Spot";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Pitchfork weapon = new Pitchfork();
            weapon.Name = "Trident of the Tides";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherLegs(), new LeatherArms(), new LeatherGloves());
            armor.Name = "Vest of the Voyager";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFishingPole()
        {
            Boots pole = new Boots();
            pole.Name = "Mystic Fishing Boots";
            pole.Hue = Utility.RandomMinMax(600, 1600);
            pole.SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            return pole;
        }

        private Item CreateSextant()
        {
            LeatherLegs sextant = new LeatherLegs();
            sextant.Name = "Ancient Mariner's Legs";
            sextant.Hue = Utility.RandomMinMax(1, 1000);
            sextant.BaseArmorRating = Utility.Random(60, 90);
            sextant.AbsorptionAttributes.EaterEnergy = 30;
            sextant.ArmorAttributes.LowerStatReq = 1;
            sextant.Attributes.BonusDex = 20;
            sextant.Attributes.AttackChance = 10;
            sextant.SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            return sextant;
        }

        private Item CreateFroe()
        {
            Kryss froe = new Kryss();
            froe.Name = "Fish Filleting Knife";
            froe.Hue = Utility.RandomMinMax(50, 250);
            froe.MinDamage = Utility.Random(30, 80);
            froe.MaxDamage = Utility.Random(80, 120);
            froe.Attributes.BonusDex = 10;
            froe.Attributes.Luck = 20;
            froe.WeaponAttributes.HitLeechStam = 30;
            froe.WeaponAttributes.SelfRepair = 5;
            froe.SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
            return froe;
        }

        public AnglersBounty(Serial serial) : base(serial)
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
