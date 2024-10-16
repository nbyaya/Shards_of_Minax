using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JestersGigglingChest : WoodenChest
    {
        [Constructable]
        public JestersGigglingChest()
        {
            Name = "Jester's Giggling Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateNamedItem<Ruby>("Jester's Ruby Nose"), 0.05);
            AddItem(CreateSimpleNote(), 0.20);
            AddItem(CreateNamedItem<TreasureLevel2>("Circus Surprise Box"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Clown's Golden Balloon Earring"), 0.15);
            AddItem(new Gold(Utility.Random(1, 3500)), 1.0);
            AddItem(CreateNamedItem<Apple>("Colorful Candy Apple"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Jester's Fizz"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Balancing Act", 1912), 0.12);
            AddItem(CreateRandomInstrument("Clown's Whistle"), 0.16);
            AddItem(CreateNamedItem<Spyglass>("Circus Spotter's Monocle"), 0.04);
            AddItem(CreateRandomClothing("Jester's Vibrant Outfit"), 0.12);
            AddItem(CreateJesterDagger(), 0.15);
            AddItem(CreateForgeMastersBoots(), 0.20);
            AddItem(CreateWinddancerBoots(), 0.20);
            AddItem(CreateKingsSword(), 0.20);
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
            note.NoteString = "Laughter is the best kind of magic.";
            note.TitleString = "Jester's Memoir";
            return note;
        }

        private Item CreateRandomInstrument(string name)
        {
            BaseInstrument instrument = Loot.RandomInstrument();
            instrument.Name = name;
            return instrument;
        }

        private Item CreateRandomClothing(string name)
        {
            Item clothing = Loot.RandomClothing();
            clothing.Name = name;
            return clothing;
        }

        private Item CreateJesterDagger()
        {
            BaseWeapon dagger = new Dagger();
            dagger.Name = "Jester's Playful Dagger";
            dagger.Hue = Utility.RandomList(1, 1910);
            dagger.MaxDamage = Utility.Random(20, 50);
            return dagger;
        }

        private Item CreateForgeMastersBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Forge Master's Boots";
            boots.Hue = Utility.RandomMinMax(600, 1650);
            boots.ClothingAttributes.LowerStatReq = 3;
            boots.Attributes.BonusDex = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
            return boots;
        }

        private Item CreateWinddancerBoots()
        {
            LeatherLegs boots = new LeatherLegs();
            boots.Name = "Winddancer Boots";
            boots.Hue = Utility.RandomMinMax(500, 1000);
            boots.BaseArmorRating = Utility.Random(25, 70);
            boots.Attributes.DefendChance = 40;
            boots.Attributes.AttackChance = -20;
            boots.Attributes.RegenStam = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            boots.ColdBonus = 10;
            boots.EnergyBonus = 10;
            boots.FireBonus = 10;
            boots.PhysicalBonus = 10;
            boots.PoisonBonus = 10;
            return boots;
        }

        private Item CreateKingsSword()
        {
            Longsword sword = new Longsword();
            sword.Name = "King's Sword of Haste";
            sword.Hue = Utility.RandomMinMax(550, 750);
            sword.MinDamage = Utility.Random(25, 60);
            sword.MaxDamage = Utility.Random(60, 90);
            sword.Attributes.BonusStr = 20;
            sword.Attributes.WeaponSpeed = 10;
            sword.WeaponAttributes.SelfRepair = 3;
            sword.WeaponAttributes.HitLightning = 20;
            sword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return sword;
        }

        public JestersGigglingChest(Serial serial) : base(serial)
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
