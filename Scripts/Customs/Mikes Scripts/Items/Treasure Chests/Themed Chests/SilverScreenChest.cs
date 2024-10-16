using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SilverScreenChest : WoodenChest
    {
        [Constructable]
        public SilverScreenChest()
        {
            Name = "Silver Screen Chest";
            Hue = Utility.Random(1, 1943);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Sapphire>("Starlet's Jewel"), 0.27);
            AddItem(CreateLootItem<FancyDress>("Hollywood Glamour Dress", 1940), 0.29);
            AddItem(CreateNamedItem<TreasureLevel3>("Vintage Film Reel"), 0.23);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.12);
            AddItem(CreateLootItem<Lute>("Director's Megaphone", 2831), 0.18);
            AddItem(CreateNamedItem<GoldEarrings>("Cinema Star Earrings"), 0.15);
            AddItem(CreateBearMask(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateKatana(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateColoredItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = Utility.RandomList(1, 1940);
            return item;
        }

        private Item CreateLootItem<T>(string name, int hue) where T : Item, new()
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
            note.NoteString = "Here's looking at you, kid.";
            note.TitleString = "Classic Movie Quotes";
            return note;
        }

        private Item CreateBearMask()
        {
            BearMask mask = new BearMask();
            mask.Name = "Rogue's Deceptive Mask";
            mask.Hue = Utility.RandomMinMax(900, 1600);
            mask.Attributes.BonusDex = 5;
            mask.Attributes.ReflectPhysical = 10;
            mask.SkillBonuses.SetValues(0, SkillName.Snooping, 20.0);
            mask.SkillBonuses.SetValues(1, SkillName.Stealth, 20.0);
            return mask;
        }

        private Item CreatePlateChest()
        {
            PlateChest plate = new PlateChest();
            plate.Name = "Knight's Aegis";
            plate.Hue = Utility.RandomMinMax(50, 550);
            plate.BaseArmorRating = Utility.RandomMinMax(45, 85);
            plate.AbsorptionAttributes.EaterDamage = 20;
            plate.ArmorAttributes.DurabilityBonus = 30;
            plate.Attributes.BonusStr = 20;
            plate.Attributes.DefendChance = 15;
            plate.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            plate.ColdBonus = 15;
            plate.EnergyBonus = 5;
            plate.FireBonus = 15;
            plate.PhysicalBonus = 20;
            plate.PoisonBonus = 10;
            return plate;
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Masamune's Grace";
            katana.Hue = Utility.RandomMinMax(200, 250);
            katana.MinDamage = Utility.RandomMinMax(25, 70);
            katana.MaxDamage = Utility.RandomMinMax(70, 100);
            katana.Attributes.BonusDex = 15;
            katana.Attributes.SpellChanneling = 1;
            katana.Slayer = SlayerName.DragonSlaying;
            katana.WeaponAttributes.HitLeechMana = 20;
            katana.WeaponAttributes.MageWeapon = 1;
            katana.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return katana;
        }

        public SilverScreenChest(Serial serial) : base(serial)
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
