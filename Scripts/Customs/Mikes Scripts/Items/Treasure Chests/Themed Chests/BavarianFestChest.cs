using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BavarianFestChest : WoodenChest
    {
        [Constructable]
        public BavarianFestChest()
        {
            Name = "Bavarian Fest Chest";
            Hue = 2212;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Blue of Bavaria", 2774), 0.20);
            AddItem(CreateSimpleNote(), 0.18);
            AddItem(CreateNamedItem<TreasureLevel2>("Bavarian Golden Stein"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Edelweiss Bloom Earring", 2310), 0.15);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.20);
            AddItem(CreateNamedItem<Apple>("Alpine Apple"), 0.09);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bavarian Brew"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Festgoer", 2208), 0.15);
            AddItem(CreateRandomInstrument("German Fiddle"), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Mountain Watcher's Spyglass"), 0.12);
            AddItem(CreateRandomClothing("Traditional Lederhosen"), 0.13);
            AddItem(CreateRandomGem("Gem of the Rhine"), 0.20);
            AddItem(CreateJArmor(), 0.20);
            AddItem(CreateKilt(), 0.20);
            AddItem(CreatePlateBoots(), 0.20);
            AddItem(CreateBow(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "Prost to good times and hearty laughter!";
            note.TitleString = "Oktoberfest Toast";
            return note;
        }

        private Item CreateRandomInstrument(string name)
        {
            // Replace with actual implementation for random instruments
            Drums instrument = new Drums();
            instrument.Name = name;
            return instrument;
        }

        private Item CreateRandomClothing(string name)
        {
            // Replace with actual implementation for random clothing
            FancyShirt clothing = new FancyShirt();
            clothing.Name = name;
            return clothing;
        }

        private Item CreateRandomGem(string name)
        {
            // Replace with actual implementation for random gems
            Ruby gem = new Ruby();
            gem.Name = name;
            return gem;
        }

        private Item CreateJArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Bavarian Knight's Armor";
            armor.Hue = Utility.RandomList(1, 2215);
            armor.BaseArmorRating = Utility.Random(25, 60);
            return armor;
        }

        private Item CreateKilt()
        {
            Kilt kilt = new Kilt();
            kilt.Name = "Tamer's Kilt";
            kilt.Hue = Utility.RandomMinMax(300, 1350);
            kilt.Attributes.BonusDex = 10;
            kilt.Attributes.RegenStam = 3;
            kilt.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            kilt.SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0);
            return kilt;
        }

        private Item CreatePlateBoots()
        {
            PlateLegs boots = new PlateLegs();
            boots.Name = "Stormforged Boots";
            boots.Hue = Utility.RandomMinMax(550, 850);
            boots.BaseArmorRating = Utility.Random(35, 65);
            boots.AbsorptionAttributes.EaterPoison = 10;
            boots.ArmorAttributes.SelfRepair = 4;
            boots.Attributes.NightSight = 1;
            boots.Attributes.BonusStam = 15;
            boots.SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
            boots.EnergyBonus = 15;
            boots.ColdBonus = 5;
            boots.FireBonus = 5;
            boots.PhysicalBonus = 15;
            boots.PoisonBonus = 10;
            return boots;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Mystic Bow of Light";
            bow.Hue = Utility.RandomMinMax(500, 700);
            bow.MinDamage = Utility.Random(20, 60);
            bow.MaxDamage = Utility.Random(60, 90);
            bow.Attributes.NightSight = 1;
            bow.Attributes.AttackChance = 10;
            bow.Slayer = SlayerName.Exorcism;
            bow.WeaponAttributes.HitLightning = 25;
            return bow;
        }

        public BavarianFestChest(Serial serial) : base(serial)
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
