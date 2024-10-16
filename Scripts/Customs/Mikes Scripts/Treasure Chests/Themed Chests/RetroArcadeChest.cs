using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RetroArcadeChest : WoodenChest
    {
        [Constructable]
        public RetroArcadeChest()
        {
            Name = "Retro Arcade";
            Hue = Utility.RandomList(1, 1400);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Sapphire>("Pac-Man's Gem"), 0.22);
            AddItem(CreateColoredItem<GreaterHealPotion>("Pixelated Elixir", 1410), 0.25);
            AddItem(CreateNamedItem<TreasureLevel2>("Joystick Relic"), 0.24);
            AddItem(CreateNamedItem<SilverNecklace>("Space Invader Charm"), 0.35);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4200)), 0.13);
            AddItem(CreateNamedItem<Apple>("Apple from Snake Game"), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("Power-Up Potion"), 0.18);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateFancyShirt(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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
            note.NoteString = "High score challenges await.";
            note.TitleString = "Arcade Master's Log";
            return note;
        }

        private Item CreateWeapon()
        {
            Mace weapon = new Mace();
            weapon.Name = "Button Masher";
            weapon.Hue = Utility.RandomList(1, 1400);
            return weapon;
        }

        private Item CreateFancyShirt()
        {
            FancyShirt shirt = new FancyShirt();
            shirt.Name = "Glam Rocker's Jacket";
            shirt.Hue = Utility.RandomMinMax(150, 1150);
            shirt.ClothingAttributes.DurabilityBonus = 4;
            shirt.Attributes.BonusDex = 10;
            shirt.Attributes.Luck = 20;
            shirt.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            return shirt;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Courtesan's Flowing Robe";
            chest.Hue = Utility.RandomMinMax(100, 500);
            chest.BaseArmorRating = Utility.Random(30, 60);
            chest.ArmorAttributes.MageArmor = 1;
            chest.Attributes.EnhancePotions = 10;
            chest.Attributes.BonusMana = 15;
            chest.SkillBonuses.SetValues(0, SkillName.Musicianship, 10.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 10;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 10;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Sword of Gideon";
            longsword.Hue = Utility.RandomMinMax(300, 500);
            longsword.MinDamage = Utility.Random(20, 60);
            longsword.MaxDamage = Utility.Random(60, 100);
            longsword.Attributes.AttackChance = 10;
            longsword.Attributes.NightSight = 1;
            longsword.Slayer = SlayerName.Repond;
            longsword.WeaponAttributes.HitLightning = 25;
            longsword.SkillBonuses.SetValues(0, SkillName.Herding, 20.0);
            return longsword;
        }

        public RetroArcadeChest(Serial serial) : base(serial)
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
