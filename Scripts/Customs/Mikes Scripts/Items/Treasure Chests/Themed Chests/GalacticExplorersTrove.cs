using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GalacticExplorersTrove : WoodenChest
    {
        [Constructable]
        public GalacticExplorersTrove()
        {
            Name = "Galactic Explorer's Trove";
            Hue = Utility.Random(1, 1550);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.03);
            AddItem(CreateRandomGem(), 0.31);
            AddItem(CreateMap(), 0.30);
            AddItem(CreateNamedItem<TreasureLevel4>("Death Star Blueprints"), 0.29);
            AddItem(CreateNamedItem<SilverNecklace>("Medallion of Alderaan"), 0.39);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5800)), 0.18);
            AddItem(CreateRandomPotion(), 0.21);
            AddItem(CreateArmor(), 0.24);
            AddItem(CreateRandomInstrument(), 0.2);
            AddItem(CreateCloak(), 0.2);
            AddItem(CreateChainChest(), 0.2);
            AddItem(CreateClub(), 0.2);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateRandomGem()
        {
            // Create a RandomGem item (Kyber Crystal)
            Ruby randomGem = new Ruby();
            randomGem.Name = "Kyber Crystal";
            return randomGem;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Endor";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
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
            note.NoteString = "May the stars guide you.";
            note.TitleString = "Explorer's Guidebook";
            return note;
        }

        private Item CreateRandomPotion()
        {
            GreaterHealPotion randomPotion = new GreaterHealPotion();
            randomPotion.Name = "Space Elixir";
            return randomPotion;
        }

        private Item CreateArmor()
        {
            LeatherChest armor = new LeatherChest();
            armor.Name = "Spacesuit Armor";
            armor.Hue = Utility.RandomMinMax(200, 250);
            armor.BaseArmorRating = Utility.Random(40, 70);
            armor.Attributes.BonusMana = 25;
            armor.Attributes.RegenMana = 5;
            armor.SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
            armor.PoisonBonus = 25;
            armor.PhysicalBonus = 20;
            armor.EnergyBonus = 5;
            armor.FireBonus = 10;
            armor.ColdBonus = 10;
            return armor;
        }

        private Item CreateRandomInstrument()
        {
            Lute randomInstrument = new Lute();
            randomInstrument.Name = "Cantina Band Instrument";
            return randomInstrument;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Festive Countdown Cloak";
            cloak.Hue = Utility.RandomMinMax(250, 1000);
            cloak.Attributes.BonusInt = 10;
            cloak.Attributes.Luck = 20;
            cloak.SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            cloak.SkillBonuses.SetValues(1, SkillName.Provocation, 10.0);
            return cloak;
        }

        private Item CreateChainChest()
        {
            ChainChest chainChest = new ChainChest();
            chainChest.Name = "Serpent Scale Armor";
            chainChest.Hue = Utility.RandomMinMax(100, 350);
            chainChest.BaseArmorRating = Utility.Random(40, 70);
            chainChest.AbsorptionAttributes.ResonancePoison = 15;
            chainChest.ArmorAttributes.SelfRepair = 10;
            chainChest.Attributes.BonusMana = 25;
            chainChest.Attributes.RegenMana = 5;
            chainChest.SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
            chainChest.PoisonBonus = 25;
            chainChest.PhysicalBonus = 20;
            chainChest.EnergyBonus = 5;
            chainChest.FireBonus = 10;
            chainChest.ColdBonus = 10;
            return chainChest;
        }

        private Item CreateClub()
        {
            Club club = new Club();
            club.Name = "Bansho Fan Club";
            club.Hue = Utility.RandomMinMax(200, 250);
            club.MinDamage = Utility.Random(20, 60);
            club.MaxDamage = Utility.Random(60, 90);
            club.Attributes.SpellChanneling = 1;
            club.Attributes.RegenMana = 3;
            club.Slayer = SlayerName.FlameDousing;
            club.WeaponAttributes.HitFireArea = 20;
            club.WeaponAttributes.HitManaDrain = 10;
            club.SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
            return club;
        }

        public GalacticExplorersTrove(Serial serial) : base(serial)
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
