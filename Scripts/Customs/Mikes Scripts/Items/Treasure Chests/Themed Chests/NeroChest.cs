using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class NeroChest : WoodenChest
    {
        [Constructable]
        public NeroChest()
        {
            Name = "Nero's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateMaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Emerald>("Emerald of the Tyrant"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Nero's Flaming Liquor", 1175), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Nero's Riches"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Lyre Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 15000)), 0.15);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Fire", 1175), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Roman Liquor"), 0.08);
            AddItem(CreateGoldItem("Aureus"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Arsonist", 1175), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Sun Earrings"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Lute>("Nero's Burning Glass"), 0.13);
            AddItem(CreateNamedItem<GreaterExplosionPotion>("Bottle of Flaming Brew"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateLongPants(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateMaul(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateMaxxiaScroll()
        {
            MaxxiaScroll scroll = new MaxxiaScroll();
            scroll.Name = "Nero's Scroll of Madness";
            scroll.Hue = 1175;
            return scroll;
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
            note.NoteString = "Quo usque tandem abutere patientia nostra?";
            note.TitleString = "Cicero's Speech Against Nero";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Nero's Hidden Palace";
            map.Bounds = new Rectangle2D(2000, -2000, -2000, -4000);
            map.NewPin = new Point2D(1500, -3000);
            map.Protected = false;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Maul());
            weapon.Name = "Fasces Infernus";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(40, 80);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Lorica Musculata";
            armor.Hue = Utility.RandomList(1, 1788);
            return armor;
        }

        private Item CreateLongPants()
        {
            LongPants pants = new LongPants();
            pants.Name = "Baggy Hip-Hop Pants";
            pants.Hue = Utility.RandomMinMax(700, 1700);
            pants.Attributes.BonusStam = 15;
            pants.Attributes.NightSight = 1;
            pants.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            pants.SkillBonuses.SetValues(1, SkillName.Musicianship, 10.0);
            return pants;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "White Sage Cap";
            cap.Hue = Utility.RandomMinMax(750, 950);
            cap.BaseArmorRating = Utility.Random(20, 45);
            cap.Attributes.BonusMana = 20;
            cap.Attributes.LowerManaCost = 10;
            cap.Attributes.RegenMana = 5;
            cap.SkillBonuses.SetValues(0, SkillName.Healing, 15.0);
            cap.SkillBonuses.SetValues(1, SkillName.Meditation, 10.0);
            cap.ColdBonus = 10;
            cap.EnergyBonus = 10;
            cap.FireBonus = 5;
            cap.PhysicalBonus = 10;
            cap.PoisonBonus = 15;
            return cap;
        }

        private Item CreateMaul()
        {
            Maul maul = new Maul();
            maul.Name = "Maul of Sulayman";
            maul.Hue = Utility.RandomMinMax(250, 450);
            maul.MinDamage = Utility.Random(35, 75);
            maul.MaxDamage = Utility.Random(75, 115);
            maul.Attributes.BonusInt = 12;
            maul.Attributes.LowerManaCost = 10;
            maul.Slayer = SlayerName.DaemonDismissal;
            maul.Slayer2 = SlayerName.ElementalHealth;
            maul.WeaponAttributes.HitDispel = 35;
            maul.WeaponAttributes.MageWeapon = 1;
            maul.SkillBonuses.SetValues(0, SkillName.MagicResist, 15.0);
            return maul;
        }

        public NeroChest(Serial serial) : base(serial)
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
