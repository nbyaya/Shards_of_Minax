using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CaesarChest : WoodenChest
    {
        [Constructable]
        public CaesarChest()
        {
            Name = "Caesar's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Ruby>("Ruby of the Republic"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Caesar's Favorite Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Caesar's Spoils"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Laurel Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Empire", 1153), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Roman Wine"), 0.08);
            AddItem(CreateGoldItem("Denarius"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Conqueror", 1153), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Eagle Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Telescope>("Caesar's Trusted Telescope"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateShoes(), 0.20);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Veni, vidi, vici.";
            note.TitleString = "Caesar's Famous Quote";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Caesar's Secret Vault";
            map.Bounds = new Rectangle2D(1000, 1000, 4000, 4000);
            map.NewPin = new Point2D(2500, 2500);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Gladius Maximus";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Lorica Segmentata";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Platform Sneakers";
            shoes.Hue = Utility.RandomMinMax(400, 1300);
            shoes.Attributes.BonusDex = 12;
            return shoes;
        }

        private Item CreateRobe()
        {
            LeatherChest robe = new LeatherChest();
            robe.Name = "BlackMage's Rune Robe";
            robe.Hue = Utility.RandomMinMax(50, 250);
            robe.BaseArmorRating = Utility.Random(20, 50);
            robe.AbsorptionAttributes.EaterFire = 15;
            robe.ArmorAttributes.MageArmor = 1;
            robe.Attributes.BonusInt = 25;
            robe.Attributes.SpellDamage = 10;
            robe.Attributes.SpellChanneling = 1;
            robe.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            robe.SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);
            robe.ColdBonus = 5;
            robe.EnergyBonus = 15;
            robe.FireBonus = 20;
            robe.PhysicalBonus = 5;
            robe.PoisonBonus = 5;
            return robe;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Sword of Al-Battal";
            longsword.Hue = Utility.RandomMinMax(400, 600);
            longsword.MinDamage = Utility.Random(30, 70);
            longsword.MaxDamage = Utility.Random(70, 110);
            longsword.Attributes.AttackChance = 10;
            longsword.Attributes.BonusHits = 20;
            longsword.Slayer = SlayerName.OrcSlaying;
            longsword.Slayer2 = SlayerName.TrollSlaughter;
            longsword.WeaponAttributes.BloodDrinker = 25;
            longsword.WeaponAttributes.HitLeechHits = 20;
            longsword.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return longsword;
        }

        public CaesarChest(Serial serial) : base(serial)
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
