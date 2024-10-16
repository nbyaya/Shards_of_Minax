using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MerchantFortuneChest : WoodenChest
    {
        [Constructable]
        public MerchantFortuneChest()
        {
            Name = "Merchant's Fortune";
            Hue = Utility.Random(1, 1350);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Sapphire>("Jewel of Prosperity"), 0.27);
            AddItem(CreateNamedItem<MaxxiaScroll>("Voucher of Riches"), 0.29);
            AddItem(CreateNamedItem<TreasureLevel4>("Merchant's Coffer"), 0.23);
            AddItem(CreateNamedItem<SilverNecklace>("Trade Baron's Pendant"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.14);
            AddItem(CreateRandomGem(), 0.17);
            AddItem(CreateWeapon(), 0.19);
            AddItem(CreateBagOfGold(), 0.20);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateStuddedGloves(), 0.20);
            AddItem(CreateWarHammer(), 0.20);
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
            item.Hue = Utility.RandomList(1, 1350);
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
            note.NoteString = "Bargains and deals of old.";
            note.TitleString = "Merchant's Ledger";
            return note;
        }

        private Item CreateRandomGem()
        {
            // Placeholder for random gem item
            Ruby gem = new Ruby();
            gem.Name = "Gem of Commerce";
            return gem;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger(), new Longsword());
            weapon.Name = "Blade of Bargaining";
            weapon.Hue = Utility.RandomList(1, 1350);
            return weapon;
        }

        private Item CreateBagOfGold()
        {
            Bag bag = new Bag();
            bag.Name = "Coin Purse of the Tycoon";
            return bag;
        }

        private Item CreateRandomClothing()
        {
            // Placeholder for random clothing item
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Merchant's Attire";
            clothing.Hue = Utility.RandomList(1, 1350);
            return clothing;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Silent Night Cloak";
            cloak.Hue = Utility.RandomMinMax(600, 1600);
            cloak.ClothingAttributes.SelfRepair = 4;
            cloak.Attributes.BonusDex = 10;
            cloak.Attributes.LowerManaCost = 5;
            cloak.SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
            cloak.SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
            return cloak;
        }

        private Item CreateStuddedGloves()
        {
            StuddedGloves gloves = new StuddedGloves();
            gloves.Name = "Masked Avenger's Precision";
            gloves.Hue = Utility.RandomMinMax(900, 999);
            gloves.BaseArmorRating = Utility.Random(25, 55);
            gloves.Attributes.BonusDex = 20;
            gloves.Attributes.AttackChance = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 5;
            gloves.FireBonus = 10;
            gloves.PhysicalBonus = 20;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateWarHammer()
        {
            WarHammer hammer = new WarHammer();
            hammer.Name = "Volendrung WarHammer";
            hammer.Hue = Utility.RandomMinMax(400, 550);
            hammer.MinDamage = Utility.Random(35, 80);
            hammer.MaxDamage = Utility.Random(80, 125);
            hammer.Attributes.BonusHits = 20;
            hammer.Attributes.AttackChance = 10;
            hammer.WeaponAttributes.HitFatigue = 40;
            hammer.WeaponAttributes.HitPhysicalArea = 20;
            hammer.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return hammer;
        }

        public MerchantFortuneChest(Serial serial) : base(serial)
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
