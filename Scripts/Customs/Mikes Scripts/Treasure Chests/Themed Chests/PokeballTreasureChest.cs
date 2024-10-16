using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PokeballTreasureChest : WoodenChest
    {
        [Constructable]
        public PokeballTreasureChest()
        {
            Name = "Pokéball Treasure Chest";
            Hue = 1151;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Pikachu's Spark", 103), 0.18);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Trainer's Secret Stash"), 0.12);
            AddItem(CreateNamedItem<GoldEarrings>("Jigglypuff's Lullaby Earring"), 0.12);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.16);
            AddItem(CreateNamedItem<Apple>("Bulbasaur's Berry"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Squirtle's Splash Brew"), 0.14);
            AddItem(CreateColoredItem<ThighBoots>("Ash's Journey Boots", 1140), 0.15);
            AddItem(CreateGem(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Trainer's Pokedex Spyglass"), 0.12);
            AddItem(CreateInstrument(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateWoodenKiteShield(), 0.20);
            AddItem(CreateBlackStaff(), 0.20);
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
            note.NoteString = "Catch 'em All!";
            note.TitleString = "Professor Oak's Note";
            return note;
        }

        private Item CreateGem()
        {
            Ruby gem = new Ruby(); // You might need to adjust this to the actual gem class
            gem.Name = "Charmander's Flame Gem";
            return gem;
        }

        private Item CreateInstrument()
        {
            Drums instrument = new Drums(); // Adjust as necessary
            instrument.Name = "Melody of Lapras";
            return instrument;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword(); // Use appropriate weapon class
            weapon.Name = "Sword of Farfetch'd";
            weapon.Hue = 1145;
            weapon.MaxDamage = Utility.Random(20, 60);
            return weapon;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Alchemist's Brewmaster Tunic";
            tunic.Hue = Utility.RandomMinMax(250, 1250);
            tunic.Attributes.BonusInt = 15;
            tunic.Attributes.EnhancePotions = 20;
            tunic.SkillBonuses.SetValues(0, SkillName.Alchemy, 25.0);
            return tunic;
        }

        private Item CreateWoodenKiteShield()
        {
            WoodenKiteShield shield = new WoodenKiteShield();
            shield.Name = "Harmony's Guard";
            shield.Hue = Utility.RandomMinMax(150, 550);
            shield.BaseArmorRating = Utility.Random(20, 55);
            shield.AbsorptionAttributes.EaterEnergy = 15;
            shield.ArmorAttributes.MageArmor = 1;
            shield.Attributes.BonusInt = 10;
            shield.Attributes.ReflectPhysical = 5;
            shield.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            shield.ColdBonus = 10;
            shield.EnergyBonus = 15;
            shield.FireBonus = 10;
            shield.PhysicalBonus = 10;
            shield.PoisonBonus = 5;
            return shield;
        }

        private Item CreateBlackStaff()
        {
            BlackStaff staff = new BlackStaff();
            staff.Name = "Wand of Woh";
            staff.Hue = Utility.RandomMinMax(200, 400);
            staff.MinDamage = Utility.Random(20, 60);
            staff.MaxDamage = Utility.Random(60, 100);
            staff.Attributes.SpellChanneling = 1;
            staff.Attributes.BonusMana = 10;
            staff.Slayer = SlayerName.ElementalBan;
            staff.WeaponAttributes.HitFireball = 30;
            staff.WeaponAttributes.HitMagicArrow = 20;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            return staff;
        }

        public PokeballTreasureChest(Serial serial) : base(serial)
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
