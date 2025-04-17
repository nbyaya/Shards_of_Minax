using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(GourmandsFork))]
    [FlipableAttribute(0x1405, 0x1404)]
    public class GourmandsFork : BaseSpear
    {
        [Constructable]
        public GourmandsFork()
            : base(0x1405)
        {
            this.Weight = 9.0;
			this.Name = "Gourmands Fork";			
        }

        public GourmandsFork(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.BleedAttack;
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
                return 10;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 14;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 43;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 2.50f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 35;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 4;
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
                return 45;
            }
        }
        public override int DefHitSound
        {
            get
            {
                return 0x236;
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
        public override WeaponAnimation DefAnimation
        {
            get
            {
                return WeaponAnimation.Pierce1H;
            }
        }
		
		public override SkillName DefSkill
        {
            get
            {
                return SkillName.TasteID;
            }
        }
		
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: TasteID");
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