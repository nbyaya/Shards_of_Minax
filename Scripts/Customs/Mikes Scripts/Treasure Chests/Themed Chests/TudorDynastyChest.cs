using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TudorDynastyChest : WoodenChest
    {
        [Constructable]
        public TudorDynastyChest()
        {
            Name = "Tudor Dynasty";
            Hue = Utility.Random(1, 1750);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateNamedItem<SilverNecklace>("Anne Boleyn's Locket"), 0.28);
            AddItem(CreateNamedItem<MaxxiaScroll>("Henry's Royal Note"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel3>("Tudor Heirloom"), 0.24);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.14);
            AddItem(CreateWeapon("Elizabethan Rapier"), 0.16);
            AddItem(CreateArmor("Tudor Noble Attire"), 0.16);
            AddItem(CreateSash(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
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
            note.NoteString = "A diary entry of Elizabeth I.";
            note.TitleString = "Tudor Journal";
            return note;
        }

        private Item CreateWeapon(string name)
        {
            BaseWeapon weapon = new Longsword();
            weapon.Name = name;
            weapon.Hue = Utility.RandomList(1, 1750);
            weapon.MaxDamage = Utility.Random(20, 50); // Adjusted to match a typical weapon
            return weapon;
        }

        private Item CreateArmor(string name)
        {
            BaseArmor armor = new PlateChest();
            armor.Name = name;
            armor.Hue = Utility.RandomList(1, 1750);
            return armor;
        }

        private Item CreateSash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Whispering Wind Sash";
            sash.Hue = Utility.RandomMinMax(300, 1300);
            sash.ClothingAttributes.DurabilityBonus = 4;
            sash.Attributes.BonusStr = 10;
            sash.Attributes.WeaponSpeed = 5;
            sash.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
            sash.SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
            return sash;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Masked Avenger's Defense";
            chest.Hue = Utility.RandomMinMax(900, 999);
            chest.BaseArmorRating = Utility.Random(35, 65);
            chest.Attributes.DefendChance = 15;
            chest.Attributes.BonusHits = 20;
            chest.SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 5;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 20;
            chest.PoisonBonus = 5;
            return chest;
        }

        private Item CreateCleaver()
        {
            Cleaver cleaver = new Cleaver();
            cleaver.Name = "Cursed Armor Cleaver";
            cleaver.Hue = Utility.RandomMinMax(700, 900);
            cleaver.MinDamage = Utility.Random(15, 65);
            cleaver.MaxDamage = Utility.Random(65, 105);
            cleaver.Attributes.BonusHits = -10;
            cleaver.Attributes.LowerManaCost = 5;
            cleaver.Slayer = SlayerName.OrcSlaying;
            cleaver.WeaponAttributes.HitCurse = 30;
            cleaver.WeaponAttributes.BloodDrinker = 10;
            return cleaver;
        }

        public TudorDynastyChest(Serial serial) : base(serial)
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
