using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class InfernalPlaneChest : WoodenChest
    {
        [Constructable]
        public InfernalPlaneChest()
        {
            Name = "Infernal Plane Chest";
            Hue = 2157;

            // Add items to the chest
            AddItem(CreateColoredItem<Ruby>("Heart of the Inferno", 2994), 0.20);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel4>("Firelord's Bounty"), 0.14);
            AddItem(CreateColoredItem<GoldEarrings>("Ember's Touch Earring", 1175), 0.10);
            AddItem(new Gold(Utility.Random(1, 6500)), 0.19);
            AddItem(CreateNamedItem<Apple>("Scorched Apple"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Lava Brew"), 0.15);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Blaze", 1163), 0.16);
            AddItem(CreateNamedItem<Spyglass>("Fireseer's Spyglass"), 0.04);
            AddItem(CreateNamedItem<Shaft>("Wand of Eternal Flame"), 0.12);
            AddItem(CreateWeapon(), 0.12);
            AddItem(CreateFurCape(), 0.20);
            AddItem(CreatePlateGloves(), 0.20);
            AddItem(CreateGnarledStaff(), 0.20);
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
            note.NoteString = "Burned by the flames, reborn in the ashes.";
            note.TitleString = "Infernal Prophecy";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Blade of the Inferno";
            weapon.Hue = 1158;
            weapon.MaxDamage = Utility.Random(35, 75);
            return weapon;
        }

        private Item CreateFurCape()
        {
            Cloak cape = new Cloak();
            cape.Name = "Healer's Fur Cape";
            cape.Hue = Utility.RandomMinMax(500, 1500);
            cape.ClothingAttributes.LowerStatReq = 2;
            cape.Attributes.BonusStr = 5;
            cape.Attributes.BonusInt = 10;
            cape.SkillBonuses.SetValues(0, SkillName.Veterinary, 25.0);
            cape.SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            return cape;
        }

        private Item CreatePlateGloves()
        {
            PlateGloves gloves = new PlateGloves();
            gloves.Name = "Sorrow's Grasp";
            gloves.Hue = Utility.RandomMinMax(10, 300);
            gloves.BaseArmorRating = Utility.Random(25, 65);
            gloves.AbsorptionAttributes.EaterFire = 20;
            gloves.ArmorAttributes.SelfRepair = -3;
            gloves.Attributes.IncreasedKarmaLoss = 10;
            gloves.Attributes.Luck = -30;
            gloves.SkillBonuses.SetValues(0, SkillName.Parry, -10.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 10;
            gloves.FireBonus = 20;
            gloves.PhysicalBonus = 15;
            gloves.PoisonBonus = 10;
            return gloves;
        }

        private Item CreateGnarledStaff()
        {
            GnarledStaff staff = new GnarledStaff();
            staff.Name = "Cetra's Staff";
            staff.Hue = Utility.RandomMinMax(400, 600);
            staff.MinDamage = Utility.Random(15, 55);
            staff.MaxDamage = Utility.Random(55, 85);
            staff.Attributes.LowerRegCost = 10;
            staff.Attributes.SpellChanneling = 1;
            staff.Slayer = SlayerName.ElementalHealth;
            staff.WeaponAttributes.HitLeechMana = 20;
            staff.WeaponAttributes.SelfRepair = 5;
            staff.SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
            staff.SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
            return staff;
        }

        public InfernalPlaneChest(Serial serial) : base(serial)
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
