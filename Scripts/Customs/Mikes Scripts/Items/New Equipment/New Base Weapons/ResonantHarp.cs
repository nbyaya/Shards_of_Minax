#region References
using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
#endregion

namespace Server.Items
{
    [FlipableAttribute(0x13B2, 0x13B1)]
    public class ResonantHarp : BaseMagicRanged
    {
        [Constructable]
        public ResonantHarp() : base(0x13B2)
        {
            this.Weight = 9.0;
            this.Layer = Layer.TwoHanded;
            this.Name = "Resonant Harp";
            this.Hue = 1083;

            // Apply random tier damage here
            ApplyRandomTier();
        }

        public ResonantHarp(Serial serial) : base(serial)
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
                this.MinDamage = rand.Next(10, 80);
                this.MaxDamage = rand.Next(80, 120);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(10, 70);
                this.MaxDamage = rand.Next(70, 100);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(10, 50);
                this.MaxDamage = rand.Next(50, 75);
            }
            else
            {
                this.MinDamage = rand.Next(10, 30);
                this.MaxDamage = rand.Next(30, 50);
            }
        }

        public override int EffectID { get { return 0x379F; } }

        public override Type ReagentAmmoType { get { return typeof(SpidersSilk); } }
        public override Item ReagentAmmo { get { return new SpidersSilk(); } }

        public override int FireDamage { get { return 20; } }

        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.MovingShot; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }
        public override int AosStrengthReq { get { return 80; } }
        public override int AosMinDamage { get { return 20; } }
        public override int AosMaxDamage { get { return 24; } }
        public override int AosSpeed { get { return 22; } }
        public override float MlSpeed { get { return 5.00f; } }

        public override int OldStrengthReq { get { return 40; } }
        public override int OldMinDamage { get { return 11; } }
        public override int OldMaxDamage { get { return 56; } }
        public override int OldSpeed { get { return 10; } }

        public override int DefMaxRange { get { return 8; } }
        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 100; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Musicianship");
            list.Add("Ammo: Spiders Silk");
        }

        public override SkillName DefSkill
        {
            get
            {
                return SkillName.Musicianship;
            }
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
