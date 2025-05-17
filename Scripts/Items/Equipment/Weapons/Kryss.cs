using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(GargishKryss))]
    [FlipableAttribute(0x1401, 0x1400)]
    public class Kryss : BaseSword
    {
        [Constructable]
        public Kryss()
            : base(0x1401)
        {
            Weight = 2.0;

            // Apply random tier damage
            ApplyRandomTier();
        }

        public Kryss(Serial serial)
            : base(serial)
        {
        }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            // 50% chance for default stats, 50% chance for a special tier
            if (chanceForSpecialTier < 0.5)
                return;

            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                this.MinDamage = rand.Next(1, 60);
                this.MaxDamage = rand.Next(60, 100);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 50);
                this.MaxDamage = rand.Next(50, 80);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 35);
                this.MaxDamage = rand.Next(35, 60);
            }
            else
            {
                this.MinDamage = rand.Next(1, 20);
                this.MaxDamage = rand.Next(20, 40);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
        public override WeaponAbility SecondaryAbility => WeaponAbility.InfectiousStrike;

        public override int AosStrengthReq => 10;
        public override int AosMinDamage => 10;
        public override int AosMaxDamage => 12;
        public override int AosSpeed => 53;
        public override float MlSpeed => 2.00f;

        public override int OldStrengthReq => 10;
        public override int OldMinDamage => 3;
        public override int OldMaxDamage => 28;
        public override int OldSpeed => 53;

        public override int DefHitSound => 0x23C;
        public override int DefMissSound => 0x238;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 90;

        public override SkillName DefSkill => SkillName.Fencing;
        public override WeaponType DefType => WeaponType.Piercing;
        public override WeaponAnimation DefAnimation => WeaponAnimation.Pierce1H;

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
