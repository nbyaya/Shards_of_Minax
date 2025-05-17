using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(GargishAxe))]
    [FlipableAttribute(0xF49, 0xF4a)]
    public class Axe : BaseAxe
    {
        [Constructable]
        public Axe()
            : base(0xF49)
        {
            this.Weight = 4.0;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public Axe(Serial serial)
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


        public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

        public override int AosStrengthReq => 35;
        public override int AosMinDamage => 14;
        public override int AosMaxDamage => 17;
        public override int AosSpeed => 37;
        public override float MlSpeed => 3.00f;

        public override int OldStrengthReq => 35;
        public override int OldMinDamage => 6;
        public override int OldMaxDamage => 33;
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

