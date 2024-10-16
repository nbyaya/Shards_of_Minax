using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GeishasGift : WoodenChest
    {
        [Constructable]
        public GeishasGift()
        {
            Name = "Geisha's Gift";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Club>("Fan of the Geisha", 1157), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Geisha's Wine"), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Geisha's Jewelry Box"), 0.25);
            AddItem(CreateNamedItem<Diamond>("Pearl of the Geisha"), 0.30);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Geisha", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Geisha's Wine"), 0.08);
            AddItem(CreateGoldItem("Geisha's Token"), 0.16);
            AddItem(CreateColoredItem<FancyShirt>("Kimono of the Geisha", 1618), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Cherry Blossom Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<MagnifyingGlass>("Geisha's Trusted Magnifying Glass"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Cleansing Brew"), 0.20);
            AddItem(CreateJewelry(), 0.20);
            AddItem(CreateSurcoat(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateButcherKnife(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
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
            note.NoteString = "I hope you enjoy this gift from me. You are always in my heart.";
            note.TitleString = "Yuki's Love Letter";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Geisha's Secret Garden";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateJewelry()
        {
            Diamond jewelry = new Diamond();
            jewelry.Name = "Pendant of the Geisha";
            jewelry.Hue = Utility.RandomList(1, 1788);
            return jewelry;
        }

        private Item CreateSurcoat()
        {
            Surcoat surcoat = new Surcoat();
            surcoat.Name = "Film Noir Detective's Trench Coat";
            surcoat.Hue = Utility.RandomMinMax(1, 900);
            surcoat.Attributes.NightSight = 1;
            surcoat.Attributes.BonusInt = 10;
            surcoat.SkillBonuses.SetValues(0, SkillName.DetectHidden, 25.0);
            surcoat.SkillBonuses.SetValues(1, SkillName.Stealth, 20.0);
            return surcoat;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Hammerlord's Chestplate";
            plateChest.Hue = Utility.RandomMinMax(350, 650);
            plateChest.BaseArmorRating = Utility.Random(50, 85);
            plateChest.AbsorptionAttributes.EaterFire = 10;
            plateChest.ArmorAttributes.LowerStatReq = 15;
            plateChest.Attributes.BonusHits = 20;
            plateChest.Attributes.DefendChance = 10;
            plateChest.SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);
            plateChest.PhysicalBonus = 20;
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 5;
            plateChest.FireBonus = 15;
            plateChest.PoisonBonus = 5;
            return plateChest;
        }

        private Item CreateButcherKnife()
        {
            ButcherKnife knife = new ButcherKnife();
            knife.Name = "Bowie's Legacy";
            knife.Hue = Utility.RandomMinMax(250, 450);
            knife.MinDamage = Utility.Random(15, 50);
            knife.MaxDamage = Utility.Random(50, 80);
            knife.Attributes.BonusStr = 10;
            knife.Attributes.WeaponSpeed = 5;
            knife.Slayer = SlayerName.TrollSlaughter;
            knife.WeaponAttributes.HitLeechHits = 20;
            knife.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            return knife;
        }

        public GeishasGift(Serial serial) : base(serial)
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
