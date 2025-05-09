using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(StoneWarSword))]
    [FlipableAttribute(0x13B9, 0x13Ba)]
    public class HolyKnightSword : BaseSword
    {
        [Constructable]
        public HolyKnightSword()
            : base(0x13B9)
        {
            this.Weight = 6.0;
			this.Name = "Holy Sword";			
        }

        public HolyKnightSword(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.CrushingBlow;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.ParalyzingBlow;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 40;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 15;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 19;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 28;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 3.75f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 40;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 6;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 34;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 30;
            }
        }
        public override int DefHitSound
        {
            get
            {
                return 0x237;
            }
        }
        public override int DefMissSound
        {
            get
            {
                return 0x23A;
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
                return 100;
            }
        }
        
		public override SkillName DefSkill
        {
            get
            {
                return SkillName.Chivalry;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Chivalry");
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