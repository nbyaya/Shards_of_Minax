using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MagesRelicChest : WoodenChest
    {
        [Constructable]
        public MagesRelicChest()
        {
            Name = "Mage's Relic Chest";
            Movable = false;
            Hue = Utility.Random(1, 500);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.2);
            AddItem(CreateNamedItem<Emerald>("Crystal of Mana"), 0.15);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Arcane Elixir"), 0.12);
            AddItem(CreateNamedItem<Spyglass>("Mage's Looking Glass"), 0.10);
            AddItem(CreateNamedItem<Spellbook>("Ancient Grimoire"), 0.15);
            AddItem(CreateNamedItem<SilverNecklace>("Amulet of the Planes"), 0.16);
            AddItem(CreateNamedItem<GoldEarrings>("Starlight Earrings"), 0.19);
            AddItem(CreateNamedItem<Spellbook>("Book of Shadows"), 0.10);
            AddItem(CreateNamedItem<Bag>("Enchanted Component Bag"), 0.17);
            AddItem(CreateSimpleNote(), 0.05);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.14);
            AddItem(CreateNamedItem<Clock>("Temporal Manipulator Clock"), 0.13);
            AddItem(new Gold(Utility.Random(1, 5000)), 1.0);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateArmor(), 0.20);
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

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Seek the power within and harness the arcane.";
            note.TitleString = "Mage's Wisdom Note";
            return note;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Necromancer's Cape";
            cloak.Hue = Utility.RandomMinMax(500, 1500);
            cloak.ClothingAttributes.SelfRepair = 4;
            cloak.Attributes.BonusInt = 20;
            cloak.Attributes.LowerManaCost = 10;
            cloak.SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
            cloak.SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 20.0);
            return cloak;
        }

        private Item CreateArmor()
        {
            PlateChest armor = new PlateChest();
            armor.Name = "Mystic Seer's Plate";
            armor.Hue = Utility.RandomMinMax(1, 1000);
            armor.BaseArmorRating = Utility.Random(30, 80);
            armor.AbsorptionAttributes.EaterEnergy = 30;
            armor.ArmorAttributes.MageArmor = 1;
            armor.Attributes.BonusMana = 50;
            armor.Attributes.RegenMana = 5;
            armor.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            armor.SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
            armor.ColdBonus = 10;
            armor.EnergyBonus = 10;
            armor.FireBonus = 10;
            armor.PhysicalBonus = 10;
            armor.PoisonBonus = 10;
            return armor;
        }

        private Item CreateCleaver()
        {
            Cleaver cleaver = new Cleaver();
            cleaver.Name = "Frostfire Cleaver";
            cleaver.Hue = Utility.RandomMinMax(1, 1000);
            cleaver.MinDamage = Utility.Random(10, 60);
            cleaver.MaxDamage = Utility.Random(60, 110);
            cleaver.Attributes.BonusStr = 15;
            cleaver.Attributes.LowerManaCost = 10;
            cleaver.Slayer = SlayerName.FlameDousing;
            cleaver.Slayer2 = SlayerName.WaterDissipation;
            cleaver.WeaponAttributes.HitFireArea = 30;
            cleaver.WeaponAttributes.HitColdArea = 30;
            cleaver.SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
            return cleaver;
        }

        public MagesRelicChest(Serial serial) : base(serial)
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
