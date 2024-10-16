using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JukeboxJewels : WoodenChest
    {
        [Constructable]
        public JukeboxJewels()
        {
            Name = "Jukebox Jewels";
            Hue = Utility.Random(1, 1940);

            // Add items to the chest
            AddItem(CreateLootItem<Lute>("Mini Jukebox"), 0.08);
            AddItem(CreateNamedItem<TreasureLevel2>("Vintage Radio"), 0.23);
            AddItem(CreateNamedItem<SilverNecklace>("Dancer's Charm"), 0.24);
            AddItem(CreateSimpleNote(), 0.38);
            AddItem(new Gold(Utility.Random(1, 4600)), 0.15);
            AddItem(CreateLootItem<Ruby>("Retro Ruby"), 0.18);
            AddItem(CreateLootItem<GraveDust>("Dancer's Dust"), 0.18);
            AddItem(CreateCap(), 0.20);
            AddItem(CreatePlateLegs(), 0.20);
            AddItem(CreateHeavyCrossbow(), 0.20);
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
            note.NoteString = "The hits keep on coming!";
            note.TitleString = "Top 50's Chart";
            return note;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "War Hero's Cap";
            cap.Hue = Utility.RandomMinMax(600, 1600);
            cap.Attributes.BonusStr = 10;
            cap.Attributes.DefendChance = 15;
            cap.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            return cap;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs legs = new PlateLegs();
            legs.Name = "Wrestler's Leggings of Balance";
            legs.Hue = Utility.RandomMinMax(10, 250);
            legs.BaseArmorRating = Utility.RandomMinMax(45, 65);
            legs.ArmorAttributes.LowerStatReq = 10;
            legs.Attributes.BonusStam = 15;
            legs.Attributes.RegenMana = 5;
            legs.SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 20;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateHeavyCrossbow()
        {
            HeavyCrossbow crossbow = new HeavyCrossbow();
            crossbow.Name = "Chu Ko Nu";
            crossbow.Hue = Utility.RandomMinMax(400, 600);
            crossbow.MinDamage = Utility.RandomMinMax(20, 50);
            crossbow.MaxDamage = Utility.RandomMinMax(50, 90);
            crossbow.Attributes.LowerRegCost = 20;
            crossbow.Attributes.AttackChance = 5;
            crossbow.Slayer = SlayerName.ReptilianDeath;
            crossbow.WeaponAttributes.HitPhysicalArea = 20;
            crossbow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return crossbow;
        }

        public JukeboxJewels(Serial serial) : base(serial)
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
