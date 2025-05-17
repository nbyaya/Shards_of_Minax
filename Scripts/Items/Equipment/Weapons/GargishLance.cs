using System;

namespace Server.Items
{
    //Based off Lance
    [FlipableAttribute(0x48CA, 0x48CB)]
    public class GargishLance : BaseSword
    {
        [Constructable]
        public GargishLance()
            : base(0x48CA)
        {
            this.Weight = 12.0;

            // Apply random tier damage
            ApplyRandomTier();
        }

        public GargishLance(Serial serial)
            : base(serial)
        {
        }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            if (chanceForSpecialTier < 0.5)
            {
                // Default stats, no modification
                return;
            }

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

        public override WeaponAbility PrimaryAbility => WeaponAbility.Dismount;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ConcussionBlow;

        public override int AosStrengthReq => 95;
        public override int AosMinDamage => 18;
        public override int AosMaxDamage => 22;
        public override int AosSpeed => 24;
        public override float MlSpeed => 4.25f;

        public override int OldStrengthReq => 95;
        public override int OldMinDamage => 17;
        public override int OldMaxDamage => 18;
        public override int OldSpeed => 24;

        public override int DefHitSound => 0x23C;
        public override int DefMissSound => 0x238;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 110;

        public override SkillName DefSkill => SkillName.Fencing;
        public override WeaponType DefType => WeaponType.Piercing;
        public override WeaponAnimation DefAnimation => WeaponAnimation.Pierce1H;

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
