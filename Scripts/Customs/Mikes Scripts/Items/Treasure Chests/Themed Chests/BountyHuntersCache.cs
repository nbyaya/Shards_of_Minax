using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BountyHuntersCache : WoodenChest
    {
        [Constructable]
        public BountyHuntersCache()
        {
            Name = "Bounty Hunter's Cache";
            Hue = Utility.Random(1, 1450);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.04);
            AddItem(CreateGoldVoucher(), 0.29);
            AddItem(CreateNamedItem<TreasureLevel4>("Han Solo in Carbonite"), 0.28);
            AddItem(CreateNamedItem<GoldEarrings>("Boba Fett's Emblem"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.16);
            AddItem(CreateRandomGem(), 0.20);
            AddItem(CreateWeapon(), 0.22);
            AddItem(CreateNamedItem<Bag>("Coin Purse of the Hutt"), 0.21);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWizardHat(), 0.20);
            AddItem(CreateNorseHelm(), 0.20);
            AddItem(CreateDagger(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateGoldVoucher()
        {
            MaxxiaScroll voucher = new MaxxiaScroll();
            voucher.Name = "Credit Chit";
            return voucher;
        }
		
        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateRandomGem()
        {
            Diamond gem = new Diamond();
            gem.Name = "Galactic Jewel";
            return gem;
        }

        private Item CreateWeapon()
        {
            Crossbow weapon = new Crossbow();
            weapon.Name = "Blaster Pistol";
            return weapon;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Smuggler's Outfit";
            clothing.Hue = Utility.RandomList(1, 1450);
            return clothing;
        }

        private Item CreateWizardHat()
        {
            WizardsHat hat = new WizardsHat();
            hat.Name = "Starlight Wizard's Hat";
            hat.Hue = Utility.RandomList(1100, 1130);
            hat.Attributes.BonusInt = 20;
            hat.Attributes.SpellDamage = 10;
            hat.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            hat.SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
            return hat;
        }

        private Item CreateNorseHelm()
        {
            NorseHelm helm = new NorseHelm();
            helm.Name = "Guardian's Helm";
            helm.Hue = Utility.RandomMinMax(500, 950);
            helm.BaseArmorRating = Utility.RandomMinMax(45, 75);
            helm.ArmorAttributes.LowerStatReq = 20;
            helm.Attributes.BonusStr = 20;
            helm.Attributes.DefendChance = 20;
            helm.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            helm.PhysicalBonus = 30;
            helm.EnergyBonus = 10;
            helm.FireBonus = 10;
            helm.ColdBonus = 5;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Destructo Disc Dagger";
            dagger.Hue = Utility.RandomMinMax(500, 550);
            dagger.MinDamage = Utility.RandomMinMax(10, 50);
            dagger.MaxDamage = Utility.RandomMinMax(50, 90);
            dagger.Attributes.AttackChance = 15;
            dagger.Attributes.SpellDamage = 10;
            dagger.Slayer = SlayerName.ElementalBan;
            dagger.WeaponAttributes.HitLightning = 20;
            dagger.WeaponAttributes.MageWeapon = 1;
            dagger.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            return dagger;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "No disintegrations.";
            note.TitleString = "Bounty Hunter's Log";
            return note;
        }

        public BountyHuntersCache(Serial serial) : base(serial)
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
