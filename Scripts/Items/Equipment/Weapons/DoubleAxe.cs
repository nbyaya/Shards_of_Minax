using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(DualShortAxes))]
    [FlipableAttribute(0xf4b, 0xf4c)]
    public class DoubleAxe : BaseAxe
    {
        [Constructable]
        public DoubleAxe()
            : base(0xF4B)
        {
            this.Weight = 8.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public DoubleAxe(Serial serial)
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
                this.MaxDamage = rand.Next(75, 110);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 55);
                this.MaxDamage = rand.Next(55, 85);
            }
            else
            {
                this.MinDamage = rand.Next(1, 35);
                this.MaxDamage = rand.Next(35, 60);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.DoubleStrike;
        public override WeaponAbility SecondaryAbility => WeaponAbility.WhirlwindAttack;

        public override int AosStrengthReq => 45;
        public override int AosMinDamage => 15;
        public override int AosMaxDamage => 18;
        public override int AosSpeed => 33;
        public override float MlSpeed => 3.25f;

        public override int OldStrengthReq => 45;
        public override int OldMinDamage => 5;
        public override int OldMaxDamage => 35;
        public override int OldSpeed => 37;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 110;

        public override SkillName DefSkill => SkillName.Lumberjacking;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Lumberjacking");
        }

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
