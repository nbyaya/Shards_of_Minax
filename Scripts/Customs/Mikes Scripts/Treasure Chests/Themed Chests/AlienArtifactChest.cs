using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AlienArtifactChest : WoodenChest
    {
        [Constructable]
        public AlienArtifactChest()
        {
            Name = "Alien Artifact";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Emerald>("Alien Crystal", 1153), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Nanite Fluid", 1154), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Alien Technology"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Star Pendant"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Alien Eye", 1175), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Mutagen Serum"), 0.08);
            AddItem(CreateGoldItem("Galactic Credit"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Explorer", 1176), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Satellite Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Alien Scanner"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Regeneration"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateDagger(), 0.20);
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
            note.NoteString = "We have found a strange device in the ruins.";
            note.TitleString = "Dr. Xeno’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Alien Base";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Bow());
            weapon.Name = "Laser Blaster";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest());
            armor.Name = "Alien Suit";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Quivermaster's Tunic";
            tunic.Hue = Utility.RandomMinMax(500, 1500);
            tunic.ClothingAttributes.DurabilityBonus = 3;
            tunic.Attributes.RegenStam = 2;
            tunic.SkillBonuses.SetValues(0, SkillName.Fletching, 20.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Archery, 10.0);
            return tunic;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Frostwarden's PlateChest";
            plateChest.Hue = Utility.RandomMinMax(600, 650);
            plateChest.BaseArmorRating = Utility.Random(50, 80);
            plateChest.AbsorptionAttributes.ResonanceCold = 20;
            plateChest.ArmorAttributes.DurabilityBonus = 10;
            plateChest.Attributes.BonusMana = 15;
            plateChest.Attributes.CastRecovery = 2;
            plateChest.SkillBonuses.SetValues(0, SkillName.Meditation, 15.0);
            plateChest.ColdBonus = 30;
            plateChest.EnergyBonus = 10;
            plateChest.FireBonus = 0;
            plateChest.PhysicalBonus = 20;
            plateChest.PoisonBonus = 5;
            return plateChest;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "MageMasher";
            dagger.Hue = Utility.RandomMinMax(500, 700);
            dagger.MinDamage = Utility.Random(15, 50);
            dagger.MaxDamage = Utility.Random(50, 75);
            dagger.Attributes.BonusInt = -10;
            dagger.Attributes.SpellDamage = -5;
            dagger.Slayer = SlayerName.ElementalBan;
            dagger.WeaponAttributes.HitDispel = 40;
            dagger.WeaponAttributes.HitManaDrain = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 15.0);
            return dagger;
        }

        public AlienArtifactChest(Serial serial) : base(serial)
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
