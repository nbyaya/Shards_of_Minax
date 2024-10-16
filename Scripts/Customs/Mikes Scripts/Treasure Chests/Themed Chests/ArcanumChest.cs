using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ArcanumChest : WoodenChest
    {
        [Constructable]
        public ArcanumChest()
        {
            Name = "Arcanum Chest";
            Movable = false;
            Hue = Utility.Random(1, 300);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.10);
            AddItem(CreateNamedItem<Spellbook>("Ancient Mage's Book"), 0.08);
            AddItem(CreateSimpleNote1(), 0.12);
            AddItem(CreateNamedItem<Emerald>("Emerald of Power"), 0.15);
            AddItem(CreateNamedItem<Spellbook>("Book of Ultimate Magic"), 0.14);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.20);
            AddItem(CreateSimpleNote2(), 1.0);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateDagger(), 0.20);
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

        private Item CreateSimpleNote1()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The spell within this chest is powerful, use with caution.";
            note.TitleString = "Note from the Archmage";
            return note;
        }

        private Item CreateSimpleNote2()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "This is the beginning of your magical journey.";
            note.TitleString = "Archmage's Journal";
            return note;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Miner's Sturdy Boots";
            boots.Hue = Utility.RandomMinMax(600, 1600);
            boots.ClothingAttributes.DurabilityBonus = 3;
            boots.Attributes.BonusStr = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
            boots.SkillBonuses.SetValues(1, SkillName.Lumberjacking, 15.0);
            return boots;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Rogue's Shadow Boots";
            legs.Hue = Utility.RandomMinMax(1, 1000);
            legs.BaseArmorRating = Utility.RandomMinMax(20, 60);
            legs.AbsorptionAttributes.EaterPoison = 25;
            legs.ArmorAttributes.LowerStatReq = 20;
            legs.Attributes.BonusDex = 40;
            legs.Attributes.NightSight = 1;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
            legs.SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Necromancer's Dagger";
            dagger.Hue = Utility.RandomMinMax(200, 800);
            dagger.MinDamage = Utility.RandomMinMax(5, 45);
            dagger.MaxDamage = Utility.RandomMinMax(45, 90);
            dagger.Attributes.SpellDamage = 15;
            dagger.Attributes.RegenMana = 5;
            dagger.Slayer = SlayerName.DaemonDismissal;
            dagger.WeaponAttributes.HitCurse = 20;
            dagger.WeaponAttributes.HitManaDrain = 25;
            dagger.SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            return dagger;
        }

        public ArcanumChest(Serial serial) : base(serial)
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
