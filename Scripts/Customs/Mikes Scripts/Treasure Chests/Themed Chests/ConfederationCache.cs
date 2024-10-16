using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ConfederationCache : WoodenChest
    {
        [Constructable]
        public ConfederationCache()
        {
            Name = "Confederation Cache";
            Hue = Utility.Random(1, 1350);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Sapphire>("Jewel of Unity"), 0.27);
            AddItem(CreateNamedItem<MaxxiaScroll>("Founding Father's Deed"), 0.26);
            AddItem(CreateNamedItem<TreasureLevel4>("Charter of Rights"), 0.21);
            AddItem(CreateNamedItem<SilverNecklace>("Macdonald's Medallion"), 0.34);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.13);
            AddItem(CreateNamedItem<Ruby>("Gem of Dominion"), 1.0);
            AddItem(CreateBoots(), 0.20);
            AddItem(CreateChainChest(), 0.20);
            AddItem(CreateScimitar(), 0.20);
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
            note.NoteString = "From sea to sea, united we stand.";
            note.TitleString = "Confederation Proclamation";
            return note;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Explorer's Boots";
            boots.Hue = Utility.RandomMinMax(750, 1550);
            boots.ClothingAttributes.LowerStatReq = 4;
            boots.Attributes.BonusInt = 5;
            boots.SkillBonuses.SetValues(0, SkillName.Cartography, 25.0);
            return boots;
        }

        private Item CreateChainChest()
        {
            ChainChest chainChest = new ChainChest();
            chainChest.Name = "Skin of the Vipermagi";
            chainChest.Hue = Utility.RandomMinMax(200, 450);
            chainChest.BaseArmorRating = Utility.Random(35, 70);
            chainChest.AbsorptionAttributes.ResonancePoison = 20;
            chainChest.Attributes.LowerManaCost = 10;
            chainChest.Attributes.SpellDamage = 20;
            chainChest.ColdBonus = 15;
            chainChest.EnergyBonus = 30;
            chainChest.FireBonus = 20;
            chainChest.PhysicalBonus = 15;
            chainChest.PoisonBonus = 35;
            return chainChest;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Mortuary Sword";
            scimitar.Hue = Utility.RandomMinMax(250, 450);
            scimitar.MinDamage = Utility.Random(20, 60);
            scimitar.MaxDamage = Utility.Random(60, 90);
            scimitar.Attributes.BonusDex = 15;
            scimitar.Attributes.AttackChance = 10;
            scimitar.Slayer = SlayerName.TrollSlaughter;
            scimitar.WeaponAttributes.HitDispel = 20;
            scimitar.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return scimitar;
        }

        public ConfederationCache(Serial serial) : base(serial)
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
