using System;

namespace Server.Items
{
    [FlipableAttribute(0x27AE, 0x27F9)]
    public class Nunchaku : BaseBashing
    {
        [Constructable]
        public Nunchaku()
            : base(0x27AE)
        {
            this.Weight = 5.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public Nunchaku(Serial serial)
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

            // Determine which special tier it falls into
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

        public override WeaponAbility PrimaryAbility => WeaponAbility.Block;
        public override WeaponAbility SecondaryAbility => WeaponAbility.DoubleStrike;

        public override int AosStrengthReq => 15;
        public override int AosMinDamage => 12;
        public override int AosMaxDamage => 15;
        public override int AosSpeed => 47;
        public override float MlSpeed => 2.50f;

        public override int OldStrengthReq => 15;
        public override int OldMinDamage => 11;
        public override int OldMaxDamage => 13;
        public override int OldSpeed => 47;

        public override int DefHitSound => 0x535;
        public override int DefMissSound => 0x239;

        public override int InitMinHits => 40;
        public override int InitMaxHits => 55;

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
