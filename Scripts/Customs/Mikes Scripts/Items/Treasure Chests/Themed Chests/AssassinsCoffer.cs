using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AssassinsCoffer : WoodenChest
    {
        [Constructable]
        public AssassinsCoffer()
        {
            Name = "Assassin's Coffer";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Ruby>("Bloodstone Jewel", 1490), 0.24);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Poisonmaster's Drink", 1490), 0.18);
            AddItem(CreateNamedItem<TreasureLevel3>("Assassin's Prize"), 0.20);
            AddItem(CreateNamedItem<Emerald>("Emerald of the Silent Night"), 0.22);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 6000)), 0.14);
            AddItem(CreateNamedItem<AssassinSpike>("Nightfall Spike"), 0.12);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Venomous Brew"), 0.13);
            AddItem(CreateRandomClothing(), 0.16);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateKryss(), 0.20);
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
            note.NoteString = "The blade's edge is sharper in the shadows.";
            note.TitleString = "Assassin's Codex";
            return note;
        }

        private Item CreateRandomClothing()
        {
            Item clothing = Loot.RandomClothing();
            clothing.Name = "Assassin's Attire";
            return clothing;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Death's Whisper";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(35, 75);
            return weapon;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Mariner's Lucky Boots";
            boots.Hue = Utility.RandomMinMax(600, 1600);
            boots.Attributes.Luck = 20;
            boots.Attributes.BonusStam = 5;
            boots.SkillBonuses.SetValues(0, SkillName.Fishing, 15.0);
            return boots;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Nature's Guard Boots";
            legs.Hue = Utility.RandomMinMax(1, 1000);
            legs.BaseArmorRating = Utility.Random(20, 55);
            legs.AbsorptionAttributes.EaterPoison = 30;
            legs.AbsorptionAttributes.EaterEnergy = 20;
            legs.Attributes.BonusStam = 30;
            legs.Attributes.NightSight = 1;
            legs.SkillBonuses.SetValues(0, SkillName.Veterinary, 20.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 15;
            return legs;
        }

        private Item CreateKryss()
        {
            Kryss kryss = new Kryss();
            kryss.Name = "Shavronne's Rapier";
            kryss.Hue = Utility.RandomMinMax(500, 700);
            kryss.MinDamage = Utility.Random(10, 40);
            kryss.MaxDamage = Utility.Random(40, 60);
            kryss.Attributes.BonusMana = 20;
            kryss.Attributes.SpellDamage = 10;
            kryss.Slayer = SlayerName.ElementalHealth;
            kryss.WeaponAttributes.HitLeechMana = 30;
            kryss.WeaponAttributes.MageWeapon = 1;
            kryss.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            kryss.SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
            return kryss;
        }

        public AssassinsCoffer(Serial serial) : base(serial)
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
