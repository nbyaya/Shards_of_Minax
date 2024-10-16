using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class VirtuesGuardianChest : WoodenChest
    {
        [Constructable]
        public VirtuesGuardianChest()
        {
            Name = "Virtue's Guardian Chest";
            Hue = 1157;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Stone of Honesty", 1231), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel2>("Compassion's Gift"), 0.16);
            AddItem(CreateColoredItem<GoldEarrings>("Valor's Sigil Earring", 1109), 0.15);
            AddItem(new Gold(Utility.Random(1, 6000)), 0.18);
            AddItem(CreateNamedItem<Apple>("Sacrifice's Fruit"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Justice's Elixir"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of Humility", 1125), 0.17);
            AddItem(CreateLootItem<Sapphire>("Spirituality's Gem"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Honor's Insight"), 0.12);
            AddItem(CreateLootItem<Lute>("Melody of Honesty"), 0.14);
            AddItem(CreateLootItem<Robe>("Robe of the Avatar"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreateBascinet(), 0.20);
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

        private Item CreateLootItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "In the path of the Avatar, truth, love, and courage guide us.";
            note.TitleString = "Virtue's Codex";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Dagger();
            weapon.Name = "Blade of Valor";
            weapon.Hue = 1159;
            weapon.MaxDamage = Utility.Random(20, 60);
            return weapon;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Bowyer's Insightful Bandana";
            bandana.Hue = Utility.RandomMinMax(400, 1400);
            bandana.Attributes.BonusInt = 15;
            bandana.Attributes.CastSpeed = 1;
            bandana.SkillBonuses.SetValues(0, SkillName.Fletching, 20.0);
            bandana.SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);
            return bandana;
        }

        private Item CreateBascinet()
        {
            Bascinet bascinet = new Bascinet();
            bascinet.Name = "Frostwarden's Bascinet";
            bascinet.Hue = Utility.RandomMinMax(600, 650);
            bascinet.BaseArmorRating = Utility.Random(45, 75);
            bascinet.AbsorptionAttributes.EaterCold = 25;
            bascinet.ArmorAttributes.SelfRepair = 5;
            bascinet.Attributes.BonusInt = 10;
            bascinet.Attributes.RegenStam = 5;
            bascinet.SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            bascinet.ColdBonus = 25;
            bascinet.EnergyBonus = 5;
            bascinet.FireBonus = 0;
            bascinet.PhysicalBonus = 15;
            bascinet.PoisonBonus = 5;
            return bascinet;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Whisperwind Bow";
            bow.Hue = Utility.RandomMinMax(150, 300);
            bow.MinDamage = Utility.Random(20, 55);
            bow.MaxDamage = Utility.Random(55, 80);
            bow.Attributes.LowerRegCost = 10;
            bow.Attributes.BonusDex = 10;
            bow.Slayer = SlayerName.SummerWind;
            bow.WeaponAttributes.HitEnergyArea = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Healing, 10.0);
            return bow;
        }

        public VirtuesGuardianChest(Serial serial) : base(serial)
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
