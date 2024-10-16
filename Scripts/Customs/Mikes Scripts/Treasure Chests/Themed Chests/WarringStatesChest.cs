using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WarringStatesChest : WoodenChest
    {
        [Constructable]
        public WarringStatesChest()
        {
            Name = "Warring States";
            Hue = Utility.Random(1, 1100);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateNamedItem<Emerald>("Jade Arrowhead"), 0.23);
            AddItem(CreateNamedItem<GreaterHealPotion>("Warlord's Brew"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel3>("General's Insignia"), 0.26);
            AddItem(CreateNamedItem<SilverNecklace>("Necklace of Bravery"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.12);
            AddItem(CreateArmor(), 0.18);
            AddItem(CreateNamedItem<Spear>("Spear of the Warrior"), 0.20);
            AddItem(CreateCap(), 0.20);
            AddItem(CreatePlateGorget(), 0.20);
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

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of the Warlord";
            armor.Hue = Utility.RandomList(1, 1100);
            armor.BaseArmorRating = Utility.Random(3, 5); // Base Armor Rating
            return armor;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Navigator's Protective Cap";
            cap.Hue = Utility.RandomMinMax(400, 1400);
            cap.ClothingAttributes.LowerStatReq = 4;
            cap.Attributes.NightSight = 1;
            cap.SkillBonuses.SetValues(0, SkillName.Cartography, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.DetectHidden, 10.0);
            return cap;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget gorget = new PlateGorget();
            gorget.Name = "Thundergod's Vigor";
            gorget.Hue = Utility.RandomMinMax(600, 900);
            gorget.BaseArmorRating = Utility.Random(35, 65);
            gorget.AbsorptionAttributes.EaterEnergy = 20;
            gorget.ArmorAttributes.DurabilityBonus = 15;
            gorget.Attributes.BonusStr = 15;
            gorget.Attributes.ReflectPhysical = 5;
            gorget.SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);
            gorget.ColdBonus = 10;
            gorget.EnergyBonus = 20;
            gorget.FireBonus = 5;
            gorget.PhysicalBonus = 10;
            gorget.PoisonBonus = 10;
            return gorget;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Guillotine Blade Dagger";
            dagger.Hue = Utility.RandomMinMax(100, 200);
            dagger.MinDamage = Utility.Random(20, 40);
            dagger.MaxDamage = Utility.Random(40, 80);
            dagger.Attributes.LowerManaCost = 10;
            dagger.Attributes.ReflectPhysical = 10;
            dagger.Slayer = SlayerName.BloodDrinking;
            dagger.WeaponAttributes.HitManaDrain = 30;
            dagger.SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
            return dagger;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "To the victor goes the spoils of war.";
            note.TitleString = "Battle Plans";
            return note;
        }

        public WarringStatesChest(Serial serial) : base(serial)
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
