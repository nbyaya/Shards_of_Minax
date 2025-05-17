using System;

namespace Server.Items
{
    [FlipableAttribute(0x0DC0, 0x0DC0)]
    public class BoltRod : BaseRanged
    {
        [Constructable]
        public BoltRod()
            : base(0x0DC0)
        {
            this.Weight = 9.0;
            this.Layer = Layer.TwoHanded;
            this.Name = "Bolt Rod";

            // Apply random tier damage
            ApplyRandomTier();
        }

        public BoltRod(Serial serial)
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
                this.MinDamage = rand.Next(1, 100);
                this.MaxDamage = rand.Next(100, 140);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 90);
                this.MaxDamage = rand.Next(90, 120);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 70);
                this.MaxDamage = rand.Next(70, 100);
            }
            else
            {
                this.MinDamage = rand.Next(1, 50);
                this.MaxDamage = rand.Next(50, 80);
            }
        }

        public override int EffectID => 0x1BFE;
        public override Type AmmoType => typeof(Bolt);
        public override Item Ammo => new Bolt();

        public override WeaponAbility PrimaryAbility => WeaponAbility.MovingShot;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

        public override int AosStrengthReq => 80;
        public override int AosMinDamage => Core.ML ? 20 : 20;
        public override int AosMaxDamage => Core.ML ? 24 : 24;
        public override int AosSpeed => 22;
        public override float MlSpeed => 5.00f;

        public override int OldStrengthReq => 40;
        public override int OldMinDamage => 11;
        public override int OldMaxDamage => 56;
        public override int OldSpeed => 10;

        public override int DefMaxRange => 8;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 100;

        public override SkillName DefSkill => SkillName.Fishing;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Fishing");
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
