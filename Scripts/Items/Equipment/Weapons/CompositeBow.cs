using System;

namespace Server.Items
{
    [FlipableAttribute(0x26C2, 0x26CC)]
    public class CompositeBow : BaseRanged
    {
        [Constructable]
        public CompositeBow()
            : base(0x26C2)
        {
            this.Weight = 5.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public CompositeBow(Serial serial)
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

        public override int EffectID => 0xF42;
        public override Type AmmoType => typeof(Arrow);
        public override Item Ammo => new Arrow();

        public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
        public override WeaponAbility SecondaryAbility => WeaponAbility.MovingShot;

        public override int AosStrengthReq => 45;
        public override int AosMinDamage => Core.ML ? 16 : 15;
        public override int AosMaxDamage => 20;
        public override int AosSpeed => 25;
        public override float MlSpeed => 4.00f;

        public override int OldStrengthReq => 45;
        public override int OldMinDamage => 15;
        public override int OldMaxDamage => 17;
        public override int OldSpeed => 25;

        public override int DefMaxRange => 10;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 70;

        public override WeaponAnimation DefAnimation => WeaponAnimation.ShootBow;

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
