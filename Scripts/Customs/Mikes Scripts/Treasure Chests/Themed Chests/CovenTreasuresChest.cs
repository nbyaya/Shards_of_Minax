using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CovenTreasuresChest : WoodenChest
    {
        [Constructable]
        public CovenTreasuresChest()
        {
            Name = "Coven's Treasures Chest";
            Hue = 1192;

            // Add items to the chest
            AddItem(CreateNamedItem<Emerald>("Stone of the Coven"), 0.05);
            AddItem(CreateSimpleNote(), 0.20);
            AddItem(CreateNamedItem<TreasureLevel4>("Coven's Sacred Relic"), 0.16);
            AddItem(CreateColoredItem<GoldEarrings>("Raven's Feather Earring", 1102), 0.15);
            AddItem(new Gold(Utility.Random(1, 5000)), 1.0);
            AddItem(CreateNamedItem<Apple>("Enchanted Berry"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Coven's Ritual Drink"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Elder Witch", 1189), 0.12);
            AddItem(CreateNamedLootItem("Coven's Sacred Gem", typeof(Ruby)), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Coven Leader's Spyglass"), 0.04);
            AddItem(CreateShoes(), 1.0);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateShortSpear(), 0.20);
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

        private Item CreateNamedLootItem(string name, Type lootType)
        {
            Item item = (Item)Activator.CreateInstance(lootType);
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Together, our power is unmatched.";
            note.TitleString = "Coven's Oath";
            return note;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Artisan's Timber Shoes";
            shoes.Hue = Utility.RandomMinMax(650, 1600);
            shoes.ClothingAttributes.LowerStatReq = 3;
            shoes.Attributes.BonusStr = 5;
            shoes.SkillBonuses.SetValues(0, SkillName.Carpentry, 20.0);
            shoes.SkillBonuses.SetValues(1, SkillName.Lumberjacking, 15.0);
            return shoes;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Rat's Nest";
            cap.Hue = Utility.RandomMinMax(500, 800);
            cap.BaseArmorRating = Utility.Random(20, 60);
            cap.Attributes.AttackChance = 20;
            cap.Attributes.BonusDex = 10;
            cap.Attributes.WeaponSpeed = 15;
            cap.Attributes.Luck = 50;
            cap.ColdBonus = 10;
            cap.EnergyBonus = 10;
            cap.FireBonus = 10;
            cap.PhysicalBonus = 10;
            cap.PoisonBonus = 10;
            return cap;
        }

        private Item CreateShortSpear()
        {
            ShortSpear spear = new ShortSpear();
            spear.Name = "Thunderstroke";
            spear.Hue = Utility.RandomMinMax(500, 700);
            spear.MinDamage = Utility.Random(20, 60);
            spear.MaxDamage = Utility.Random(60, 90);
            spear.Attributes.IncreasedKarmaLoss = 5;
            spear.Attributes.AttackChance = 10;
            spear.WeaponAttributes.HitLightning = 40;
            spear.WeaponAttributes.HitMagicArrow = 20;
            spear.SkillBonuses.SetValues(0, SkillName.Throwing, 20.0);
            return spear;
        }

        public CovenTreasuresChest(Serial serial) : base(serial)
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
