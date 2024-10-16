using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CivilRightsStrongbox : WoodenChest
    {
        [Constructable]
        public CivilRightsStrongbox()
        {
            Name = "Civil Rights Strongbox";
            Hue = Utility.RandomList(1, 2100);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.09);
            AddItem(CreateColoredItem<Sapphire>("Freedom Stone", 2439), 0.29);
            AddItem(CreateNamedItem<GreaterHealPotion>("Hope Elixir"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel4>("Liberty Bell"), 0.25);
            AddItem(CreateNamedItem<SilverNecklace>("Dream Medallion"), 0.42);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.17);
            AddItem(CreateNamedItem<MaxxiaScroll>("March on Washington Diary"), 0.1);
            AddItem(CreateColoredItem<PlainDress>("Pin-Up Halter Dress", Utility.RandomMinMax(400, 1300)), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateClub(), 0.20);
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
            note.NoteString = "I have a dream...";
            note.TitleString = "Activist's Notes";
            return note;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Alchemist's Heart";
            plateChest.Hue = Utility.RandomMinMax(500, 750);
            plateChest.BaseArmorRating = Utility.Random(40, 80);
            plateChest.AbsorptionAttributes.EaterFire = 15;
            plateChest.ArmorAttributes.LowerStatReq = 20;
            plateChest.Attributes.BonusMana = 25;
            plateChest.Attributes.EnhancePotions = 15;
            plateChest.SkillBonuses.SetValues(0, SkillName.TasteID, 20.0);
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 10;
            plateChest.FireBonus = 15;
            plateChest.PhysicalBonus = 10;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateClub()
        {
            Club club = new Club();
            club.Name = "Voyageur's Paddle";
            club.Hue = Utility.RandomMinMax(300, 500);
            club.MinDamage = Utility.Random(15, 55);
            club.MaxDamage = Utility.Random(55, 70);
            club.Attributes.BonusDex = 5;
            club.Attributes.LowerRegCost = 10;
            club.Slayer = SlayerName.TrollSlaughter;
            club.WeaponAttributes.HitDispel = 20;
            club.WeaponAttributes.HitFatigue = 20;
            club.SkillBonuses.SetValues(0, SkillName.Fishing, 15.0);
            club.SkillBonuses.SetValues(1, SkillName.Herding, 10.0);
            return club;
        }

        public CivilRightsStrongbox(Serial serial) : base(serial)
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
