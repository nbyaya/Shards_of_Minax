using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SwordsmanshipBonusChest : WoodenChest
    {
        [Constructable]
        public SwordsmanshipBonusChest()
        {
            Name = "Chest of the Swordmaster";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateSwordsmanshipItem<ThighBoots>("Boots of the Swordmaster"), 0.20);
            AddItem(CreateSwordsmanshipItem<PlateGloves>("Gauntlets of the Swordmaster"), 0.20);
            AddItem(CreateSwordsmanshipItem<GoldRing>("Ring of the Swordmaster"), 0.25);
            AddItem(CreateSwordsmanshipItem<GoldBracelet>("Bracelet of the Swordmaster"), 0.25);
            AddItem(CreateSwordsmanshipItem<LeatherCap>("Cap of the Swordmaster"), 0.20);
            AddItem(CreateSwordsmanshipItem<Cloak>("Cloak of the Swordmaster"), 0.20);
            AddItem(CreateSwordsmanshipItem<BodySash>("Sash of the Swordmaster"), 0.20);
            AddItem(CreateSwordsmanshipItem<Necklace>("Necklace of the Swordmaster"), 0.20);
            AddItem(CreateSwordsmanshipItem<LeatherLegs>("Leggings of the Swordmaster"), 0.20);
            AddItem(CreateSwordsmanshipItem<LeatherArms>("Arms of the Swordmaster"), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSwordsmanshipItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = Utility.RandomList(1, 1788);

            // Apply swordsmanship bonus
            if (item is BaseClothing)
            {
                BaseClothing clothing = item as BaseClothing;
                clothing.SkillBonuses.SetValues(0, SkillName.Swords, Utility.RandomMinMax(10, 20));
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = item as BaseArmor;
                armor.SkillBonuses.SetValues(0, SkillName.Swords, Utility.RandomMinMax(10, 20));
            }
            else if (item is BaseJewel)
            {
                BaseJewel jewel = item as BaseJewel;
                jewel.SkillBonuses.SetValues(0, SkillName.Swords, Utility.RandomMinMax(10, 20));
            }

            return item;
        }

        public SwordsmanshipBonusChest(Serial serial) : base(serial)
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
