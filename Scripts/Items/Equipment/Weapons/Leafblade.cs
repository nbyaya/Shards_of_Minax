using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(BloodBlade))]
    [FlipableAttribute(0x2D22, 0x2D2E)]
    public class Leafblade : BaseKnife
    {
        [Constructable]
        public Leafblade()
            : base(0x2D22)
        {
            this.Weight = 8.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public Leafblade(Serial serial)
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
                this.MinDamage = rand.Next(1, 70);
                this.MaxDamage = rand.Next(70, 100);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 60);
                this.MaxDamage = rand.Next(60, 90);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 40);
                this.MaxDamage = rand.Next(40, 60);
            }
            else
            {
                this.MinDamage = rand.Next(1, 25);
                this.MaxDamage = rand.Next(25, 40);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.Feint;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ArmorIgnore;

        public override int AosStrengthReq => 20;
        public override int AosMinDamage => 11;
        public override int AosMaxDamage => 15;
        public override int AosSpeed => 42;
        public override float MlSpeed => 2.75f;

        public override int OldStrengthReq => 20;
        public override int OldMinDamage => 13;
        public override int OldMaxDamage => 15;
        public override int OldSpeed => 42;

        public override int DefMissSound => 0x239;

        public override SkillName DefSkill => SkillName.Fencing;

        public override int InitMinHits => 30;
        public override int InitMaxHits => 60;

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
