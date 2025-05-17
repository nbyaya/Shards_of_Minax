#region References
using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
#endregion

namespace Server.Items
{    
    [FlipableAttribute(0xA22, 0xA22)]
    public class CampingLanturn : BaseMagicRanged
    {
        [Constructable]
        public CampingLanturn() : base(0xA22)
        {
            this.Weight = 9.0;
            this.Layer = Layer.TwoHanded;
            this.Name = "Camping Lanturn";
            this.Hue = 674;

            // Apply random tier damage here
            ApplyRandomTier();
        }
        
        public CampingLanturn(Serial serial) : base(serial)
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
                this.MinDamage = rand.Next(10, 100);
                this.MaxDamage = rand.Next(100, 150);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(10, 80);
                this.MaxDamage = rand.Next(80, 120);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(10, 60);
                this.MaxDamage = rand.Next(60, 90);
            }
            else
            {
                this.MinDamage = rand.Next(10, 40);
                this.MaxDamage = rand.Next(40, 70);
            }
        }

        public override int EffectID => 0x36D4;

        public override Type ReagentAmmoType => typeof(Kindling);
        public override Item ReagentAmmo => new Kindling();

        public override int FireDamage => 20;

        public override WeaponAbility PrimaryAbility => WeaponAbility.MovingShot;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

        public override int AosStrengthReq => 80;
        public override int AosMinDamage => 20;
        public override int AosMaxDamage => 24;
        public override int AosSpeed => 22;
        public override float MlSpeed => 5.00f;

        public override int OldStrengthReq => 40;
        public override int OldMinDamage => 11;
        public override int OldMaxDamage => 56;
        public override int OldSpeed => 10;
        public override int DefMaxRange => 8;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 100;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Camping");
            list.Add("Ammo: Kindling");
        }

        public override SkillName DefSkill => SkillName.Camping;

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
