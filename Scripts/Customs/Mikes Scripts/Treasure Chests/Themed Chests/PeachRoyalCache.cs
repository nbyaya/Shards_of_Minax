using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PeachRoyalCache : WoodenChest
    {
        [Constructable]
        public PeachRoyalCache()
        {
            Name = "Peach's Royal Cache";
            Hue = Utility.Random(1, 1855);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Peach's Coins"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Peach's Sparkling Juice", 1155), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Toadstool Trove"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of the Mushroom Kingdom"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<Cake>("Peach's Pink Cake", 2112), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Mushroom Elixir"), 0.08);
            AddItem(CreateGoldItem("Star Coin"), 0.16);
            AddItem(CreateColoredItem<GoldRing>("Peach's Royal Tiara", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Power-Up Ring"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Peach's Starry Spyglass"), 0.13);
            AddItem(CreateColoredItem<PinkCarnation>("Bouquet from Mario's Garden", 1155), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Invincibility"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateArmoire(), 0.30);
            AddItem(CreatePowerUpFlower(), 0.30);
            AddItem(CreateDyeTub(), 0.30);
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
            note.NoteString = "A note from the Mushroom Kingdom";
            note.TitleString = "From Peach's Desk";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Peach's Castle";
            map.Bounds = new Rectangle2D(2500, 2500, 700, 700);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Mace weapon = new Mace();
            weapon.Name = "Peach's Royal Umbrella";
            weapon.Hue = Utility.RandomList(1, 1855);
            weapon.MaxDamage = Utility.Random(30, 60);
            return weapon;
        }

        private Item CreateArmor()
        {
            PlainDress armor = new PlainDress();
            armor.Name = "Peach's Dress";
            armor.Hue = Utility.RandomList(1, 1855);
            return armor;
        }

        private Item CreateArmoire()
        {
            FancyDress armoire = new FancyDress();
            armoire.Name = "Peaches Dress";
            armoire.Hue = Utility.RandomMinMax(500, 1455);
            armoire.ClothingAttributes.DurabilityBonus = 5;
            armoire.Attributes.DefendChance = 8;
            armoire.SkillBonuses.SetValues(0, SkillName.Magery, 55.0);
            return armoire;
        }

        private Item CreatePowerUpFlower()
        {
            LeatherGloves flower = new LeatherGloves();
            flower.Name = "Power-Up Flower";
            flower.Hue = Utility.RandomMinMax(10, 250);
            flower.Attributes.BonusInt = 10;
            flower.Attributes.RegenStam = 5;
            flower.SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
            flower.ColdBonus = 15;
            flower.EnergyBonus = 15;
            flower.FireBonus = 15;
            flower.PhysicalBonus = 20;
            flower.PoisonBonus = 15;
            return flower;
        }

        private Item CreateDyeTub()
        {
            FancyDress dyeTub = new FancyDress();
            dyeTub.Name = "Peach's Old Dress";
            dyeTub.Hue = Utility.RandomMinMax(10, 255);
            dyeTub.Attributes.BonusStr = 8;
            dyeTub.Attributes.SpellDamage = 4;
            dyeTub.SkillBonuses.SetValues(0, SkillName.Magery, 18.0);
            return dyeTub;
        }

        public PeachRoyalCache(Serial serial) : base(serial)
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
