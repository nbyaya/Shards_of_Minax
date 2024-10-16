using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GamersLootbox : WoodenChest
    {
        [Constructable]
        public GamersLootbox()
        {
            Name = "Gamer's Lootbox";
            Hue = Utility.Random(1, 1600);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Emerald>("Pixelated Power-up"), 0.25);
            AddItem(CreateNamedItem<GoldEarrings>("Headset of Strategy"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel4>("Cartridge Collection"), 0.26);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.16);
            AddItem(CreateNamedItem<MaxxiaScroll>("Game Console of Sorcery"), 0.19);
            AddItem(CreateJArmor(), 1.0);
            AddItem(CreateWideBrimHat(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateLongsword(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "High scores and boss battles.";
            note.TitleString = "Gamer's Diary";
            return note;
        }

        private Item CreateJArmor()
        {
            PlateArms armor = new PlateArms(); // Assume JArmor is a custom class you have or need to define.
            armor.Name = "Digital Camo Gear";
            armor.Hue = Utility.RandomList(1, 1600);
            return armor;
        }

        private Item CreateWideBrimHat()
        {
            WideBrimHat hat = new WideBrimHat();
            hat.Name = "Pumpkin King's Crown";
            hat.Hue = Utility.RandomMinMax(35, 38);
            hat.Attributes.BonusStr = 10;
            hat.Attributes.RegenStam = 2;
            hat.SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
            hat.SkillBonuses.SetValues(1, SkillName.Cooking, 20.0);
            return hat;
        }

        private Item CreatePlateChest()
        {
            PlateChest chest = new PlateChest();
            chest.Name = "Avatar's Vestments";
            chest.Hue = Utility.RandomMinMax(100, 300);
            chest.BaseArmorRating = Utility.RandomMinMax(50, 80);
            chest.AbsorptionAttributes.EaterFire = 15;
            chest.ArmorAttributes.DurabilityBonus = 25;
            chest.Attributes.BonusInt = 20;
            chest.Attributes.SpellDamage = 10;
            chest.ColdBonus = 15;
            chest.EnergyBonus = 10;
            chest.FireBonus = 20;
            chest.PhysicalBonus = 10;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Alucard's Blade";
            longsword.Hue = Utility.RandomMinMax(900, 950);
            longsword.MinDamage = Utility.RandomMinMax(30, 60);
            longsword.MaxDamage = Utility.RandomMinMax(60, 90);
            longsword.Attributes.BonusInt = 15;
            longsword.Attributes.LowerManaCost = 5;
            longsword.Slayer = SlayerName.Ophidian;
            longsword.Slayer2 = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitMagicArrow = 30;
            longsword.WeaponAttributes.MageWeapon = 1;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            longsword.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
            return longsword;
        }

        public GamersLootbox(Serial serial) : base(serial)
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
