#region References
using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
#endregion

namespace Server.Items
{
    [FlipableAttribute(0x2D25, 0x2D31)]
    public class MysticStaff : BaseMagicRanged
    {
        private int _tierMinDamage;
        private int _tierMaxDamage;

        [Constructable]
        public MysticStaff() : base(0x2D25)
        {
            this.Weight = 9.0;
            this.Layer = Layer.TwoHanded;
            this.Name = "Mystic Staff";
            this.Hue = 2533;

            ApplyRandomTier();
        }

        public MysticStaff(Serial serial) : base(serial)
        {
        }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            // 50% chance for default stats, 50% chance for a special tier
            if (chanceForSpecialTier < 0.5)
            {
                _tierMinDamage = 20; // default AosMinDamage
                _tierMaxDamage = 24; // default AosMaxDamage
                return;
            }

            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                _tierMinDamage = rand.Next(1, 80);
                _tierMaxDamage = rand.Next(80, 120);
            }
            else if (tierChance < 0.2)
            {
                _tierMinDamage = rand.Next(1, 70);
                _tierMaxDamage = rand.Next(70, 100);
            }
            else if (tierChance < 0.5)
            {
                _tierMinDamage = rand.Next(1, 50);
                _tierMaxDamage = rand.Next(50, 75);
            }
            else
            {
                _tierMinDamage = rand.Next(1, 30);
                _tierMaxDamage = rand.Next(30, 50);
            }
        }

        public override int AosMinDamage => _tierMinDamage;
        public override int AosMaxDamage => _tierMaxDamage;

        public override int EffectID => 0x36D4;

        public override Type ReagentAmmoType => typeof(BatWing);
        public override Item ReagentAmmo => new BatWing();

        public override int ColdDamage => 20;

        public override WeaponAbility PrimaryAbility => WeaponAbility.MovingShot;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

        public override int AosStrengthReq => 80;
        public override int AosSpeed => 22;
        public override float MlSpeed => 5.00f;

        public override int OldStrengthReq => 40;
        public override int OldMinDamage => 11;
        public override int OldMaxDamage => 56;
        public override int OldSpeed => 10;

        public override int DefMaxRange => 8;
        public override int InitMinHits => 31;
        public override int InitMaxHits => 100;

        public override SkillName DefSkill => SkillName.Mysticism;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Mysticism");
            list.Add("Ammo: Bat Wing");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(_tierMinDamage);
            writer.Write(_tierMaxDamage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _tierMinDamage = reader.ReadInt();
            _tierMaxDamage = reader.ReadInt();
        }
    }
}
