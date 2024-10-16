using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RevolutionaryRelicChest : WoodenChest
    {
        [Constructable]
        public RevolutionaryRelicChest()
        {
            Name = "Revolutionary Relic";
            Hue = Utility.Random(1, 1150);

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Liberty Gem"), 0.22);
            AddItem(CreateSimpleNote(), 0.31);
            AddItem(new Gold(Utility.Random(1, 4200)), 0.26);
            AddItem(CreateNamedItem<Spyglass>("General's Spyglass"), 0.28);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateClub(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateColoredItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = Utility.Random(1, 1150); // Random hue for simplicity
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
            note.NoteString = "Give me liberty or give me death!";
            note.TitleString = "Patriot's Notes";
            return note;
        }

        private Item CreateRandomClothing()
        {
            BaseClothing clothing = new Shirt(); // Use appropriate clothing type
            clothing.Name = "Revolutionary Uniform";
            clothing.Hue = Utility.Random(1, 1150);
            return clothing;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Club(); // Use appropriate weapon type
            weapon.Name = "Rebel's Saber";
            return weapon;
        }

        private Item CreateCloak()
        {
            BaseClothing cloak = new Cloak();
            cloak.Name = "Rogue's Shadow Cloak";
            cloak.Hue = Utility.RandomMinMax(1100, 1800);
            cloak.ClothingAttributes.SelfRepair = 3;
            cloak.Attributes.BonusDex = 15;
            cloak.Attributes.NightSight = 1;
            cloak.SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
            cloak.SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
            return cloak;
        }

        private Item CreateLeatherCap()
        {
            BaseArmor cap = new LeatherCap();
            cap.Name = "Sherwood Archer's Cap";
            cap.Hue = Utility.RandomMinMax(250, 550);
            cap.BaseArmorRating = Utility.RandomMinMax(20, 40);
            cap.Attributes.BonusDex = 15;
            cap.Attributes.AttackChance = 10;
            cap.SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
            cap.PhysicalBonus = 10;
            cap.PoisonBonus = 10;
            return cap;
        }

        private Item CreateClub()
        {
            Club club = new Club();
            club.Name = "Wabbajack Club";
            club.Hue = Utility.RandomMinMax(250, 300);
            club.MinDamage = Utility.RandomMinMax(15, 55);
            club.MaxDamage = Utility.RandomMinMax(55, 85);
            club.Attributes.LowerManaCost = 10;
            club.Attributes.Luck = 100;
            club.WeaponAttributes.HitDispel = 50;
            club.WeaponAttributes.HitMagicArrow = 20;
            club.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            return club;
        }

        public RevolutionaryRelicChest(Serial serial) : base(serial)
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
