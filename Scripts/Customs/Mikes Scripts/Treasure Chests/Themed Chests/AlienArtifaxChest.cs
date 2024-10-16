using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AlienArtifaxChest : WoodenChest
    {
        [Constructable]
        public AlienArtifaxChest()
        {
            Name = "Alien Artifact";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Galaxy", 0xFFFF), 0.20); // Assuming hue 0xFFFF
            AddItem(CreateColoredItem<GreaterHealPotion>("Serum of Mutation", 1152), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Xenomorph’s Egg"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Alien Skull Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Void", 1153), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Alien Nectar"), 0.08);
            AddItem(CreateGoldItem("UFO Coin"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Explorer", 1154), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Alien Antenna Earring"), 0.17);
            AddItem(CreateMap(), 1.0); // Assuming high probability
            AddItem(CreateNamedItem<Spyglass>("Alien Scanner’s Spyglass"), 1.0); // Assuming high probability
            AddItem(CreateNamedItem<GreaterStrengthPotion>("Bottle of Mysterious Power"), 1.0); // Assuming high probability
            AddItem(CreateWeapon(), 1.0); // Assuming high probability
            AddItem(CreateMask(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateBow(), 0.20);
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
            note.NoteString = "We are not alone!";
            note.TitleString = "Area 51 Report";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Crash Site";
            map.Bounds = new Rectangle2D(2000, 2000, 400, 400);
            map.NewPin = new Point2D(2100, 2050);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Longsword(); // Example; modify as needed
            weapon.Name = "The Predator";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            weapon.MinDamage = Utility.Random(20, 50);
            return weapon;
        }

        private Item CreateMask()
        {
            TribalMask mask = new TribalMask();
            mask.Name = "Luchador's Mask";
            mask.Hue = Utility.RandomMinMax(10, 1000);
            mask.Attributes.BonusDex = 15;
            mask.Attributes.Luck = 20;
            mask.SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
            return mask;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Serpent's Embrace";
            plateChest.Hue = Utility.RandomMinMax(100, 300);
            plateChest.BaseArmorRating = Utility.Random(40, 80);
            plateChest.AbsorptionAttributes.ResonancePoison = 15;
            plateChest.ArmorAttributes.LowerStatReq = 20;
            plateChest.Attributes.BonusStam = 20;
            plateChest.Attributes.RegenStam = 5;
            plateChest.SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
            plateChest.ColdBonus = 5;
            plateChest.EnergyBonus = 10;
            plateChest.FireBonus = 10;
            plateChest.PhysicalBonus = 15;
            plateChest.PoisonBonus = 30;
            return plateChest;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Apollo's Song";
            bow.Hue = Utility.RandomMinMax(880, 900);
            bow.MinDamage = Utility.Random(25, 45);
            bow.MaxDamage = Utility.Random(75, 95);
            bow.Attributes.BonusInt = 10;
            bow.Attributes.NightSight = 1;
            bow.Slayer = SlayerName.DragonSlaying;
            bow.Slayer2 = SlayerName.Exorcism;
            bow.WeaponAttributes.HitFireball = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
            return bow;
        }

        public AlienArtifaxChest(Serial serial) : base(serial)
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
