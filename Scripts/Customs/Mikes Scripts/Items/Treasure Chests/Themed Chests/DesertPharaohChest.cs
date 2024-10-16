using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DesertPharaohChest : WoodenChest
    {
        [Constructable]
        public DesertPharaohChest()
        {
            Name = "Desert Pharaoh's Chest";
            Hue = 1153;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Desert Star"), 0.18);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Pharaoh's Golden Relic"), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Sandstone Sun Earring"), 1.0);
            AddItem(new Gold(Utility.Random(1, 6000)), 1.0);
            AddItem(CreateNamedItem<GreaterHealPotion>("Desert Oasis Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Desert Wanderer", 1122), 0.10);
            AddItem(CreateNamedItem<Spyglass>("Sand Seer's Spyglass"), 0.04);
            AddItem(CreateSandals(), 0.20);
            AddItem(CreateGorget(), 0.20);
            AddItem(CreateSpear(), 0.20);
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
            note.NoteString = "Beware those who seek the Pharaoh's secrets.";
            note.TitleString = "Pharaoh's Warning";
            return note;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Tidecaller's Sandals";
            sandals.Hue = Utility.RandomMinMax(350, 900);
            sandals.Attributes.SpellDamage = 5;
            sandals.Attributes.BonusDex = 7;
            sandals.SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            return sandals;
        }

        private Item CreateGorget()
        {
            LeatherGorget gorget = new LeatherGorget();
            gorget.Name = "Nature's Embrace Belt";
            gorget.Hue = Utility.RandomMinMax(1, 1000);
            gorget.BaseArmorRating = Utility.RandomMinMax(18, 50);
            gorget.AbsorptionAttributes.EaterEnergy = 25;
            gorget.Attributes.RegenMana = 5;
            gorget.Attributes.BonusInt = 30;
            gorget.SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
            gorget.SkillBonuses.SetValues(1, SkillName.Herding, 20.0);
            gorget.ColdBonus = 10;
            gorget.EnergyBonus = 15;
            gorget.FireBonus = 10;
            gorget.PhysicalBonus = 15;
            gorget.PoisonBonus = 10;
            return gorget;
        }

        private Item CreateSpear()
        {
            Spear spear = new Spear();
            spear.Name = "Voltaxic Rift Lance";
            spear.Hue = Utility.RandomMinMax(800, 1000);
            spear.MinDamage = Utility.RandomMinMax(25, 65);
            spear.MaxDamage = Utility.RandomMinMax(65, 95);
            spear.Attributes.LowerManaCost = 10;
            spear.Attributes.SpellChanneling = 1;
            spear.Slayer = SlayerName.ElementalBan;
            spear.WeaponAttributes.HitEnergyArea = 25;
            spear.WeaponAttributes.ResistPoisonBonus = 15;
            spear.SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            return spear;
        }

        public DesertPharaohChest(Serial serial) : base(serial)
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
