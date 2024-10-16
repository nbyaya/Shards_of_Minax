using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class FunkyFashionChest : WoodenChest
    {
        [Constructable]
        public FunkyFashionChest()
        {
            Name = "Funky Fashion";
            Hue = Utility.Random(1, 1750);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateColoredItem<Sapphire>("Bell-Bottom Gem"), 0.30);
            AddItem(CreateNamedItem<MaxxiaScroll>("Groovy Glasses Voucher"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel4>("Retro Runway Relic"), 0.26);
            AddItem(CreateNamedItem<GoldEarrings>("Funky Fro Earrings"), 0.37);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4900)), 0.12);
            AddItem(CreateClothingItem("‘70s Suit Jacket"), 0.15);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreateBow(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateColoredItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = Utility.RandomList(1, 1750); // Random hue, similar to the XML
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
            note.NoteString = "Patterns, paisleys, and platforms.";
            note.TitleString = "Fashionista's Folio";
            return note;
        }

        private Item CreateClothingItem(string name)
        {
            BaseClothing clothing = Utility.RandomList<BaseClothing>(new Shirt(), new Robe());
            clothing.Name = name;
            clothing.Hue = Utility.RandomList(1, 1750); // Random hue
            return clothing;
        }

        private Item CreateBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Disco Diva Boots";
            boots.Hue = Utility.RandomMinMax(200, 999);
            boots.ClothingAttributes.LowerStatReq = 3;
            boots.Attributes.BonusDex = 15;
            boots.Attributes.Luck = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Provocation, 15.0);
            return boots;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Courtier's Silken Robe";
            robe.Hue = Utility.RandomMinMax(700, 950);
            robe.Attributes.BonusMana = 20;
            robe.Attributes.LowerManaCost = 5;
            robe.SkillBonuses.SetValues(0, SkillName.Peacemaking, 10.0);
            return robe;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Ma'at's Balanced Bow";
            bow.Hue = Utility.RandomMinMax(250, 450);
            bow.MinDamage = Utility.RandomMinMax(15, 55);
            bow.MaxDamage = Utility.RandomMinMax(55, 85);
            bow.Attributes.AttackChance = 10;
            bow.Attributes.DefendChance = 10;
            bow.Slayer = SlayerName.TrollSlaughter;
            bow.WeaponAttributes.HitLeechStam = 20;
            bow.WeaponAttributes.MageWeapon = 1;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return bow;
        }

        public FunkyFashionChest(Serial serial) : base(serial)
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
