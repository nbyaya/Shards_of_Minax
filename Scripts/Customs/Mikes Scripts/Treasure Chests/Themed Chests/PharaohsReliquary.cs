using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PharaohsReliquary : WoodenChest
    {
        [Constructable]
        public PharaohsReliquary()
        {
            Name = "Pharaoh's Reliquary";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Ancient Egyptian Drachmas"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Nile's Sacred Water", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Tomb Treasures"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of the Nile"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<GreaterHealPotion>("Old Kingdom Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Scarab"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Sphinx", 1177), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Signet Ring of Cleopatra"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Pharaoh's Star-gazing Lens"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of the Anubis"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateElegantArmoire(), 0.30);
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
            note.NoteString = "By the power of Ra and the grace of the Nile, this treasure belongs to the great Pharaohs of Egypt.";
            note.TitleString = "Hieroglyphic Inscription";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Valley of the Kings";
            map.Bounds = new Rectangle2D(3000, 3000, 800, 800);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Mace weapon = new Mace();
            weapon.Name = "Ankh-bladed Scepter";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = new PlateChest();
            armor.Name = "Desert Guardian's Plate";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateElegantArmoire()
        {
            PlateChest armoire = new PlateChest();
            armoire.Name = "Canopic Chest";
            armoire.Hue = Utility.RandomMinMax(600, 1600);
            armoire.Attributes.DefendChance = 10;
            armoire.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            armoire.ColdBonus = 20;
            return armoire;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Robe of Osiris";
            robe.Hue = Utility.RandomMinMax(1, 1000);
            robe.Attributes.BonusInt = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            return robe;
        }

        private Item CreateScorp()
        {
            Cleaver scorp = new Cleaver();
            scorp.Name = "Horus's Sacred Cleaver";
            scorp.Hue = Utility.RandomMinMax(50, 250);
            scorp.MinDamage = Utility.Random(30, 80);
            scorp.MaxDamage = Utility.Random(80, 120);
            scorp.Attributes.BonusStr = 10;
            scorp.Attributes.SpellDamage = 5;
            scorp.WeaponAttributes.HitFireArea = 30;
            scorp.WeaponAttributes.SelfRepair = 5;
            scorp.SkillBonuses.SetValues(0, SkillName.Cooking, 25.0);
            return scorp;
        }

        public PharaohsReliquary(Serial serial) : base(serial)
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
