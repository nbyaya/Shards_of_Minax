using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SamuraiStash : WoodenChest
    {
        [Constructable]
        public SamuraiStash()
        {
            Name = "Samurai's Stash";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateKatana(), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Samurai's Drink"), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Samurai's Treasure"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Dragon Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Jade of the East", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Samurai's Sake"), 0.08);
            AddItem(CreateGoldItem("Samurai's Coin"), 0.16);
            AddItem(CreateColoredItem<LeatherChest>("Jacket of the Ninja", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Lotus Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Telescope>("Samurai's Trusted Telescope"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Brew"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreatePlateHelm(), 0.20);
            AddItem(CreateScimitar(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Sword of the Shogun";
            katana.Hue = 1157;
            return katana;
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
            note.NoteString = "I have hidden my most precious belongings here. Do not dare to touch them!";
            note.TitleString = "Musashi’s Memoir";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Samurai's Secret Hideout";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Club();
            weapon.Name = "Naginata of the Warrior";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherLegs(), new LeatherGloves(), new LeatherCap());
            armor.Name = "Samurai's Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Jazz Musician's Muffler";
            muffler.Hue = Utility.RandomMinMax(300, 1400);
            muffler.Attributes.BonusDex = 5;
            muffler.Attributes.Luck = 20;
            muffler.SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
            muffler.SkillBonuses.SetValues(1, SkillName.Discordance, 15.0);
            return muffler;
        }

        private Item CreatePlateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Hammerlord's Helm";
            helm.Hue = Utility.RandomMinMax(350, 650);
            helm.BaseArmorRating = Utility.Random(40, 80);
            helm.AbsorptionAttributes.EaterKinetic = 10;
            helm.ArmorAttributes.DurabilityBonus = 25;
            helm.Attributes.BonusStr = 10;
            helm.SkillBonuses.SetValues(0, SkillName.Macing, 10.0);
            helm.PhysicalBonus = 15;
            helm.ColdBonus = 5;
            helm.EnergyBonus = 10;
            helm.FireBonus = 10;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Revolutionary Sabre";
            scimitar.Hue = Utility.RandomMinMax(100, 300);
            scimitar.MinDamage = Utility.Random(25, 60);
            scimitar.MaxDamage = Utility.Random(60, 90);
            scimitar.Attributes.BonusHits = 15;
            scimitar.Attributes.DefendChance = 5;
            scimitar.Slayer = SlayerName.OrcSlaying;
            scimitar.WeaponAttributes.HitLightning = 20;
            scimitar.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            scimitar.SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
            return scimitar;
        }

        public SamuraiStash(Serial serial) : base(serial)
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
