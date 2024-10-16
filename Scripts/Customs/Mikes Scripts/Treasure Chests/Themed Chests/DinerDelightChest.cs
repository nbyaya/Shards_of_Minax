using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DinerDelightChest : WoodenChest
    {
        [Constructable]
        public DinerDelightChest()
        {
            Name = "Diner Delight Chest";
            Hue = Utility.Random(1, 1920);

            // Add items to the chest
            AddItem(CreateNamedItem<GreaterHealPotion>("Vintage Milkshake"), 0.27);
            AddItem(CreateNamedItem<SilverNecklace>("Waitress's Locket"), 0.22);
            AddItem(CreateNamedItem<TreasureLevel1>("Diner Tip Jar"), 0.37);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.16);
            AddItem(CreateRandomClothing("Poodle Skirt"), 0.18);
            AddItem(CreateShoes(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
            AddItem(CreateBattleAxe(), 0.20);
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
            note.NoteString = "Best burgers in town!";
            note.TitleString = "Diner Menu";
            return note;
        }

        private Item CreateRandomClothing(string name)
        {
            Kilt clothing = new Kilt();
            clothing.Name = name;
            return clothing;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Swing Dancer's Shoes";
            shoes.Hue = Utility.RandomMinMax(100, 1500);
            shoes.Attributes.BonusDex = 10;
            shoes.Attributes.Luck = 20;
            shoes.SkillBonuses.SetValues(0, SkillName.Musicianship, 15.0);
            shoes.SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
            return shoes;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Wrestler's Arms of Precision";
            arms.Hue = Utility.RandomMinMax(10, 250);
            arms.BaseArmorRating = Utility.Random(45, 65);
            arms.ArmorAttributes.SelfRepair = 5;
            arms.Attributes.BonusDex = 15;
            arms.Attributes.DefendChance = 10;
            arms.SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
            arms.ColdBonus = 10;
            arms.EnergyBonus = 5;
            arms.FireBonus = 10;
            arms.PhysicalBonus = 15;
            arms.PoisonBonus = 5;
            return arms;
        }

        private Item CreateBattleAxe()
        {
            BattleAxe axe = new BattleAxe();
            axe.Name = "Fu Hao's Battle Axe";
            axe.Hue = Utility.RandomMinMax(250, 450);
            axe.MinDamage = Utility.Random(25, 65);
            axe.MaxDamage = Utility.Random(65, 105);
            axe.Attributes.BonusHits = 15;
            axe.Attributes.AttackChance = 10;
            axe.Slayer = SlayerName.OrcSlaying;
            axe.WeaponAttributes.HitLeechHits = 25;
            axe.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return axe;
        }

        public DinerDelightChest(Serial serial) : base(serial)
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
