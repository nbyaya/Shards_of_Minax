using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RockersVault : WoodenChest
    {
        [Constructable]
        public RockersVault()
        {
            Name = "Rocker's Vault";
            Hue = Utility.Random(1155, 1156);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Sapphire>("Mosh Pit Jewel", 2337), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel3>("Gig's Treasure"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Spiked Leather Band", 1172), 0.15);
            AddItem(new Gold(Utility.Random(1, 3500)), 0.18);
            AddItem(CreateNamedItem<Apple>("Pogo Apple"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Band's Brew"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Headbanger", 1154), 0.17);
            AddItem(CreateNamedItem<Spyglass>("Frontman's Focus"), 0.12);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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

        private Item CreateLootItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "If it's too loud, you're too old.";
            note.TitleString = "Rocker's Creed";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Stage Warrior's Armor";
            armor.Hue = Utility.RandomList(1, 1156);
            armor.BaseArmorRating = Utility.Random(25, 55);
            return armor;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Submission Artist's Muffler";
            muffler.Hue = Utility.RandomMinMax(300, 1300);
            muffler.Attributes.AttackChance = 10;
            muffler.Attributes.RegenStam = 2;
            muffler.SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
            return muffler;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Venomweave";
            legs.Hue = Utility.RandomMinMax(100, 300);
            legs.BaseArmorRating = Utility.Random(30, 70);
            legs.AbsorptionAttributes.EaterPoison = 20;
            legs.ArmorAttributes.MageArmor = 1;
            legs.Attributes.BonusDex = 20;
            legs.Attributes.DefendChance = 10;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            legs.ColdBonus = 5;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 15;
            legs.PoisonBonus = 25;
            return legs;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Atma Blade";
            longsword.Hue = Utility.RandomMinMax(900, 1000);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusHits = 20;
            longsword.Attributes.AttackChance = 10;
            longsword.Attributes.RegenStam = 5;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.WeaponAttributes.HitMagicArrow = 20;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return longsword;
        }

        public RockersVault(Serial serial) : base(serial)
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
