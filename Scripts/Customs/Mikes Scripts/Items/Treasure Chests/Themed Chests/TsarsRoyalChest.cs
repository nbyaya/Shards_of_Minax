using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TsarsRoyalChest : WoodenChest
    {
        [Constructable]
        public TsarsRoyalChest()
        {
            Name = "Tsar's Royal Chest";
            Hue = 2154;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Blue of the Russian Court", 2843), 0.18);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel4>("Romanov's Hidden Jewel"), 0.20);
            AddItem(CreateColoredItem<GoldEarrings>("Empress's Favorite Earring", 1175), 0.18);
            AddItem(new Gold(Utility.Random(1, 7000)), 0.18);
            AddItem(CreateNamedItem<Apple>("Royal Orchard Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Tsar's Feast Mead"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Imperial Guard", 1170), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Tsar's Personal Spyglass"), 0.04);
            AddItem(CreateNamedItem<Diamond>("Gem of the Ural Mountains"), 0.12);
            AddItem(CreateNamedItem<FancyShirt>("Court Attire of the Kremlin"), 0.14);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateBodySash(), 0.20);
            AddItem(CreateBoneLegs(), 0.20);
            AddItem(CreateTwoHandedAxe(), 0.20);
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
            note.NoteString = "The power of the Tsars is unparalleled in history.";
            note.TitleString = "Tsar's Edict";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Tsar's Royal Armor";
            armor.Hue = Utility.RandomList(1, 1174);
            armor.BaseArmorRating = Utility.Random(40, 80);
            return armor;
        }

        private Item CreateBodySash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Gaze-Capturing Veil";
            sash.Hue = Utility.RandomMinMax(200, 1300);
            sash.Attributes.BonusInt = 10;
            sash.Attributes.Luck = 20;
            sash.SkillBonuses.SetValues(0, SkillName.Begging, 20.0);
            sash.SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
            return sash;
        }

        private Item CreateBoneLegs()
        {
            BoneLegs legs = new BoneLegs();
            legs.Name = "Necromancer's Dark Leggings";
            legs.Hue = Utility.RandomMinMax(10, 250);
            legs.BaseArmorRating = Utility.Random(35, 75);
            legs.AbsorptionAttributes.EaterEnergy = 15;
            legs.ArmorAttributes.LowerStatReq = 20;
            legs.Attributes.BonusStam = 20;
            legs.Attributes.SpellChanneling = 1;
            legs.SkillBonuses.SetValues(0, SkillName.Focus, 20.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 20;
            legs.FireBonus = 5;
            legs.PhysicalBonus = 15;
            legs.PoisonBonus = 15;
            return legs;
        }

        private Item CreateTwoHandedAxe()
        {
            TwoHandedAxe axe = new TwoHandedAxe();
            axe.Name = "Paladin's Chrysblade";
            axe.Hue = Utility.RandomMinMax(50, 100);
            axe.MinDamage = Utility.Random(40, 70);
            axe.MaxDamage = Utility.Random(70, 110);
            axe.Attributes.BonusHits = 20;
            axe.Attributes.ReflectPhysical = 10;
            axe.Slayer = SlayerName.DragonSlaying;
            axe.WeaponAttributes.HitFireball = 25;
            axe.WeaponAttributes.ResistFireBonus = 20;
            axe.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            return axe;
        }

        public TsarsRoyalChest(Serial serial) : base(serial)
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
