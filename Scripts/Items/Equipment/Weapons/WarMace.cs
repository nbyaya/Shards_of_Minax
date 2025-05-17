using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(DiscMace))]
    [FlipableAttribute(0x1407, 0x1406)]
    public class WarMace : BaseBashing
    {
        [Constructable]
        public WarMace()
            : base(0x1407)
        {
            this.Weight = 17.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public WarMace(Serial serial)
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
                this.MaxDamage = rand.Next(100, 140);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 80);
                this.MaxDamage = rand.Next(80, 110);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 60);
                this.MaxDamage = rand.Next(60, 85);
            }
            else
            {
                this.MinDamage = rand.Next(1, 40);
                this.MaxDamage = rand.Next(40, 60);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
        public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

        public override int AosStrengthReq => 80;
        public override int AosMinDamage => 16;
        public override int AosMaxDamage => 20;
        public override int AosSpeed => 26;
        public override float MlSpeed => 4.00f;

        public override int OldStrengthReq => 30;
        public override int OldMinDamage => 10;
        public override int OldMaxDamage => 30;
        public override int OldSpeed => 32;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 110;

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
