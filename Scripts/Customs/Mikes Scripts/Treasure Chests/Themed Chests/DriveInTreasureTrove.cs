using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DriveInTreasureTrove : WoodenChest
    {
        [Constructable]
        public DriveInTreasureTrove()
        {
            Name = "Drive-in Treasure Trove";
            Hue = Utility.Random(1, 1910);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldVoucher(), 0.24);
            AddItem(CreateColoredItem<Apple>("Popcorn Bucket"), 0.29);
            AddItem(CreateNamedItem<TreasureLevel3>("Old Film Reel"), 0.25);
            AddItem(CreateNamedItem<GoldEarrings>("Starlet's Earrings"), 0.33);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateRandomWand(), 0.20);
            AddItem(CreateCap(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateGnarledStaff(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateGoldVoucher()
        {
            MaxxiaScroll voucher = new MaxxiaScroll();
            voucher.Name = "Movie Ticket";
            return voucher;
        }

        private Item CreateColoredItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
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
            note.NoteString = "The magic of the silver screen.";
            note.TitleString = "1950's Movie Poster";
            return note;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Director's Baton";
            return wand;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Pop Star's Glittering Cap";
            cap.Hue = Utility.RandomMinMax(500, 1500);
            cap.Attributes.BonusDex = 5;
            cap.Attributes.IncreasedKarmaLoss = -5;
            cap.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.Provocation, 10.0);
            return cap;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Wrestler's Chest of Power";
            plateChest.Hue = Utility.RandomMinMax(10, 250);
            plateChest.BaseArmorRating = Utility.RandomMinMax(50, 70);
            plateChest.ArmorAttributes.LowerStatReq = 15;
            plateChest.Attributes.BonusStr = 20;
            plateChest.Attributes.AttackChance = 10;
            plateChest.SkillBonuses.SetValues(1, SkillName.Wrestling, 15.0);
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 10;
            plateChest.FireBonus = 10;
            plateChest.PhysicalBonus = 20;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateGnarledStaff()
        {
            GnarledStaff staff = new GnarledStaff();
            staff.Name = "Zhuge's Feather Fan";
            staff.Hue = Utility.RandomMinMax(100, 200);
            staff.MinDamage = Utility.RandomMinMax(10, 30);
            staff.MaxDamage = Utility.RandomMinMax(30, 60);
            staff.Attributes.BonusInt = 15;
            staff.Attributes.SpellChanneling = 1;
            staff.Slayer = SlayerName.Ophidian;
            staff.WeaponAttributes.MageWeapon = 1;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            staff.SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
            return staff;
        }

        public DriveInTreasureTrove(Serial serial) : base(serial)
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
