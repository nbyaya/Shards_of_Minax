using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RomanEmperorsVault : WoodenChest
    {
        [Constructable]
        public RomanEmperorsVault()
        {
            Name = "Roman Emperor's Vault";
            Hue = 2307;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Sapphire>("Caesar's Gem"), 0.19);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel3>("Emperor's Golden Laurel"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Roman Sun Earring", 1155), 0.12);
            AddItem(new Gold(Utility.Random(1, 6500)), 0.18);
            AddItem(CreateNamedItem<Apple>("Gladiator's Reward"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Roman Wine"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Centurion", 1306), 0.17);
            AddItem(CreateNamedItem<Spyglass>("Emperor's Vision"), 0.04);
            AddItem(CreateNamedItem<Robe>("Toga of the Senate"), 0.14);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateLeatherGorget(), 0.20);
            AddItem(CreateDoubleAxe(), 0.20);
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
            note.NoteString = "Veni, Vidi, Vici.";
            note.TitleString = "Caesar's Note";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Longsword();
            weapon.Name = "Sword of the Legion";
            weapon.Hue = 1154;
            weapon.MaxDamage = Utility.Random(25, 60);
            return weapon;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Elementalist's Protective Cloak";
            cloak.Hue = Utility.RandomMinMax(500, 1700);
            cloak.Attributes.CastRecovery = 2;
            cloak.Attributes.NightSight = 1;
            cloak.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return cloak;
        }

        private Item CreateLeatherGorget()
        {
            LeatherGorget gorget = new LeatherGorget();
            gorget.Name = "Necklace of Aromatic Protection";
            gorget.Hue = Utility.RandomMinMax(250, 750);
            gorget.BaseArmorRating = Utility.Random(22, 52);
            gorget.AbsorptionAttributes.EaterPoison = 15;
            gorget.ArmorAttributes.MageArmor = 1;
            gorget.Attributes.LowerManaCost = 10;
            gorget.Attributes.SpellDamage = 5;
            gorget.SkillBonuses.SetValues(0, SkillName.Cooking, 10.0);
            gorget.ColdBonus = 10;
            gorget.EnergyBonus = 5;
            gorget.FireBonus = 5;
            gorget.PhysicalBonus = 5;
            gorget.PoisonBonus = 20;
            return gorget;
        }

        private Item CreateDoubleAxe()
        {
            DoubleAxe axe = new DoubleAxe();
            axe.Name = "Magic Axe of Great Strength";
            axe.Hue = Utility.RandomMinMax(100, 200);
            axe.MinDamage = Utility.Random(20, 60);
            axe.MaxDamage = Utility.Random(60, 100);
            axe.Attributes.BonusStr = 25;
            axe.Attributes.AttackChance = 10;
            axe.WeaponAttributes.HitFireball = 30;
            axe.SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
            return axe;
        }

        public RomanEmperorsVault(Serial serial) : base(serial)
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
