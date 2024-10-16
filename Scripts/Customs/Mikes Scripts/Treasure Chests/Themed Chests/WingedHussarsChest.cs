using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WingedHussarsChest : WoodenChest
    {
        [Constructable]
        public WingedHussarsChest()
        {
            Name = "Winged Hussars Chest";
            Hue = 2154;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Sapphire>("Hussar's Pride", 1152), 0.20);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel3>("Hussar's Treasure"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Eagle's Talon Earring", 1198), 0.18);
            AddItem(new Gold(Utility.Random(1, 5200)), 0.18);
            AddItem(CreateNamedItem<Apple>("Apple of Sobieski"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Warsaw Brew"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Winged Cavalry", 1196), 0.15);
            AddItem(CreateRandomItem<WoodenShield>("Hussar's Shield"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Hussar's Battlefield Viewer"), 0.14);
            AddItem(CreateRandomItem<Shaft>("Wand of Polish Might"), 0.12);
            AddItem(CreateRandomItem<LeatherChest>("Hussar's Uniform"), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateFancyDress(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateWarHammer(), 0.20);
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
            note.NoteString = "For the honor and glory of Poland!";
            note.TitleString = "Hussar's Oath";
            return note;
        }

        private Item CreateRandomItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of the Winged Hussar";
            armor.Hue = Utility.Random(1, 1152);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFancyDress()
        {
            FancyDress dress = new FancyDress();
            dress.Name = "Courtisan's Refined Gown";
            dress.Hue = Utility.RandomMinMax(200, 700);
            dress.Attributes.BonusInt = 10;
            dress.Attributes.Luck = 20;
            dress.SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
            return dress;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Witch's Enchanted Hat";
            cap.Hue = Utility.RandomMinMax(500, 750);
            cap.BaseArmorRating = Utility.Random(25, 55);
            cap.AbsorptionAttributes.EaterEnergy = 20;
            cap.ArmorAttributes.MageArmor = 1;
            cap.Attributes.BonusInt = 15;
            cap.Attributes.CastRecovery = 1;
            cap.Attributes.SpellDamage = 10;
            cap.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            cap.ColdBonus = 5;
            cap.EnergyBonus = 15;
            cap.FireBonus = 5;
            cap.PhysicalBonus = 10;
            cap.PoisonBonus = 10;
            return cap;
        }

        private Item CreateWarHammer()
        {
            WarHammer warHammer = new WarHammer();
            warHammer.Name = "Whelm";
            warHammer.Hue = Utility.RandomMinMax(500, 600);
            warHammer.MinDamage = Utility.Random(30, 80);
            warHammer.MaxDamage = Utility.Random(80, 120);
            warHammer.Attributes.BonusStr = 15;
            warHammer.Attributes.AttackChance = 10;
            warHammer.Slayer2 = SlayerName.TrollSlaughter;
            warHammer.WeaponAttributes.HitHarm = 20;
            warHammer.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return warHammer;
        }

        public WingedHussarsChest(Serial serial) : base(serial)
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
