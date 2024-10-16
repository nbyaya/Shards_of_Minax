using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class InnovatorVault : WoodenChest
    {
        [Constructable]
        public InnovatorVault()
        {
            Name = "Innovator's Vault";
            Hue = Utility.Random(1, 1941);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.08);
            AddItem(CreateGoldVoucher(), 0.30);
            AddItem(CreateNamedItem<TreasureLevel4>("Blueprints of Inventions"), 0.22);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4200)), 0.14);
            AddItem(CreateLoot<Diamond>("Manhattan Project Crystal"), 0.16);
            AddItem(CreateLoot<Lute>("1940s Radio Set"), 0.19);
            AddItem(CreateGloves(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreateKatana(), 0.20);
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
            voucher.Name = "Patent Certificate";
            return voucher;
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
            note.NoteString = "A leap into the atomic age.";
            note.TitleString = "Scientist's Notes";
            return note;
        }

        private Item CreateLoot<T>(string name) where T : Item, new()
        {
            T loot = new T();
            loot.Name = name;
            return loot;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Pickpocket's Nimble Gloves";
            gloves.Hue = Utility.RandomMinMax(800, 1700);
            gloves.Attributes.BonusDex = 15;
            gloves.Attributes.AttackChance = 5;
            gloves.SkillBonuses.SetValues(0, SkillName.Stealing, 25.0);
            gloves.SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
            gloves.PhysicalBonus = 10;
            gloves.ColdBonus = 10;
            return gloves;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "BlackMage's Mystic Robe";
            chest.Hue = Utility.RandomMinMax(100, 600);
            chest.BaseArmorRating = Utility.RandomMinMax(25, 60);
            chest.AbsorptionAttributes.EaterFire = 15;
            chest.ArmorAttributes.MageArmor = 1;
            chest.Attributes.BonusInt = 30;
            chest.Attributes.SpellDamage = 20;
            chest.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 20;
            chest.FireBonus = 20;
            chest.PhysicalBonus = 5;
            chest.PoisonBonus = 5;
            return chest;
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Muramasa's Bloodlust";
            katana.Hue = Utility.RandomMinMax(500, 550);
            katana.MinDamage = Utility.RandomMinMax(30, 75);
            katana.MaxDamage = Utility.RandomMinMax(75, 110);
            katana.Attributes.AttackChance = 10;
            katana.Attributes.BonusStr = 10;
            katana.Slayer = SlayerName.BloodDrinking;
            katana.WeaponAttributes.BloodDrinker = 20;
            katana.WeaponAttributes.HitLeechHits = 15;
            katana.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return katana;
        }

        public InnovatorVault(Serial serial) : base(serial)
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
