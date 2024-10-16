using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SlavicLegendsChest : WoodenChest
    {
        [Constructable]
        public SlavicLegendsChest()
        {
            Name = "Slavic Legends Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateEmerald(), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel2>("Slavic Artifact"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Veles Charm Earring", 1165), 0.15);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.18);
            AddItem(CreateNamedItem<Apple>("Baba Yaga's Cursed Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Slavic Mead"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Rusalka", 1192), 0.19);
            AddItem(CreateNamedItem<Spyglass>("Bard's Tale Viewer"), 0.12);
            AddItem(CreateRandomInstrument(), 0.14);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateCap(), 0.20);
            AddItem(CreateLeatherGloves(), 0.20);
            AddItem(CreateScimitar(), 0.20);
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
            emerald.Name = "Jewel of Perun";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Heed the tales of old Slavic lands.";
            note.TitleString = "Slavic Bard's Song";
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

        private Item CreateRandomInstrument()
        {
            Drums instrument = new Drums();
            instrument.Name = "Slavic Lute";
            return instrument;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Slavic Folk Garb";
            return clothing;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Sword of Slavic Heroes";
            weapon.Hue = 1157;
            weapon.MaxDamage = Utility.Random(25, 65);
            return weapon;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Street Performer's Cap";
            cap.Hue = Utility.RandomMinMax(100, 1500);
            cap.Attributes.AttackChance = 5;
            cap.Attributes.Luck = 15;
            cap.SkillBonuses.SetValues(0, SkillName.Begging, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.Musicianship, 10.0);
            return cap;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Shadow Grip Gloves";
            gloves.Hue = Utility.RandomMinMax(500, 600);
            gloves.BaseArmorRating = Utility.Random(25, 55);
            gloves.AbsorptionAttributes.EaterCold = 10;
            gloves.Attributes.WeaponSpeed = 5;
            gloves.Attributes.DefendChance = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Lockpicking, 10.0);
            gloves.SkillBonuses.SetValues(1, SkillName.RemoveTrap, 10.0);
            gloves.ColdBonus = 15;
            gloves.EnergyBonus = 5;
            gloves.FireBonus = 10;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Sunblade";
            scimitar.Hue = Utility.RandomMinMax(100, 200);
            scimitar.MinDamage = Utility.Random(20, 70);
            scimitar.MaxDamage = Utility.Random(70, 100);
            scimitar.Attributes.Luck = 50;
            scimitar.Attributes.ReflectPhysical = 10;
            scimitar.Slayer = SlayerName.Exorcism;
            scimitar.WeaponAttributes.HitFireball = 30;
            scimitar.WeaponAttributes.ResistColdBonus = 20;
            scimitar.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            return scimitar;
        }

        public SlavicLegendsChest(Serial serial) : base(serial)
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
