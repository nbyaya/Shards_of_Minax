using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ArcadeKingsTreasure : WoodenChest
    {
        [Constructable]
        public ArcadeKingsTreasure()
        {
            Name = "Arcade King's Treasure";
            Hue = Utility.Random(1, 2400);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Sapphire>("Pixel Power Stone"), 0.26);
            AddItem(CreateNamedItem<Apple>("Power-Up Apple"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel2>("Token Trove"), 0.24);
            AddItem(CreateNamedItem<SilverNecklace>("Controller Charm"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 3500)), 0.12);
            AddItem(CreateRandomWand(), 0.15);
            AddItem(CreateJArmor(), 0.20);
            AddItem(CreateLongPants(), 0.20);
            AddItem(CreateChainChest(), 0.20);
            AddItem(CreateQuarterStaff(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateColoredItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = Utility.RandomList(1, 2400); // This can be adjusted as needed
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
            note.NoteString = "High Score Achieved!";
            note.TitleString = "Gamer's Guide";
            return note;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Wand of the Game Master";
            return wand;
        }

        private Item CreateJArmor()
        {
            LeatherArms armor = new LeatherArms();
            armor.Name = "Gameboy Armor";
            armor.Hue = Utility.RandomList(1, 2400); // Adjust hue range as needed
            armor.Attributes.BonusDex = 15;
            armor.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return armor;
        }

        private Item CreateLongPants()
        {
            LongPants pants = new LongPants();
            pants.Name = "Skater's Baggy Pants";
            pants.Hue = Utility.RandomMinMax(400, 1400);
            pants.Attributes.BonusDex = 15;
            pants.ClothingAttributes.LowerStatReq = 3;
            pants.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return pants;
        }

        private Item CreateChainChest()
        {
            ChainChest chainChest = new ChainChest();
            chainChest.Name = "Edgar's Engineer Chainmail";
            chainChest.Hue = Utility.RandomMinMax(250, 550);
            chainChest.BaseArmorRating = Utility.RandomMinMax(40, 75);
            chainChest.AbsorptionAttributes.ResonanceKinetic = 15;
            chainChest.Attributes.Luck = 20;
            chainChest.Attributes.BonusStr = 10;
            chainChest.SkillBonuses.SetValues(0, SkillName.Tinkering, 20.0);
            chainChest.SkillBonuses.SetValues(1, SkillName.Blacksmith, 20.0);
            chainChest.ColdBonus = 5;
            chainChest.EnergyBonus = 10;
            chainChest.FireBonus = 20;
            chainChest.PhysicalBonus = 15;
            chainChest.PoisonBonus = 10;
            return chainChest;
        }

        private Item CreateQuarterStaff()
        {
            QuarterStaff staff = new QuarterStaff();
            staff.Name = "Caduceus Staff";
            staff.Hue = Utility.RandomMinMax(350, 550);
            staff.MinDamage = Utility.RandomMinMax(15, 55);
            staff.MaxDamage = Utility.RandomMinMax(55, 85);
            staff.Attributes.CastSpeed = 3;
            staff.Attributes.LowerManaCost = 10;
            staff.Slayer = SlayerName.ReptilianDeath;
            staff.WeaponAttributes.MageWeapon = 1;
            staff.WeaponAttributes.HitLeechMana = 15;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return staff;
        }

        public ArcadeKingsTreasure(Serial serial) : base(serial)
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
