using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(GourmandsFork))]
    [FlipableAttribute(0x1405, 0x1404)]
    public class GourmandsFork : BaseSpear
    {
        [Constructable]
        public GourmandsFork()
            : base(0x1405)
        {
            this.Weight = 9.0;
            this.Name = "Gourmands Fork";

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public GourmandsFork(Serial serial)
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

        public override WeaponAbility PrimaryAbility => WeaponAbility.BleedAttack;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Disarm;

        public override int AosStrengthReq => 45;
        public override int AosMinDamage => 10;
        public override int AosMaxDamage => 14;
        public override int AosSpeed => 43;
        public override float MlSpeed => 2.50f;

        public override int OldStrengthReq => 35;
        public override int OldMinDamage => 4;
        public override int OldMaxDamage => 32;
        public override int OldSpeed => 45;

        public override int DefHitSound => 0x236;
        public override int DefMissSound => 0x238;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 110;

        public override WeaponAnimation DefAnimation => WeaponAnimation.Pierce1H;

        public override SkillName DefSkill => SkillName.TasteID;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: TasteID");
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
