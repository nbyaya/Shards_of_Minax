using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TreasureChestOfTheQinDynasty : WoodenChest
    {
        [Constructable]
        public TreasureChestOfTheQinDynasty()
        {
            Name = "Treasure Chest of the Qin Dynasty";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateJade(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Qin Shi Huang’s Elixir of Immortality", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Terracotta Army"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Dragon Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Qin Palace", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Grape Wine"), 0.08);
            AddItem(CreateGoldItem("Qin Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Qin General", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Phoenix Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Qin Shi Huang’s Navigational Tool"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Brew"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateLongPants(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateDagger(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateJade()
        {
            Diamond jade = new Diamond();
            jade.Name = "Jade of the First Emperor";
            return jade;
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
            note.NoteString = "I have unified the six warring states and established the first empire of China. I have ordered the construction of the Great Wall and the Grand Canal to protect and connect my realm. I have standardized the writing system, the currency, the weights and measures, and the laws. I have burned the books and buried the scholars who opposed me. I have sought the secret of eternal life and sent many expeditions to find the legendary islands of immortality.";
            note.TitleString = "Qin Shi Huang’s Memoir";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Qin Shi Huang’s Mausoleum";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Sword of Qin";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Qin Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateLongPants()
        {
            LongPants pants = new LongPants();
            pants.Name = "Street Artist's Baggy Pants";
            pants.Hue = Utility.RandomMinMax(300, 1100);
            pants.Attributes.Luck = 20;
            pants.Attributes.BonusMana = 5;
            pants.SkillBonuses.SetValues(0, SkillName.Begging, 15.0);
            pants.SkillBonuses.SetValues(1, SkillName.Provocation, 10.0);
            return pants;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Thief's Nimble Cap";
            cap.Hue = Utility.RandomMinMax(400, 700);
            cap.BaseArmorRating = Utility.Random(15, 50);
            cap.AbsorptionAttributes.EaterEnergy = 10;
            cap.ArmorAttributes.LowerStatReq = 20;
            cap.Attributes.BonusDex = 25;
            cap.Attributes.NightSight = 1;
            cap.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.Stealing, 15.0);
            cap.ColdBonus = 5;
            cap.EnergyBonus = 15;
            cap.FireBonus = 5;
            cap.PhysicalBonus = 20;
            cap.PoisonBonus = 10;
            return cap;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Tanto of the 47 Ronin";
            dagger.Hue = Utility.RandomMinMax(350, 400);
            dagger.MinDamage = Utility.Random(15, 50);
            dagger.MaxDamage = Utility.Random(50, 85);
            dagger.Attributes.BonusHits = 10;
            dagger.Attributes.ReflectPhysical = 10;
            dagger.Slayer = SlayerName.OrcSlaying;
            dagger.WeaponAttributes.HitHarm = 15;
            dagger.WeaponAttributes.SelfRepair = 5;
            dagger.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return dagger;
        }

        public TreasureChestOfTheQinDynasty(Serial serial) : base(serial)
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
