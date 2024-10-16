using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ClownsWhimsicalChest : WoodenChest
    {
        [Constructable]
        public ClownsWhimsicalChest()
        {
            Name = "Clown's Whimsical Chest";
            Hue = 1167;

            // Add items to the chest
            AddItem(CreateNamedItem<Emerald>("Clown's Emerald Eye"), 0.05);
            AddItem(CreateSimpleNote(), 0.18);
            AddItem(CreateNamedItem<TreasureLevel3>("Box of Tricks"), 0.16);
            AddItem(CreateNamedItem<GoldEarrings>("Rainbow Bubble Earring"), 0.15);
            AddItem(new Gold(Utility.Random(1, 4000)));
            AddItem(CreateNamedItem<Apple>("Juggling Apple"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Comedy Concoction"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Dancing Clown", 1925), 0.14);
            AddItem(CreateRandomInstrument("Comedy Drum"), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Clown's Viewing Glass"), 0.04);
            AddItem(CreateRandomWand("Wand of Whimsy"), 0.12);
            AddItem(CreateRandomClothing("Comedian's Outfit"), 0.13);
            AddItem(CreateBandana(), 1.0);
            AddItem(CreateShield(), 0.20);
            AddItem(CreateCleaver(), 0.20);
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
            note.NoteString = "The world is a circus and we are its performers.";
            note.TitleString = "Clown's Diary";
            return note;
        }

        private Item CreateRandomInstrument(string name)
        {
            BaseInstrument instrument = Loot.RandomInstrument();
            instrument.Name = name;
            return instrument;
        }

        private Item CreateRandomWand(string name)
        {
            BaseWand wand = Loot.RandomWand();
            wand.Name = name;
            return wand;
        }

        private Item CreateRandomClothing(string name)
        {
            BaseClothing clothing = Loot.RandomClothing();
            clothing.Name = name;
            return clothing;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Blacksmith's Reinforced Gloves";
            bandana.Hue = Utility.RandomMinMax(500, 1500);
            bandana.ClothingAttributes.SelfRepair = 4;
            bandana.Attributes.BonusDex = 15;
            bandana.Attributes.WeaponDamage = 5;
            bandana.SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
            return bandana;
        }

        private Item CreateShield()
        {
            HeaterShield shield = new HeaterShield();
            shield.Name = "Solaris Aegis";
            shield.Hue = Utility.RandomMinMax(1, 250);
            shield.BaseArmorRating = Utility.Random(40, 90);
            shield.AbsorptionAttributes.EaterFire = 40;
            shield.Attributes.RegenHits = 5;
            shield.PhysicalBonus = 10;
            shield.PoisonBonus = 10;
            return shield;
        }

        private Item CreateCleaver()
        {
            Cleaver cleaver = new Cleaver();
            cleaver.Name = "The Butcher's Cleaver";
            cleaver.Hue = Utility.RandomMinMax(50, 100);
            cleaver.MinDamage = Utility.Random(40, 70);
            cleaver.MaxDamage = Utility.Random(70, 100);
            cleaver.Attributes.AttackChance = 15;
            cleaver.WeaponAttributes.BloodDrinker = 25;
            cleaver.WeaponAttributes.HitFireArea = 20;
            return cleaver;
        }

        public ClownsWhimsicalChest(Serial serial) : base(serial)
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
