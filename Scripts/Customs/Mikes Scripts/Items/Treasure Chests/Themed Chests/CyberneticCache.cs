using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CyberneticCache : WoodenChest
    {
        [Constructable]
        public CyberneticCache()
        {
            Name = "Cybernetic Cache";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Diamond>("Cybernetic Core", 2406), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Energy Drink", 2407), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Cybernetic Upgrade"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Circuit Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Amethyst>("Cybernetic Chip", 2425), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Virus Injector"), 0.08);
            AddItem(CreateGoldItem("Crypto Coin"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Runner", 2426), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Data Earring"), 0.20);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Cybernetic Eye"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Antidote"), 0.20);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreateLeggings(), 0.20);
            AddItem(CreateWarFork(), 0.20);
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
            note.NoteString = "We have hacked into the mainframe of the corporation.";
            note.TitleString = "Hacker’s Log";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Cyber Vault";
            map.Bounds = new Rectangle2D(2000, -200, 400, 400);
            map.NewPin = new Point2D(1800, -300);
            map.Protected = true;
            return map;
        }

        private Item CreateBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Arrowsmith's Sturdy Boots";
            boots.Hue = Utility.RandomMinMax(600, 1600);
            boots.Attributes.WeaponSpeed = 5;
            boots.Attributes.AttackChance = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Fletching, 20.0);
            return boots;
        }

        private Item CreateLeggings()
        {
            PlateLegs leggings = new PlateLegs();
            leggings.Name = "Frostwarden's PlateLegs";
            leggings.Hue = Utility.RandomMinMax(600, 650);
            leggings.BaseArmorRating = Utility.Random(48, 77);
            leggings.AbsorptionAttributes.EaterCold = 20;
            leggings.ArmorAttributes.LowerStatReq = 15;
            leggings.Attributes.BonusDex = 10;
            leggings.Attributes.Luck = 20;
            leggings.SkillBonuses.SetValues(0, SkillName.MagicResist, 15.0);
            leggings.ColdBonus = 25;
            leggings.EnergyBonus = 10;
            leggings.FireBonus = 0;
            leggings.PhysicalBonus = 15;
            leggings.PoisonBonus = 10;
            return leggings;
        }

        private Item CreateWarFork()
        {
            WarFork warFork = new WarFork();
            warFork.Name = "BlackTail Whip";
            warFork.Hue = Utility.RandomMinMax(600, 800);
            warFork.MinDamage = Utility.Random(20, 60);
            warFork.MaxDamage = Utility.Random(60, 85);
            warFork.Attributes.SpellDamage = 10;
            warFork.Attributes.RegenMana = 3;
            warFork.Slayer = SlayerName.DragonSlaying;
            warFork.Slayer2 = SlayerName.ElementalHealth;
            warFork.WeaponAttributes.HitFireball = 25;
            warFork.WeaponAttributes.HitPoisonArea = 20;
            warFork.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            warFork.SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
            return warFork;
        }

        public CyberneticCache(Serial serial) : base(serial)
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
