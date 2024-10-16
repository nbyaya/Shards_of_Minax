using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class StarfleetsVault : WoodenChest
    {
        [Constructable]
        public StarfleetsVault()
        {
            Name = "Starfleet's Vault";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateGoldItem("Gold-Pressed Latinum"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Romulan Ale", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Galactic Cache"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Communicator Badge"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Starfleet Issue Telescope"), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Klingon Blood Wine", 1160), 0.09);
            AddItem(CreateGoldItem("Federation Credit"), 0.17);
            AddItem(CreateColoredItem<SaddleArtifact>("Captain's Chair Upholstery", 1177), 0.18);
            AddItem(CreateNamedItem<GoldRing>("Ring of the United Federation"), 0.16);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Clock>("Stardate Clock"), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("Borg Nanite Infusion"), 0.21);
            AddItem(CreateWeapon(), 0.22);
            AddItem(CreateArmor(), 0.31);
            AddItem(CreateElegantArmoire(), 0.29);
            AddItem(CreateRobe(), 0.31);
            AddItem(CreateScorp(), 0.31);
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
            note.NoteString = "Live long and prosper.";
            note.TitleString = "Vulcan Proverb";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Starbase 74";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Crossbow weapon = new Crossbow();
            weapon.Name = "Phaser Type-II";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            FancyShirt armor = new FancyShirt();
            armor.Name = "Starfleet Uniform";
            armor.Hue = Utility.RandomList(1, 1788);
            return armor;
        }

        private Item CreateElegantArmoire()
        {
            Robe armoire = new Robe();
            armoire.Name = "Holodeck Robe";
            armoire.Hue = Utility.RandomMinMax(600, 1600);
            armoire.ClothingAttributes.DurabilityBonus = 5;
            armoire.Attributes.DefendChance = 10;
			armoire.Hue = Utility.RandomList(1, 1788);
            armoire.SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
            return armoire;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Robe of Q";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.Attributes.BonusMana = 20;
            robe.Attributes.CastRecovery = 2;
            robe.SkillBonuses.SetValues(0, SkillName.Spellweaving, 60.0);
            return robe;
        }

        private Item CreateScorp()
        {
            CrescentBlade scorp = new CrescentBlade();
            scorp.Name = "Bat'leth";
            scorp.Hue = Utility.RandomMinMax(50, 250);
            scorp.MinDamage = Utility.Random(30, 80);
            scorp.MaxDamage = Utility.Random(80, 120);
            scorp.Attributes.BonusStr = 10;
            scorp.Attributes.SpellDamage = 5;
            scorp.Slayer = SlayerName.ReptilianDeath;
            scorp.WeaponAttributes.HitEnergyArea = 40;
            scorp.WeaponAttributes.SelfRepair = 5;
            scorp.SkillBonuses.SetValues(0, SkillName.Swords, 45.0);
            return scorp;
        }

        public StarfleetsVault(Serial serial) : base(serial)
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
