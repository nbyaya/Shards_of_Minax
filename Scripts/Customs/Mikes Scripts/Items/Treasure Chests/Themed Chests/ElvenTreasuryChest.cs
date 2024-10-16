using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ElvenTreasuryChest : WoodenChest
    {
        [Constructable]
        public ElvenTreasuryChest()
        {
            Name = "Elven Treasury";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Emerald>("Emerald of the Elves"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Elven Wine", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Elven Riches"), 0.17);
            AddItem(CreateNamedItem<SilverRing>("Silver Elven Ring"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of the Elven King", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Elven Mead"), 0.08);
            AddItem(CreateGoldItem("Elven Gold Coin"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Elven Ranger", 1618), 0.10);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Elven Earring"), 0.10);
            AddItem(CreateMap(), 0.10);
            AddItem(CreateTunic(), 0.2);
            AddItem(CreateArmor(), 0.2);
            AddItem(CreateBattleAxe(), 0.2);
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
            note.NoteString = "The elves are known for their wealth and craftsmanship.";
            note.TitleString = "Elven Ledger";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Elven City";
            map.Bounds = new Rectangle2D(3000, 3000, 1000, 1000); // Adjust bounds as needed
            return map;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Smith's Protective Tunic";
            tunic.Hue = Utility.RandomMinMax(600, 1600);
            tunic.ClothingAttributes.DurabilityBonus = 4;
            tunic.Attributes.BonusStr = 20;
            tunic.SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
            return tunic;
        }

        private Item CreateArmor()
        {
            PlateChest armor = new PlateChest();
            armor.Name = "Vortex Mantle";
            armor.Hue = Utility.RandomList(1, 500);
            armor.BaseArmorRating = Utility.Random(35, 85);
            armor.AbsorptionAttributes.ResonanceEnergy = 50;
            armor.Attributes.LowerManaCost = -15;
            armor.SkillBonuses.SetValues(0, SkillName.EvalInt, 25.0);
            armor.ColdBonus = 10;
            armor.FireBonus = 10;
            armor.PhysicalBonus = 10;
            armor.PoisonBonus = 10;
            return armor;
        }

        private Item CreateBattleAxe()
        {
            BattleAxe axe = new BattleAxe();
            axe.Name = "Soul Taker";
            axe.Hue = Utility.RandomMinMax(800, 1000);
            axe.MinDamage = Utility.Random(30, 80);
            axe.MaxDamage = Utility.Random(80, 115);
            axe.Attributes.LowerManaCost = 20;
            axe.Attributes.WeaponSpeed = 10;
            axe.Slayer = SlayerName.ElementalHealth;
            axe.WeaponAttributes.HitManaDrain = 30;
            axe.WeaponAttributes.HitLeechHits = 20;
            axe.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            return axe;
        }

        public ElvenTreasuryChest(Serial serial) : base(serial)
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
