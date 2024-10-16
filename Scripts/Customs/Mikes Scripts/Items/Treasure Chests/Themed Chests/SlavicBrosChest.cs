using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SlavicBrosChest : WoodenChest
    {
        [Constructable]
        public SlavicBrosChest()
        {
            Name = "Slavic Legends Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateEmerald(), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel2>("Slavic Artifact"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Veles Charm Earring", 1165), 0.15);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.18);
            AddItem(CreateNamedItem<Apple>("Baba Yaga's Cursed Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Slavic Mead"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Rusalka", 1192), 0.19);
            AddItem(CreateRandomStatue(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Bard's Tale Viewer"), 0.12);
            AddItem(CreateRandomInstrument(), 0.14);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateShoes(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateLongsword(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Jewel of Perun";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Heed the tales of old Slavic lands.";
            note.TitleString = "Slavic Bard's Song";
            return note;
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

        private Item CreateRandomStatue()
        {
            Diamond statue = new Diamond();
            statue.Name = "Statue of Domovoi";
            return statue;
        }

        private Item CreateRandomInstrument()
        {
            Lute instrument = new Lute();
            instrument.Name = "Slavic Lute";
            return instrument;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Slavic Folk Garb";
            return clothing;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Sword of Slavic Heroes";
            weapon.Hue = 1157;
            weapon.MaxDamage = Utility.Random(25, 65);
            return weapon;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Seductress' Silken Shoes";
            shoes.Hue = Utility.RandomMinMax(400, 1400);
            shoes.Attributes.BonusDex = 5;
            shoes.Attributes.SpellChanneling = 1;
            shoes.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            return shoes;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Witch's Whispering Boots";
            legs.Hue = Utility.RandomMinMax(500, 750);
            legs.BaseArmorRating = Utility.Random(25, 55);
            legs.AbsorptionAttributes.EaterCold = 10;
            legs.Attributes.NightSight = 1;
            legs.Attributes.RegenMana = 5;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
            legs.SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 5;
            legs.FireBonus = 5;
            legs.PhysicalBonus = 15;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Ghoul Slayer's Longsword";
            longsword.Hue = Utility.RandomMinMax(500, 700);
            longsword.MinDamage = Utility.Random(20, 60);
            longsword.MaxDamage = Utility.Random(60, 90);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.RegenStam = 3;
            longsword.Slayer = SlayerName.OrcSlaying;
            longsword.Slayer2 = SlayerName.TrollSlaughter;
            longsword.WeaponAttributes.HitDispel = 20;
            longsword.WeaponAttributes.HitPoisonArea = 20;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            longsword.SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0);
            return longsword;
        }

        public SlavicBrosChest(Serial serial) : base(serial)
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
