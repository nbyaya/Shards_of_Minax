using System;

namespace Server.Items
{
    //Based Off War Hammer
    [FlipableAttribute(0x48C0, 0x481)]
    public class GargishWarHammer : BaseBashing
    {
        [Constructable]
        public GargishWarHammer()
            : base(0x48C0)
        {
            this.Weight = 10.0;
            this.Layer = Layer.TwoHanded;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public GargishWarHammer(Serial serial)
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
                this.MaxDamage = rand.Next(55, 80);
            }
            else
            {
                this.MinDamage = rand.Next(1, 35);
                this.MaxDamage = rand.Next(35, 60);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
        public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

        public override int AosStrengthReq => 95;
        public override int AosMinDamage => 17;
        public override int AosMaxDamage => 20;
        public override int AosSpeed => 28;
        public override float MlSpeed => 3.75f;

        public override int OldStrengthReq => 40;
        public override int OldMinDamage => 8;
        public override int OldMaxDamage => 36;
        public override int OldSpeed => 31;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 110;

        public override WeaponAnimation DefAnimation => WeaponAnimation.Bash2H;
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
