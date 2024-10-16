using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TeutonicTreasuresChest : WoodenChest
    {
        [Constructable]
        public TeutonicTreasuresChest()
        {
            Name = "Teutonic Treasures";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("German Gold Marks"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Black Forest Brew", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Luther's Legacy"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of the Hohenzollerns"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.17);
            AddItem(CreateColoredItem<Painting5NorthArtifact>("Painting of Neuschwanstein Castle", 2119), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Pure Rhine Wine"), 0.09);
            AddItem(CreateGoldItem("Prussian Gold Coin"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Bavarian Alps", 1177), 0.18);
            AddItem(CreateNamedItem<GoldRing>("Signet Ring of Barbarossa"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("German Explorer's Spyglass"), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of German Might"), 0.21);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.29);
            AddItem(CreateShortMusicStand(), 0.30);
            AddItem(CreateGauntlets(), 0.30);
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
            note.NoteString = "In remembrance of the mighty Germanic tribes and their successors";
            note.TitleString = "Memories of Deutschland";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Brandenburg Gate";
            map.Bounds = new Rectangle2D(2500, 2500, 700, 700);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Teutonic Knight's Blade";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Germanic Warrior's Plate";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateShortMusicStand()
        {
            PlateLegs musicStand = new PlateLegs();
            musicStand.Name = "Bach's Musical Legs";
            musicStand.Hue = Utility.RandomMinMax(600, 1600);
            musicStand.Attributes.DefendChance = 10;
            musicStand.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            musicStand.EnergyBonus = 20;
            return musicStand;
        }

        private Item CreateGauntlets()
        {
            PlateGloves gauntlets = new PlateGloves();
            gauntlets.Name = "Gauntlets of the Rhine";
            gauntlets.Hue = Utility.RandomMinMax(1, 1000);
            gauntlets.BaseArmorRating = Utility.Random(60, 90);
            gauntlets.AbsorptionAttributes.EaterEnergy = 30;
            gauntlets.ArmorAttributes.ReactiveParalyze = 1;
            gauntlets.Attributes.BonusDex = 20;
            gauntlets.Attributes.AttackChance = 10;
            gauntlets.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            gauntlets.PhysicalBonus = 25;
            gauntlets.PoisonBonus = 25;
            return gauntlets;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Bismarck's Sabre";
            longsword.Hue = Utility.RandomMinMax(50, 250);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.WeaponAttributes.HitLightning = 30;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
            return longsword;
        }

        public TeutonicTreasuresChest(Serial serial) : base(serial)
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
