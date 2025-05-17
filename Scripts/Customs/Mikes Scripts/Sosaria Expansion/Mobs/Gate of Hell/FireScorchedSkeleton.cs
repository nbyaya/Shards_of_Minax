using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a scorched skeletal corpse")]
    public class FireScorchedSkeleton : BaseCreature
    {
        [Constructable]
        public FireScorchedSkeleton() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fire-scorched skeleton";
            Body = 50;
            Hue = 1175;
            BaseSoundID = 0x48D;

            SetStr(80, 100);
            SetDex(60, 80);
            SetInt(30, 50);

            SetHits(90, 110);

            SetDamage(8, 12);
            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Physical, 50);

            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, -10, 0);
            SetResistance(ResistanceType.Poison, 20, 30);

            SetSkill(SkillName.MagicResist, 60.0);
            SetSkill(SkillName.Tactics, 70.0);
            SetSkill(SkillName.Wrestling, 65.0);

            Fame = 1800;
            Karma = -1800;
            VirtualArmor = 25;

            if (Utility.RandomDouble() < 0.8)
                PackItem(new BurnedBoneFragment());

            if (Utility.RandomDouble() < 0.001)
                PackItem(new FireWardedBand());
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Regular;

        public FireScorchedSkeleton(Serial serial) : base(serial) { }

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
