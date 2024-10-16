using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class NecroAlchemicalChest : WoodenChest
    {
        [Constructable]
        public NecroAlchemicalChest()
        {
            Name = "Necro-Alchemical Chest";
            Hue = 1157;

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Necro-Emerald", 1664), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel4>("Necrotic Nugget"), 0.14);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Dreadful Draught"), 0.14);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.17);
            AddItem(CreateColoredItem<GreaterHealPotion>("Brew of the Undead", 1156), 0.11);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Soul Trapping"), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Bone Observed"), 0.12);
            AddItem(CreateNamedItem<Shaft>("Wand of Death's Alchemy"), 0.13);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateBearMask(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
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
            note.NoteString = "Where death meets craft.";
            note.TitleString = "Necro-Alchemist's Note";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Halberd());
            weapon.Name = "Blade of the Death Alchemist";
            weapon.Hue = 1158;
            weapon.MaxDamage = Utility.Random(25, 60);
            return weapon;
        }

        private Item CreateBearMask()
        {
            BearMask mask = new BearMask();
            mask.Name = "Surgeon's Insightful Mask";
            mask.Hue = Utility.RandomMinMax(240, 1240);
            mask.Attributes.BonusInt = 10;
            mask.Attributes.RegenHits = 3;
            mask.SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
            mask.SkillBonuses.SetValues(1, SkillName.Anatomy, 20.0);
            return mask;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Fortune's Plate Arms";
            arms.Hue = Utility.RandomMinMax(700, 950);
            arms.BaseArmorRating = Utility.Random(35, 70);
            arms.AbsorptionAttributes.EaterKinetic = 10;
            arms.ArmorAttributes.ReactiveParalyze = 1;
            arms.Attributes.Luck = 125;
            arms.SkillBonuses.SetValues(0, SkillName.Carpentry, 10.0);
            arms.ColdBonus = 5;
            arms.EnergyBonus = 10;
            arms.FireBonus = 15;
            arms.PhysicalBonus = 10;
            arms.PoisonBonus = 10;
            return arms;
        }

        private Item CreateHalberd()
        {
            Halberd halberd = new Halberd();
            halberd.Name = "Ultima Glaive";
            halberd.Hue = Utility.RandomMinMax(250, 450);
            halberd.MinDamage = Utility.Random(40, 80);
            halberd.MaxDamage = Utility.Random(80, 130);
            halberd.Attributes.LowerManaCost = 10;
            halberd.Attributes.SpellDamage = 5;
            halberd.Slayer = SlayerName.ElementalBan;
            halberd.WeaponAttributes.MageWeapon = 1;
            halberd.WeaponAttributes.HitFireball = 15;
            halberd.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return halberd;
        }

        public NecroAlchemicalChest(Serial serial) : base(serial)
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
