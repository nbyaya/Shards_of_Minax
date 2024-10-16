using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MaharajaTreasureChest : WoodenChest
    {
        [Constructable]
        public MaharajaTreasureChest()
        {
            Name = "Maharaja's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Ruby>("Ruby of the Rajas"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Royal Nectar", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Maharaja’s Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Peacock Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Taj Mahal", 1153), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Soma Juice"), 0.08);
            AddItem(CreateGoldItem("Sacred Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Enlightened", 1154), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Lotus Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Telescope>("Maharaja’s Astronomical Telescope"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Ayurvedic Medicine"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreateChainLegs(), 0.20);
            AddItem(CreateScimitar(), 0.20);
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
            note.NoteString = "I have hidden the most precious jewels in a secret chamber.";
            note.TitleString = "Maharaja’s Memoir";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Secret Chamber";
            map.Bounds = new Rectangle2D(5000, 5000, 400, 400);
            map.NewPin = new Point2D(5100, 5150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Scimitar());
            weapon.Name = "Khanda Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Maharaja’s Finest";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Elven Snow Boots";
            boots.Hue = Utility.RandomMinMax(1150, 1200);
            boots.ClothingAttributes.LowerStatReq = 4;
            boots.Attributes.BonusDex = 12;
            boots.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return boots;
        }

        private Item CreateChainLegs()
        {
            ChainLegs chainLegs = new ChainLegs();
            chainLegs.Name = "Shamino's Greaves";
            chainLegs.Hue = Utility.RandomMinMax(250, 450);
            chainLegs.BaseArmorRating = Utility.Random(40, 75);
            chainLegs.AbsorptionAttributes.EaterPoison = 10;
            chainLegs.ArmorAttributes.DurabilityBonus = 20;
            chainLegs.Attributes.BonusDex = 25;
            chainLegs.Attributes.AttackChance = 15;
            chainLegs.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            chainLegs.ColdBonus = 10;
            chainLegs.EnergyBonus = 5;
            chainLegs.FireBonus = 15;
            chainLegs.PhysicalBonus = 15;
            chainLegs.PoisonBonus = 20;
            return chainLegs;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Crissaegrim Edge";
            scimitar.Hue = Utility.RandomMinMax(250, 300);
            scimitar.MinDamage = Utility.Random(35, 70);
            scimitar.MaxDamage = Utility.Random(70, 105);
            scimitar.Attributes.BonusDex = 15;
            scimitar.Attributes.WeaponSpeed = 10;
            scimitar.Slayer = SlayerName.Fey;
            scimitar.Slayer2 = SlayerName.Repond;
            scimitar.WeaponAttributes.BattleLust = 25;
            scimitar.WeaponAttributes.HitLightning = 20;
            scimitar.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return scimitar;
        }

        public MaharajaTreasureChest(Serial serial) : base(serial)
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
