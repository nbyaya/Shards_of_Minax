using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EliteFoursVault : WoodenChest
    {
        [Constructable]
        public EliteFoursVault()
        {
            Name = "Elite Four's Vault";
            Hue = 1149;

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Lance's Dragon Scale", 1223), 0.20);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel4>("Elite's Golden Badge"), 0.13);
            AddItem(CreateColoredItem<GoldEarrings>("Gengar's Shadow Earring", 1125), 0.20);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.19);
            AddItem(CreateNamedItem<Apple>("Machamp's Power Fruit"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Alakazam's Psychic Brew"), 0.15);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Battler", 1130), 0.14);
            AddItem(CreateNamedItem<Shield>("Shield of Onix"), 0.03);
            AddItem(CreateNamedItem<Spyglass>("Gym Leader's Insight Spyglass"), 0.11);
            AddItem(CreateNamedItem<Shaft>("Wand of Eevee's Evolution"), 0.18);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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
            note.NoteString = "To be the best, you must challenge the best.";
            note.TitleString = "Champion's Decree";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of Steelix";
            armor.Hue = Utility.RandomMinMax(1, 1135);
            armor.BaseArmorRating = Utility.Random(20, 55);
            return armor;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Mixmaster's Muffler";
            muffler.Hue = Utility.RandomMinMax(350, 1350);
            muffler.Attributes.BonusDex = 10;
            muffler.Attributes.RegenStam = 5;
            muffler.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            return muffler;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Lyricist's Insight";
            cap.Hue = Utility.RandomMinMax(50, 500);
            cap.BaseArmorRating = Utility.Random(15, 50);
            cap.AbsorptionAttributes.EaterFire = 10;
            cap.Attributes.BonusMana = 20;
            cap.Attributes.CastRecovery = 1;
            cap.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
            cap.ColdBonus = 5;
            cap.EnergyBonus = 10;
            cap.FireBonus = 15;
            cap.PhysicalBonus = 5;
            cap.PoisonBonus = 10;
            return cap;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Excalibur Longsword";
            longsword.Hue = Utility.RandomMinMax(400, 600);
            longsword.MinDamage = Utility.Random(40, 70);
            longsword.MaxDamage = Utility.Random(70, 110);
            longsword.Attributes.BonusStr = 20;
            longsword.Attributes.DefendChance = 10;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitLightning = 40;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return longsword;
        }

        public EliteFoursVault(Serial serial) : base(serial)
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
