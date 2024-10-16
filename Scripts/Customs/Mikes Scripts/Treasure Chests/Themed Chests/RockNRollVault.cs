using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RockNRollVault : WoodenChest
    {
        [Constructable]
        public RockNRollVault()
        {
            Name = "Rock 'n' Roll Vault";
            Hue = Utility.Random(1, 1900);

            // Add items to the chest
            AddItem(CreateRandomInstrument(), 0.08);
            AddItem(CreateNamedItem<TreasureLevel2>("Golden Record"), 0.22);
            AddItem(CreateNamedItem<SilverNecklace>("Elvis's Pendant"), 0.35);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.12);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Classic Soda Pop"), 0.19);
            AddItem(CreateColoredItem<Shoes>("Blue Suede Shoes", 1600), 0.20);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreateHelm(), 0.20);
            AddItem(CreateHalberd(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateRandomInstrument()
        {
            Lute instrument = new Lute(); // Define this class or use an existing instrument class
            instrument.Name = "Vintage Guitar";
            return instrument;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt(); // Define this class or use an existing clothing class
            clothing.Name = "Retro Jacket";
            return clothing;
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
            note.NoteString = "Rock 'n' Roll will never die!";
            note.TitleString = "1950's Concert Poster";
            return note;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Tech Guru's Glasses";
            bandana.Hue = Utility.Random(1, 50);
            bandana.Attributes.BonusInt = 15;
            bandana.Attributes.LowerManaCost = 10;
            bandana.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            bandana.SkillBonuses.SetValues(1, SkillName.ItemID, 15.0);
            return bandana;
        }

        private Item CreateHelm()
        {
            NorseHelm helm = new NorseHelm();
            helm.Name = "Wrestler's Helm of Focus";
            helm.Hue = Utility.Random(10, 250);
            helm.BaseArmorRating = Utility.Random(40, 65);
            helm.ArmorAttributes.DurabilityBonus = 10;
            helm.Attributes.BonusInt = 10;
            helm.Attributes.RegenStam = 5;
            helm.SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
            helm.ColdBonus = 5;
            helm.EnergyBonus = 5;
            helm.FireBonus = 5;
            helm.PhysicalBonus = 15;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateHalberd()
        {
            Halberd halberd = new Halberd();
            halberd.Name = "Green Dragon Crescent Blade";
            halberd.Hue = Utility.Random(200, 300);
            halberd.MinDamage = Utility.Random(35, 75);
            halberd.MaxDamage = Utility.Random(75, 120);
            halberd.Attributes.BonusStr = 20;
            halberd.Attributes.RegenHits = 5;
            halberd.Slayer = SlayerName.DragonSlaying;
            halberd.WeaponAttributes.HitFireArea = 40;
            halberd.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return halberd;
        }

        public RockNRollVault(Serial serial) : base(serial)
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
