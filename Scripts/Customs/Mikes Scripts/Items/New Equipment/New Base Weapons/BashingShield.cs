using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(BashingShield))]
    [FlipableAttribute(0x1B76, 0x1B76)]
    public class BashingShield : BaseBashing
    {
        [Constructable]
        public BashingShield()
            : base(0x1B76)
        {
            this.Weight = 14.0;
			this.Name = "Bashing Shield";			
        }

        public BashingShield(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.ConcussionBlow;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.Disarm;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 45;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 11;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 15;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 40;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 2.75f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 20;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 8;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 32;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 30;
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
                return 70;
            }
        }
		
		public override SkillName DefSkill
        {
            get
            {
                return SkillName.Parry;
            }
        }
		
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Parry");
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