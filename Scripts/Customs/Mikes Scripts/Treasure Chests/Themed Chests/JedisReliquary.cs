using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JedisReliquary : WoodenChest
    {
        [Constructable]
        public JedisReliquary()
        {
            Name = "Jedi's Reliquary";
            Hue = Utility.Random(1, 1900);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Crystal of the Force"), 0.24);
            AddItem(CreateLootItem<Shaft>("Lightsaber", 1100), 0.30);
            AddItem(CreateNamedItem<TreasureLevel2>("Holocron of Wisdom"), 0.26);
            AddItem(CreateNamedItem<GoldEarrings>("Padawan's Braid"), 0.37);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateLootItem<GreaterHealPotion>("Jedi Meditation Elixir", 2298), 0.18);
            AddItem(CreateArmor(), 0.22);
            AddItem(CreateLootItem<Lute>("Music of the Cantina", 2457), 1.0);
            AddItem(CreateSkullCap(), 0.2);
            AddItem(CreateMetalKiteShield(), 0.2);
            AddItem(CreateKatana(), 0.2);
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

        private Item CreateLootItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The force will be with you, always.";
            note.TitleString = "Jedi's Journal";
            return note;
        }

        private Item CreateArmor()
        {
            Robe armor = new Robe();
            armor.Name = "Robes of the Force";
            armor.ItemID = Utility.RandomList(3, 5);
            return armor;
        }

        private Item CreateSkullCap()
        {
            SkullCap cap = new SkullCap();
            cap.Name = "Reindeer Fur Cap";
            cap.Hue = Utility.RandomMinMax(45, 55);
            cap.ClothingAttributes.SelfRepair = 3;
            cap.Attributes.BonusStam = 10;
            cap.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            return cap;
        }

        private Item CreateMetalKiteShield()
        {
            MetalKiteShield shield = new MetalKiteShield();
            shield.Name = "Exodus Barrier";
            shield.Hue = Utility.RandomMinMax(333, 444);
            shield.BaseArmorRating = Utility.RandomMinMax(45, 75);
            shield.AbsorptionAttributes.ResonanceEnergy = 20;
            shield.ArmorAttributes.MageArmor = 1;
            shield.Attributes.BonusHits = 20;
            shield.Attributes.SpellChanneling = 1;
            shield.SkillBonuses.SetValues(0, SkillName.MagicResist, 25.0);
            shield.ColdBonus = 5;
            shield.EnergyBonus = 25;
            shield.FireBonus = 10;
            shield.PhysicalBonus = 10;
            shield.PoisonBonus = 10;
            return shield;
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Mablung's Defender";
            katana.Hue = Utility.RandomMinMax(600, 800);
            katana.MinDamage = Utility.RandomMinMax(20, 55);
            katana.MaxDamage = Utility.RandomMinMax(55, 85);
            katana.Attributes.DefendChance = 10;
            katana.Attributes.RegenMana = 3;
            katana.Slayer = SlayerName.ElementalHealth;
            katana.Slayer2 = SlayerName.Terathan;
            katana.WeaponAttributes.HitManaDrain = 30;
            katana.WeaponAttributes.ResistPhysicalBonus = 10;
            katana.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            katana.SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
            return katana;
        }

        public JedisReliquary(Serial serial) : base(serial)
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
