using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DynastyRelicsChest : WoodenChest
    {
        [Constructable]
        public DynastyRelicsChest()
        {
            Name = "Dynasty's Relics";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Ancient Yuan"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Aged Rice Wine", 1168), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Forbidden Treasury"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of the Ming Dynasty"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("Traditional Chinese Tea"), 0.08);
            AddItem(CreateGoldItem("Imperial Taels"), 0.16);
            AddItem(CreateColoredItem<DragonBrazier2>("Emperor's Dragon Brazier", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Signet Ring of Confucius"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Dynasty's Navigational Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Daoist Alchemy"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateElegantArmoire(), 0.30);
            AddItem(CreateRuinedFallenChair(), 0.30);
            AddItem(CreateMalletAndChisel(), 0.30);
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
            note.NoteString = "In honor of the great Dynasties of China";
            note.TitleString = "Chinese Proverbs";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Forbidden City";
            map.Bounds = new Rectangle2D(3000, 3000, 700, 700);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Blade of the Terracotta";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Warrior's Silk Garment";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateElegantArmoire()
        {
            JesterHat armoire = new JesterHat();
            armoire.Name = "Dynasty's Ornate Wardrobe";
            armoire.Hue = Utility.RandomMinMax(600, 1600);
            armoire.SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);
            return armoire;
        }

        private Item CreateRuinedFallenChair()
        {
            PlateChest chair = new PlateChest();
            chair.Name = "Chest of the Scholars";
            chair.Hue = Utility.RandomMinMax(1, 1000);
            chair.BaseArmorRating = Utility.Random(60, 90);
            chair.AbsorptionAttributes.EaterEnergy = 30;
            chair.ArmorAttributes.ReactiveParalyze = 1;
            chair.Attributes.BonusInt = 20;
            chair.Attributes.AttackChance = 10;
            chair.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            chair.ColdBonus = 20;
            chair.EnergyBonus = 20;
            chair.FireBonus = 20;
            chair.PhysicalBonus = 25;
            chair.PoisonBonus = 25;
            return chair;
        }

        private Item CreateMalletAndChisel()
        {
            Mace malletAndChisel = new Mace();
            malletAndChisel.Name = "Craftsman's Pride of China";
            malletAndChisel.Hue = Utility.RandomMinMax(50, 250);
            malletAndChisel.MinDamage = Utility.Random(30, 80);
            malletAndChisel.MaxDamage = Utility.Random(80, 120);
            malletAndChisel.Attributes.BonusDex = 10;
            malletAndChisel.Attributes.SpellDamage = 5;
            malletAndChisel.WeaponAttributes.HitLightning = 30;
            malletAndChisel.WeaponAttributes.SelfRepair = 5;
            malletAndChisel.SkillBonuses.SetValues(0, SkillName.Carpentry, 25.0);
            return malletAndChisel;
        }

        public DynastyRelicsChest(Serial serial) : base(serial)
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
