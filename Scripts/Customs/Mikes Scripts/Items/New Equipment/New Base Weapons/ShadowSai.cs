using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(DualPointedSpear))]
    [FlipableAttribute(0x27AF, 0x27FA)]
    public class ShadowSai : BaseKnife
    {
        private int _minDamage;
        private int _maxDamage;

        [Constructable]
        public ShadowSai()
            : base(0x27AF)
        {
            this.Weight = 7.0;
            this.Layer = Layer.TwoHanded;
            this.Name = "Shadow Sai";

            ApplyRandomTier();
        }

        public ShadowSai(Serial serial)
            : base(serial)
        {
        }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            if (chanceForSpecialTier < 0.5)
            {
                // Default stats
                _minDamage = 10;
                _maxDamage = 13;
                return;
            }

            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                _minDamage = rand.Next(1, 60);
                _maxDamage = rand.Next(60, 90);
            }
            else if (tierChance < 0.2)
            {
                _minDamage = rand.Next(1, 50);
                _maxDamage = rand.Next(50, 75);
            }
            else if (tierChance < 0.5)
            {
                _minDamage = rand.Next(1, 35);
                _maxDamage = rand.Next(35, 55);
            }
            else
            {
                _minDamage = rand.Next(1, 20);
                _maxDamage = rand.Next(20, 35);
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.DualWield;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ArmorPierce;

        public override int AosStrengthReq => 15;
        public override int AosMinDamage => _minDamage > 0 ? _minDamage : 10;
        public override int AosMaxDamage => _maxDamage > 0 ? _maxDamage : 13;
        public override int AosSpeed => 55;
        public override float MlSpeed => 2.00f;

        public override int OldStrengthReq => 15;
        public override int OldMinDamage => 9;
        public override int OldMaxDamage => 11;
        public override int OldSpeed => 55;

        public override int DefHitSound => 0x23C;
        public override int DefMissSound => 0x232;

        public override int InitMinHits => 55;
        public override int InitMaxHits => 60;

        public override SkillName DefSkill => SkillName.Hiding;

        public override WeaponType DefType => WeaponType.Piercing;
        public override WeaponAnimation DefAnimation => WeaponAnimation.Pierce1H;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Hiding");
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
