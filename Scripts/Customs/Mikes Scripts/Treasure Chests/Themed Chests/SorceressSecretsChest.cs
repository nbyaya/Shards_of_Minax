using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SorceressSecretsChest : WoodenChest
    {
        [Constructable]
        public SorceressSecretsChest()
        {
            Name = "Sorceress's Secrets Chest";
            Hue = 1154;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Crystal of the Moons"), 0.18);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Charm of the Ancients"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Starlight Earring", 1160), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 1.0);
            AddItem(CreateNamedItem<Apple>("Moonlit Apple"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Sorceress's Elixir"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Starry Night", 1157), 0.10);
            AddItem(CreateRandomItem<Spellbook>("Ancient Grimoire"), 0.17);
            AddItem(CreateNamedItem<Spyglass>("Sorceress's Trusted Spyglass"), 0.04);
            AddItem(CreateRandomItem<Shaft>("Wand of Moon Magic"), 0.14);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateTunic(), 1.0);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateBlackStaff(), 0.20);
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

        private Item CreateRandomItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The true magic lies within.";
            note.TitleString = "Sorceress's Diary";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new BlackStaff());
            weapon.Name = "Sorceress's Staff";
            weapon.Hue = Utility.RandomList(1, 1163);
            weapon.MaxDamage = Utility.Random(20, 55);
            return weapon;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Carpenter's Stalwart Tunic";
            tunic.Hue = Utility.RandomMinMax(700, 1650);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusDex = 10;
            tunic.Attributes.DefendChance = 5;
            tunic.SkillBonuses.SetValues(0, SkillName.Carpentry, 25.0);
            return tunic;
        }

        private Item CreatePlateChest()
        {
            PlateChest chest = new PlateChest();
            chest.Name = "Solaris Lorica";
            chest.Hue = Utility.RandomMinMax(300, 700);
            chest.BaseArmorRating = Utility.Random(30, 80);
            chest.AbsorptionAttributes.ResonancePoison = 40;
            chest.ArmorAttributes.SelfRepair = 10;
            chest.Attributes.LowerManaCost = 10;
            chest.Attributes.SpellDamage = 15;
            chest.ColdBonus = 10;
            chest.EnergyBonus = 10;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 10;
            chest.PoisonBonus = 20;
            return chest;
        }

        private Item CreateBlackStaff()
        {
            BlackStaff staff = new BlackStaff();
            staff.Name = "Staff of Apocalypse";
            staff.Hue = Utility.RandomMinMax(900, 1000);
            staff.MinDamage = Utility.Random(10, 40);
            staff.MaxDamage = Utility.Random(40, 60);
            staff.Attributes.SpellDamage = 20;
            staff.Attributes.LowerRegCost = 15;
            staff.WeaponAttributes.HitFireball = 30;
            staff.WeaponAttributes.HitManaDrain = 10;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            return staff;
        }

        public SorceressSecretsChest(Serial serial) : base(serial)
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
