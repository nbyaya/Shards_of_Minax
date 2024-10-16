using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CaptainCooksTreasure : WoodenChest
    {
        [Constructable]
        public CaptainCooksTreasure()
        {
            Name = "Captain Cook’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(Utility.Random(1, 2)), 1.0);
            AddItem(CreateNamedItem<Diamond>("Diamond of the Explorer"), 1.0);
            AddItem(CreateColoredItem<GreaterHealPotion>("Captain Cook’s Rum", 1175), 1.0);
            AddItem(CreateNamedItem<TreasureLevel3>("Cook’s Loot"), 1.0);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Necklace of Discovery"), 1.0);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 20000)), 1.0);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Ocean", 2122), 1.0);
            AddItem(CreateColoredItem<GreaterHealPotion>("Old Captain Cook’s Rum", 1175), 1.0);
            AddItem(CreateColoredItem<Gold>("Silver Coin", 1154), 1.0);
            AddItem(CreateColoredItem<Boots>("Boots of the Explorer", 1172), 1.0);
            AddItem(CreateNamedItem<SilverEarrings>("Silver Earrings of Adventure"), 1.0);
            AddItem(CreateMap(), 1.0);
            AddItem(CreateNamedItem<Sextant>("Captain Cook’s Compass"), 1.0);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Antidote"), 1.0);
            AddItem(CreateWeapon(), 1.0);
            AddItem(CreateArmor(), 1.0);
            AddItem(CreateSandals(), 0.2);
            AddItem(CreateLeatherLegs(), 0.2);
            AddItem(CreateScimitar(), 0.2);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "I have discovered these islands and named them the Sandwich Islands.";
            note.TitleString = "Captain Cook’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Captain Cook’s Secret Stash";
            map.Bounds = new Rectangle2D(6000, 6000, 400, 400);
            map.NewPin = new Point2D(6100, 6150);
            map.Protected = false;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Scimitar();
            weapon.Name = "The Endeavour";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Cook’s Uniform";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Hippie’s Peaceful Sandals";
            sandals.Hue = Utility.RandomMinMax(500, 1200);
            sandals.Attributes.RegenMana = 3;
            sandals.Attributes.Luck = 20;
            sandals.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            sandals.SkillBonuses.SetValues(1, SkillName.Herding, 10.0);
            return sandals;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Alchemist's Resilient Leggings";
            legs.Hue = Utility.RandomMinMax(500, 750);
            legs.BaseArmorRating = Utility.Random(25, 60);
            legs.AbsorptionAttributes.ResonanceCold = 15;
            legs.ArmorAttributes.MageArmor = 1;
            legs.Attributes.RegenMana = 5;
            legs.Attributes.LowerRegCost = 10;
            legs.SkillBonuses.SetValues(0, SkillName.Herding, 15.0);
            legs.ColdBonus = 15;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Riel’s Rebellion Sabre";
            scimitar.Hue = Utility.RandomMinMax(400, 600);
            scimitar.MinDamage = Utility.Random(25, 65);
            scimitar.MaxDamage = Utility.Random(65, 90);
            scimitar.Attributes.BonusHits = 10;
            scimitar.Attributes.RegenStam = 5;
            scimitar.Slayer = SlayerName.Repond;
            scimitar.WeaponAttributes.HitFireball = 20;
            scimitar.WeaponAttributes.MageWeapon = 1;
            scimitar.SkillBonuses.SetValues(0, SkillName.Herding, 20.0);
            scimitar.SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
            return scimitar;
        }

        public CaptainCooksTreasure(Serial serial) : base(serial)
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
