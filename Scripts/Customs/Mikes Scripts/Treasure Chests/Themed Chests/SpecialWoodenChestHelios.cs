using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialWoodenChestHelios : WoodenChest
    {
        [Constructable]
        public SpecialWoodenChestHelios()
        {
            Name = "Helios's Bounty";
            Hue = Utility.Random(1, 1700);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateGoldItem("Solar Daric"), 0.22);
            AddItem(CreateColoredItem<GreaterHealPotion>("Solar Wine", 1180), 0.18);
            AddItem(CreateNamedItem<TreasureLevel2>("Helios's Radiance"), 0.20);
            AddItem(CreateNamedItem<GoldBracelet>("Glowing Bracelet of Helios"), 0.55);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.18);
            AddItem(CreateColoredItem<Ruby>("Sunstone of the God", 2150), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("Sunrise Nectar"), 0.09);
            AddItem(CreateGoldItem("Solar Stater"), 0.20);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Sunlord", 1200), 0.22);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Radiance"), 0.20);
            AddItem(CreateMap(), 0.06);
            AddItem(CreateNamedItem<Spyglass>("Helios's Sun Viewer"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Sun's Embrace"), 0.23);
            AddItem(CreateWeapon(), 0.24);
            AddItem(CreateArmor(), 0.35);
            AddItem(CreateBoots(), 0.35);
            AddItem(CreateGauntlets(), 0.35);
            AddItem(CreateLongsword(), 0.35);
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
            note.NoteString = "I am Helios, guardian of the sun, radiant deity, shining above all, bringer of light.";
            note.TitleString = "Helios's Proclamation";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Helios's Altar";
            map.Bounds = new Rectangle2D(1500, 1500, 500, 500);
            map.NewPin = new Point2D(1600, 1600);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Helios's Blade";
            weapon.Hue = Utility.RandomList(1, 1700);
            weapon.MaxDamage = Utility.Random(32, 75);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Helios's Guard";
            armor.Hue = Utility.RandomList(1, 1700);
            armor.BaseArmorRating = Utility.Random(32, 75);
            return armor;
        }

        private Item CreateBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Sunlit Walkers";
            boots.Hue = Utility.RandomMinMax(650, 1650);
            boots.ClothingAttributes.DurabilityBonus = 8;
            boots.Attributes.DefendChance = 12;
            boots.SkillBonuses.SetValues(0, SkillName.Swords, 22.0);
            return boots;
        }

        private Item CreateGauntlets()
        {
            PlateGloves gauntlets = new PlateGloves();
            gauntlets.Name = "Gauntlets of Dawn";
            gauntlets.Hue = Utility.RandomMinMax(1, 1050);
            gauntlets.BaseArmorRating = Utility.Random(62, 95);
            gauntlets.AbsorptionAttributes.EaterPoison = 33;
            gauntlets.ArmorAttributes.ReactiveParalyze = 2;
            gauntlets.Attributes.BonusDex = 22;
            gauntlets.Attributes.AttackChance = 12;
            gauntlets.SkillBonuses.SetValues(0, SkillName.Swords, 22.0);
            gauntlets.ColdBonus = 23;
            gauntlets.EnergyBonus = 23;
            gauntlets.FireBonus = 23;
            gauntlets.PhysicalBonus = 28;
            gauntlets.PoisonBonus = 28;
            return gauntlets;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Sunblade";
            longsword.Hue = Utility.RandomMinMax(55, 255);
            longsword.MinDamage = Utility.Random(32, 85);
            longsword.MaxDamage = Utility.Random(85, 125);
            longsword.Attributes.BonusStr = 12;
            longsword.Attributes.SpellDamage = 6;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitFireball = 32;
            longsword.WeaponAttributes.SelfRepair = 6;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 27.0);
            return longsword;
        }

        public SpecialWoodenChestHelios(Serial serial) : base(serial)
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
