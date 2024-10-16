using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MirageChest : WoodenChest
    {
        [Constructable]
        public MirageChest()
        {
            Name = "Mirage Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateAmethyst(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Mirage Liquor", 1156), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Mirage Treasure"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Camel Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Topaz of the Horizon", 1157), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Mirage Liquor"), 0.08);
            AddItem(CreateGoldItem("Mirage Coin"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Mirage Hunter", 1158), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Palm Tree Earrings"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Mirage Hunter’s Trusted Sextant"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Purifying Water"), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreateBoneGloves(), 0.20);
            AddItem(CreateTwoHandedAxe(), 0.20);
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
            amethyst.Name = "Amethyst of the Mirage";
            return amethyst;
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
            note.NoteString = "I have seen a strange city in the distance!";
            note.TitleString = "Mirage Hunter’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Mirage City";
            map.Bounds = new Rectangle2D(2000, 2000, 400, 400);
            map.NewPin = new Point2D(2200, 2200);
            map.Protected = true;
            return map;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Melodious Muffler";
            muffler.Hue = Utility.RandomMinMax(400, 1600);
            muffler.Attributes.BonusInt = 10;
            muffler.Attributes.CastSpeed = 1;
            muffler.SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
            muffler.SkillBonuses.SetValues(1, SkillName.Discordance, 15.0);
            return muffler;
        }

        private Item CreateBoneGloves()
        {
            BoneGloves boneGloves = new BoneGloves();
            boneGloves.Name = "Necromancer's BoneGrips";
            boneGloves.Hue = Utility.RandomMinMax(10, 250);
            boneGloves.BaseArmorRating = Utility.Random(35, 70);
            boneGloves.AbsorptionAttributes.EaterFire = 15;
            boneGloves.ArmorAttributes.MageArmor = 1;
            boneGloves.Attributes.CastRecovery = 2;
            boneGloves.Attributes.RegenMana = 5;
            boneGloves.SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);
            boneGloves.ColdBonus = 10;
            boneGloves.EnergyBonus = 15;
            boneGloves.FireBonus = 15;
            boneGloves.PhysicalBonus = 10;
            boneGloves.PoisonBonus = 15;
            return boneGloves;
        }

        private Item CreateTwoHandedAxe()
        {
            TwoHandedAxe axe = new TwoHandedAxe();
            axe.Name = "Grognak's Axe";
            axe.Hue = Utility.RandomMinMax(100, 300);
            axe.MinDamage = Utility.Random(35, 90);
            axe.MaxDamage = Utility.Random(90, 130);
            axe.Attributes.BonusStr = 15;
            axe.Attributes.RegenHits = 5;
            axe.Slayer2 = SlayerName.TrollSlaughter;
            axe.WeaponAttributes.BattleLust = 25;
            axe.WeaponAttributes.HitHarm = 25;
            axe.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return axe;
        }

        public MirageChest(Serial serial) : base(serial)
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
