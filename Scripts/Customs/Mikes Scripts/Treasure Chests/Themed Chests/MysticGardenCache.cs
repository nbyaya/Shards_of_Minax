using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MysticGardenCache : WoodenChest
    {
        [Constructable]
        public MysticGardenCache()
        {
            Name = "Mystic Garden's Cache";
            Hue = Utility.Random(1, 1188);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Ruby>("Peridot of the Mystic Garden"), 0.24);
            AddItem(CreateNamedItem<TreasureLevel1>("Secrets of the Mystic Garden"), 0.09);
            AddItem(CreateArmor(), 0.10);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 3500)), 0.16);
            AddItem(CreateNamedItem<Emerald>("Emerald of the Green Glade"), 0.15);
            AddItem(CreateBuckler(), 0.2);
            AddItem(CreateMaul(), 0.2);
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
            note.NoteString = "In the heart of the garden, magic blooms eternal.";
            note.TitleString = "Garden's Whisper";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeafChest(), new LeafArms(), new LeafLegs(), new LeafChest());
            armor.Name = "Leafy Armor of the Garden";
            armor.Hue = Utility.RandomList(1, 1188);
            return armor;
        }

        private Item CreateBuckler()
        {
            Buckler buckler = new Buckler();
            buckler.Name = "Jester's Mischievous Buckler";
            buckler.Hue = Utility.RandomMinMax(500, 800);
            buckler.BaseArmorRating = Utility.Random(25, 65);
            buckler.AbsorptionAttributes.EaterFire = 25;
            buckler.ArmorAttributes.ReactiveParalyze = 1;
            buckler.Attributes.ReflectPhysical = 20;
            buckler.Attributes.AttackChance = 15;
            buckler.SkillBonuses.SetValues(0, SkillName.Parry, 25.0);
            buckler.ColdBonus = 10;
            buckler.EnergyBonus = 10;
            buckler.FireBonus = 10;
            buckler.PhysicalBonus = 10;
            buckler.PoisonBonus = 10;
            return buckler;
        }

        private Item CreateMaul()
        {
            Maul maul = new Maul();
            maul.Name = "Earthshaker Maul";
            maul.Hue = Utility.RandomMinMax(100, 300);
            maul.MinDamage = Utility.Random(30, 80);
            maul.MaxDamage = Utility.Random(80, 120);
            maul.Attributes.BonusHits = 15;
            maul.Attributes.AttackChance = 5;
            maul.Slayer = SlayerName.EarthShatter;
            maul.Slayer2 = SlayerName.Terathan;
            maul.WeaponAttributes.HitHarm = 25;
            maul.WeaponAttributes.HitPhysicalArea = 20;
            maul.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
            maul.SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
            return maul;
        }

        public MysticGardenCache(Serial serial) : base(serial)
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
