using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class VHSAdventureCache : WoodenChest
    {
        [Constructable]
        public VHSAdventureCache()
        {
            Name = "VHS Adventure Cache";
            Hue = Utility.Random(1, 2090);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.09);
            AddItem(CreateEmerald(), 0.23);
            AddItem(CreateApple(), 0.29);
            AddItem(CreateNamedItem<TreasureLevel4>("Critics' Choice Award"), 0.28);
            AddItem(CreateNamedItem<SilverNecklace>("Star's Silver Locket"), 0.31);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4600)), 0.15);
            AddItem(CreatePotion(), 0.10);
            AddItem(CreateCap(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateScimitar(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Reel Emerald";
            return emerald;
        }

        private Item CreateApple()
        {
            Apple apple = new Apple();
            apple.Name = "Popcorn Kernel of Cinema";
            return apple;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Potion of Instant Replay";
            return potion;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Breakdancer's Cap";
            cap.Hue = Utility.RandomMinMax(50, 1000);
            cap.Attributes.BonusDex = 15;
            cap.Attributes.DefendChance = 10;
            cap.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return cap;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Dragoon's Aegis";
            plateChest.Hue = Utility.RandomMinMax(450, 650);
            plateChest.BaseArmorRating = Utility.RandomMinMax(50, 85);
            plateChest.ArmorAttributes.LowerStatReq = 10;
            plateChest.Attributes.BonusStr = 20;
            plateChest.Attributes.AttackChance = 10;
            plateChest.Attributes.ReflectPhysical = 5;
            plateChest.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 10;
            plateChest.FireBonus = 20;
            plateChest.PhysicalBonus = 25;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Zulfiqar";
            scimitar.Hue = Utility.RandomMinMax(500, 700);
            scimitar.MinDamage = Utility.RandomMinMax(30, 60);
            scimitar.MaxDamage = Utility.RandomMinMax(60, 90);
            scimitar.Attributes.BonusStr = 15;
            scimitar.Attributes.DefendChance = 10;
            scimitar.Slayer = SlayerName.DragonSlaying;
            scimitar.WeaponAttributes.HitLightning = 40;
            scimitar.WeaponAttributes.BattleLust = 30;
            scimitar.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return scimitar;
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
            note.NoteString = "Epic adventures, unforgettable lines.";
            note.TitleString = "Blockbuster's Catalog";
            return note;
        }

        public VHSAdventureCache(Serial serial) : base(serial)
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
