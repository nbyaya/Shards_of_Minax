using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EnchantedForestChest : WoodenChest
    {
        [Constructable]
        public EnchantedForestChest()
        {
            Name = "Enchanted Forest Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Heart of the Forest", 1628), 0.05);
            AddItem(CreateSimpleNote(), 0.20);
            AddItem(CreateNamedItem<TreasureLevel2>("Forest's Hidden Treasure"), 0.17);
            AddItem(CreateColoredItem<GoldEarrings>("Elven Moon Earring", 1628), 0.15);
            AddItem(new Gold(Utility.Random(1, 4500)), 1.0);
            AddItem(CreateNamedItem<Apple>("Enchanted Apple"), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Forest Brew"), 0.08);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Forest Spirit", 1632), 0.16);
            AddItem(CreateNamedItem<Spyglass>("Fae Seer's Spyglass"), 0.04);
            AddItem(CreateArmor(), 0.18);
            AddItem(CreateTunic(), 0.2);
            AddItem(CreateGloves(), 0.2);
            AddItem(CreateBow(), 0.2);
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
            note.NoteString = "The magic of the forest is in its silence and tranquillity.";
            note.TitleString = "Forest Guardian's Whisper";
            return note;
        }

        private Item CreateRandomLoot<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Forest Protector's Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 60);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Deep Sea Tunic";
            tunic.Hue = Utility.RandomMinMax(500, 1500);
            tunic.Attributes.BonusMana = 10;
            tunic.SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
			tunic.SkillBonuses.SetValues(0, SkillName.DetectHidden, 15.0);
            return tunic;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Beastmaster's Grips";
            gloves.Hue = Utility.RandomMinMax(1, 1000);
            gloves.BaseArmorRating = Utility.Random(18, 52);
            gloves.AbsorptionAttributes.EaterFire = 20;
            gloves.Attributes.BonusStr = 20;
            gloves.Attributes.RegenStam = 4;
            gloves.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
            gloves.SkillBonuses.SetValues(1, SkillName.Herding, 15.0);
            gloves.ColdBonus = 10;
            gloves.EnergyBonus = 15;
            gloves.FireBonus = 15;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 10;
            return gloves;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Windripper Bow";
            bow.Hue = Utility.RandomMinMax(650, 850);
            bow.MinDamage = Utility.Random(20, 60);
            bow.MaxDamage = Utility.Random(60, 90);
            bow.Attributes.Luck = 150;
            bow.Attributes.AttackChance = 10;
            bow.Slayer = SlayerName.ElementalBan;
            bow.WeaponAttributes.HitLightning = 20;
            bow.WeaponAttributes.HitColdArea = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return bow;
        }

        public EnchantedForestChest(Serial serial) : base(serial)
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
