using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BritainsRoyalTreasuryChest : WoodenChest
    {
        [Constructable]
        public BritainsRoyalTreasuryChest()
        {
            Name = "Britain's Royal Treasury Chest";
            Hue = 1105;

            // Add items to the chest
            AddItem(CreateSapphire(), 0.20);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel4>("Royal Treasury Box"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Lady's Regal Earring", 1130), 0.15);
            AddItem(new Gold(Utility.Random(1, 7000)), 0.18);
            AddItem(CreateNamedItem<Apple>("Royal Gala Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("King's Festive Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Courtier", 1140), 0.15);
            AddItem(CreateRandomGem(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Seer of the Realm"), 0.12);
            AddItem(CreateRandomInstrument(), 0.13);
            AddItem(CreateRandomClothing(), 0.19);
            AddItem(CreateWoodenShield(), 0.20);
            AddItem(CreateKatana(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSapphire()
        {
            Sapphire sapphire = new Sapphire();
            sapphire.Name = "Crown Jewel of Britain";
            return sapphire;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "In the heart of Britannia, Britain stands tall and proud.";
            note.TitleString = "Lord British's Proclamation";
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

        private Item CreateRandomGem()
        {
            // Example: Random gem could be a Sapphire, Ruby, or Emerald
            Item gem = new Sapphire();
            gem.Name = "Britannian Diamond"; // Override with a more specific name if needed
            return gem;
        }

        private Item CreateRandomInstrument()
        {
            // Example: Random instrument could be a Lute or a Drum
            Item instrument = new Drums();
            instrument.Name = "Harmony of the Castle"; // Override with a more specific name if needed
            return instrument;
        }

        private Item CreateRandomClothing()
        {
            // Example: Random clothing could be a Tunic or a Robe
            Item clothing = new Tunic();
            clothing.Name = "Royal Tunic"; // Override with a more specific name if needed
            return clothing;
        }

        private Item CreateWoodenShield()
        {
            WoodenShield shield = new WoodenShield();
            shield.Name = "Frostwarden's Wooden Shield";
            shield.Hue = Utility.RandomMinMax(600, 650);
            shield.BaseArmorRating = Utility.RandomMinMax(43, 73);
            shield.AbsorptionAttributes.ResonanceCold = 25;
            shield.ArmorAttributes.ReactiveParalyze = 1;
            shield.Attributes.DefendChance = 15;
            shield.Attributes.ReflectPhysical = 10;
            shield.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            shield.ColdBonus = 25;
            shield.EnergyBonus = 5;
            shield.FireBonus = 0;
            shield.PhysicalBonus = 20;
            shield.PoisonBonus = 5;
            return shield;
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Masamune's Edge";
            katana.Hue = Utility.RandomMinMax(910, 930);
            katana.MinDamage = Utility.RandomMinMax(35, 55);
            katana.MaxDamage = Utility.RandomMinMax(85, 105);
            katana.Attributes.BonusDex = 10;
            katana.Attributes.AttackChance = 10;
            katana.Slayer = SlayerName.OrcSlaying;
            katana.WeaponAttributes.HitLeechMana = 20;
            katana.WeaponAttributes.MageWeapon = 1;
            katana.SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
            return katana;
        }

        public BritainsRoyalTreasuryChest(Serial serial) : base(serial)
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
