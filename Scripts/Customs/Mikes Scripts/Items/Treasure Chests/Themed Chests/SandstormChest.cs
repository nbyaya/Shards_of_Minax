using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SandstormChest : WoodenChest
    {
        [Constructable]
        public SandstormChest()
        {
            Name = "Sandstorm Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Ruby>("Ruby of the Desert"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Sandstorm Wine", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Sandstorm Loot"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Cobra Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Dunes", 1154), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Desert Wine"), 0.08);
            AddItem(CreateGoldItem("Sandstorm Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Explorer", 1155), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Scorpion Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Explorer’s Trusted Compass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Water"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateSandals(), 0.20);
            AddItem(CreateBoneChest(), 0.20);
            AddItem(CreateWarHammer(), 0.20);
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
            note.NoteString = "I have found a hidden oasis in the desert!";
            note.TitleString = "Explorer’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Oasis";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Sandstorm Blade";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Sandstorm Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Whispering Sandals";
            sandals.Hue = Utility.RandomMinMax(300, 1400);
            sandals.Attributes.BonusDex = 15;
            sandals.Attributes.NightSight = 1;
            sandals.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return sandals;
        }

        private Item CreateBoneChest()
        {
            BoneChest boneChest = new BoneChest();
            boneChest.Name = "Necromancer's Robe";
            boneChest.Hue = Utility.RandomMinMax(10, 250);
            boneChest.BaseArmorRating = Utility.Random(40, 80);
            boneChest.AbsorptionAttributes.EaterCold = 20;
            boneChest.ArmorAttributes.DurabilityBonus = 10;
            boneChest.Attributes.BonusMana = 40;
            boneChest.Attributes.LowerManaCost = 10;
            boneChest.SkillBonuses.SetValues(0, SkillName.Spellweaving, 15.0);
            boneChest.ColdBonus = 20;
            boneChest.EnergyBonus = 10;
            boneChest.FireBonus = 5;
            boneChest.PhysicalBonus = 10;
            boneChest.PoisonBonus = 15;
            return boneChest;
        }

        private Item CreateWarHammer()
        {
            WarHammer warHammer = new WarHammer();
            warHammer.Name = "Plasma Infused WarHammer";
            warHammer.Hue = Utility.RandomMinMax(150, 350);
            warHammer.MinDamage = Utility.Random(30, 85);
            warHammer.MaxDamage = Utility.Random(85, 125);
            warHammer.Attributes.SpellChanneling = 1;
            warHammer.Attributes.ReflectPhysical = 10;
            warHammer.Slayer = SlayerName.ElementalHealth;
            warHammer.Slayer2 = SlayerName.ElementalBan;
            warHammer.WeaponAttributes.HitEnergyArea = 30;
            warHammer.WeaponAttributes.HitDispel = 15;
            warHammer.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return warHammer;
        }

        public SandstormChest(Serial serial) : base(serial)
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
