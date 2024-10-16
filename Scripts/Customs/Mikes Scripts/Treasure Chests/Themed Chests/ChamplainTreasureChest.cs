using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ChamplainTreasureChest : WoodenChest
    {
        [Constructable]
        public ChamplainTreasureChest()
        {
            Name = "Champlain’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateGoldItem("Golden Beaver"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("French Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Champlain’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of the Huron"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the St. Lawrence", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Ice Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Maple Leaf"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Explorer", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of the Order of Good Cheer"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Champlain’s Cartographic Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Champlain’s Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTricorneHat(), 0.30);
            AddItem(CreateGauntlets(), 0.30);
            AddItem(CreateRapier(), 0.30);
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
            note.NoteString = "I am Samuel de Champlain, founder of Quebec City and father of New France";
            note.TitleString = "Champlain’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Champlain’s Fort";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Champlain’s Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Champlain’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTricorneHat()
        {
            TricorneHat hat = new TricorneHat();
            hat.Name = "Hat of the Governor";
            hat.Hue = Utility.RandomMinMax(600, 1600);
            hat.ClothingAttributes.DurabilityBonus = 5;
            hat.Attributes.DefendChance = 10;
            hat.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            return hat;
        }

        private Item CreateGauntlets()
        {
            PlateGloves gauntlets = new PlateGloves();
            gauntlets.Name = "Gauntlets of the Voyageur";
            gauntlets.Hue = Utility.RandomMinMax(1, 1000);
            gauntlets.BaseArmorRating = Utility.Random(60, 90);
            gauntlets.AbsorptionAttributes.EaterFire = 30;
            gauntlets.ArmorAttributes.ReactiveParalyze = 1;
            gauntlets.Attributes.BonusDex = 20;
            gauntlets.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            return gauntlets;
        }

        private Item CreateRapier()
        {
            Kryss rapier = new Kryss();
            rapier.Name = "Montcalm’s Blade";
            rapier.Hue = Utility.RandomMinMax(50, 250);
            rapier.MinDamage = Utility.Random(30, 80);
            rapier.MaxDamage = Utility.Random(80, 120);
            rapier.Attributes.BonusStr = 10;
            rapier.Attributes.SpellDamage = 5;
            rapier.Slayer = SlayerName.OrcSlaying;
            rapier.WeaponAttributes.HitFireball = 30;
            rapier.SkillBonuses.SetValues(0, SkillName.Fencing, 25.0);
            return rapier;
        }

        public ChamplainTreasureChest(Serial serial) : base(serial)
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
