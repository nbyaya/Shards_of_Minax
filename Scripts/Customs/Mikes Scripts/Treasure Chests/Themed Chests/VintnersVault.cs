using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class VintnersVault : WoodenChest
    {
        [Constructable]
        public VintnersVault()
        {
            Name = "Vintner's Vault";
            Hue = Utility.Random(1, 1750);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateGoldItem("Vineyard Coins"), 0.22);
            AddItem(CreateColoredItem<GreaterHealPotion>("Fine Bordeaux", 1128), 0.18);
            AddItem(CreateNamedItem<TreasureLevel2>("Cask of Vintage"), 0.19);
            AddItem(CreateNamedItem<GoldBracelet>("Grapevine Bracelet"), 0.55);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.17);
            AddItem(CreateColoredItem<Grapes>("Rare Sun-Drenched Grapes", 2119), 0.14);
            AddItem(CreateColoredItem<GreaterHealPotion>("Ancient Chardonnay", 1128), 0.09);
            AddItem(CreateGoldItem("Wine Merchant's Payment"), 0.18);
            AddItem(CreateColoredItem<MaxxiaScroll>("Vintage Wine Cork", 1177), 0.21);
            AddItem(CreateColoredItem<GoldRing>("Ring of the Vineyard", 1289), 0.18);
            AddItem(CreateMap(), 0.06);
            AddItem(CreateNamedItem<Spyglass>("Wine Explorer's Scope"), 0.14);
            AddItem(CreateNamedItem<GreenGourd>("Wine Flask"), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of the Grape"), 0.21);
            AddItem(CreateWeapon(), 0.23);
            AddItem(CreateArmor(), 0.32);
            AddItem(CreateBarrelTap(), 0.31);
            AddItem(CreateWineCork(), 0.29);
            AddItem(CreateMalletAndChisel(), 0.29);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
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
            note.NoteString = "The taste of time, the aroma of history, the essence of the earth.";
            note.TitleString = "Wine Connoisseur's Notes";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Hidden Vineyard";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger());
            weapon.Name = "Vintner's Dagger";
            weapon.Hue = Utility.RandomList(1, 1789);
            weapon.MaxDamage = Utility.Random(20, 60);
            return weapon;
        }

        private Item CreateArmor()
        {
            Robe armor = new Robe();
            armor.Name = "Grapemaster's Robe";
            armor.Hue = Utility.RandomList(1, 1789);
			armor.SkillBonuses.SetValues(0, SkillName.TasteID, 29.0);
            return armor;
        }

        private Item CreateBarrelTap()
        {
            FancyShirt tap = new FancyShirt();
            tap.Name = "Wine Shirt";
            tap.Hue = Utility.RandomMinMax(650, 1650);
            tap.ClothingAttributes.DurabilityBonus = 5;
            tap.Attributes.DefendChance = 8;
            tap.SkillBonuses.SetValues(0, SkillName.TasteID, 22.0);
            return tap;
        }

        private Item CreateWineCork()
        {
            LeatherArms cork = new LeatherArms();
            cork.Name = "Arms of the Vintner";
            cork.Hue = Utility.RandomMinMax(1, 1100);
            cork.BaseArmorRating = Utility.Random(50, 85);
            cork.AbsorptionAttributes.EaterFire = 28;
            cork.ArmorAttributes.ReactiveParalyze = 1;
            cork.Attributes.BonusDex = 18;
            cork.Attributes.AttackChance = 9;
            cork.SkillBonuses.SetValues(0, SkillName.TasteID, 22.0);
            cork.FireBonus = 18;
            cork.EnergyBonus = 18;
            cork.PhysicalBonus = 23;
            cork.PoisonBonus = 23;
            return cork;
        }

        private Item CreateMalletAndChisel()
        {
            Mace tools = new Mace();
            tools.Name = "Vineyard Crafting Tools";
            tools.Hue = Utility.RandomMinMax(52, 252);
            tools.MinDamage = Utility.Random(18, 78);
            tools.MaxDamage = Utility.Random(78, 118);
            tools.Attributes.BonusStr = 9;
            tools.Attributes.SpellDamage = 4;
            tools.WeaponAttributes.HitFireArea = 28;
            tools.WeaponAttributes.SelfRepair = 4;
            tools.SkillBonuses.SetValues(0, SkillName.TasteID, 23.0);
            return tools;
        }

        public VintnersVault(Serial serial) : base(serial)
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
