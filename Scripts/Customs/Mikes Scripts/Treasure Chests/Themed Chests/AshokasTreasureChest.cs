using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AshokasTreasureChest : WoodenChest
    {
        [Constructable]
        public AshokasTreasureChest()
        {
            Name = "Ashoka’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateAmethyst(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("King’s Elixir", 1175), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Ashoka’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Lion Necklace"), 0.30);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 15000)), 0.15);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Dhamma", 1176), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Old Magadha Whiskey"), 0.08);
            AddItem(CreateGoldItem("Dharma Coin"), 0.16);
            AddItem(CreateColoredItem<Shoes>("Shoes of the Compassionate", 1177), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Chakra Earrings"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Ashoka’s Archaeological Magnifying Glass"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Healing Balm"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreateBuckler(), 0.20);
            AddItem(CreateCutlass(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateAmethyst()
        {
            Amethyst amethyst = new Amethyst();
            amethyst.Name = "Amethyst of the Mauryas";
            return amethyst;
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
            note.NoteString = "I renounce war and violence and embrace Buddhism.";
            note.TitleString = "Ashoka’s Edict";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Ashoka’s Stupa";
            map.Bounds = new Rectangle2D(6000, 6000, 400, 400);
            map.NewPin = new Point2D(6100, 6150);
            map.Protected = true;
            return map;
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateWeapon()
        {
            Hatchet weapon = new Hatchet();
            weapon.Name = "Ashoka’s Chakram";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Santa's Enchanted Robe";
            robe.Hue = Utility.RandomMinMax(30, 35);
            robe.ClothingAttributes.SelfRepair = 5;
            robe.Attributes.BonusInt = 15;
            robe.Attributes.Luck = 20;
            robe.SkillBonuses.SetValues(0, SkillName.Peacemaking, 25.0);
            return robe;
        }

        private Item CreateBuckler()
        {
            Buckler buckler = new Buckler();
            buckler.Name = "Virtue Guard";
            buckler.Hue = Utility.RandomMinMax(444, 555);
            buckler.BaseArmorRating = Utility.Random(35, 65);
            buckler.AbsorptionAttributes.ResonanceKinetic = 15;
            buckler.ArmorAttributes.SelfRepair = 10;
            buckler.Attributes.IncreasedKarmaLoss = -10;
            buckler.Attributes.LowerRegCost = 20;
            buckler.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            buckler.ColdBonus = 10;
            buckler.EnergyBonus = 10;
            buckler.FireBonus = 10;
            buckler.PhysicalBonus = 20;
            buckler.PoisonBonus = 5;
            return buckler;
        }

        private Item CreateCutlass()
        {
            Cutlass cutlass = new Cutlass();
            cutlass.Name = "Chakram Blade";
            cutlass.Hue = Utility.RandomMinMax(500, 700);
            cutlass.MinDamage = Utility.Random(20, 60);
            cutlass.MaxDamage = Utility.Random(60, 85);
            cutlass.Attributes.LowerRegCost = 10;
            cutlass.Attributes.AttackChance = 10;
            cutlass.Slayer = SlayerName.ElementalBan;
            cutlass.WeaponAttributes.HitFireball = 20;
            cutlass.WeaponAttributes.HitPhysicalArea = 20;
            cutlass.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            cutlass.SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
            return cutlass;
        }

        public AshokasTreasureChest(Serial serial) : base(serial)
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
