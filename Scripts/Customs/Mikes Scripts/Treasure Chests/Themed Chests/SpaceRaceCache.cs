using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpaceRaceCache : WoodenChest
    {
        [Constructable]
        public SpaceRaceCache()
        {
            Name = "Space Race Cache";
            Hue = Utility.Random(1, 2200);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateColoredItem<Sapphire>("Moon Rock", 1889), 0.26);
            AddItem(CreateNamedItem<Apple>("Star Fruit"), 0.29);
            AddItem(CreateNamedItem<TreasureLevel4>("Astronaut's Helmet"), 0.22);
            AddItem(CreateNamedItem<SilverNecklace>("Orion's Belt"), 0.39);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Space Observer"), 0.19);
            AddItem(CreateShoes(), 0.20);
            AddItem(CreateMetalKiteShield(), 0.20);
            AddItem(CreateClub(), 0.20);
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
            note.NoteString = "One small step for man...";
            note.TitleString = "Astronaut's Log";
            return note;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Bobby Soxer's Shoes";
            shoes.Hue = Utility.RandomMinMax(1, 500);
            shoes.Attributes.BonusDex = 10;
            shoes.Attributes.RegenStam = 3;
            return shoes;
        }

        private Item CreateMetalKiteShield()
        {
            MetalKiteShield shield = new MetalKiteShield();
            shield.Name = "Hammerlord's Shield";
            shield.Hue = Utility.RandomMinMax(350, 650);
            shield.BaseArmorRating = Utility.Random(30, 70);
            shield.AbsorptionAttributes.EaterDamage = 10;
            shield.ArmorAttributes.ReactiveParalyze = 1;
            shield.Attributes.BonusDex = 10;
            shield.Attributes.ReflectPhysical = 5;
            shield.SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
            shield.PhysicalBonus = 25;
            shield.ColdBonus = 5;
            shield.EnergyBonus = 10;
            shield.FireBonus = 10;
            shield.PoisonBonus = 5;
            return shield;
        }

        private Item CreateClub()
        {
            Club club = new Club();
            club.Name = "Prohibition Club";
            club.Hue = Utility.RandomMinMax(50, 250);
            club.MinDamage = Utility.Random(10, 40);
            club.MaxDamage = Utility.Random(40, 70);
            club.Attributes.BonusDex = 5;
            club.Attributes.LowerManaCost = 10;
            club.Slayer = SlayerName.LizardmanSlaughter;
            club.WeaponAttributes.HitLowerAttack = 20;
            club.SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
            club.SkillBonuses.SetValues(1, SkillName.Stealing, 10.0);
            return club;
        }

        public SpaceRaceCache(Serial serial) : base(serial)
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
