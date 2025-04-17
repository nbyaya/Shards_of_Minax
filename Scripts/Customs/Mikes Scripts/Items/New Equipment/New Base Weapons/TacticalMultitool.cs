using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(TacticalMultitool))]
    [FlipableAttribute(0x143D, 0x143C)]
    public class TacticalMultitool : BaseBashing
    {
        [Constructable]
        public TacticalMultitool()
            : base(0x143D)
        {
            Weight = 9.0;
            Layer = Layer.OneHanded;
			this.Name = "Tactical Multitool";			
        }

        public TacticalMultitool(Serial serial)
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
                return WeaponAbility.MortalStrike;
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
                return 13;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 17;
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
                return 3.25f;
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
                return 6;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 33;
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
                return SkillName.Tactics;
            }
        }
		
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Tactics");
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