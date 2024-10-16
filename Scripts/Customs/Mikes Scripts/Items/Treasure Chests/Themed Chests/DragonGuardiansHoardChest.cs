using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DragonGuardiansHoardChest : WoodenChest
    {
        [Constructable]
        public DragonGuardiansHoardChest()
        {
            Name = "Dragon Guardian's Hoard";
            Hue = Utility.Random(1, 1588);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<TreasureLevel3>("Dragon's Treasured Cache"), 0.22);
            AddItem(CreateWeapon(), 0.10);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 6500)), 0.14);
            AddItem(CreateNamedItem<Spellbook>("Dragon Spellcaster's Tome"), 0.03);
            AddItem(CreateKilt(), 0.20);
            AddItem(CreateStuddedGloves(), 0.20);
            AddItem(CreateHarmonyBow(), 0.20);
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
            note.NoteString = "Here lies the treasure of the mighty Dragon Guardian.";
            note.TitleString = "Dragon's Roar";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Dragon Claw Blade";
            weapon.Hue = Utility.RandomList(1, 1588);
            weapon.MaxDamage = Utility.Random(2, 5);
            return weapon;
        }

        private Item CreateKilt()
        {
            Kilt kilt = new Kilt();
            kilt.Name = "Fishmonger's Kilt";
            kilt.Hue = Utility.RandomMinMax(500, 1500);
            kilt.Attributes.BonusDex = 5;
            kilt.Attributes.RegenHits = 2;
            kilt.SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            return kilt;
        }

        private Item CreateStuddedGloves()
        {
            StuddedGloves gloves = new StuddedGloves();
            gloves.Name = "Jester's Gleeful Gloves";
            gloves.Hue = Utility.RandomMinMax(500, 800);
            gloves.BaseArmorRating = Utility.Random(20, 55);
            gloves.AbsorptionAttributes.EaterCold = 20;
            gloves.ArmorAttributes.MageArmor = 1;
            gloves.Attributes.CastSpeed = 1;
            gloves.Attributes.BonusInt = 30;
            gloves.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            gloves.ColdBonus = 10;
            gloves.EnergyBonus = 10;
            gloves.FireBonus = 10;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 10;
            return gloves;
        }

        private Item CreateHarmonyBow()
        {
            Bow bow = new Bow();
            bow.Name = "Harmony Bow";
            bow.Hue = Utility.RandomMinMax(200, 400);
            bow.MinDamage = Utility.Random(15, 55);
            bow.MaxDamage = Utility.Random(55, 85);
            bow.Attributes.LowerRegCost = 10;
            bow.Attributes.Luck = 100;
            bow.Slayer = SlayerName.Fey;
            bow.WeaponAttributes.HitLeechMana = 20;
            bow.WeaponAttributes.MageWeapon = 1;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
            return bow;
        }

        public DragonGuardiansHoardChest(Serial serial) : base(serial)
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
