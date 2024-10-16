using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TacticsBonusChest : WoodenChest
    {
        [Constructable]
        public TacticsBonusChest()
        {
            Name = "Warrior's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateTacticsBonusItem<LeatherGloves>("Gloves of Strategy", 1153), 0.20);
            AddItem(CreateTacticsBonusItem<ChainChest>("Chainmail of the Tactician", 1150), 0.15);
            AddItem(CreateTacticsBonusItem<PlateHelm>("Helm of Battle", 1109), 0.17);
            AddItem(CreateTacticsBonusItem<LeatherLegs>("Leggings of the Warrior", 1175), 0.10);
            AddItem(CreateTacticsBonusItem<StuddedGorget>("Gorget of the Strategist", 1157), 0.12);
            AddItem(CreateTacticsBonusItem<RingmailArms>("Arms of the Commander", 1161), 0.08);
            AddItem(CreateTacticsBonusItem<PlateGloves>("Gauntlets of War", 1152), 0.25);
            AddItem(CreateTacticsBonusItem<ThighBoots>("Boots of the Fighter", 1107), 0.30);
            AddItem(CreateTacticsBonusItem<GoldRing>("Ring of Tactics Mastery", 1281), 0.18);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateTacticsBonusItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;

            BaseArmor armor = item as BaseArmor;
            if (armor != null)
            {
                armor.SkillBonuses.SetValues(0, SkillName.Tactics, Utility.Random(10, 30));
                armor.Attributes.DefendChance = Utility.Random(5, 15);
                return armor;
            }

            BaseClothing clothing = item as BaseClothing;
            if (clothing != null)
            {
                clothing.SkillBonuses.SetValues(0, SkillName.Tactics, Utility.Random(10, 30));
                clothing.Attributes.DefendChance = Utility.Random(5, 15);
                return clothing;
            }

            BaseJewel jewel = item as BaseJewel;
            if (jewel != null)
            {
                jewel.SkillBonuses.SetValues(0, SkillName.Tactics, Utility.Random(10, 30));
                jewel.Attributes.DefendChance = Utility.Random(5, 15);
                return jewel;
            }

            return item;
        }

        public TacticsBonusChest(Serial serial) : base(serial)
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
