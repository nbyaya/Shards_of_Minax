using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RhineValleyChest : WoodenChest
    {
        [Constructable]
        public RhineValleyChest()
        {
            Name = "Rhine Valley Chest";
            Hue = 2102;

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Jewel of the Rhine", 2773), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel4>("Rhine's Ancient Coin"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Grapevine Earring", 1105), 0.20);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.20);
            AddItem(CreateNamedItem<Apple>("Rhenish Apple"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Rhine Wine"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Vinekeeper", 1108), 0.15);
            AddItem(CreateNamedItem<Spyglass>("River Navigator's Spyglass"), 0.12);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateWizardsHat(), 0.20);
            AddItem(CreateApronOfFlames(), 0.20);
            AddItem(CreateHalberd(), 0.20);
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
            note.NoteString = "Where the river flows, stories unfold.";
            note.TitleString = "Rhine's Legacy";
            return note;
        }

        private Item CreateRandomLootItem<T>() where T : Item, new()
        {
            return new T();
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherArms(), new LeatherLegs(), new LeatherCap());
            armor.Name = "Rhine Guardian's Armor";
            armor.Hue = Utility.Random(1, 1109);
            armor.BaseArmorRating = Utility.Random(25, 60);
            return armor;
        }

        private Item CreateWizardsHat()
        {
            WizardsHat hat = new WizardsHat();
            hat.Name = "Witch's Brewed Hat";
            hat.Hue = Utility.RandomMinMax(200, 900);
            hat.Attributes.RegenMana = 3;
            hat.Attributes.EnhancePotions = 15;
            hat.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            return hat;
        }

        private Item CreateApronOfFlames()
        {
            StuddedChest apron = new StuddedChest();
            apron.Name = "Apron of Flames";
            apron.Hue = Utility.RandomMinMax(300, 700);
            apron.BaseArmorRating = Utility.Random(30, 65);
            apron.AbsorptionAttributes.EaterFire = 20;
            apron.ArmorAttributes.LowerStatReq = 20;
            apron.Attributes.ReflectPhysical = 10;
            apron.Attributes.CastRecovery = 1;
            apron.SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
            apron.FireBonus = 25;
            apron.PhysicalBonus = 10;
            apron.PoisonBonus = 5;
            return apron;
        }

        private Item CreateHalberd()
        {
            Halberd halberd = new Halberd();
            halberd.Name = "Halberd of Honesty";
            halberd.Hue = Utility.RandomMinMax(250, 450);
            halberd.MinDamage = Utility.Random(25, 70);
            halberd.MaxDamage = Utility.Random(70, 100);
            halberd.Attributes.ReflectPhysical = 10;
            halberd.Attributes.BonusInt = 15;
            halberd.Slayer = SlayerName.Fey;
            halberd.WeaponAttributes.HitManaDrain = 20;
            return halberd;
        }

        public RhineValleyChest(Serial serial) : base(serial)
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
