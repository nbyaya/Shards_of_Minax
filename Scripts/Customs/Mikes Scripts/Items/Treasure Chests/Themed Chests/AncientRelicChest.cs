using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AncientRelicChest : WoodenChest
    {
        [Constructable]
        public AncientRelicChest()
        {
            Name = "Ancient Relic";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Sapphire of the Stars"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Elixir of Madness", 1175), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Cultist’s Offering"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Tentacle Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Abyss", 1176), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Bloodwine"), 0.08);
            AddItem(CreateGoldItem("Eldritch Coin"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Follower", 1177), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Eye of Horus Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Ancient Oracle’s Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Mysterious Cure"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateChainCoif(), 0.20);
            AddItem(CreateWarAxe(), 0.20);
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
            note.NoteString = "The stars are right!";
            note.TitleString = "Cthulhu’s Call";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Lost City";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new WarAxe());
            weapon.Name = "Nyarlathotep";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseClothing armor = Utility.RandomList<BaseClothing>(new Robe(), new Tunic());
            armor.Name = "Cultist’s Robe";
            armor.Hue = Utility.RandomList(1, 1788);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Grappler's Tunic";
            tunic.Hue = Utility.RandomMinMax(100, 1100);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusStr = 20;
            tunic.Attributes.DefendChance = 10;
            tunic.SkillBonuses.SetValues(0, SkillName.Wrestling, 25.0);
            return tunic;
        }

        private Item CreateChainCoif()
        {
            ChainCoif coif = new ChainCoif();
            coif.Name = "Viper's Coif";
            coif.Hue = Utility.RandomMinMax(100, 300);
            coif.BaseArmorRating = Utility.Random(30, 65);
            coif.AbsorptionAttributes.EaterPoison = 20;
            coif.ArmorAttributes.SelfRepair = 5;
            coif.Attributes.BonusInt = 15;
            coif.Attributes.RegenMana = 5;
            coif.SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            coif.ColdBonus = 5;
            coif.EnergyBonus = 5;
            coif.FireBonus = 5;
            coif.PhysicalBonus = 10;
            coif.PoisonBonus = 25;
            return coif;
        }

        private Item CreateWarAxe()
        {
            WarAxe warAxe = new WarAxe();
            warAxe.Name = "Axe of the Runeweaver";
            warAxe.Hue = Utility.RandomMinMax(890, 910);
            warAxe.MinDamage = Utility.Random(30, 60);
            warAxe.MaxDamage = Utility.Random(80, 110);
            warAxe.Attributes.SpellDamage = 10;
            warAxe.Attributes.LowerManaCost = 10;
            warAxe.Slayer = SlayerName.ElementalHealth;
            warAxe.WeaponAttributes.HitMagicArrow = 25;
            warAxe.WeaponAttributes.MageWeapon = 1;
            warAxe.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            warAxe.SkillBonuses.SetValues(1, SkillName.Inscribe, 15.0);
            return warAxe;
        }

        public AncientRelicChest(Serial serial) : base(serial)
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
