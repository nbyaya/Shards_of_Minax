using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class Radical90sRelicsChest : WoodenChest
    {
        [Constructable]
        public Radical90sRelicsChest()
        {
            Name = "Radical '90s Relics";
            Hue = Utility.Random(1, 2350);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateColoredItem<Emerald>("Tamagotchi Gem"), 0.23);
            AddItem(CreateNamedItem<Apple>("Furbie Fruit"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel1>("Beanie Baby Bag"), 0.22);
            AddItem(CreateNamedItem<GoldEarrings>("Dial-Up Loop"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 2800)), 0.13);
            AddItem(CreateRandomInstrument(), 0.15);
            AddItem(CreateWeapon(), 1.0);
            AddItem(CreateBodySash(), 0.20);
            AddItem(CreateChainCoif(), 0.20);
            AddItem(CreateWarMace(), 0.20);
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
            item.Hue = Utility.RandomList(1, 2350);
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
            note.NoteString = "Dude, it's the '90s!";
            note.TitleString = "Radical Memoir";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            BaseInstrument instrument = Utility.RandomList<BaseInstrument>(new Lute(), new Drums(), new Tambourine());
            instrument.Name = "Slap Bracelet Snap";
            return instrument;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new WarMace());
            weapon.Name = "Pogs Slammer";
            return weapon;
        }

        private Item CreateBodySash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Emo Scene Hairpin";
            sash.Hue = Utility.RandomMinMax(700, 1700);
            sash.Attributes.ReflectPhysical = 5;
            sash.ClothingAttributes.MageArmor = 1;
            sash.SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
            return sash;
        }

        private Item CreateChainCoif()
        {
            ChainCoif coif = new ChainCoif();
            coif.Name = "Mako Resonance";
            coif.Hue = Utility.RandomMinMax(200, 600);
            coif.BaseArmorRating = Utility.Random(35, 70);
            coif.AbsorptionAttributes.EaterEnergy = 25;
            coif.ArmorAttributes.SelfRepair = 10;
            coif.Attributes.BonusMana = 40;
            coif.Attributes.EnhancePotions = 20;
            coif.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            coif.EnergyBonus = 20;
            coif.FireBonus = 10;
            coif.PhysicalBonus = 10;
            coif.ColdBonus = 10;
            coif.PoisonBonus = 10;
            return coif;
        }

        private Item CreateWarMace()
        {
            WarMace mace = new WarMace();
            mace.Name = "Pluto's Abyssal Mace";
            mace.Hue = Utility.RandomMinMax(700, 900);
            mace.MinDamage = Utility.Random(30, 85);
            mace.MaxDamage = Utility.Random(85, 125);
            mace.Attributes.BonusHits = 25;
            mace.Attributes.NightSight = 1;
            mace.Slayer = SlayerName.Exorcism;
            mace.Slayer2 = SlayerName.DaemonDismissal;
            mace.WeaponAttributes.HitFireArea = 25;
            mace.WeaponAttributes.HitManaDrain = 25;
            mace.SkillBonuses.SetValues(0, SkillName.Necromancy, 30.0);
            return mace;
        }

        public Radical90sRelicsChest(Serial serial) : base(serial)
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
