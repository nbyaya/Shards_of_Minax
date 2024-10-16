using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MacingBonusChest : WoodenChest
    {
        [Constructable]
        public MacingBonusChest()
        {
            Name = "Chest of the Mace Master";
            Hue = Utility.Random(1109, 1153);

            // Add items to the chest
            AddItem(CreateColoredItem<ThighBoots>("Mace Master's Boots", 1157), 0.20);
            AddItem(CreateColoredItem<LeatherGloves>("Mace Master's Gloves", 1175), 0.25);
            AddItem(CreateColoredItem<LeatherArms>("Mace Master's Arms", 1109), 0.30);
            AddItem(CreateColoredItem<LeatherChest>("Mace Master's Chest", 1153), 0.20);
            AddItem(CreateColoredItem<LeatherLegs>("Mace Master's Legs", 1175), 0.25);
            AddItem(CreateNamedItem<GoldRing>("Ring of the Mace Master"), 0.15);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of the Mace Master"), 0.15);
            AddItem(CreateMacingWeapon(), 0.40);
            AddItem(new Gold(Utility.Random(500, 2000)), 0.50);
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
            AddMacingBonus(item);
            return item;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            AddMacingBonus(item);
            return item;
        }

        private void AddMacingBonus(Item item)
        {
            if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;
                armor.Attributes.WeaponDamage = 10;
                armor.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            }
            else if (item is BaseJewel)
            {
                BaseJewel jewel = (BaseJewel)item;
                jewel.Attributes.WeaponDamage = 10;
                jewel.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            }
            else if (item is BaseClothing)
            {
                BaseClothing clothing = (BaseClothing)item;
                clothing.Attributes.WeaponDamage = 10;
                clothing.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            }
        }

        private Item CreateMacingWeapon()
        {
            BaseBashing mace = Utility.RandomList<BaseBashing>(new Mace(), new WarMace(), new Maul(), new WarHammer());
            mace.Name = "Mace Master's Weapon";
            mace.Hue = Utility.Random(1109, 1153);
            mace.WeaponAttributes.HitLowerAttack = 30;
            mace.WeaponAttributes.HitEnergyArea = 20;
            mace.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return mace;
        }

        public MacingBonusChest(Serial serial) : base(serial)
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
