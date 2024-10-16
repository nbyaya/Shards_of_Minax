using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MagesArcaneChest : WoodenChest
    {
        [Constructable]
        public MagesArcaneChest()
        {
            Name = "Mage's Arcane Chest";
            Hue = 1109;

            // Add items to the chest
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Arcane Box of Components"), 0.16);
            AddItem(CreateColoredItem<GoldEarrings>("Earring of the Spellweaver", 1300), 0.17);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.17);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Mana Regeneration"), 0.13);
            AddItem(CreateArmor(), 0.12);
            AddItem(CreateLeatherGloves(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
            AddItem(CreateWarHammer(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Magic is not to be trifled with.";
            note.TitleString = "Archmage's Note";
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

        private Item CreateRandomLoot<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = new LeatherChest();
            armor.Name = "Robes of the Archmage";
            armor.Hue = Utility.Random(1, 1322);
            armor.BaseArmorRating = Utility.Random(2, 5);
            return armor;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Fletcher's Precision Gloves";
            gloves.Hue = Utility.RandomMinMax(300, 1300);
            gloves.Attributes.BonusDex = 20;
            gloves.Attributes.Luck = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Fletching, 25.0);
            gloves.PhysicalBonus = 10;
            gloves.ColdBonus = 5;
            return gloves;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Ember PlateArms";
            arms.Hue = Utility.RandomMinMax(500, 600);
            arms.BaseArmorRating = Utility.Random(40, 75);
            arms.Attributes.BonusInt = 20;
            arms.FireBonus = 20;
            arms.EnergyBonus = 5;
            arms.PoisonBonus = -5;
            arms.PhysicalBonus = 10;
            return arms;
        }

        private Item CreateWarHammer()
        {
            WarHammer hammer = new WarHammer();
            hammer.Name = "Thor's Hammer";
            hammer.Hue = Utility.RandomMinMax(550, 750);
            hammer.MinDamage = Utility.Random(35, 70);
            hammer.MaxDamage = Utility.Random(70, 110);
            hammer.Attributes.RegenMana = 5;
            hammer.Attributes.LowerManaCost = 10;
            hammer.WeaponAttributes.HitEnergyArea = 30;
            hammer.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
            return hammer;
        }

        public MagesArcaneChest(Serial serial) : base(serial)
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
