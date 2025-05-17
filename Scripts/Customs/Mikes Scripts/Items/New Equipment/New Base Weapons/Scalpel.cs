using System;

namespace Server.Items
{
    [FlipableAttribute(0xEC4, 0xEC5)]
    public class Scalpel : BaseKnife
    {
        [Constructable]
        public Scalpel()
            : base(0xEC4)
        {
            this.Weight = 1.0;
            this.Name = "Scalpel";

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public Scalpel(Serial serial)
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

            // Determine special tier
            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                this.MinDamage = rand.Next(1, 50);
                this.MaxDamage = rand.Next(50, 80);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 40);
                this.MaxDamage = rand.Next(40, 65);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 25);
                this.MaxDamage = rand.Next(25, 45);
            }
            else
            {
                this.MinDamage = rand.Next(1, 15);
                this.MaxDamage = rand.Next(15, 30);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.ShadowStrike;
        public override WeaponAbility SecondaryAbility => WeaponAbility.BleedAttack;

        public override int AosStrengthReq => 5;
        public override int AosMinDamage => 10;
        public override int AosMaxDamage => 13;
        public override int AosSpeed => 49;
        public override float MlSpeed => 2.25f;

        public override int OldStrengthReq => 5;
        public override int OldMinDamage => 1;
        public override int OldMaxDamage => 10;
        public override int OldSpeed => 40;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 40;

        public override SkillName DefSkill => SkillName.Healing;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Healing");
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
