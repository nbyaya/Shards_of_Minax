using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GrungeRockersCache : WoodenChest
    {
        [Constructable]
        public GrungeRockersCache()
        {
            Name = "Grunge Rocker's Cache";
            Hue = Utility.Random(1, 2500);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Emerald>("Nirvana's Gem"), 0.24);
            AddItem(CreateNamedItem<GreaterHealPotion>("Cobain's Brew"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel3>("Vinyl of the '90s"), 0.22);
            AddItem(CreateNamedItem<GoldEarrings>("Pearl Jam Loop"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 3000)), 0.12);
            AddItem(CreateRandomInstrument(), 0.14);
            AddItem(CreateJWeapon(), 0.2);
            AddItem(CreateBandana(), 0.2);
            AddItem(CreateFemaleStuddedChest(), 0.2);
            AddItem(CreateWarHelmet(), 0.2);
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
            note.NoteString = "It smells like teen spirit.";
            note.TitleString = "Grunge Diary";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            Lute instrument = new Lute();
            instrument.Name = "Seattle Sound Guitar";
            return instrument;
        }

        private Item CreateJWeapon()
        {
            Club jWeapon = new Club();
            jWeapon.Name = "MTV Award Bat";
            jWeapon.MaxDamage = Utility.Random(40, 80);
            jWeapon.MinDamage = Utility.Random(20, 50);
            return jWeapon;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Pop Star's Sparkling Bandana";
            bandana.Hue = Utility.RandomMinMax(250, 1250);
            bandana.Attributes.NightSight = 1;
            bandana.Attributes.Luck = 20;
            bandana.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            return bandana;
        }

        private Item CreateFemaleStuddedChest()
        {
            FemaleStuddedChest chest = new FemaleStuddedChest();
            chest.Name = "Terra's Mystic Robe";
            chest.Hue = Utility.RandomMinMax(600, 900);
            chest.BaseArmorRating = Utility.Random(25, 60);
            chest.AbsorptionAttributes.EaterEnergy = 20;
            chest.Attributes.BonusMana = 30;
            chest.Attributes.SpellChanneling = 1;
            chest.Attributes.EnhancePotions = 10;
            chest.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 20;
            chest.FireBonus = 15;
            chest.PhysicalBonus = 5;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateWarHelmet()
        {
            Longsword helmet = new Longsword();
            helmet.Name = "Sword of Darkness";
            helmet.Hue = Utility.RandomMinMax(100, 300);
            helmet.MaxDamage = Utility.Random(50, 80);
            helmet.MinDamage = Utility.Random(20, 50);
            helmet.Attributes.NightSight = 1;
            helmet.Attributes.RegenStam = 5;
            helmet.Slayer = SlayerName.DaemonDismissal;
            helmet.WeaponAttributes.HitManaDrain = 20;
            helmet.WeaponAttributes.LowerStatReq = 10;
            helmet.SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
            return helmet;
        }

        public GrungeRockersCache(Serial serial) : base(serial)
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
