using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a flesh golem corpse")]
    public class FleshGolem : BaseCreature
    {
        [Constructable]
        public FleshGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flesh golem";
            Body = 304;
            BaseSoundID = 684;
			

            SetStr(176, 200);
            SetDex(51, 75);
            SetInt(46, 70);

            SetHits(106, 120);

            SetDamage(18, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 55.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 70.0);

            Fame = 1000;
            Karma = -1800;

            VirtualArmor = 34;

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public FleshGolem(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 1;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new NecromancersMantle());
            }			
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}