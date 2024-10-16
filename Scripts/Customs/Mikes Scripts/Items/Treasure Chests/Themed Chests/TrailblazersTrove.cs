using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TrailblazersTrove : WoodenChest
    {
        [Constructable]
        public TrailblazersTrove()
        {
            Name = "Trailblazer's Trove";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Coins of Empowerment"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Liquid Courage", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Legacy of Pioneers"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of Unity"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<Painting6NorthArtifact>("Portrait of a Visionary", 2120), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Tea of Tenacity", 1160), 0.08);
            AddItem(CreateGoldItem("Token of Equality"), 0.16);
            AddItem(CreateNamedItem<GoldRing>("Ring of Resilience"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Vision of Equality"), 0.13);
            AddItem(CreatePotion(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateArmoire(), 0.30);
            AddItem(CreateRobe(), 0.30);
            AddItem(CreateMapOfLandmarks(), 0.30);
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
            note.NoteString = "Here's to strong women: May we know them. May we be them. May we raise them.";
            note.TitleString = "Words of Strength";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Women's Hall of Fame";
            map.Bounds = new Rectangle2D(3000, 3000, 700, 700);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Elixir of Confidence";
            return potion;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Sword of Susan B.";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of Amelia";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateArmoire()
        {
            FancyDress armoire = new FancyDress();
            armoire.Name = "Dress of Feminism";
            armoire.Hue = Utility.RandomMinMax(650, 1650);
            armoire.SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
            return armoire;
        }

        private Item CreateRobe()
        {
            StuddedChest robe = new StuddedChest();
            robe.Name = "Chest of Malala";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.BaseArmorRating = Utility.Random(60, 90);
            robe.AbsorptionAttributes.EaterPoison = 30;
            robe.ArmorAttributes.ReactiveParalyze = 1;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
            robe.ColdBonus = 20;
            robe.EnergyBonus = 20;
            robe.FireBonus = 20;
            robe.PhysicalBonus = 25;
            robe.PoisonBonus = 25;
            return robe;
        }

        private Item CreateMapOfLandmarks()
        {
            Dagger map = new Dagger();
            map.Name = "Dagger of Inspirational Landmarks";
            map.Hue = Utility.RandomMinMax(55, 260);
            map.MinDamage = Utility.Random(30, 80);
            map.MaxDamage = Utility.Random(80, 120);
            map.Attributes.BonusStr = 10;
            map.Attributes.SpellDamage = 5;
            map.WeaponAttributes.HitHarm = 30;
            map.WeaponAttributes.SelfRepair = 5;
            map.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 25.0);
            return map;
        }

        public TrailblazersTrove(Serial serial) : base(serial)
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
