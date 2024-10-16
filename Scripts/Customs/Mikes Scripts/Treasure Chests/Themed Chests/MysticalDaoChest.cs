using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MysticalDaoChest : WoodenChest
    {
        [Constructable]
        public MysticalDaoChest()
        {
            Name = "Mystical Dao";
            Hue = Utility.Random(1, 1500);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Sapphire>("Yin Yang Crystal"), 0.29);
            AddItem(CreateNamedItem<Apple>("Heavenly Fruit"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel2>("Talisman of Balance"), 0.25);
            AddItem(CreateNamedItem<GoldEarrings>("Earthsong Earrings"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4200)), 0.14);
            AddItem(CreateLootItem<Diamond>("Spiritual Stone"), 0.20);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
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

        private Item CreateLootItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Balance in all things is the key to enlightenment.";
            note.TitleString = "Daoist Teachings";
            return note;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Scriptorium Master's Robe";
            robe.Hue = Utility.RandomMinMax(450, 1450);
            robe.ClothingAttributes.SelfRepair = 4;
            robe.Attributes.BonusInt = 15;
            robe.Attributes.SpellChanneling = 1;
            robe.SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
            robe.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
            return robe;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Tyrael's Vigil";
            plateChest.Hue = Utility.RandomMinMax(400, 700);
            plateChest.BaseArmorRating = Utility.RandomMinMax(50, 85);
            plateChest.AbsorptionAttributes.EaterDamage = 20;
            plateChest.ArmorAttributes.DurabilityBonus = 50;
            plateChest.Attributes.DefendChance = 20;
            plateChest.Attributes.RegenHits = 10;
            plateChest.SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
            plateChest.ColdBonus = 15;
            plateChest.EnergyBonus = 15;
            plateChest.FireBonus = 15;
            plateChest.PhysicalBonus = 25;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Grimmblade";
            longsword.Hue = Utility.RandomMinMax(500, 700);
            longsword.MinDamage = Utility.RandomMinMax(25, 75);
            longsword.MaxDamage = Utility.RandomMinMax(75, 105);
            longsword.Attributes.BonusStr = 20;
            longsword.Attributes.NightSight = 1;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitFireball = 25;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return longsword;
        }

        public MysticalDaoChest(Serial serial) : base(serial)
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
