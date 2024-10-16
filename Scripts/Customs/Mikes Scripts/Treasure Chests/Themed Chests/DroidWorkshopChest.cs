using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DroidWorkshopChest : WoodenChest
    {
        [Constructable]
        public DroidWorkshopChest()
        {
            Name = "Droid Workshop";
            Hue = Utility.Random(1, 1650);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.04);
            AddItem(CreateLootItem<Shaft>("Lightsaber Repair Kit"), 0.32);
            AddItem(CreateNamedItem<TreasureLevel1>("R2-D2 Schematics"), 0.27);
            AddItem(CreateNamedItem<GoldEarrings>("C-3PO's Spare Part"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5300)), 0.17);
            AddItem(CreateLootItem<Ruby>("Tatooine Sunstone"), 0.19);
            AddItem(CreateNamedItem<MaxxiaScroll>("Blaster Repair Tool"), 0.23);
            AddItem(CreateNamedItem<Bag>("Droid Maintenance Credits"), 0.22);
            AddItem(CreateLootItem<BlackPearl>("Droid Power Source"), 1.0);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateClub(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateLootItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
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
            note.NoteString = "These are the droids you're looking for.";
            note.TitleString = "Droid Engineer's Manual";
            return note;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Midnight Reveler's Boots";
            boots.Hue = Utility.RandomMinMax(100, 900);
            boots.Attributes.BonusDex = 15;
            boots.Attributes.NightSight = 1;
            return boots;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Boots of Swiftness";
            legs.Hue = Utility.RandomMinMax(250, 650);
            legs.BaseArmorRating = Utility.RandomMinMax(25, 55);
            legs.ArmorAttributes.MageArmor = 1;
            legs.Attributes.BonusDex = 25;
            legs.Attributes.RegenStam = 7;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            legs.PhysicalBonus = 15;
            legs.EnergyBonus = 5;
            legs.FireBonus = 5;
            legs.PoisonBonus = 10;
            legs.ColdBonus = 10;
            return legs;
        }

        private Item CreateClub()
        {
            Club club = new Club();
            club.Name = "Potara Earring Club";
            club.Hue = Utility.RandomMinMax(300, 350);
            club.MinDamage = Utility.RandomMinMax(20, 60);
            club.MaxDamage = Utility.RandomMinMax(60, 100);
            club.Attributes.BonusInt = 15;
            club.Attributes.RegenHits = 3;
            club.Slayer = SlayerName.DaemonDismissal;
            club.WeaponAttributes.MageWeapon = 1;
            club.WeaponAttributes.HitMagicArrow = 20;
            club.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            return club;
        }

        public DroidWorkshopChest(Serial serial) : base(serial)
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
