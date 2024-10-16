using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PatriotCache : WoodenChest
    {
        [Constructable]
        public PatriotCache()
        {
            Name = "Patriot's Cache";
            Hue = Utility.Random(1, 1776);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Colonial Coins"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Revolutionary Brew", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Founding Father's Stash"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of Liberty"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("Colonial Cider"), 0.08);
            AddItem(CreateGoldItem("Continental Currency"), 0.16);
            AddItem(CreateColoredItem<Spyglass>("Paul Revere's Lookout", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Ring of Independence"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Clock>("Freedom Clock"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Tea Party Tonic"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateTricornHat(), 0.30);
            AddItem(CreateRobe(), 0.30);
            AddItem(CreateQuillPen(), 0.30);
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
            note.NoteString = "We hold these truths to be self-evident, that all men are created equal...";
            note.TitleString = "Declaration Excerpt";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Bunker Hill";
            map.Bounds = new Rectangle2D(3000, 3000, 700, 700);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Musketeer's Rifle";
            weapon.Hue = Utility.RandomList(1, 1776);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Continental Uniform";
            armor.Hue = Utility.RandomList(1, 1776);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTricornHat()
        {
            Cap hat = new Cap();
            hat.Name = "General's Tricorn";
            hat.Hue = Utility.RandomMinMax(600, 1600);
            hat.ClothingAttributes.DurabilityBonus = 5;
            hat.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return hat;
        }

        private Item CreateRobe()
        {
            LeatherChest robe = new LeatherChest();
            robe.Name = "Benjamin's Robe";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.BaseArmorRating = Utility.Random(60, 90);
            robe.AbsorptionAttributes.EaterPoison = 30;
            robe.ArmorAttributes.ReactiveParalyze = 1;
            robe.Attributes.BonusInt = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            robe.ColdBonus = 20;
            robe.EnergyBonus = 20;
            robe.FireBonus = 20;
            robe.PhysicalBonus = 25;
            robe.PoisonBonus = 25;
            return robe;
        }

        private Item CreateQuillPen()
        {
            BaseWeapon quillPen = Utility.RandomList<BaseWeapon>(new Longsword());
            quillPen.Name = "Constitutional Quill";
            quillPen.Hue = Utility.RandomMinMax(50, 250);
            quillPen.MinDamage = Utility.Random(30, 80);
            quillPen.MaxDamage = Utility.Random(80, 120);
            quillPen.Attributes.BonusStr = 10;
            quillPen.Attributes.SpellDamage = 5;
            quillPen.WeaponAttributes.HitColdArea = 30;
            quillPen.WeaponAttributes.SelfRepair = 5;
            quillPen.SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
            return quillPen;
        }

        public PatriotCache(Serial serial) : base(serial)
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
