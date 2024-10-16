using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ElvenEnchantressChest : WoodenChest
    {
        [Constructable]
        public ElvenEnchantressChest()
        {
            Name = "Elven Enchantress's Chest";
            Hue = Utility.Random(1, 1288);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Elven Court", 1288), 0.25);
            AddItem(CreateColoredItem<Emerald>("Elven Heartstone", 1485), 0.18);
            AddItem(CreateNamedItem<TreasureLevel2>("Enchantress's Secret Cache"), 0.10);
            AddItem(CreateJewelry(), 0.15);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.16);
            AddItem(CreateNamedItem<Sapphire>("Sapphire of Elven Grace"), 0.14);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Elixir of Elven Might"), 0.13);
            AddItem(CreateRandomGem(), 0.20);
            AddItem(CreateRandomInstrument(), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Elven Moon Earrings"), 0.17);
            AddItem(CreateNamedItem<Spellbook>("Enchantress's Spellbook"), 0.04);
            AddItem(CreateMysticArmor(), 0.2);
            AddItem(CreateBodySash(), 0.2);
            AddItem(CreateLeatherLegs(), 0.2);
            AddItem(CreateWarMace(), 0.2);
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

        private Item CreateJewelry()
        {
            BaseJewel jewelry = Utility.RandomList<BaseJewel>(new GoldRing(), new SilverRing(), new GoldBracelet(), new SilverBracelet());
            jewelry.Name = "Elven Enchanted Jewel";
            jewelry.Hue = Utility.RandomList(1, 1288);
            return jewelry;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "To those who find this, beware the Elven curse!";
            note.TitleString = "Enchantress's Warning";
            return note;
        }

        private Item CreateRandomGem()
        {
            Item gem = Loot.RandomGem();
            gem.Name = "Elven Gem";
            return gem;
        }

        private Item CreateRandomInstrument()
        {
            BaseInstrument instrument = Loot.RandomInstrument();
            instrument.Name = "Elven Harp";
            return instrument;
        }

        private Item CreateMysticArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherArms(), new LeatherLegs(), new LeatherGloves());
            armor.Name = "Elven Mystic Armor";
            armor.Hue = Utility.RandomList(1, 1288);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBodySash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Sommelier's Body Sash";
            sash.Hue = Utility.RandomMinMax(400, 1200);
            sash.Attributes.BonusInt = 7;
            sash.Attributes.Luck = 10;
            sash.SkillBonuses.SetValues(0, SkillName.TasteID, 20.0);
            return sash;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Jester's Trickster Boots";
            legs.Hue = Utility.RandomMinMax(500, 800);
            legs.BaseArmorRating = Utility.Random(20, 55);
            legs.ArmorAttributes.ReactiveParalyze = 1;
            legs.Attributes.BonusDex = 35;
            legs.Attributes.DefendChance = 20;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateWarMace()
        {
            WarMace mace = new WarMace();
            mace.Name = "Bane of the Dead";
            mace.Hue = Utility.RandomMinMax(600, 800);
            mace.MinDamage = Utility.Random(25, 70);
            mace.MaxDamage = Utility.Random(70, 110);
            mace.Attributes.SpellChanneling = 1;
            mace.Attributes.NightSight = 1;
            mace.Slayer = SlayerName.Exorcism;
            mace.Slayer2 = SlayerName.DaemonDismissal;
            mace.WeaponAttributes.HitMagicArrow = 20;
            mace.WeaponAttributes.HitManaDrain = 10;
            mace.SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            return mace;
        }

        public ElvenEnchantressChest(Serial serial) : base(serial)
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
