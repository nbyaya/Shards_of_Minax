using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DragonHodChest : WoodenChest
    {
        [Constructable]
        public DragonHodChest()
        {
            Name = "Dragon's Hoard Chest";
            Hue = 1260;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Emerald>("Dragon's Eye", 1322), 0.20);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel4>("Dragon's Precious Stash"), 0.18);
            AddItem(CreateColoredItem<GoldEarrings>("Scale of the Red Dragon", 1325), 0.14);
            AddItem(new Gold(Utility.Random(1, 8000)), 0.17);
            AddItem(CreateNamedItem<Emerald>("Dragon's Favorite Gem"), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("Draught of Fire Resistance"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Dragon Slayer", 1268), 0.12);
            AddItem(CreateNamedItem<PlateChest>("Dragon Knight's Armor"), 0.13);
            AddItem(CreateNamedItem<Drums>("Dragonbone Flute"), 0.12);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreatePlateGorget(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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
            note.NoteString = "Here lies the treasures of the mighty wyrm.";
            note.TitleString = "Dragon's Warning";
            return note;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Bowyer's Insightful Bandana";
            bandana.Hue = Utility.RandomMinMax(400, 1400);
            bandana.Attributes.BonusInt = 15;
            bandana.Attributes.CastSpeed = 1;
            bandana.SkillBonuses.SetValues(0, SkillName.Fletching, 20.0);
            bandana.SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);
            return bandana;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget plateGorget = new PlateGorget();
            plateGorget.Name = "Flame Plate Gorget";
            plateGorget.Hue = Utility.RandomMinMax(500, 600);
            plateGorget.BaseArmorRating = Utility.RandomMinMax(35, 70);
            plateGorget.Attributes.CastSpeed = 1;
            plateGorget.Attributes.SpellDamage = 10;
            return plateGorget;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Excalibur";
            longsword.Hue = Utility.RandomMinMax(850, 950);
            longsword.MinDamage = Utility.RandomMinMax(40, 70);
            longsword.MaxDamage = Utility.RandomMinMax(70, 100);
            longsword.Attributes.BonusStr = 20;
            longsword.Attributes.AttackChance = 10;
            longsword.Slayer = SlayerName.DaemonDismissal;
            longsword.Slayer2 = SlayerName.BalronDamnation;
            longsword.WeaponAttributes.HitLightning = 30;
            longsword.WeaponAttributes.SelfRepair = 3;
            longsword.SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
            return longsword;
        }

        public DragonHodChest(Serial serial) : base(serial)
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
