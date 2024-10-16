using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CivilWarCache : WoodenChest
    {
        [Constructable]
        public CivilWarCache()
        {
            Name = "Civil War Cache";
            Hue = Utility.Random(1, 1200);

            // Add items to the chest
            AddItem(CreateNamedItem<SilverNecklace>("Union Medallion"), 0.21);
            AddItem(CreateSimpleNote(), 0.30);
            AddItem(CreateNamedItem<GreaterHealPotion>("Battlefield Brew"), 0.30);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.24);
            AddItem(CreateNamedItem<Spyglass>("Commander's Monocular"), 0.27);
            AddItem(CreateClothingItem(), 0.19);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateGloves(), 0.20);
            AddItem(CreateStuddedGloves(), 0.20);
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
            note.NoteString = "A house divided against itself cannot stand.";
            note.TitleString = "Soldier's Letter";
            return note;
        }

        private Item CreateClothingItem()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Civil War Uniform";
            clothing.Hue = Utility.RandomList(1, 1200);
            return clothing;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Infantryman's Bayonet";
            return weapon;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Snooper's Soft Gloves";
            gloves.Hue = Utility.RandomMinMax(600, 1600);
            gloves.Attributes.BonusDex = 10;
            gloves.Attributes.LowerManaCost = 5;
            gloves.SkillBonuses.SetValues(0, SkillName.Snooping, 25.0);
            gloves.SkillBonuses.SetValues(1, SkillName.Stealing, 20.0);
            gloves.ColdBonus = 5;
            return gloves;
        }

        private Item CreateStuddedGloves()
        {
            StuddedGloves gloves = new StuddedGloves();
            gloves.Name = "Merry Men's Studded Gloves";
            gloves.Hue = Utility.RandomMinMax(250, 550);
            gloves.BaseArmorRating = Utility.RandomMinMax(25, 45);
            gloves.ArmorAttributes.LowerStatReq = 15;
            gloves.Attributes.BonusStr = 10;
            gloves.Attributes.WeaponSpeed = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Stealing, 20.0);
            gloves.PhysicalBonus = 10;
            gloves.EnergyBonus = 5;
            return gloves;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Chillrend Longsword";
            longsword.Hue = Utility.RandomMinMax(450, 490);
            longsword.MinDamage = Utility.RandomMinMax(20, 65);
            longsword.MaxDamage = Utility.RandomMinMax(65, 95);
            longsword.Attributes.BonusDex = 10;
            longsword.Attributes.WeaponSpeed = 5;
            longsword.Slayer = SlayerName.WaterDissipation;
            longsword.WeaponAttributes.HitColdArea = 50;
            longsword.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return longsword;
        }

        public CivilWarCache(Serial serial) : base(serial)
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
