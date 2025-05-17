using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(DreadSword))]
    [FlipableAttribute(0xF5E, 0xF5F)]
    public class Broadsword : BaseSword
    {
        [Constructable]
        public Broadsword()
            : base(0xF5E)
        {
            this.Weight = 6.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public Broadsword(Serial serial)
            : base(serial)
        {
        }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            // 50% chance for default stats, 50% chance for a special tier
            if (chanceForSpecialTier < 0.5)
            {
                // Default stats, don't modify anything
                return;
            }

            // Now determine which special tier it falls into
            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                this.MinDamage = rand.Next(1, 80);
                this.MaxDamage = rand.Next(80, 120);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 70);
                this.MaxDamage = rand.Next(70, 100);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 50);
                this.MaxDamage = rand.Next(50, 75);
            }
            else
            {
                this.MinDamage = rand.Next(1, 30);
                this.MaxDamage = rand.Next(30, 50);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ArmorIgnore;

        public override int AosStrengthReq => 30;
        public override int AosMinDamage => 13;
        public override int AosMaxDamage => 17;
        public override int AosSpeed => 33;
        public override float MlSpeed => 3.25f;

        public override int OldStrengthReq => 25;
        public override int OldMinDamage => 5;
        public override int OldMaxDamage => 29;
        public override int OldSpeed => 45;

        public override int DefHitSound => 0x237;
        public override int DefMissSound => 0x23A;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 100;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
