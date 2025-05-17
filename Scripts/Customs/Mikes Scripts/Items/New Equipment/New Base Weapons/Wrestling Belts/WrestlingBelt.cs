using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x2790, 0x27DB)] // Same visuals as LeatherNinjaBelt
    public class WrestlingBelt : BaseWrestlingBelt
    {
        [Constructable]
        public WrestlingBelt() : base(0x2790)
        {
            Weight = 1.0;
            Name = "Wrestler's Belt";

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public WrestlingBelt(Serial serial) : base(serial) { }

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
                this.MinDamage = rand.Next(1, 50);  // Wrestling damage tiers scaled down
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
                this.MaxDamage = rand.Next(25, 40);
            }
            else
            {
                this.MinDamage = rand.Next(1, 15);
                this.MaxDamage = rand.Next(15, 25);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.Disarm;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

        public override int AosStrengthReq => 0;
        public override int AosMinDamage => 5;
        public override int AosMaxDamage => 10;
        public override int AosSpeed => 50;

        public override float MlSpeed => 2.50f;

        public override int OldStrengthReq => 0;
        public override int OldMinDamage => 1;
        public override int OldMaxDamage => 6;
        public override int OldSpeed => 50;

        public override int InitMinHits => 30;
        public override int InitMaxHits => 40;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Wrestling");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
