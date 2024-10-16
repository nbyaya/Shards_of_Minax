using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WingedHusChest : WoodenChest
    {
        [Constructable]
        public WingedHusChest()
        {
            Name = "Winged Hussars Chest";
            Hue = 1150;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Sapphire>("Hussar's Pride"), 0.20);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel3>("Hussar's Treasure"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Eagle's Talon Earring", 1198), 0.18);
            AddItem(new Gold(Utility.Random(1, 5200)), 0.18);
            AddItem(CreateNamedItem<Apple>("Apple of Sobieski"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Warsaw Brew"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Winged Cavalry", 1196), 0.15);
            AddItem(CreateNamedItem<WoodenShield>("Hussar's Shield"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Hussar's Battlefield Viewer"), 0.14);
            AddItem(CreateNamedItem<Shaft>("Wand of Polish Might"), 0.12);
            AddItem(CreateNamedItem<PlateChest>("Hussar's Uniform"), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateStuddedGorget(), 0.20);
            AddItem(CreateDagger(), 0.20);
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
            note.NoteString = "For the honor and glory of Poland!";
            note.TitleString = "Hussar's Oath";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of the Winged Hussar";
            armor.Hue = Utility.Random(1, 1152);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Diplomat's Tunic";
            tunic.Hue = Utility.RandomMinMax(250, 1150);
            tunic.Attributes.BonusInt = 20;
            tunic.Attributes.LowerManaCost = 10;
            tunic.SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0); // Use the appropriate skill for your server
            return tunic;
        }

        private Item CreateStuddedGorget()
        {
            StuddedGorget gorget = new StuddedGorget();
            gorget.Name = "Witch's Heart Amulet";
            gorget.Hue = Utility.RandomMinMax(500, 750);
            gorget.BaseArmorRating = Utility.Random(20, 50);
            gorget.AbsorptionAttributes.EaterFire = 15;
            gorget.ArmorAttributes.ReactiveParalyze = 1;
            gorget.Attributes.ReflectPhysical = 10;
            gorget.Attributes.BonusHits = 20;
            gorget.SkillBonuses.SetValues(0, SkillName.Meditation, 15.0);
            gorget.ColdBonus = 5;
            gorget.EnergyBonus = 10;
            gorget.FireBonus = 20;
            gorget.PhysicalBonus = 10;
            gorget.PoisonBonus = 5;
            return gorget;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "VATS Enhanced Dagger";
            dagger.Hue = Utility.RandomMinMax(300, 500);
            dagger.MinDamage = Utility.Random(10, 45);
            dagger.MaxDamage = Utility.Random(45, 80);
            dagger.Attributes.AttackChance = 20;
            dagger.Attributes.WeaponSpeed = 10;
            dagger.Slayer = SlayerName.ReptilianDeath;
            dagger.WeaponAttributes.HitLeechMana = 10;
            dagger.WeaponAttributes.HitMagicArrow = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return dagger;
        }

        public WingedHusChest(Serial serial) : base(serial)
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
