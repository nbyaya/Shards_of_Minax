using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CelticLegendsChest : WoodenChest
    {
        [Constructable]
        public CelticLegendsChest()
        {
            Name = "Celtic Legends Chest";
            Hue = 2309;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Cauldron's Brew", 2843), 0.18);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel3>("Brigid's Blessing"), 0.14);
            AddItem(CreateColoredItem<GoldEarrings>("Stone Circle Earring", 1355), 0.05);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.20);
            AddItem(CreateNamedItem<Apple>("Golden Apple of Emain Ablach"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Celtic Mead"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Tuatha Dé Danann", 1302), 0.17);
            AddItem(CreateLootItem<Ruby>("Crystal of Tir na nÓg"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Hero's Gaze"), 0.12);
            AddItem(CreateLootItem<Lute>("Bard's Lyre"), 0.15);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateVikingSword(), 0.20);
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
            note.NoteString = "In the shadows of ancient stones, legends are born.";
            note.TitleString = "Druid's Prophecy";
            return note;
        }

        private Item CreateLootItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new VikingSword());
            weapon.Name = "Sword of Cú Chulainn";
            weapon.Hue = 1350;
            weapon.MaxDamage = Utility.Random(28, 65);
            return weapon;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Herder's Muffler";
            muffler.Hue = Utility.RandomMinMax(400, 1300);
            muffler.ClothingAttributes.LowerStatReq = 3;
            muffler.Attributes.BonusInt = 10;
            muffler.SkillBonuses.SetValues(0, SkillName.Herding, 20.0);
            muffler.SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);
            return muffler;
        }

        private Item CreatePlateChest()
        {
            PlateChest chest = new PlateChest();
            chest.Name = "Stormforged PlateChest";
            chest.Hue = Utility.RandomMinMax(550, 850);
            chest.BaseArmorRating = Utility.Random(50, 80);
            chest.AbsorptionAttributes.ResonanceEnergy = 15;
            chest.ArmorAttributes.DurabilityBonus = 20;
            chest.Attributes.CastRecovery = 1;
            chest.Attributes.BonusMana = 20;
            chest.SkillBonuses.SetValues(0, SkillName.EvalInt, 15.0);
            chest.EnergyBonus = 25;
            chest.ColdBonus = 10;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 10;
            chest.PoisonBonus = 5;
            return chest;
        }

        private Item CreateVikingSword()
        {
            VikingSword sword = new VikingSword();
            sword.Name = "Reflection Shield";
            sword.Hue = Utility.RandomMinMax(150, 350);
            sword.MinDamage = Utility.Random(30, 70);
            sword.MaxDamage = Utility.Random(70, 110);
            sword.Attributes.ReflectPhysical = 10;
            sword.Attributes.BonusHits = 10;
            sword.Slayer = SlayerName.ArachnidDoom;
            sword.WeaponAttributes.ResistPhysicalBonus = 10;
            sword.WeaponAttributes.HitDispel = 20;
            sword.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            return sword;
        }

        public CelticLegendsChest(Serial serial) : base(serial)
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
