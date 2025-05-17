using System;

namespace Server.Items
{
    [FlipableAttribute(0x2D2B, 0x2D1F)]
    public class MagicalShortbow : BaseRanged
    {
        [Constructable]
        public MagicalShortbow()
            : base(0x2D2B)
        {
            this.Weight = 6.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public MagicalShortbow(Serial serial)
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

        public override int EffectID => 0xF42;
        public override Type AmmoType => typeof(Arrow);
        public override Item Ammo => new Arrow();

        public override WeaponAbility PrimaryAbility => WeaponAbility.LightningArrow;
        public override WeaponAbility SecondaryAbility => WeaponAbility.PsychicAttack;

        public override int AosStrengthReq => 45;
        public override int AosMinDamage => 12;
        public override int AosMaxDamage => 16;
        public override int AosSpeed => 38;
        public override float MlSpeed => 3.00f;

        public override int OldStrengthReq => 45;
        public override int OldMinDamage => 9;
        public override int OldMaxDamage => 13;
        public override int OldSpeed => 38;

        public override int DefMaxRange => 10;
        public override int InitMinHits => 41;
        public override int InitMaxHits => 90;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
