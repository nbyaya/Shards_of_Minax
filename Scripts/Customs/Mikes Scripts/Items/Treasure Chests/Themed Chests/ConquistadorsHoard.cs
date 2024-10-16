using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ConquistadorsHoard : WoodenChest
    {
        [Constructable]
        public ConquistadorsHoard()
        {
            Name = "Conquistador's Hoard";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Spanish Doubloons"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Sangria of the Elders", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Armada's Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Isabella"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("Vintage Sherry"), 0.08);
            AddItem(CreateGoldItem("Golden Peseta"), 0.16);
            AddItem(CreateNamedItem<GoldRing>("Signet Ring of Ferdinand"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Explorer's Sight"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Rose of Castile"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Valiant Matadors"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateMusicStand(), 0.30);
            AddItem(CreateArmoire(), 0.30);
            AddItem(CreateRunicSewingKit(), 0.30);
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
            note.NoteString = "For the glory of Spain and the pursuit of discovery.";
            note.TitleString = "Spanish Proclamation";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to El Escorial";
            map.Bounds = new Rectangle2D(3000, 3000, 700, 700);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Spanish Espada";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherArms(), new LeatherLegs(), new LeatherCap());
            armor.Name = "Matador's Vestment";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateMusicStand()
        {
            LongPants stand = new LongPants();
            stand.Name = "Cervantes' Music Pants";
            stand.Hue = Utility.RandomMinMax(600, 1600);
            stand.ClothingAttributes.DurabilityBonus = 5;
            stand.Attributes.DefendChance = 10;
            stand.SkillBonuses.SetValues(0, SkillName.Musicianship, 40.0);
            return stand;
        }

        private Item CreateArmoire()
        {
            PlateArms armoire = new PlateArms();
            armoire.Name = "Arms of the Inquisition";
            armoire.Hue = Utility.RandomMinMax(1, 1000);
            armoire.BaseArmorRating = Utility.Random(60, 90);
            armoire.AbsorptionAttributes.EaterPoison = 30;
            armoire.ArmorAttributes.ReactiveParalyze = 1;
            armoire.Attributes.BonusMana = 20;
            armoire.Attributes.AttackChance = 10;
            armoire.SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            armoire.FireBonus = 20;
            armoire.EnergyBonus = 20;
            armoire.ColdBonus = 20;
            armoire.PhysicalBonus = 25;
            armoire.PoisonBonus = 25;
            return armoire;
        }

        private Item CreateRunicSewingKit()
        {
            Hatchet kit = new Hatchet();
            kit.Name = "Seville's Craft Tool";
            kit.Hue = Utility.RandomMinMax(50, 250);
            kit.MinDamage = Utility.Random(30, 80);
            kit.MaxDamage = Utility.Random(80, 120);
            kit.Attributes.BonusDex = 10;
            kit.Attributes.SpellDamage = 5;
            kit.WeaponAttributes.HitEnergyArea = 30;
            kit.WeaponAttributes.SelfRepair = 5;
            kit.SkillBonuses.SetValues(0, SkillName.Tailoring, 25.0);
            return kit;
        }

        public ConquistadorsHoard(Serial serial) : base(serial)
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
