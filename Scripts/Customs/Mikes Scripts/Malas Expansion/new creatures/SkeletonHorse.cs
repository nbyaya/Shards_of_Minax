using System;

namespace Server.Mobiles
{
    public class SkeletonHorse : BaseMount
    {
        [Constructable]
        public SkeletonHorse()
            : this("Skeleton Horse")
        {
        }

        [Constructable]
        public SkeletonHorse(string name)
            : base(name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            SetStr(100);
            SetDex(80);
            SetInt(100);

            SetHits(100);
            SetMana(0);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 0, 5);
            SetSkill(SkillName.MagicResist, 0, 95);
            SetSkill(SkillName.Tactics, 0, 85);
            SetSkill(SkillName.Wrestling, 0, 85);
            SetSkill(SkillName.Necromancy, 18);
            SetSkill(SkillName.SpiritSpeak, 18);

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 30.0;
        }

        public SkeletonHorse(Serial serial)
            : base(serial)
        {
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool DeleteOnRelease { get { return true; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);			
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);			
            int version = reader.ReadInt();

            if (version < 2)
                reader.ReadInt();
        }
    }
}