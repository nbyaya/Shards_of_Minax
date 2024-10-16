using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class UndergroundAnarchistsCache : WoodenChest
    {
        [Constructable]
        public UndergroundAnarchistsCache()
        {
            Name = "Underground Anarchist's Cache";
            Hue = 1159;

            // Add items to the chest
            AddItem(CreateEmerald(), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel4>("Underground's Gold"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Chain Link Earring", 1174), 0.15);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.18);
            AddItem(CreateNamedItem<Apple>("Graffiti Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Street Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Squatter", 1160), 0.15);
            AddItem(CreateRandomInstrument(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("DIY Vision"), 0.12);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateSash(), 0.20);
            AddItem(CreateRingmailGloves(), 0.20);
            AddItem(CreateDagger(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Revolution Stone";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "DIY or die!";
            note.TitleString = "Anarchist's Memo";
            return note;
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

        private Item CreateRandomInstrument()
        {
            BaseInstrument instrument = Utility.RandomList<BaseInstrument>(new Lute(), new Tambourine(), new Drums(), new Harp());
            instrument.Name = "Rusty Harmonica";
            return instrument;
        }

        private Item CreateRandomClothing()
        {
            BaseClothing clothing = Utility.RandomList<BaseClothing>(new Robe(), new Robe(), new Kilt(), new Shirt());
            clothing.Name = "Patchwork Vest";
            return clothing;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger(), new Longsword(), new Axe());
            weapon.Name = "Street Warrior's Blade";
            weapon.Hue = Utility.RandomList(1, 1159);
            weapon.MaxDamage = Utility.Random(25, 65);
            return weapon;
        }

        private Item CreateSash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Warrior's Belt";
            sash.Hue = Utility.RandomMinMax(500, 1500);
            sash.Attributes.BonusHits = 15;
            sash.SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
            return sash;
        }

        private Item CreateRingmailGloves()
        {
            RingmailGloves gloves = new RingmailGloves();
            gloves.Name = "Slithering Seal";
            gloves.Hue = Utility.RandomMinMax(100, 300);
            gloves.BaseArmorRating = Utility.Random(30, 65);
            gloves.AbsorptionAttributes.ResonancePoison = 10;
            gloves.ArmorAttributes.SelfRepair = 5;
            gloves.Attributes.BonusMana = 10;
            gloves.Attributes.CastRecovery = 1;
            gloves.SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 10;
            gloves.FireBonus = 5;
            gloves.PhysicalBonus = 15;
            gloves.PoisonBonus = 20;
            return gloves;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Illumina Dagger";
            dagger.Hue = Utility.RandomMinMax(500, 700);
            dagger.MinDamage = Utility.Random(20, 60);
            dagger.MaxDamage = Utility.Random(60, 90);
            dagger.Attributes.SpellDamage = 10;
            dagger.Attributes.Luck = 150;
            dagger.Slayer = SlayerName.DaemonDismissal;
            dagger.WeaponAttributes.HitLightning = 30;
            dagger.WeaponAttributes.HitFireball = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            return dagger;
        }

        public UndergroundAnarchistsCache(Serial serial) : base(serial)
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
