using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(GargishTalwar))]
    [FlipableAttribute(0x143E, 0x143F)]
    public class Halberd : BasePoleArm
    {
        [Constructable]
        public Halberd()
            : base(0x143E)
        {
            this.Weight = 16.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public Halberd(Serial serial)
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
                this.MinDamage = rand.Next(1, 100);
                this.MaxDamage = rand.Next(100, 150);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 85);
                this.MaxDamage = rand.Next(85, 120);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 60);
                this.MaxDamage = rand.Next(60, 90);
            }
            else
            {
                this.MinDamage = rand.Next(1, 40);
                this.MaxDamage = rand.Next(40, 60);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ConcussionBlow;

        public override int AosStrengthReq => 95;
        public override int AosMinDamage => 18;
        public override int AosMaxDamage => 21;
        public override int AosSpeed => 25;
        public override float MlSpeed => 4.00f;

        public override int OldStrengthReq => 45;
        public override int OldMinDamage => 5;
        public override int OldMaxDamage => 49;
        public override int OldSpeed => 25;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 80;

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
