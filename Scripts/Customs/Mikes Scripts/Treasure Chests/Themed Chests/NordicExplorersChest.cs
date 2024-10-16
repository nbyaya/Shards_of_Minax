using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class NordicExplorersChest : WoodenChest
    {
        [Constructable]
        public NordicExplorersChest()
        {
            Name = "Nordic Explorer's Chest";
            Hue = 1152;

            // Add items to the chest
            AddItem(CreateNamedItem<Emerald>("Ice of the North"), 0.05);
            AddItem(CreateSimpleNote(), 0.20);
            AddItem(CreateNamedItem<TreasureLevel4>("Viking's Prize"), 0.16);
            AddItem(CreateColoredItem<GoldEarrings>("Nordic Bear Claw Earring", 1190), 0.15);
            AddItem(new Gold(Utility.Random(1, 5500)), 1.0);
            AddItem(CreateNamedItem<Apple>("Frostbitten Apple"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Viking's Brew"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Northern Explorer", 1150), 0.12);
            AddItem(CreateNamedItem<Spyglass>("Explorer's Trusted Spyglass"), 0.04);
            AddItem(CreateRandomWand("Wand of the Northern Winds"), 0.12);
            AddItem(CreateRandomClothing("Nordic Warrior's Garb"), 0.14);
            AddItem(CreateJArmor(), 0.20);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreateDagger(), 0.20);
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

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The winds of the north carry tales of valor.";
            note.TitleString = "Nordic Saga";
            return note;
        }

        private Item CreateRandomArmorOrShieldOrWeaponOrJewelry(string name)
        {
            Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();
            item.Name = name;
            return item;
        }

        private Item CreateRandomWand(string name)
        {
            Item item = Loot.RandomWand();
            item.Name = name;
            return item;
        }

        private Item CreateRandomClothing(string name)
        {
            Item item = Loot.RandomClothing();
            item.Name = name;
            return item;
        }

        private Item CreateJArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Nordic Guardian's Armor";
            armor.Hue = Utility.RandomList(1, 1154);
            armor.BaseArmorRating = Utility.Random(25, 60);
            return armor;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Ore Seeker's Bandana";
            bandana.Hue = Utility.RandomMinMax(200, 900);
            bandana.ClothingAttributes.SelfRepair = 4;
            bandana.Attributes.BonusStr = 5;
            bandana.SkillBonuses.SetValues(0, SkillName.ItemID, 20.0);
            bandana.SkillBonuses.SetValues(1, SkillName.Mining, 15.0);
            return bandana;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Witch's Enchanted Robe";
            chest.Hue = Utility.RandomMinMax(1, 1000);
            chest.BaseArmorRating = Utility.Random(20, 60);
            chest.AbsorptionAttributes.EaterEnergy = 30;
            chest.ArmorAttributes.MageArmor = 1;
            chest.Attributes.EnhancePotions = 20;
            chest.Attributes.SpellDamage = 10;
            chest.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            chest.SkillBonuses.SetValues(1, SkillName.Alchemy, 15.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 10;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 10;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Tabula's Dagger";
            dagger.Hue = Utility.RandomMinMax(900, 1000);
            dagger.MinDamage = Utility.Random(10, 35);
            dagger.MaxDamage = Utility.Random(35, 55);
            dagger.Attributes.CastSpeed = 1;
            dagger.Attributes.SpellChanneling = 1;
            dagger.Slayer = SlayerName.Fey;
            dagger.WeaponAttributes.MageWeapon = 1;
            dagger.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            dagger.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
            return dagger;
        }

        public NordicExplorersChest(Serial serial) : base(serial)
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
