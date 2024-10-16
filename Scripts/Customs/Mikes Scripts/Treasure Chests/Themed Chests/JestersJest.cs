using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JestersJest : WoodenChest
    {
        [Constructable]
        public JestersJest()
        {
            Name = "Jester's Jest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Fool's Gold"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Jester's Elixir", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Court Jester's Surprise"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of Folly"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<TarotCardsArtifact>("Jester's Fateful Cards", 2119), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Bubbly Laughter Potion", 1159), 0.08);
            AddItem(CreateGoldItem("Coin of Comedy"), 0.16);
            AddItem(CreateColoredItem<Robe>("Cloak of Capers", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Ring of Riddles"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Jester's Trick Spyglass"), 0.13);
            AddItem(CreatePotion("Potion of Giggles"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateFlyingCarpet(), 0.30);
            AddItem(CreateRobe(), 0.30);
            AddItem(CreateScorp(), 0.30);
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
            note.NoteString = "He who laughs last, laughs best!";
            note.TitleString = "Jester's Proverb";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Carnival";
            map.Bounds = new Rectangle2D(3000, 3000, 700, 700);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreatePotion(string name)
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = name;
            return potion;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Jester's Jingle Stick";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armour of Antics";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFlyingCarpet()
        {
            JesterHat carpet = new JesterHat();
            carpet.Name = "Jester's Flying Carpet";
            carpet.Hue = Utility.RandomMinMax(600, 1600);
            carpet.ClothingAttributes.DurabilityBonus = 5;
            carpet.Attributes.DefendChance = 10;
            carpet.SkillBonuses.SetValues(0, SkillName.Begging, 20.0);
            return carpet;
        }

        private Item CreateRobe()
        {
            LeatherChest robe = new LeatherChest();
            robe.Name = "Chest of Revelry";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.BaseArmorRating = Utility.Random(60, 90);
            robe.AbsorptionAttributes.EaterEnergy = 30;
            robe.ArmorAttributes.ReactiveParalyze = 1;
            robe.Attributes.BonusDex = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
            robe.EnergyBonus = 20;
            robe.FireBonus = 20;
            robe.PhysicalBonus = 25;
            robe.PoisonBonus = 25;
            return robe;
        }

        private Item CreateScorp()
        {
            Mace scorp = new Mace();
            scorp.Name = "Prankster's Tool";
            scorp.Hue = Utility.RandomMinMax(50, 250);
            scorp.MinDamage = Utility.Random(30, 80);
            scorp.MaxDamage = Utility.Random(80, 120);
            scorp.Attributes.BonusInt = 10;
            scorp.Attributes.SpellDamage = 5;
            scorp.WeaponAttributes.HitEnergyArea = 30;
            scorp.WeaponAttributes.SelfRepair = 5;
            scorp.SkillBonuses.SetValues(0, SkillName.Provocation, 25.0);
            return scorp;
        }

        public JestersJest(Serial serial) : base(serial)
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
