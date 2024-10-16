using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class VenetianMerchantsStash : WoodenChest
    {
        [Constructable]
        public VenetianMerchantsStash()
        {
            Name = "Venetian Merchant's Stash";
            Hue = 2321;

            // Add items to the chest
            AddItem(new Emerald(), 0.18);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Merchant's Secret Bargain"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Venetian Sea Earring", 1300), 0.15);
            AddItem(new Gold(Utility.Random(1, 6000)), 0.19);
            AddItem(CreateNamedItem<Apple>("Canal's Sweet Fruit"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Venetian Ambrosia"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Gondolier", 1302), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Trade Master's Scope"), 0.13);
            AddItem(CreateLoot<TribalMask>("Mask of the Carnival", 1303), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreateWarHammer(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Trade is the lifeblood of Venice.";
            note.TitleString = "Merchant's Ledger";
            return note;
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

        private Item CreateLoot<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateArmor()
        {
            BaseClothing armor = Utility.RandomList<BaseClothing>(new Robe(), new Tunic(), new Shirt(), new Skirt());
            armor.Name = "Venetian Noble's Robe";
            armor.Hue = Utility.RandomMinMax(1, 1305);
            return armor;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Beggar's Lucky Bandana";
            bandana.Hue = Utility.RandomMinMax(700, 1700);
            bandana.Attributes.Luck = 20;
            bandana.SkillBonuses.SetValues(0, SkillName.Begging, 25.0);
            bandana.SkillBonuses.SetValues(1, SkillName.DetectHidden, 10.0);
            return bandana;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Ninja Wrappings";
            chest.Hue = Utility.RandomMinMax(500, 600);
            chest.BaseArmorRating = Utility.Random(35, 65);
            chest.AbsorptionAttributes.EaterEnergy = 15;
            chest.ArmorAttributes.LowerStatReq = 20;
            chest.Attributes.BonusDex = 15;
            chest.Attributes.RegenStam = 5;
            chest.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 15;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 15;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateWarHammer()
        {
            WarHammer hammer = new WarHammer();
            hammer.Name = "Juggernaut Hammer";
            hammer.Hue = Utility.RandomMinMax(300, 400);
            hammer.MinDamage = Utility.Random(40, 80);
            hammer.MaxDamage = Utility.Random(80, 120);
            hammer.Attributes.BonusHits = 20;
            hammer.Attributes.DefendChance = 10;
            hammer.Slayer = SlayerName.TrollSlaughter;
            hammer.WeaponAttributes.HitHarm = 35;
            hammer.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
            return hammer;
        }

        public VenetianMerchantsStash(Serial serial) : base(serial)
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
