using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MedievalEnglandChest : WoodenChest
    {
        [Constructable]
        public MedievalEnglandChest()
        {
            Name = "Medieval England";
            Hue = Utility.Random(1, 1650);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<GoldEarrings>("Jewel of the Crown"), 0.29);
            AddItem(CreateNamedItem<Emerald>("Chalice Gem"), 0.26);
            AddItem(CreateNamedItem<TreasureLevel2>("Norman Treasure"), 0.25);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateWeapon(), 0.17);
            AddItem(CreateArmor(), 0.17);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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
            note.NoteString = "A letter from William the Conqueror.";
            note.TitleString = "Norman Parchment";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Knight's Longsword";
            weapon.Hue = Utility.RandomList(1, 1650);
            weapon.MinDamage = Utility.Random(3, 4);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new ChainCoif(), new ChainChest(), new ChainLegs(), new LeatherGloves());
            armor.Name = "Chainmail of the Round Table";
            armor.Hue = Utility.RandomList(1, 1650);
            armor.BaseArmorRating = Utility.Random(3, 4);
            return armor;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Assassin's Bandana";
            bandana.Hue = Utility.RandomList(1, 1000);
            bandana.Attributes.BonusDex = 20;
            bandana.Attributes.AttackChance = 10;
            bandana.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 15.0);
            bandana.SkillBonuses.SetValues(1, SkillName.Fencing, 15.0);
            return bandana;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Masked Avenger's Focus";
            cap.Hue = Utility.RandomMinMax(900, 999);
            cap.BaseArmorRating = Utility.Random(20, 50);
            cap.Attributes.CastSpeed = 1;
            cap.Attributes.BonusMana = 10;
            cap.SkillBonuses.SetValues(0, SkillName.Focus, 15.0);
            cap.ColdBonus = 10;
            cap.EnergyBonus = 10;
            cap.FireBonus = 5;
            cap.PhysicalBonus = 10;
            cap.PoisonBonus = 5;
            return cap;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Erdrick's Blade";
            longsword.Hue = Utility.RandomMinMax(50, 250);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitFireball = 30;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return longsword;
        }

        public MedievalEnglandChest(Serial serial) : base(serial)
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
