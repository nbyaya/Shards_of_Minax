#region References
using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
#endregion

namespace Server.Items
{    
    [FlipableAttribute(0xF6B, 0xF6B)]
    public class WitchBurningTorch : BaseMagicRanged
    {
        private int _tierMinDamage;
        private int _tierMaxDamage;

        [Constructable]
        public WitchBurningTorch() : base(0xF6B)
        {
            this.Weight = 9.0;
            this.Layer = Layer.TwoHanded;
			this.Name = "Witch Burning Torch";
			this.Hue = 674;

            ApplyRandomTier();
        }
        
        public WitchBurningTorch(Serial serial) : base(serial)
        {
        }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            // 50% chance for default stats, 50% chance for a special tier
            if (chanceForSpecialTier < 0.5)
            {
                _tierMinDamage = 20; // Default AosMinDamage
                _tierMaxDamage = 24; // Default AosMaxDamage
                return;
            }

            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                _tierMinDamage = rand.Next(10, 80);
                _tierMaxDamage = rand.Next(80, 120);
            }
            else if (tierChance < 0.2)
            {
                _tierMinDamage = rand.Next(10, 70);
                _tierMaxDamage = rand.Next(70, 100);
            }
            else if (tierChance < 0.5)
            {
                _tierMinDamage = rand.Next(10, 50);
                _tierMaxDamage = rand.Next(50, 75);
            }
            else
            {
                _tierMinDamage = rand.Next(10, 30);
                _tierMaxDamage = rand.Next(30, 50);
            }
        }

        public override int AosMinDamage => _tierMinDamage;
        public override int AosMaxDamage => _tierMaxDamage;

        public override int EffectID { get { return 0x36D4; } }

        public override Type ReagentAmmoType { get { return typeof(Log); } }
        public override Item ReagentAmmo { get { return new Log(); } }

        public override int FireDamage { get { return 20; } }

        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.MovingShot; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }
        public override int AosStrengthReq { get { return 80; } }
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
            list.Add("Skill Required: MagicResist");
			list.Add("Ammo: Log");
        }	

		public override SkillName DefSkill => SkillName.MagicResist;

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
