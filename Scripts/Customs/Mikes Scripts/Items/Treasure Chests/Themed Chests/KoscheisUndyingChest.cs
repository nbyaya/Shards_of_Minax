using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class KoscheisUndyingChest : WoodenChest
    {
        [Constructable]
        public KoscheisUndyingChest()
        {
            Name = "Koschei's Undying Chest";
            Hue = 2156;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Crystal of the Immortal", 2851), 0.18);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel4>("Koschei's Iron Relic"), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Deathless Glow"), 0.18);
            AddItem(new Gold(Utility.Random(1, 6000)), 0.18);
            AddItem(CreateNamedItem<Apple>("Forbidden Siberian Apple"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Immortal's Mead"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Undying", 1159), 0.15);
            AddItem(CreateNamedItem<WoodenShield>("Koschei's Cursed Shield"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Deathless Observer's Spyglass"), 0.04);
            AddItem(CreateNamedItem<Garlic>("Reagent of the Undying"), 0.12);
            AddItem(CreateNamedItem<BlackPearl>("Necromancy of Koschei"), 0.13);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateSkirt(), 0.20);
            AddItem(CreateBoneArms(), 0.20);
            AddItem(CreateDagger(), 0.20);
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
            note.NoteString = "In the depths of the chest, lies the secret of immortality.";
            note.TitleString = "Koschei's Secret";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of the Deathless";
            armor.Hue = Utility.RandomList(1, 1171);
            armor.BaseArmorRating = Utility.Random(35, 75);
            return armor;
        }

        private Item CreateSkirt()
        {
            Skirt skirt = new Skirt();
            skirt.Name = "Dancer's Enchanted Skirt";
            skirt.Hue = Utility.RandomMinMax(50, 1400);
            skirt.Attributes.BonusDex = 20;
            skirt.Attributes.WeaponSpeed = 5;
            skirt.SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
            skirt.SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);
            return skirt;
        }

        private Item CreateBoneArms()
        {
            BoneArms boneArms = new BoneArms();
            boneArms.Name = "Necromancer's Shadow Boots";
            boneArms.Hue = Utility.RandomMinMax(10, 250);
            boneArms.BaseArmorRating = Utility.Random(25, 65);
            boneArms.AbsorptionAttributes.EaterKinetic = 10;
            boneArms.ArmorAttributes.ReactiveParalyze = 1;
            boneArms.Attributes.NightSight = 1;
            boneArms.Attributes.RegenHits = 5;
            boneArms.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            boneArms.ColdBonus = 10;
            boneArms.EnergyBonus = 5;
            boneArms.FireBonus = 10;
            boneArms.PhysicalBonus = 15;
            boneArms.PoisonBonus = 20;
            return boneArms;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Keenstrike";
            dagger.Hue = Utility.RandomMinMax(200, 400);
            dagger.MinDamage = Utility.Random(30, 50);
            dagger.MaxDamage = Utility.Random(50, 90);
            dagger.Attributes.AttackChance = 10;
            dagger.Attributes.SpellDamage = 15;
            dagger.Slayer = SlayerName.ElementalBan;
            dagger.WeaponAttributes.HitMagicArrow = 30;
            dagger.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return dagger;
        }

        public KoscheisUndyingChest(Serial serial) : base(serial)
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
