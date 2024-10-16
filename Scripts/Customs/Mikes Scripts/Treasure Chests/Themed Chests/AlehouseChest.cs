using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AlehouseChest : WoodenChest
    {
        [Constructable]
        public AlehouseChest()
        {
            Name = "Alehouse Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Amethyst>("Amethyst of the Amber Lager", 2413), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Alehouse Ale", 2413), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Alehouse Loot"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Mug Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Topaz of the Pale Ale", 1281), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Honey Mead", 2415), 0.08);
            AddItem(CreateGoldItem("Silver Coin"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Barkeep", 2413), 0.20);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Keg Earrings"), 0.20);
            AddItem(CreateMap(), 0.10);
            AddItem(CreateNamedItem<BarrelStaves>("Alehouse Barrel Staves"), 0.10);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Sobering Brew"), 0.15);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateSash(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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
            note.NoteString = "Come to the alehouse for a good time and a good drink.";
            note.TitleString = "Alehouse Invitation";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Alehouse Cellar";
            map.Bounds = new Rectangle2D(2000, 2000, 400, 400);
            map.NewPin = new Point2D(2100, 2100);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Maul());
            weapon.Name = "Keg Smasher";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateSash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Seer's Mystic Sash";
            sash.Hue = Utility.RandomMinMax(150, 800);
            sash.Attributes.BonusInt = 15;
            sash.Attributes.SpellDamage = 8;
            sash.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            return sash;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Boots of Fleetness";
            legs.Hue = Utility.RandomMinMax(200, 650);
            legs.BaseArmorRating = Utility.RandomMinMax(28, 58);
            legs.AbsorptionAttributes.EaterEnergy = 10;
            legs.ArmorAttributes.SelfRepair = 3;
            legs.Attributes.BonusStam = 10;
            legs.Attributes.RegenStam = 3;
            legs.SkillBonuses.SetValues(0, SkillName.Cooking, 5.0);
            legs.ColdBonus = 5;
            legs.EnergyBonus = 15;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 5;
            return legs;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Black Sword of Mondain";
            longsword.Hue = Utility.RandomMinMax(800, 900);
            longsword.MinDamage = Utility.RandomMinMax(30, 70);
            longsword.MaxDamage = Utility.RandomMinMax(70, 110);
            longsword.Attributes.SpellChanneling = 1;
            longsword.Attributes.BonusStr = 20;
            longsword.Slayer = SlayerName.DaemonDismissal;
            longsword.WeaponAttributes.HitMagicArrow = 40;
            longsword.WeaponAttributes.HitManaDrain = 20;
            longsword.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            return longsword;
        }

        public AlehouseChest(Serial serial) : base(serial)
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
