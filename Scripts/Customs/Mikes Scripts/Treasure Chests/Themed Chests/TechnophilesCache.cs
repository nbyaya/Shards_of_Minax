using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TechnophilesCache : WoodenChest
    {
        [Constructable]
        public TechnophilesCache()
        {
            Name = "Technophile's Cache";
            Hue = Utility.Random(1, 1900);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<SilverNecklace>("USB Necklace"), 0.28);
            AddItem(CreateNamedItem<GoldEarrings>("Earbuds of Clarity"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel3>("Gadget Box"), 0.24);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateRandomWand(), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Connectivity"), 0.21);
            AddItem(CreateCyberVest(), 1.0); // Adjust probability if needed
            AddItem(CreateWitchRobe(), 0.20);
            AddItem(CreatePlateGorget(), 0.20);
            AddItem(CreateWarHammer(), 0.20);
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
            note.NoteString = "The dawn of the digital age.";
            note.TitleString = "Tech Blogger's Notes";
            return note;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Magical Smartphone";
            return wand;
        }

        private Item CreateCyberVest()
        {
            BaseArmor vest = Utility.RandomList<BaseArmor>(new ChainCoif(), new PlateChest(), new LeatherChest());
            vest.Name = "Cyber Vest";
            vest.Hue = Utility.RandomList(1, 1900);
            return vest;
        }

        private Item CreateWitchRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Witch's Bewitching Robe";
            robe.Hue = Utility.RandomList(1, 500);
            robe.ClothingAttributes.SelfRepair = 5;
            robe.Attributes.BonusInt = 20;
            robe.Attributes.SpellDamage = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            robe.SkillBonuses.SetValues(1, SkillName.Alchemy, 15.0);
            return robe;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget gorget = new PlateGorget();
            gorget.Name = "Cetra's Blessing";
            gorget.Hue = Utility.RandomMinMax(400, 800);
            gorget.BaseArmorRating = Utility.Random(40, 80);
            gorget.AbsorptionAttributes.ResonancePoison = 20;
            gorget.ArmorAttributes.MageArmor = 1;
            gorget.Attributes.RegenHits = 10;
            gorget.Attributes.RegenMana = 5;
            gorget.SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
            gorget.ColdBonus = 15;
            gorget.FireBonus = 10;
            gorget.PhysicalBonus = 10;
            gorget.PoisonBonus = 20;
            gorget.EnergyBonus = 10;
            return gorget;
        }

        private Item CreateWarHammer()
        {
            WarHammer hammer = new WarHammer();
            hammer.Name = "Vulcan's Forge Hammer";
            hammer.Hue = Utility.RandomMinMax(500, 700);
            hammer.MinDamage = Utility.Random(35, 90);
            hammer.MaxDamage = Utility.Random(90, 130);
            hammer.Attributes.DefendChance = 10;
            hammer.Attributes.RegenStam = 5;
            hammer.Slayer = SlayerName.FlameDousing;
            hammer.Slayer2 = SlayerName.BloodDrinking;
            hammer.WeaponAttributes.HitFireball = 40;
            hammer.WeaponAttributes.SelfRepair = 5;
            hammer.SkillBonuses.SetValues(0, SkillName.Blacksmith, 30.0);
            return hammer;
        }

        public TechnophilesCache(Serial serial) : base(serial)
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
