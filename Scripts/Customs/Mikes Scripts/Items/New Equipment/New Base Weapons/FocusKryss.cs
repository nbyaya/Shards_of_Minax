using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(FocusKryss))]
    [FlipableAttribute(0x1401, 0x1400)]
    public class FocusKryss : BaseSword
    {
        [Constructable]
        public FocusKryss()
            : base(0x1401)
        {
            Weight = 2.0;
			this.Name = "Focus Kryss";
        }

        public FocusKryss(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.ArmorIgnore;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.InfectiousStrike;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 10;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 10;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 12;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 53;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 2.00f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 10;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 3;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 28;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 53;
            }
        }
        public override int DefHitSound
        {
            get
            {
                return 0x23C;
            }
        }
        public override int DefMissSound
        {
            get
            {
                return 0x238;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 31;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 90;
            }
        }
        public override SkillName DefSkill
        {
            get
            {
                return SkillName.Focus;
            }
        }
        public override WeaponType DefType
        {
            get
            {
                return WeaponType.Piercing;
            }
        }
        public override WeaponAnimation DefAnimation
        {
            get
            {
                return WeaponAnimation.Pierce1H;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Focus");
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