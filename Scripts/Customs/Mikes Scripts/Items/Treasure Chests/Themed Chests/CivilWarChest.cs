using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CivilWarChest : WoodenChest
    {
        [Constructable]
        public CivilWarChest()
        {
            Name = "Civil War Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
			AddItem(CreateColoredItem<MaxxiaScroll>("Emancipation Proclamation", 2117), 0.20);
            AddItem(CreateColoredItem<Ruby>("Blood of the Fallen", 2117), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Soldier’s Ration", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("War Bonds"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Medal of Honor"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Union", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Moonshine of the Confederacy"), 0.08);
            AddItem(CreateGoldItem("Copper Penny"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the General", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Star Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Ulysses S. Grant’s Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Clara Barton’s Remedy"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateCap(), 0.20);
            AddItem(CreateNorseHelm(), 0.20);
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
            note.NoteString = "Four score and seven years ago…";
            note.TitleString = "Abraham Lincoln’s Speech";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Underground Railroad";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "The Rebel";
            return weapon;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Ranger's Cap";
            cap.Hue = Utility.RandomMinMax(850, 1850);
            cap.Attributes.BonusDex = 10;
            cap.Attributes.BonusInt = 5;
            cap.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
            return cap;
        }

        private Item CreateNorseHelm()
        {
            NorseHelm helm = new NorseHelm();
            helm.Name = "Royal Circlet Helm";
            helm.Hue = Utility.RandomMinMax(300, 900);
            helm.BaseArmorRating = Utility.Random(40, 80);
            helm.Attributes.BonusStam = 20;
            helm.Attributes.RegenMana = 5;
            helm.Attributes.LowerManaCost = 10;
            helm.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            helm.SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 10;
            helm.FireBonus = 10;
            helm.PhysicalBonus = 15;
            helm.PoisonBonus = 10;
            return helm;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Excalibur";
            longsword.Hue = Utility.RandomMinMax(500, 750);
            longsword.MinDamage = Utility.Random(40, 60);
            longsword.MaxDamage = Utility.Random(60, 90);
            longsword.Attributes.BonusStr = 20;
            longsword.Attributes.SpellChanneling = 1;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.Slayer2 = SlayerName.Repond;
            longsword.WeaponAttributes.MageWeapon = 1;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return longsword;
        }

        public CivilWarChest(Serial serial) : base(serial)
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
