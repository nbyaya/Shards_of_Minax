using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RenaissanceCollectorsChest : WoodenChest
    {
        [Constructable]
        public RenaissanceCollectorsChest()
        {
            Name = "Renaissance Collector's Chest";
            Hue = 1220;

            // Add items to the chest
            AddItem(CreateEmerald(), 0.20);
            AddItem(CreateSimpleNote(), 0.18);
            AddItem(CreateNamedItem<TreasureLevel4>("Artisan's Prize"), 0.16);
            AddItem(CreateColoredItem<GoldEarrings>("Florentine Sunburst Earring", 1225), 0.15);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.20);
            AddItem(CreateNamedItem<Apple>("Artist's Delight"), 0.08);
            AddItem(CreateNamedItem<GreaterHealPotion>("Tuscan Vintage"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Sculptor", 1222), 0.15);
            AddItem(CreateLoot<Lute>("Medici's Lute"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Renaissance Observer"), 0.13);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateNorseHelm(), 0.20);
            AddItem(CreateLongsword(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Da Vinci's Masterpiece";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Art is the window to the soul.";
            note.TitleString = "Leonardo's Musings";
            return note;
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

        private Item CreateLoot<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Minstrel's Tuned Tunic";
            tunic.Hue = Utility.RandomMinMax(300, 1300);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusDex = 10;
            tunic.Attributes.BonusInt = 5;
            tunic.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
            return tunic;
        }

        private Item CreateNorseHelm()
        {
            NorseHelm helm = new NorseHelm();
            helm.Name = "Shinobi Hood";
            helm.Hue = Utility.RandomMinMax(500, 600);
            helm.BaseArmorRating = Utility.Random(30, 60);
            helm.AbsorptionAttributes.EaterPoison = 10;
            helm.ArmorAttributes.SelfRepair = 3;
            helm.Attributes.BonusInt = 10;
            helm.Attributes.NightSight = 1;
            helm.SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
            helm.ColdBonus = 5;
            helm.EnergyBonus = 10;
            helm.FireBonus = 5;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 15;
            return helm;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Glass Sword";
            longsword.Hue = 0;
            longsword.MinDamage = 1;
            longsword.MaxDamage = 200;
            longsword.Attributes.Luck = 150;
            longsword.Slayer = SlayerName.ElementalBan;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 30.0);
            return longsword;
        }

        public RenaissanceCollectorsChest(Serial serial) : base(serial)
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
