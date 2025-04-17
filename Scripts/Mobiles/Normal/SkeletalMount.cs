using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an undead horse corpse")]
    public class SkeletalMount : BaseMount
    {
        [Constructable] 
        public SkeletalMount()
            : this("a skeletal steed")
        {
        }

        [Constructable]
        public SkeletalMount(string name)
            : base(name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.SetStr(91, 100);
            this.SetDex(46, 55);
            this.SetInt(46, 60);
			Team = 4;

            this.SetHits(41, 50);

            this.SetDamage(5, 12);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Cold, 50);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Cold, 90, 95);
            this.SetResistance(ResistanceType.Poison, 100);
            this.SetResistance(ResistanceType.Energy, 10, 15);

            this.SetSkill(SkillName.MagicResist, 95.1, 100.0);
            this.SetSkill(SkillName.Tactics, 50.0);
            this.SetSkill(SkillName.Wrestling, 70.1, 80.0);

            this.Fame = 0;
            this.Karma = 0;
            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 0.0;
        }

        public SkeletalMount(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
		
        public override void GenerateLoot()
        {
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new SkeletalReapersBardiche());
            }
		}		
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        this.Name = "a skeletal steed";
                        this.Tamable = true;
                        this.MinTameSkill = 0.0;
                        this.ControlSlots = 1;
                        break;
                    }
            }
        }
    }
}