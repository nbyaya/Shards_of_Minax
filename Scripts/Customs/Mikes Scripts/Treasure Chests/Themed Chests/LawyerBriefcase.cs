using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class LawyerBriefcase : WoodenChest
    {
        [Constructable]
        public LawyerBriefcase()
        {
            Name = "Lawyer’s Briefcase";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Emerald>("Emerald of Justice", 2773), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Fine Whiskey"), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Lawyer’s Fee"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Scales Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of Truth", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Old Brandy"), 0.08);
            AddItem(CreateGoldItem("Golden Gavel"), 0.16);
            AddItem(CreateColoredItem<Boots>("Shoes of the Advocate", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Lawyer Earrings"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Hammer>("Lawyer’s Trusted Hammer"), 0.10);
            AddItem(CreateNamedItem<TotalRefreshPotion>("Bottle of Energy Drink"), 0.10);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateJewelry(), 0.20);
            AddItem(CreateSandals(), 0.20);
            AddItem(CreatePlateLegs(), 0.20);
            AddItem(CreateVikingSword(), 0.20);
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
            note.NoteString = "I have won the case of the century!";
            note.TitleString = "Lawyer’s Memoirs";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Lawyer’s Secret Vault";
            map.Bounds = new Rectangle2D(2000, 2200, 300, 300);
            map.NewPin = new Point2D(2100, 2150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = Utility.RandomList<Longsword>(new Longsword()); // BaseWeapon should be replaced with a specific weapon type
            weapon.Name = "The Verdict";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateJewelry()
        {
            GoldNecklace jewelry = Utility.RandomList<GoldNecklace>(new GoldNecklace()); // BaseJewelery should be replaced with a specific jewelry type
            jewelry.Name = "The Contract";
            jewelry.Hue = Utility.RandomList(1, 1788);
            return jewelry;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Whispering Sandals";
            sandals.Hue = Utility.RandomMinMax(400, 1400);
            sandals.ClothingAttributes.MageArmor = 1;
            sandals.Attributes.BonusDex = 5;
            sandals.Attributes.NightSight = 1;
            sandals.SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
            sandals.SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
            return sandals;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs plateLegs = new PlateLegs();
            plateLegs.Name = "Doombringer";
            plateLegs.Hue = Utility.RandomMinMax(10, 300);
            plateLegs.BaseArmorRating = Utility.Random(30, 70);
            plateLegs.AbsorptionAttributes.EaterPoison = 20;
            plateLegs.ArmorAttributes.LowerStatReq = 10;
            plateLegs.Attributes.IncreasedKarmaLoss = 15;
            plateLegs.Attributes.Luck = -40;
            plateLegs.SkillBonuses.SetValues(0, SkillName.Poisoning, 15.0);
            plateLegs.ColdBonus = 10;
            plateLegs.EnergyBonus = 5;
            plateLegs.FireBonus = 15;
            plateLegs.PhysicalBonus = 15;
            plateLegs.PoisonBonus = 20;
            return plateLegs;
        }

        private Item CreateVikingSword()
        {
            VikingSword vikingSword = new VikingSword();
            vikingSword.Name = "Masamune Blade";
            vikingSword.Hue = Utility.RandomMinMax(600, 800);
            vikingSword.MinDamage = Utility.Random(30, 80);
            vikingSword.MaxDamage = Utility.Random(80, 130);
            vikingSword.Attributes.BonusHits = 10;
            vikingSword.Attributes.ReflectPhysical = 5;
            vikingSword.Slayer = SlayerName.Exorcism;
            vikingSword.WeaponAttributes.HitManaDrain = 15;
            vikingSword.WeaponAttributes.ResistColdBonus = 10;
            vikingSword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return vikingSword;
        }

        public LawyerBriefcase(Serial serial) : base(serial)
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
