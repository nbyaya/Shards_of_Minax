using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PharaohsTreasure : WoodenChest
    {
        [Constructable]
        public PharaohsTreasure()
        {
            Name = "Pharaoh's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Ruby>("Ruby of the Nile"), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Ancient Wine of Egypt"), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Pharaoh's Riches"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Scarab Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Sun", 1153), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Cleopatra’s Favorite Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Ankh"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Pharaoh", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Cobra Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Pharaoh’s Navigational Tool"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Mysterious Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateWideBrimHat(), 0.20);
            AddItem(CreatePlateGorget(), 0.20);
            AddItem(CreateMaul(), 0.20);
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
            note.NoteString = "I have hidden the secret of the pyramids in this chest.";
            note.TitleString = "Pharaoh’s Last Words";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Hidden Tomb";
            map.Bounds = new Rectangle2D(5000, 5000, 400, 400);
            map.NewPin = new Point2D(5100, 5150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Sword of the Pharaoh";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Pharaoh’s Best";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateWideBrimHat()
        {
            WideBrimHat hat = new WideBrimHat();
            hat.Name = "Forester's Wide-Brim Hat";
            hat.Hue = Utility.RandomMinMax(500, 1500);
            hat.Attributes.RegenMana = 2;
            hat.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
            return hat;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget gorget = new PlateGorget();
            gorget.Name = "Minstrel's Melody";
            gorget.Hue = Utility.RandomMinMax(400, 800);
            gorget.BaseArmorRating = Utility.Random(30, 65);
            gorget.AbsorptionAttributes.ResonanceEnergy = 10;
            gorget.ArmorAttributes.SelfRepair = 5;
            gorget.Attributes.BonusMana = 25;
            gorget.Attributes.EnhancePotions = 10;
            gorget.Attributes.RegenMana = 5;
            gorget.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            gorget.SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
            gorget.ColdBonus = 10;
            gorget.EnergyBonus = 10;
            gorget.FireBonus = 5;
            gorget.PhysicalBonus = 5;
            gorget.PoisonBonus = 5;
            return gorget;
        }

        private Item CreateMaul()
        {
            Maul maul = new Maul();
            maul.Name = "The Furnace";
            maul.Hue = Utility.RandomMinMax(300, 500);
            maul.MinDamage = Utility.Random(35, 85);
            maul.MaxDamage = Utility.Random(85, 125);
            maul.Attributes.BonusStr = 10;
            maul.Attributes.LowerManaCost = 5;
            maul.Slayer = SlayerName.DragonSlaying;
            maul.Slayer2 = SlayerName.DaemonDismissal;
            maul.WeaponAttributes.HitFireArea = 30;
            maul.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return maul;
        }

        public PharaohsTreasure(Serial serial) : base(serial)
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
