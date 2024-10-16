using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TangDynastyChest : WoodenChest
    {
        [Constructable]
        public TangDynastyChest()
        {
            Name = "Treasure Chest of the Tang Dynasty";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Diamond>("Jewel of the East"), 1.0);
            AddItem(CreateColoredItem<GreaterHealPotion>("Tang Dynasty’s Finest Liquor", 1486), 1.0);
            AddItem(CreateNamedItem<TreasureLevel3>("Silk Road Treasures"), 1.0);
            AddItem(CreateNamedItem<GoldRing>("Golden Lotus Ring"), 1.0);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 1.0);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Tang Court", 1775), 1.0);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Plum Wine"), 1.0);
            AddItem(CreateGoldItem("Tang Coin"), 1.0);
            AddItem(CreateColoredItem<Boots>("Boots of the Tang Explorer", 1618), 1.0);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Peony Necklace"), 1.0);
            AddItem(CreateMap(), 1.0);
            AddItem(CreateNamedItem<Sextant>("Empress Wu Zetian’s Compass"), 1.0);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Curative Brew"), 1.0);
            AddItem(CreateWeapon(), 1.0);
            AddItem(CreateArmor(), 1.0);
            AddItem(CreateSundress(), 0.2);
            AddItem(CreateBoneGloves(), 0.2);
            AddItem(CreateHalberd(), 0.2);
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
            note.NoteString = "I have ruled over the golden age of China with wisdom and benevolence. I have promoted culture and trade and made my empire prosperous and cosmopolitan. I have patronized poets and artists and encouraged religious diversity. I have expanded my territory and made alliances with neighboring states. I have faced rebellions and invasions but always restored peace and order.";
            note.TitleString = "Empress Wu Zetian’s Diary";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Empress Wu Zetian’s Tomb";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Spear());
            weapon.Name = "Spear of Tang";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            Robe armor = new Robe();
            armor.Name = "Tang Robe";
            armor.Hue = Utility.RandomList(1, 1788);
            return armor;
        }

        private Item CreateSundress()
        {
            PlainDress sundress = new PlainDress();
            sundress.Name = "Couturier's Sundress";
            sundress.Hue = Utility.RandomMinMax(100, 600);
            sundress.Attributes.EnhancePotions = 15;
            sundress.Attributes.RegenMana = 3;
            sundress.SkillBonuses.SetValues(0, SkillName.Tailoring, 20.0);
            return sundress;
        }

        private Item CreateBoneGloves()
        {
            BoneGloves boneGloves = new BoneGloves();
            boneGloves.Name = "Monk's Battle Wraps";
            boneGloves.Hue = Utility.RandomMinMax(300, 650);
            boneGloves.BaseArmorRating = Utility.Random(35, 70);
            boneGloves.AbsorptionAttributes.EaterKinetic = 20;
            boneGloves.ArmorAttributes.SelfRepair = 10;
            boneGloves.Attributes.BonusStr = 15;
            boneGloves.Attributes.WeaponSpeed = 10;
            boneGloves.SkillBonuses.SetValues(0, SkillName.Wrestling, 25.0);
            boneGloves.ColdBonus = 10;
            boneGloves.EnergyBonus = 10;
            boneGloves.FireBonus = 20;
            boneGloves.PhysicalBonus = 15;
            boneGloves.PoisonBonus = 5;
            return boneGloves;
        }

        private Item CreateHalberd()
        {
            Halberd halberd = new Halberd();
            halberd.Name = "Naginata of Tomoe Gozen";
            halberd.Hue = Utility.RandomMinMax(250, 300);
            halberd.MinDamage = Utility.Random(25, 75);
            halberd.MaxDamage = Utility.Random(75, 110);
            halberd.Attributes.BonusDex = 15;
            halberd.Attributes.DefendChance = 10;
            halberd.Slayer = SlayerName.TrollSlaughter;
            halberd.WeaponAttributes.HitLightning = 20;
            halberd.WeaponAttributes.BattleLust = 10;
            halberd.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            halberd.SkillBonuses.SetValues(1, SkillName.Wrestling, 15.0);
            return halberd;
        }

        public TangDynastyChest(Serial serial) : base(serial)
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
