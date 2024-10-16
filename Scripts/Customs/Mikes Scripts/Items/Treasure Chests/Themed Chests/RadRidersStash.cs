using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RadRidersStash : WoodenChest
    {
        [Constructable]
        public RadRidersStash()
        {
            Name = "Rad Rider's Stash";
            Hue = Utility.Random(1, 2080);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateNamedItem<Emerald>("Green Grip Gem"), 0.25);
            AddItem(CreateNamedItem<MaxxiaScroll>("Skate Park Pass"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel4>("Golden Skateboard"), 0.30);
            AddItem(CreateNamedItem<SilverNecklace>("Extreme Sports Pendant"), 0.34);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4700)), 0.18);
            AddItem(CreateWeapon(), 0.2);
            AddItem(CreateNamedItem<Bandana>("Grunge Bandana"), 0.2);
            AddItem(CreateNamedItem<BoneGloves>("Monk's Soul Gloves"), 0.2);
            AddItem(CreateNamedItem<Bow>("Bow of Israfil"), 0.20);
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

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Ride the ramps, rule the streets!";
            note.TitleString = "Skate Pro's Manual";
            return note;
        }

        private Item CreateWeapon()
        {
            Maul weapon = new Maul();
            weapon.Name = "Wheelie Warhammer";
            weapon.Hue = Utility.RandomList(1, 2080);
            return weapon;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Grunge Bandana";
            bandana.Hue = Utility.RandomMinMax(250, 750);
            bandana.Attributes.BonusDex = 10;
            bandana.Attributes.AttackChance = 5;
            bandana.SkillBonuses.SetValues(0, SkillName.Focus, 20.0);
            bandana.SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
            return bandana;
        }

        private Item CreateBoneGloves()
        {
            BoneGloves gloves = new BoneGloves();
            gloves.Name = "Monk's Soul Gloves";
            gloves.Hue = Utility.RandomMinMax(100, 300);
            gloves.BaseArmorRating = Utility.Random(30, 60);
            gloves.Attributes.BonusStam = 20;
            gloves.Attributes.AttackChance = 15;
            gloves.Attributes.RegenHits = 5;
            gloves.SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
            gloves.SkillBonuses.SetValues(1, SkillName.Macing, 10.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 5;
            gloves.FireBonus = 15;
            gloves.PhysicalBonus = 20;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Bow of Israfil";
            bow.Hue = Utility.RandomMinMax(600, 800);
            bow.MinDamage = Utility.Random(25, 55);
            bow.MaxDamage = Utility.Random(55, 85);
            bow.Attributes.RegenMana = 5;
            bow.Attributes.SpellChanneling = 1;
            bow.Slayer = SlayerName.ElementalBan;
            bow.WeaponAttributes.HitFireArea = 30;
            bow.WeaponAttributes.HitEnergyArea = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
            return bow;
        }

        public RadRidersStash(Serial serial) : base(serial)
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
