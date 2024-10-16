using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class VictorianEraChest : WoodenChest
    {
        [Constructable]
        public VictorianEraChest()
        {
            Name = "Victorian Era";
            Hue = Utility.Random(1, 1850);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.08);
            AddItem(CreateNamedItem<GoldEarrings>("Empress's Jewels"), 0.30);
            AddItem(CreateNamedItem<Sapphire>("Colonial Sapphire"), 0.29);
            AddItem(CreateNamedItem<TreasureLevel4>("Victorian Keepsake"), 0.28);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5200)), 0.15);
            AddItem(CreateWeapon(), 0.17);
            AddItem(CreateArmor(), 0.17);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateHelmet(), 0.20);
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
            note.NoteString = "Notes from Queen Victoria's diary.";
            note.TitleString = "Victorian Memoirs";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Longsword(); // Replace JWeapon with actual weapon class if it exists
            weapon.Name = "Victorian Cane Sword";
            weapon.Hue = Utility.RandomList(1, 1850);
            weapon.MaxDamage = Utility.Random(3, 5);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = new LeatherChest(); // Replace JArmor with actual armor class if it exists
            armor.Name = "Gentleman's Attire";
            armor.Hue = Utility.RandomList(1, 1850);
            armor.BaseArmorRating = Utility.Random(3, 5);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Beastmaster's Tunic";
            tunic.Hue = Utility.RandomMinMax(700, 1700);
            tunic.ClothingAttributes.SelfRepair = 4;
            tunic.Attributes.BonusDex = 15;
            tunic.Attributes.BonusInt = 10;
            tunic.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Herding, 20.0);
            return tunic;
        }

        private Item CreateHelmet()
        {
            Helmet helmet = new Helmet();
            helmet.Name = "The Thinking Cap";
            helmet.Hue = Utility.RandomMinMax(150, 650);
            helmet.BaseArmorRating = Utility.RandomMinMax(40, 80);
            helmet.AbsorptionAttributes.EaterEnergy = 20;
            helmet.ArmorAttributes.MageArmor = 1;
            helmet.Attributes.BonusMana = 40;
            helmet.Attributes.CastRecovery = 2;
            helmet.Attributes.SpellChanneling = 1;
            helmet.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            helmet.ColdBonus = 10;
            helmet.EnergyBonus = 20;
            helmet.FireBonus = 10;
            helmet.PhysicalBonus = 10;
            helmet.PoisonBonus = 5;
            return helmet;
        }

        private Item CreateWarAxe()
        {
            WarAxe warAxe = new WarAxe();
            warAxe.Name = "Flamebane WarAxe";
            warAxe.Hue = Utility.RandomMinMax(400, 600);
            warAxe.MinDamage = Utility.RandomMinMax(20, 70);
            warAxe.MaxDamage = Utility.RandomMinMax(70, 110);
            warAxe.Attributes.BonusStam = 10;
            warAxe.Attributes.Luck = 100;
            warAxe.Slayer = SlayerName.FlameDousing;
            warAxe.WeaponAttributes.HitFireArea = 40;
            warAxe.WeaponAttributes.SelfRepair = 3;
            return warAxe;
        }

        public VictorianEraChest(Serial serial) : base(serial)
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
