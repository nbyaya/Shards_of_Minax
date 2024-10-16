using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SocialMediaMavensChest : WoodenChest
    {
        [Constructable]
        public SocialMediaMavensChest()
        {
            Name = "Social Media Maven's";
            Hue = Utility.Random(1, 1800);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateColoredItem<Emerald>("Tweeting Gem", 0.25));
            AddItem(CreateNamedItem<Apple>("Apple of Selfies"), 0.32);
            AddItem(CreateNamedItem<TreasureLevel4>("Influencer's Kit"), 0.22);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.17);
            AddItem(CreateRandomInstrument(), 0.19);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateBow(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateColoredItem<T>(string name, double probability) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = Utility.Random(1, 1800);
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
            note.NoteString = "Likes, shares, and comments.";
            note.TitleString = "Online Journal";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            Drums instrument = new Drums();
            instrument.Name = "Viral Video Recorder";
            return instrument;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Hashtag Blade";
            weapon.Hue = Utility.Random(1, 1800);
            weapon.MaxDamage = Utility.Random(65, 100);
            return weapon;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Vampire's Midnight Cloak";
            cloak.Hue = Utility.RandomMinMax(490, 600);
            cloak.Attributes.NightSight = 1;
            cloak.Attributes.BonusDex = 15;
            cloak.SkillBonuses.SetValues(0, SkillName.Stealth, 30.0);
            cloak.SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);
            return cloak;
        }

        private Item CreateArmor()
        {
            PlateArms armor = new PlateArms();
            armor.Name = "SOLDIER's Might";
            armor.Hue = Utility.RandomMinMax(100, 500);
            armor.BaseArmorRating = Utility.Random(45, 85);
            armor.AbsorptionAttributes.EaterKinetic = 15;
            armor.ArmorAttributes.DurabilityBonus = 30;
            armor.Attributes.BonusStam = 30;
            armor.Attributes.AttackChance = 20;
            armor.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            armor.PhysicalBonus = 20;
            armor.FireBonus = 15;
            armor.EnergyBonus = 10;
            armor.ColdBonus = 5;
            armor.PoisonBonus = 10;
            return armor;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Diana's Moon Bow";
            bow.Hue = Utility.RandomMinMax(150, 350);
            bow.MinDamage = Utility.Random(20, 65);
            bow.MaxDamage = Utility.Random(65, 105);
            bow.Attributes.LowerRegCost = 20;
            bow.Attributes.IncreasedKarmaLoss = -10;
            bow.Slayer = SlayerName.Fey;
            bow.Slayer2 = SlayerName.Repond;
            bow.WeaponAttributes.HitLeechStam = 20;
            bow.WeaponAttributes.MageWeapon = 1;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
            bow.SkillBonuses.SetValues(1, SkillName.AnimalTaming, 20.0);
            return bow;
        }

        public SocialMediaMavensChest(Serial serial) : base(serial)
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
