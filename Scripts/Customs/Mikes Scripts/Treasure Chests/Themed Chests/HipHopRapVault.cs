using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class HipHopRapVault : WoodenChest
    {
        [Constructable]
        public HipHopRapVault()
        {
            Name = "Hip-Hop & Rap Vault";
            Hue = Utility.Random(1, 2600);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Emerald>("Ice Cube"), 0.22);
            AddItem(CreateNamedItem<GreaterHealPotion>("Biggie's Brew"), 0.29);
            AddItem(CreateNamedItem<TreasureLevel1>("MC's Microphone"), 0.23);
            AddItem(CreateNamedItem<GoldEarrings>("Tupac Hoop"), 0.37);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 3200)), 0.13);
            AddItem(CreateLoot<Lute>("DJ Turntable"), 0.16);
            AddItem(CreateJWeapon(), 0.2);
            AddItem(CreateFancyDress(), 0.2);
            AddItem(CreateHeaterShield(), 0.2);
            AddItem(CreateCutlass(), 0.2);
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

        private Item CreateLoot<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "East Coast vs. West Coast";
            note.TitleString = "Rapper's Rhyme Book";
            return note;
        }

        private Item CreateJWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Breakdance Sword";
            return weapon;
        }

        private Item CreateFancyDress()
        {
            FancyDress dress = new FancyDress();
            dress.Name = "Boho Chic Sundress";
            dress.Hue = Utility.RandomMinMax(600, 1600);
            dress.Attributes.RegenMana = 2;
            dress.Attributes.CastRecovery = 1;
            dress.SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);
            return dress;
        }

        private Item CreateHeaterShield()
        {
            HeaterShield shield = new HeaterShield();
            shield.Name = "Celes' Runeblade Buckler";
            shield.Hue = Utility.RandomMinMax(300, 800);
            shield.BaseArmorRating = Utility.Random(35, 70);
            shield.Attributes.ReflectPhysical = 5;
            shield.Attributes.BonusInt = 15;
            shield.SkillBonuses.SetValues(0, SkillName.MagicResist, 25.0);
            shield.ColdBonus = 10;
            shield.EnergyBonus = 15;
            shield.FireBonus = 5;
            shield.PhysicalBonus = 15;
            shield.PoisonBonus = 10;
            return shield;
        }

        private Item CreateCutlass()
        {
            Cutlass cutlass = new Cutlass();
            cutlass.Name = "Makhaira of Achilles";
            cutlass.Hue = Utility.RandomMinMax(400, 600);
            cutlass.MinDamage = Utility.Random(30, 80);
            cutlass.MaxDamage = Utility.Random(80, 110);
            cutlass.Attributes.BonusHits = 20;
            cutlass.Attributes.BonusDex = 10;
            cutlass.Slayer = SlayerName.DragonSlaying;
            cutlass.WeaponAttributes.BloodDrinker = 25;
            cutlass.WeaponAttributes.HitPhysicalArea = 20;
            cutlass.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return cutlass;
        }

        public HipHopRapVault(Serial serial) : base(serial)
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
