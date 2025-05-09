using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(VeterinaryLance))]
    [FlipableAttribute(0x26C0, 0x26CA)]
    public class VeterinaryLance : BaseSword
    {
        [Constructable]
        public VeterinaryLance()
            : base(0x26C0)
        {
            this.Weight = 12.0;
			this.Name = "Veterinary Lance";				
        }

        public VeterinaryLance(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.Dismount;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.ConcussionBlow;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 95;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 18;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 22;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 24;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 4.25f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 95;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 17;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 18;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 24;
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
                return 110;
            }
        }
        public override SkillName DefSkill
        {
            get
            {
                return SkillName.Veterinary;
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
            list.Add("Skill Required: Veterinary");
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