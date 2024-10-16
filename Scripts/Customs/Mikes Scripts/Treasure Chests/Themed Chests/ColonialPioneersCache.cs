using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ColonialPioneersCache : WoodenChest
    {
        [Constructable]
        public ColonialPioneersCache()
        {
            Name = "Colonial Pioneer's Cache";
            Hue = Utility.Random(1, 1100);

            // Add items to the chest
            AddItem(CreateNamedItem<SilverNecklace>("Pilgrim's Locket"), 0.09);
            AddItem(CreateSimpleNote(), 0.30);
            AddItem(CreateNamedItem<GreaterHealPotion>("Colonial Brew"), 0.30);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.24);
            AddItem(CreateNamedItem<Spyglass>("Settler's Looking Glass"), 0.27);
            AddItem(CreateRandomClothing(), 0.19);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateOrderShield(), 0.20);
            AddItem(CreateWarAxe(), 0.20);
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
            note.NoteString = "To the New World we journey!";
            note.TitleString = "Colonist's Diary";
            return note;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Colonial Garb";
            clothing.Hue = Utility.RandomList(1, 1100);
            return clothing;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Naturalist's Cloak";
            cloak.Hue = Utility.RandomMinMax(450, 1400);
            cloak.ClothingAttributes.SelfRepair = 2;
            cloak.Attributes.BonusMana = 10;
            cloak.SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
            cloak.SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
            return cloak;
        }

        private Item CreateOrderShield()
        {
            OrderShield shield = new OrderShield();
            shield.Name = "Blade Dancer's OrderShield";
            shield.Hue = Utility.RandomMinMax(600, 900);
            shield.BaseArmorRating = Utility.RandomMinMax(35, 70);
            shield.AbsorptionAttributes.EaterPoison = 10;
            shield.ArmorAttributes.ReactiveParalyze = 1;
            shield.Attributes.Luck = 25;
            shield.Attributes.ReflectPhysical = 15;
            shield.SkillBonuses.SetValues(0, SkillName.Swords, 10.0);
            shield.SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
            return shield;
        }

        private Item CreateWarAxe()
        {
            WarAxe axe = new WarAxe();
            axe.Name = "Umbra WarAxe";
            axe.Hue = Utility.RandomMinMax(600, 650);
            axe.MinDamage = Utility.RandomMinMax(30, 70);
            axe.MaxDamage = Utility.RandomMinMax(70, 110);
            axe.Attributes.SpellChanneling = 1;
            axe.Attributes.NightSight = 1;
            axe.Slayer = SlayerName.DaemonDismissal;
            axe.WeaponAttributes.HitManaDrain = 50;
            axe.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return axe;
        }

        public ColonialPioneersCache(Serial serial) : base(serial)
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
