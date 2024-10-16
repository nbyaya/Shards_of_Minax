using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GroovyVibesChest : WoodenChest
    {
        [Constructable]
        public GroovyVibesChest()
        {
            Name = "Groovy Vibes";
            Hue = Utility.Random(1, 2069);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.08);
            AddItem(CreateNamedItem<Sapphire>("Peace Crystal"), 0.25);
            AddItem(CreateNamedItem<Apple>("Magic Mushroom"), 0.20);
            AddItem(CreateNamedItem<TreasureLevel2>("Love Beads"), 0.24);
            AddItem(CreateNamedItem<SilverNecklace>("Flower Power Pendant"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4200)), 0.15);
            AddItem(CreateLootItem("Tie-Dye Shirt", 1, 2069), 0.20);
            AddItem(CreateNamedItem<Bandage>("Peace Band"), 0.18);
            AddItem(CreateNamedItem<Bag>("Groovy Coin Purse"), 1.0);
            AddItem(CreateCloseHelm(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
            AddItem(CreateBow(), 0.20);
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

        private Item CreateLootItem(string name, int minHue, int maxHue)
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = name;
            clothing.Hue = Utility.Random(minHue, maxHue);
            return clothing;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Make love, not war.";
            note.TitleString = "Hippie's Journal";
            return note;
        }

        private Item CreateCloseHelm()
        {
            CloseHelm helm = new CloseHelm();
            helm.Name = "Rockabilly Rebel Jacket";
            helm.Hue = Utility.Random(1, 1000);
            helm.Attributes.BonusStr = 15;
            helm.Attributes.DefendChance = 10;
            helm.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            helm.PhysicalBonus = 25;
            return helm;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Hammerlord's Armguards";
            arms.Hue = Utility.RandomMinMax(350, 650);
            arms.BaseArmorRating = Utility.Random(35, 75);
            arms.AbsorptionAttributes.EaterPoison = 10;
            arms.ArmorAttributes.SelfRepair = 5;
            arms.Attributes.AttackChance = 10;
            arms.SkillBonuses.SetValues(0, SkillName.Parry, 10.0);
            arms.PhysicalBonus = 20;
            arms.ColdBonus = 5;
            arms.EnergyBonus = 10;
            arms.FireBonus = 10;
            arms.PoisonBonus = 10;
            return arms;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Custer's Last Stand Bow";
            bow.Hue = Utility.RandomMinMax(350, 550);
            bow.MinDamage = Utility.Random(20, 60);
            bow.MaxDamage = Utility.Random(60, 90);
            bow.Attributes.AttackChance = 5;
            bow.Attributes.Luck = 100;
            bow.Slayer = SlayerName.DragonSlaying;
            bow.WeaponAttributes.HitFireball = 25;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
            return bow;
        }

        public GroovyVibesChest(Serial serial) : base(serial)
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
