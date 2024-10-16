using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AnarchistsCache : WoodenChest
    {
        [Constructable]
        public AnarchistsCache()
        {
            Name = "Anarchist's Cache";
            Hue = Utility.Random(1, 1899);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Rebellion Funds"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Molotov Cocktail", 1301), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Insurgent's Stash"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of the Uprising"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<GreaterHealPotion>("Underground Brew", 2026), 0.08);
            AddItem(CreateGoldItem("Rebel Coin"), 0.16);
            AddItem(CreateNamedItem<GoldRing>("Ring of Solidarity"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Insurgent's Lookout Tool"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Disobedience"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateRobeOfDissent(), 0.30);
            AddItem(CreateRobeOfTheVanguard(), 0.30);
            AddItem(CreateLongsword(), 0.30);
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
            note.NoteString = "Down with the Establishment!";
            note.TitleString = "Manifesto of the Anarchist";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Hidden Base";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Rebel's Blade";
            weapon.Hue = Utility.RandomList(1, 1899);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Protester's Guard";
            armor.Hue = Utility.RandomList(1, 1899);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateRobeOfDissent()
        {
            Robe robe = new Robe();
            robe.Name = "Robe of Dissent";
            robe.Hue = Utility.RandomMinMax(600, 1699);
            robe.ClothingAttributes.DurabilityBonus = 5;
            robe.Attributes.DefendChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return robe;
        }

        private Item CreateRobeOfTheVanguard()
        {
            PlateChest robe = new PlateChest();
            robe.Name = "Vest of the Vanguard";
            robe.Hue = Utility.RandomMinMax(1, 1100);
            robe.BaseArmorRating = Utility.Random(60, 90);
            robe.AbsorptionAttributes.EaterEnergy = 30;
            robe.ArmorAttributes.SoulCharge = 1;
            robe.Attributes.BonusDex = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Stealing, 20.0);
            robe.EnergyBonus = 20;
            robe.FireBonus = 20;
            robe.PhysicalBonus = 25;
            robe.PoisonBonus = 25;
            return robe;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Revolutionary's Edge";
            longsword.Hue = Utility.RandomMinMax(50, 299);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.WeaponAttributes.HitHarm = 30;
            longsword.WeaponAttributes.HitPhysicalArea = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Fencing, 55.0);
            return longsword;
        }

        public AnarchistsCache(Serial serial) : base(serial)
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
