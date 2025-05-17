using System;

namespace Server.Items
{
    [FlipableAttribute(0x908, 0x4075)]
    public class GargishTalwar : BaseSword
    {
        [Constructable]
        public GargishTalwar()
            : base(0x908)
        {
            // Weight = 16.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public GargishTalwar(Serial serial)
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
                this.MinDamage = rand.Next(1, 90);
                this.MaxDamage = rand.Next(90, 130);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 75);
                this.MaxDamage = rand.Next(75, 105);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 55);
                this.MaxDamage = rand.Next(55, 80);
            }
            else
            {
                this.MinDamage = rand.Next(1, 35);
                this.MaxDamage = rand.Next(35, 60);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

        public override int AosStrengthReq => 40;
        public override int AosMinDamage => 16;
        public override int AosMaxDamage => 19;
        public override int AosSpeed => 25;
        public override float MlSpeed => 3.50f;

        public override int OldStrengthReq => 45;
        public override int OldMinDamage => 5;
        public override int OldMaxDamage => 49;
        public override int OldSpeed => 25;

        public override int DefHitSound => 0x237;
        public override int DefMissSound => 0x238;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 80;

        public override Race RequiredRace => Race.Gargoyle;
        public override bool CanBeWornByGargoyles => true;

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
