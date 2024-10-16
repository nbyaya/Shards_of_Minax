using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PopStarsTrove : WoodenChest
    {
        [Constructable]
        public PopStarsTrove()
        {
            Name = "Pop Star's Trove";
            Hue = Utility.Random(1, 1750);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Sapphire>("Diva's Jewel"), 0.29);
            AddItem(CreateInstrument("Auto-Tuned Lute"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel2>("Platinum Album"), 0.24);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4600)), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Charisma"), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreateBuckler(), 0.20);
            AddItem(CreateBattleAxe(), 0.20);
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
            return item;
        }

        private Item CreateInstrument(string name)
        {
            Lute instrument = new Lute(); // You need to use a specific type of instrument, if applicable
            instrument.Name = name;
            return instrument;
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
            note.NoteString = "Chart-toppers and breakthroughs.";
            note.TitleString = "Pop Culture Magazine";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Star-studded Armor";
            armor.Hue = Utility.RandomList(1, 1750);
            return armor;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Ghostly Shroud";
            robe.Hue = Utility.RandomMinMax(800, 900);
            robe.ClothingAttributes.MageArmor = 1;
            robe.Attributes.RegenMana = 3;
            robe.SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 30.0);
            return robe;
        }

        private Item CreateBuckler()
        {
            Buckler buckler = new Buckler();
            buckler.Name = "Meteor Ward";
            buckler.Hue = Utility.RandomMinMax(500, 900);
            buckler.BaseArmorRating = Utility.Random(30, 65);
            buckler.AbsorptionAttributes.ResonanceFire = 25;
            buckler.ArmorAttributes.ReactiveParalyze = 1;
            buckler.Attributes.DefendChance = 25;
            buckler.Attributes.SpellDamage = -10;
            buckler.SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
            buckler.FireBonus = 20;
            buckler.ColdBonus = 10;
            buckler.PhysicalBonus = 15;
            buckler.EnergyBonus = 10;
            buckler.PoisonBonus = 10;
            return buckler;
        }

        private Item CreateBattleAxe()
        {
            BattleAxe axe = new BattleAxe();
            axe.Name = "Mars's BattleAxe of Valor";
            axe.Hue = Utility.RandomMinMax(400, 600);
            axe.MinDamage = Utility.Random(40, 95);
            axe.MaxDamage = Utility.Random(95, 135);
            axe.Attributes.BonusStr = 20;
            axe.Attributes.AttackChance = 15;
            axe.Slayer = SlayerName.OrcSlaying;
            axe.Slayer2 = SlayerName.DragonSlaying;
            axe.WeaponAttributes.HitHarm = 35;
            axe.WeaponAttributes.BattleLust = 25;
            axe.SkillBonuses.SetValues(0, SkillName.Tactics, 30.0);
            return axe;
        }

        public PopStarsTrove(Serial serial) : base(serial)
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
