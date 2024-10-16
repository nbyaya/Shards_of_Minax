using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MysticEnigmaChest : WoodenChest
    {
        [Constructable]
        public MysticEnigmaChest()
        {
            Name = "Mystic's Enigma";
            Hue = Utility.Random(1, 1700);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateColoredItem<Sapphire>("Crystal of the Mages", 2994), 0.25);
            AddItem(CreateColoredItem<Emerald>("Gem of Enchantment", 1650), 0.20);
            AddItem(CreateNamedItem<TreasureLevel2>("Mystic's Relic"), 0.22);
            AddItem(CreateNamedItem<SilverNecklace>("Necklace of Divination"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Mystical Elixir", 1500), 0.11);
            AddItem(CreateArmor(), 0.16);
            AddItem(CreateScroll(), 0.21);
            AddItem(CreateRandomWand(), 0.20);
            AddItem(CreateJewelry(), 0.22);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateBuckler(), 0.20);
            AddItem(CreateMace(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "The prophecy shall be revealed.";
            note.TitleString = "Mystic's Journal";
            return note;
        }

        private Item CreateScroll()
        {
            MaxxiaScroll scroll = new MaxxiaScroll();
            scroll.Name = "Scroll of the Seer";
            return scroll;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Mystic's Wand";
            return wand;
        }

        private Item CreateJewelry()
        {
            Diamond jewelry = new Diamond();
            jewelry.Name = "Amulet of Clairvoyance";
            jewelry.Hue = Utility.RandomList(1, 1650);
            return jewelry;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Pickpocket's Sleek Tunic";
            tunic.Hue = Utility.RandomMinMax(300, 1200);
            tunic.ClothingAttributes.SelfRepair = 2;
            tunic.Attributes.BonusDex = 15;
            tunic.Attributes.RegenStam = 2;
            tunic.SkillBonuses.SetValues(0, SkillName.Snooping, 20.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Stealing, 25.0);
            return tunic;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherLegs(), new LeatherArms(), new LeatherCap());
            armor.Name = "Arcane Protection";
            armor.Hue = Utility.RandomList(1, 1700);
            return armor;
        }

        private Item CreateBuckler()
        {
            Buckler buckler = new Buckler();
            buckler.Name = "Outlaw's Forest Buckler";
            buckler.Hue = Utility.RandomMinMax(250, 550);
            buckler.BaseArmorRating = Utility.RandomMinMax(20, 40);
            buckler.ArmorAttributes.LowerStatReq = 10;
            buckler.Attributes.ReflectPhysical = 10;
            buckler.Attributes.DefendChance = 10;
            buckler.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            buckler.PhysicalBonus = 15;
            buckler.ColdBonus = 5;
            return buckler;
        }

        private Item CreateMace()
        {
            Mace mace = new Mace();
            mace.Name = "Dawnbreaker Mace";
            mace.Hue = Utility.RandomMinMax(500, 600);
            mace.MinDamage = Utility.RandomMinMax(25, 60);
            mace.MaxDamage = Utility.RandomMinMax(60, 95);
            mace.Attributes.ReflectPhysical = 10;
            mace.Attributes.SpellChanneling = 1;
            mace.Slayer = SlayerName.Exorcism;
            mace.WeaponAttributes.HitFireArea = 30;
            mace.WeaponAttributes.HitDispel = 20;
            mace.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            return mace;
        }

        public MysticEnigmaChest(Serial serial) : base(serial)
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
