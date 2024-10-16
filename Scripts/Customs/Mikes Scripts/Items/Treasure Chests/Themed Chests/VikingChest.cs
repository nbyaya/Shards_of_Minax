using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class VikingChest : WoodenChest
    {
        [Constructable]
        public VikingChest()
        {
            Name = "Viking Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Axe>("Viking Axe"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Viking Mead"), 0.10);
            AddItem(CreateNamedItem<TreasureLevel4>("Viking Hoard"), 0.20);
            AddItem(CreateNamedItem<GoldRing>("Golden Rune Ring"), 0.25);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(100, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the North", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Viking Mead"), 0.08);
            AddItem(CreateNamedItem<Gold>("Viking Coin"), 0.16);
            AddItem(CreateColoredItem<FurBoots>("Boots of the Berserker", 2301), 0.20);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Thor’s Hammer Earring"), 0.18);
            AddItem(CreateMap(), 0.10);
            AddItem(CreateNamedItem<GreaterStrengthPotion>("Bottle of Mighty Brew"), 0.15);
            AddItem(CreateColoredItem<FullApron>("Sawyer's Mighty Apron", Utility.RandomMinMax(730, 1690)), 0.20);
            AddItem(CreateHeaterShield(), 0.20);
            AddItem(CreateMaul(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The glory of Valhalla awaits the brave.";
            note.TitleString = "Viking Saga";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Viking Shipwreck";
            map.Bounds = new Rectangle2D(3000, 3000, 400, 400); // Adjusted bounds as needed
            return map;
        }

        private Item CreateHeaterShield()
        {
            HeaterShield shield = new HeaterShield();
            shield.Name = "Lioneye's Remorse";
            shield.Hue = Utility.RandomMinMax(200, 500);
            shield.BaseArmorRating = Utility.Random(40, 85);
            shield.Attributes.BonusHits = 50;
            shield.ArmorAttributes.DurabilityBonus = 20;
            shield.Attributes.ReflectPhysical = 10;
            shield.ColdBonus = 10;
            shield.EnergyBonus = 10;
            shield.FireBonus = 10;
            shield.PhysicalBonus = 15;
            shield.PoisonBonus = 10;
            return shield;
        }

        private Item CreateMaul()
        {
            Maul maul = new Maul();
            maul.Name = "Bonehew";
            maul.Hue = Utility.RandomMinMax(400, 600);
            maul.MinDamage = Utility.Random(35, 80);
            maul.MaxDamage = Utility.Random(80, 120);
            maul.Attributes.DefendChance = 5;
            maul.WeaponAttributes.HitHarm = 25;
            maul.WeaponAttributes.HitPoisonArea = 20;
            maul.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return maul;
        }

        public VikingChest(Serial serial) : base(serial)
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
