using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RadBoomboxTrove : WoodenChest
    {
        [Constructable]
        public RadBoomboxTrove()
        {
            Name = "Rad Boombox Trove";
            Hue = Utility.Random(1, 2040);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateNamedItem<Sapphire>("Mixtape Sapphire"), 0.24);
            AddItem(CreateNamedItem<GreaterHealPotion>("Pop Rock Potion"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel2>("Dance-Off Medal"), 0.27);
            AddItem(CreateNamedItem<GoldEarrings>("Disco Ball Earrings"), 0.32);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4200)), 0.16);
            AddItem(CreateInstrument(), 1.0);
            AddItem(CreateClothing(), 0.20);
            AddItem(CreateBuckler(), 0.20);
            AddItem(CreateScimitar(), 0.20);
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

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Break it down to the freshest beats!";
            note.TitleString = "Top Hits of the '80s";
            return note;
        }

        private Item CreateInstrument()
        {
            Lute instrument = new Lute(); // Assuming SynthOfTheStars is a class that inherits from BaseInstrument
            instrument.Name = "Synth of the Stars";
            return instrument;
        }

        private Item CreateClothing()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Pop Star's Fingerless Gloves";
            bandana.Hue = Utility.RandomMinMax(600, 1600);
            bandana.ClothingAttributes.SelfRepair = 4;
            bandana.Attributes.BonusDex = 20;
            bandana.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            return bandana;
        }

        private Item CreateBuckler()
        {
            Buckler buckler = new Buckler();
            buckler.Name = "Courtesan's Dainty Buckler";
            buckler.Hue = Utility.RandomMinMax(100, 500);
            buckler.BaseArmorRating = Utility.Random(20, 50);
            buckler.AbsorptionAttributes.ResonanceCold = 10;
            buckler.ArmorAttributes.SelfRepair = 3;
            buckler.Attributes.CastRecovery = 1;
            buckler.SkillBonuses.SetValues(0, SkillName.Focus, 10.0);
            buckler.ColdBonus = 15;
            buckler.EnergyBonus = 5;
            buckler.FireBonus = 5;
            buckler.PhysicalBonus = 5;
            buckler.PoisonBonus = 5;
            return buckler;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Cherub's Blade";
            scimitar.Hue = Utility.RandomMinMax(550, 750);
            scimitar.MinDamage = Utility.Random(25, 65);
            scimitar.MaxDamage = Utility.Random(65, 105);
            scimitar.Attributes.SpellDamage = 10;
            scimitar.Attributes.ReflectPhysical = 5;
            scimitar.Slayer = SlayerName.Exorcism;
            scimitar.WeaponAttributes.HitFireball = 30;
            scimitar.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            scimitar.SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
            return scimitar;
        }

        public RadBoomboxTrove(Serial serial) : base(serial)
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
