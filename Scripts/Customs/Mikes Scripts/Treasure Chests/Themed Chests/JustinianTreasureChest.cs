using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JustinianTreasureChest : WoodenChest
    {
        [Constructable]
        public JustinianTreasureChest()
        {
            Name = "Justinian's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Solidus"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Byzantine Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Justinian’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of Theodora"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Byzantines", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Nicaean Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Nomisma"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Emperor", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Christ Pantocrator"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Justinian’s Reconnaissance Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Orthodox Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateCloak(), 0.30);
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
            note.NoteString = "I am Justinian, emperor of the Romans, restorer of the empire, builder of Hagia Sophia, codifier of Roman law";
            note.TitleString = "Justinian’s Edict";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Justinian’s Mausoleum";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Justinian’s Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Justinian’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Cloak of the Purple Born";
            cloak.Hue = Utility.RandomMinMax(600, 1600);
            cloak.ClothingAttributes.DurabilityBonus = 5;
            cloak.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return cloak;
        }

        private Item CreateGauntlets()
        {
            PlateGloves gauntlets = new PlateGloves();
            gauntlets.Name = "Gauntlets of the Lawgiver";
            gauntlets.Hue = Utility.RandomMinMax(1, 1000);
            gauntlets.BaseArmorRating = Utility.Random(60, 90);
            gauntlets.AbsorptionAttributes.EaterFire = 30;
            gauntlets.AbsorptionAttributes.EaterEnergy = 30;
            gauntlets.AbsorptionAttributes.EaterCold = 30;
            gauntlets.AbsorptionAttributes.EaterPoison = 30;
            gauntlets.AbsorptionAttributes.EaterDamage = 30;
            gauntlets.AbsorptionAttributes.ResonanceFire = 30;
            gauntlets.AbsorptionAttributes.ResonanceEnergy = 30;
            gauntlets.AbsorptionAttributes.ResonanceCold = 30;
            gauntlets.AbsorptionAttributes.ResonancePoison = 30;
            gauntlets.AbsorptionAttributes.ResonanceKinetic = 30;
            gauntlets.SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
            return gauntlets;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Belisarius’s Blade";
            longsword.Hue = Utility.RandomMinMax(50, 250);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.Slayer2 = SlayerName.DaemonDismissal;
			longsword.SkillBonuses.SetValues(0, SkillName.Swords, 50.0);
            return longsword;
        }

        public JustinianTreasureChest(Serial serial) : base(serial)
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
