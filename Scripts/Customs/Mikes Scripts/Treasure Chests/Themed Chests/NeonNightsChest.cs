using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class NeonNightsChest : WoodenChest
    {
        [Constructable]
        public NeonNightsChest()
        {
            Name = "Neon Nights Chest";
            Hue = Utility.Random(1, 2060);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.08);
            AddItem(CreateSapphireItem(), 0.26);
            AddItem(CreateNamedItem<GreaterHealPotion>("Glowing Elixir"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel1>("Fashionista's Fabric"), 0.29);
            AddItem(CreateNamedItem<GoldEarrings>("Earrings of Electric Avenue"), 0.33);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4400)), 0.19);
            AddItem(CreateRandomClothing(), 1.0);
            AddItem(CreateBodySash(), 0.2);
            AddItem(CreateLeatherChest(), 0.2);
            AddItem(CreateDagger(), 0.2);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSapphireItem()
        {
            Sapphire sapphire = new Sapphire();
            sapphire.Name = "Neon Blue Charm";
            return sapphire;
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
            note.NoteString = "Shine bright, party all night.";
            note.TitleString = "Fashion Flashback";
            return note;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Neon Nights Attire";
            clothing.Hue = Utility.RandomList(1, 2060);
            return clothing;
        }

        private Item CreateBodySash()
        {
            BodySash sash = new BodySash();
            sash.Name = "New Wave Neon Shades";
            sash.Hue = Utility.RandomMinMax(700, 1700);
            sash.Attributes.BonusInt = 10;
            sash.Attributes.NightSight = 1;
            sash.SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);
            return sash;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Summoner's Embrace";
            chest.Hue = Utility.RandomMinMax(250, 500);
            chest.BaseArmorRating = Utility.Random(25, 55);
            chest.AbsorptionAttributes.EaterEnergy = 15;
            chest.ArmorAttributes.MageArmor = 1;
            chest.Attributes.BonusInt = 25;
            chest.Attributes.CastRecovery = 2;
            chest.Attributes.SpellChanneling = 1;
            chest.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            chest.SkillBonuses.SetValues(1, SkillName.Spellweaving, 10.0);
            chest.ColdBonus = 15;
            chest.EnergyBonus = 20;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 5;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Qamar Dagger";
            dagger.Hue = Utility.RandomMinMax(350, 550);
            dagger.MinDamage = Utility.Random(20, 50);
            dagger.MaxDamage = Utility.Random(50, 80);
            dagger.Attributes.NightSight = 1;
            dagger.Attributes.BonusDex = 10;
            dagger.Slayer = SlayerName.OrcSlaying;
            dagger.WeaponAttributes.HitHarm = 25;
            dagger.WeaponAttributes.HitManaDrain = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            return dagger;
        }

        public NeonNightsChest(Serial serial) : base(serial)
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
