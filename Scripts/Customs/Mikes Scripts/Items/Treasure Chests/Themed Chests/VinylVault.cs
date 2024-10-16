using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class VinylVault : WoodenChest
    {
        [Constructable]
        public VinylVault()
        {
            Name = "Vinyl Vault";
            Hue = Utility.Random(1, 1650);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.09);
            AddItem(CreateGoldVoucher(), 0.27);
            AddItem(CreateRandomInstrument(), 0.28);
            AddItem(CreateNamedItem<TreasureLevel3>("Rock Legend's Keepsake"), 0.24);
            AddItem(CreateNamedItem<SilverNecklace>("Rockstar Amulet"), 0.35);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.14);
            AddItem(CreateRandomWand(), 0.18);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateWizardHat(), 0.20);
            AddItem(CreatePlateHelm(), 0.20);
            AddItem(CreateWarMace(), 0.20);
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

        private Item CreateGoldVoucher()
        {
            MaxxiaScroll voucher = new MaxxiaScroll();
            voucher.Name = "Golden Record";
            return voucher;
        }

        private Item CreateRandomInstrument()
        {
            Lute instrument = new Lute();
            instrument.Name = "Vintage Guitar";
            return instrument;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Ledgers of legendary licks.";
            note.TitleString = "Rockstar's Journal";
            return note;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Mic of the Maestro";
            return wand;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Rhythm Blade";
            weapon.Hue = Utility.RandomList(1, 1650);
            weapon.MaxDamage = Utility.Random(2, 3); // Adjust if needed
            return weapon;
        }

        private Item CreateWizardHat()
        {
            WizardsHat hat = new WizardsHat();
            hat.Name = "Psychedelic Wizard's Hat";
            hat.Hue = Utility.RandomMinMax(100, 1100);
            hat.Attributes.BonusInt = 20;
            hat.Attributes.SpellDamage = 10;
            hat.SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
            hat.SkillBonuses.SetValues(1, SkillName.Spellweaving, 10.0);
            return hat;
        }

        private Item CreatePlateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Courtier's Regal Circlet";
            helm.Hue = Utility.RandomMinMax(700, 950);
            helm.BaseArmorRating = Utility.RandomMinMax(35, 60);
            helm.AbsorptionAttributes.EaterCold = 10;
            helm.ArmorAttributes.SelfRepair = 3;
            helm.Attributes.BonusInt = 15;
            helm.Attributes.NightSight = 1;
            helm.SkillBonuses.SetValues(0, SkillName.Provocation, 15.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 10;
            helm.FireBonus = 5;
            helm.PhysicalBonus = 5;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateWarMace()
        {
            WarMace mace = new WarMace();
            mace.Name = "Anubis WarMace";
            mace.Hue = Utility.RandomMinMax(500, 700);
            mace.MinDamage = Utility.RandomMinMax(25, 70);
            mace.MaxDamage = Utility.RandomMinMax(70, 110);
            mace.Attributes.BonusStr = 10;
            mace.Attributes.RegenHits = 3;
            mace.Slayer = SlayerName.Exorcism;
            mace.Slayer2 = SlayerName.BalronDamnation;
            mace.WeaponAttributes.HitHarm = 20;
            mace.WeaponAttributes.BloodDrinker = 15;
            mace.SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            return mace;
        }

        public VinylVault(Serial serial) : base(serial)
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
