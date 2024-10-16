using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class KagesTreasureChest : WoodenChest
    {
        [Constructable]
        public KagesTreasureChest()
        {
            Name = "Kage's Treasure Chest";
            Hue = 1171;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateEmeraldItem(), 0.18);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel3>("Kage's Golden Stash"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Star of the Kage", 1172), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.18);
            AddItem(CreateNamedItem<Tessen>("Fan of Illusions"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Mist Elixir"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Kage", 1173), 0.17);
            AddItem(CreateRandomGem(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Mist Seer's Spyglass"), 0.12);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateLoggerBoots(), 0.20);
            AddItem(CreateBoneGloves(), 0.20);
            AddItem(CreateLongsword(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmeraldItem()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Kage's Heartstone";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Legends are not born, they are made.";
            note.TitleString = "Kage's Doctrine";
            return note;
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

        private Item CreateRandomGem()
        {
            return Loot.RandomGem(); // Assuming Loot class provides RandomGem method
        }

        private Item CreateRandomClothing()
        {
            return Loot.RandomClothing(); // Assuming Loot class provides RandomClothing method
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword()); // Adjust for actual weapon class
            weapon.Name = "Kage's Fury";
            weapon.Hue = 1174;
            weapon.MaxDamage = Utility.Random(25, 65);
            return weapon;
        }

        private Item CreateLoggerBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Logger's Sturdy Boots";
            boots.Hue = Utility.RandomMinMax(600, 1700);
            boots.ClothingAttributes.SelfRepair = 3;
            boots.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
            return boots;
        }

        private Item CreateBoneGloves()
        {
            BoneGloves gloves = new BoneGloves();
            gloves.Name = "Berserker's Embrace";
            gloves.Hue = Utility.RandomMinMax(300, 700);
            gloves.BaseArmorRating = Utility.Random(30, 65);
            gloves.AbsorptionAttributes.EaterKinetic = 15;
            gloves.ArmorAttributes.LowerStatReq = 20;
            gloves.Attributes.BonusStr = 30;
            gloves.Attributes.AttackChance = 15;
            gloves.Attributes.WeaponDamage = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 10;
            gloves.FireBonus = 15;
            gloves.PhysicalBonus = 20;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Thunderfury";
            longsword.Hue = Utility.RandomMinMax(500, 700);
            longsword.MinDamage = Utility.Random(25, 70);
            longsword.MaxDamage = Utility.Random(70, 110);
            longsword.Attributes.AttackChance = 10;
            longsword.Attributes.RegenStam = 5;
            longsword.Slayer = SlayerName.SummerWind;
            longsword.WeaponAttributes.HitLightning = 30;
            longsword.WeaponAttributes.HitManaDrain = 20;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return longsword;
        }

        public KagesTreasureChest(Serial serial) : base(serial)
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
