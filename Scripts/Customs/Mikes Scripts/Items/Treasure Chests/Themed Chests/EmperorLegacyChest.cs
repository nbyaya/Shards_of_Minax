using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EmperorLegacyChest : WoodenChest
    {
        [Constructable]
        public EmperorLegacyChest()
        {
            Name = "Emperor's Legacy";
            Hue = Utility.Random(1, 1400);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Jade Seal of the Emperor"), 0.30);
            AddItem(CreateNamedItem<Gold>("Golden Tribute"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel4>("Imperial Regalia"), 0.25);
            AddItem(CreateNamedItem<GoldEarrings>("Dragon's Whisper Earrings"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 6000)), 0.15);
            AddItem(CreateRandomGem(), 0.20);
            AddItem(CreateCeremonialArmor(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreateSilksOfVictor(), 0.20);
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
            note.NoteString = "Long live the Emperor, beacon of our dynasty.";
            note.TitleString = "Royal Decree";
            return note;
        }

        private Item CreateRandomGem()
        {
            // Assuming a RandomGem is an item that needs to be created
            Ruby gem = new Ruby();
            gem.Name = "Heaven's Blessing";
            return gem;
        }

        private Item CreateCeremonialArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Ceremonial Armor";
            armor.Hue = Utility.RandomList(1, 1400);
            armor.BaseArmorRating = Utility.Random(30, 65);
            armor.Attributes.BonusStr = 20;
            armor.Attributes.BonusMana = 25;
            armor.Attributes.Luck = 15;
            armor.ArmorAttributes.LowerStatReq = 10;
            armor.PhysicalBonus = 15;
            armor.ColdBonus = 10;
            armor.EnergyBonus = 15;
            armor.FireBonus = 10;
            armor.PoisonBonus = 15;
            return armor;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Tamer's Muffler";
            muffler.Hue = Utility.RandomMinMax(250, 1300);
            muffler.ClothingAttributes.SelfRepair = 3;
            muffler.Attributes.Luck = 20;
            muffler.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            muffler.SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
            return muffler;
        }

        private Item CreateSilksOfVictor()
        {
            StuddedChest chest = new StuddedChest();
            chest.Name = "Silks of the Victor";
            chest.Hue = Utility.RandomMinMax(100, 400);
            chest.BaseArmorRating = Utility.Random(30, 65);
            chest.ArmorAttributes.LowerStatReq = 10;
            chest.Attributes.BonusStr = 20;
            chest.Attributes.BonusMana = 25;
            chest.Attributes.Luck = 15;
            chest.PhysicalBonus = 15;
            chest.ColdBonus = 10;
            chest.EnergyBonus = 15;
            chest.FireBonus = 10;
            chest.PoisonBonus = 15;
            return chest;
        }

        private Item CreateWarAxe()
        {
            WarAxe axe = new WarAxe();
            axe.Name = "Charlemagne's WarAxe";
            axe.Hue = Utility.RandomMinMax(200, 400);
            axe.MinDamage = Utility.Random(25, 70);
            axe.MaxDamage = Utility.Random(70, 120);
            axe.Attributes.AttackChance = 10;
            axe.Attributes.DefendChance = 10;
            axe.Slayer = SlayerName.OrcSlaying;
            axe.WeaponAttributes.BattleLust = 25;
            axe.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            return axe;
        }

        public EmperorLegacyChest(Serial serial) : base(serial)
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
