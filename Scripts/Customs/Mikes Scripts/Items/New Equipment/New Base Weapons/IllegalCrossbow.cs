using System;

namespace Server.Items
{
    [FlipableAttribute(0xF50, 0xF4F)]
    public class IllegalCrossbow : BaseRanged
    {
        [Constructable]
        public IllegalCrossbow()
            : base(0xF50)
        {
            this.Weight = 7.0;
            this.Layer = Layer.TwoHanded;
            this.Name = "Illegal Crossbow";

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public IllegalCrossbow(Serial serial)
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

        public override int EffectID => 0x1BFE;
        public override Type AmmoType => typeof(Bolt);
        public override Item Ammo => new Bolt();

        public override WeaponAbility PrimaryAbility => WeaponAbility.ConcussionBlow;
        public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

        public override int AosStrengthReq => 35;
        public override int AosMinDamage => 18;
        public override int AosMaxDamage => Core.ML ? 22 : 22;
        public override int AosSpeed => 24;
        public override float MlSpeed => 4.50f;

        public override int OldStrengthReq => 30;
        public override int OldMinDamage => 8;
        public override int OldMaxDamage => 43;
        public override int OldSpeed => 18;

        public override int DefMaxRange => 8;
        public override int InitMinHits => 31;
        public override int InitMaxHits => 80;

        public override SkillName DefSkill => SkillName.Stealing;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Stealing");
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
