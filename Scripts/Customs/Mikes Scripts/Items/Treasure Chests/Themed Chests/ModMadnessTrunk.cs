using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ModMadnessTrunk : WoodenChest
    {
        [Constructable]
        public ModMadnessTrunk()
        {
            Name = "Mod Madness Trunk";
            Hue = Utility.Random(1, 2040);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Emerald>("Union Jack Jewel"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel1>("Mini Skirt"), 0.28);
            AddItem(CreateNamedItem<GoldEarrings>("Beatle Boots"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4400)), 0.12);
            AddItem(CreateRandomClothing("Mod Suit"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("British Brew"), 0.20);
            AddItem(CreateWizardsHat(), 0.20);
            AddItem(CreatePlateHelm(), 0.20);
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
            note.NoteString = "Twist and shout!";
            note.TitleString = "Fashionista's Diary";
            return note;
        }

        private Item CreateRandomClothing(string name)
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = name;
            clothing.Hue = Utility.RandomList(1, 2040);
            return clothing;
        }

        private Item CreateWizardsHat()
        {
            WizardsHat hat = new WizardsHat();
            hat.Name = "Dapper Fedora of Insight";
            hat.Hue = Utility.RandomMinMax(300, 1100);
            hat.Attributes.BonusInt = 12;
            hat.Attributes.RegenMana = 3;
            hat.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return hat;
        }

        private Item CreatePlateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Alchemist's Visionary Helm";
            helm.Hue = Utility.RandomMinMax(500, 750);
            helm.BaseArmorRating = Utility.Random(30, 65);
            helm.AbsorptionAttributes.EaterPoison = 20;
            helm.ArmorAttributes.SelfRepair = 4;
            helm.Attributes.BonusInt = 15;
            helm.Attributes.EnhancePotions = 10;
            helm.SkillBonuses.SetValues(0, SkillName.Alchemy, 25.0);
            helm.ColdBonus = 5;
            helm.EnergyBonus = 5;
            helm.FireBonus = 10;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 15;
            return helm;
        }

        private Item CreateWarAxe()
        {
            WarAxe axe = new WarAxe();
            axe.Name = "Tomahawk of Tecumseh";
            axe.Hue = Utility.RandomMinMax(250, 450);
            axe.MinDamage = Utility.Random(20, 60);
            axe.MaxDamage = Utility.Random(60, 80);
            axe.Attributes.BonusStr = 10;
            axe.Attributes.DefendChance = 5;
            axe.Slayer = SlayerName.OrcSlaying;
            axe.WeaponAttributes.HitHarm = 25;
            axe.WeaponAttributes.BattleLust = 15;
            axe.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return axe;
        }

        public ModMadnessTrunk(Serial serial) : base(serial)
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
