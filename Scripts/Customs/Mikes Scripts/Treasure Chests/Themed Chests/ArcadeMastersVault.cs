using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ArcadeMastersVault : WoodenChest
    {
        [Constructable]
        public ArcadeMastersVault()
        {
            Name = "Arcade Master's Vault";
            Hue = Utility.Random(1, 2070);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.08);
            AddItem(CreateEmerald("Pixel Gem"), 0.22);
            AddItem(CreateGoldVoucher(), 0.25);
            AddItem(CreateNamedItem<TreasureLevel3>("High Score Trophy"), 0.26);
            AddItem(CreateNamedItem<SilverNecklace>("Power-Up Pendant"), 0.30);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.18);
            AddItem(CreatePotion(), 0.15);
            AddItem(CreateJoystickJavelin(), 0.20);
            AddItem(CreateThighBoots(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateQuarterStaff(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald(string name)
        {
            Emerald emerald = new Emerald();
            emerald.Name = name;
            return emerald;
        }

        private Item CreateGoldVoucher()
        {
            MaxxiaScroll voucher = new MaxxiaScroll();
            voucher.Name = "Coin-op Voucher";
            return voucher;
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
            note.NoteString = "Save the princess, avoid the ghosts!";
            note.TitleString = "Game Guide";
            return note;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Potion of Extra Life";
            return potion;
        }

        private Item CreateJoystickJavelin()
        {
            Spear weapon = new Spear();
            weapon.Name = "Joystick Javelin";
            weapon.Hue = Utility.RandomList(1, 2070);
            return weapon;
        }

        private Item CreateThighBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Aerobics Instructor's Legwarmers";
            boots.Hue = Utility.RandomMinMax(300, 1300);
            boots.Attributes.BonusStam = 15;
            boots.Attributes.RegenStam = 5;
            boots.SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
            return boots;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Courtesan's Whispering Boots";
            legs.Hue = Utility.RandomMinMax(100, 500);
            legs.BaseArmorRating = Utility.RandomMinMax(25, 55);
            legs.AbsorptionAttributes.EaterEnergy = 10;
            legs.Attributes.RegenStam = 5;
            legs.Attributes.NightSight = 1;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
            legs.ColdBonus = 5;
            legs.EnergyBonus = 10;
            legs.FireBonus = 5;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 5;
            return legs;
        }

        private Item CreateQuarterStaff()
        {
            QuarterStaff staff = new QuarterStaff();
            staff.Name = "Moses' Staff";
            staff.Hue = Utility.RandomMinMax(500, 700);
            staff.MinDamage = Utility.RandomMinMax(15, 55);
            staff.MaxDamage = Utility.RandomMinMax(55, 90);
            staff.Attributes.SpellChanneling = 1;
            staff.Attributes.BonusMana = 15;
            staff.Slayer = SlayerName.ElementalBan;
            staff.WeaponAttributes.HitFireArea = 20;
            staff.WeaponAttributes.MageWeapon = 1;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return staff;
        }

        public ArcadeMastersVault(Serial serial) : base(serial)
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
