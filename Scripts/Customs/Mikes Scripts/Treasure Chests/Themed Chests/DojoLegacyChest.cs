using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DojoLegacyChest : WoodenChest
    {
        [Constructable]
        public DojoLegacyChest()
        {
            Name = "Dojo Legacy Chest";
            Hue = 1152;

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Spirit of the Dojo", 1223), 0.20);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel4>("Dojo's Ancestral Relic"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Dojo Dragon Earring", 1176), 0.18);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.18);
            AddItem(CreateNamedItem<Apple>("Dojo's Blessing"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Ninja's Meditation Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Trainee", 1177), 0.15);
            AddItem(CreateRandomItem<WoodenShield>(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Dojo's Trusted Spyglass"), 0.12);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreateBuckler(), 0.20);
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

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Discipline is the bridge between goals and accomplishment.";
            note.TitleString = "Dojo Principles";
            return note;
        }

        private Item CreateRandomItem<T>() where T : Item, new()
        {
            return new T();
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Dojo Protector's Armor";
            armor.Hue = Utility.RandomList(1, 1154);
            armor.BaseArmorRating = Utility.Random(25, 60);
            return armor;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Woodcutter's Protective Gloves";
            bandana.Hue = Utility.RandomMinMax(550, 1450);
            bandana.Attributes.BonusDex = 10;
            bandana.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
            return bandana;
        }

        private Item CreateBuckler()
        {
            Buckler buckler = new Buckler();
            buckler.Name = "Rogue's Stealth Shield";
            buckler.Hue = Utility.RandomMinMax(500, 800);
            buckler.BaseArmorRating = Utility.Random(25, 60);
            buckler.AbsorptionAttributes.ResonanceKinetic = 10;
            buckler.ArmorAttributes.ReactiveParalyze = 1;
            buckler.Attributes.BonusDex = 20;
            buckler.Attributes.NightSight = 1;
            buckler.Attributes.RegenStam = 5;
            buckler.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            buckler.SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
            buckler.ColdBonus = 5;
            buckler.EnergyBonus = 5;
            buckler.FireBonus = 5;
            buckler.PhysicalBonus = 20;
            buckler.PoisonBonus = 10;
            return buckler;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Dead Man's Legacy";
            bow.Hue = Utility.RandomMinMax(400, 600);
            bow.MinDamage = Utility.Random(15, 55);
            bow.MaxDamage = Utility.Random(55, 95);
            bow.Attributes.WeaponSpeed = 5;
            bow.Attributes.BonusDex = 15;
            bow.Slayer = SlayerName.ArachnidDoom;
            bow.WeaponAttributes.HitPoisonArea = 30;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
            return bow;
        }

        public DojoLegacyChest(Serial serial) : base(serial)
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
