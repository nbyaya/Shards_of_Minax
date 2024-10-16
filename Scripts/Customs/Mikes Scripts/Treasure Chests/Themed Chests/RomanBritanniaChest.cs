using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RomanBritanniaChest : WoodenChest
    {
        [Constructable]
        public RomanBritanniaChest()
        {
            Name = "Roman Britannia";
            Hue = Utility.Random(1, 1550);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<SilverNecklace>("Centurion's Medallion"), 0.27);
            AddItem(CreateNamedItem<Gold>("Caesar's Coin"), 0.25);
            AddItem(CreateNamedItem<TreasureLevel1>("Roman Relic"), 0.23);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.14);
            AddItem(CreateWeapon(), 0.16);
            AddItem(CreateNamedItem<Sapphire>("Jewel of the Empire"), 0.18);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateKasa(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateBow(), 0.20);
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
            note.NoteString = "Orders from Emperor Hadrian.";
            note.TitleString = "Roman Dispatch";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Gladius of Legionnaire";
            weapon.Hue = Utility.RandomList(1, 1550);
            weapon.MaxDamage = Utility.Random(2, 4);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Roman Centurion's Gear";
            armor.Hue = Utility.RandomList(1, 1550);
            armor.BaseArmorRating = Utility.Random(2, 4);
            return armor;
        }

        private Item CreateKasa()
        {
            Kasa kasa = new Kasa();
            kasa.Name = "Ninja's Kasa";
            kasa.Hue = Utility.RandomMinMax(300, 1400);
            kasa.ClothingAttributes.LowerStatReq = 3;
            kasa.Attributes.BonusInt = 10;
            kasa.Attributes.RegenStam = 2;
            kasa.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 25.0);
            kasa.SkillBonuses.SetValues(1, SkillName.Focus, 20.0);
            return kasa;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Masked Avenger's Agility";
            legs.Hue = Utility.RandomMinMax(900, 999);
            legs.BaseArmorRating = Utility.Random(30, 60);
            legs.Attributes.BonusStam = 20;
            legs.Attributes.NightSight = 1;
            legs.Attributes.RegenHits = 5;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 10;
            legs.FireBonus = 5;
            legs.PhysicalBonus = 15;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Bow of Auriel";
            bow.Hue = Utility.RandomMinMax(100, 250);
            bow.MinDamage = Utility.Random(20, 55);
            bow.MaxDamage = Utility.Random(55, 85);
            bow.Attributes.LowerRegCost = 15;
            bow.Attributes.Luck = 150;
            bow.Slayer = SlayerName.DragonSlaying;
            bow.WeaponAttributes.HitLightning = 25;
            bow.WeaponAttributes.HitManaDrain = 10;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
            return bow;
        }

        public RomanBritanniaChest(Serial serial) : base(serial)
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
