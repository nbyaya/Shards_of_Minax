using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mystic tiger corpse")]
    public class MysticTiger : BaseCreature
    {
        private DateTime m_NextCubSummon;
        private static readonly int[] CubBody = { 0xD6, 0xD7 };

        [Constructable]
        public MysticTiger()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Mystic Tiger";
            Body = 0xD6;
            Hue = 1175; // Mystic blue hue
            BaseSoundID = 0x462;

            SetStr(500, 520);
            SetDex(200, 220);
            SetInt(150, 170);

            SetHits(400, 420);

            SetDamage(18, 24);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Parry, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 80.0);

            Fame = 12000;
            Karma = -12000;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 98.0;

            m_NextCubSummon = DateTime.UtcNow;
        }

        public MysticTiger(Serial serial)
            : base(serial)
        {
        }

        public override int GetIdleSound() { return 0x671; }
        public override int GetAngerSound() { return 0x670; }
        public override int GetHurtSound() { return 0x672; }
        public override int GetDeathSound() { return 0x671; }

        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Regular; } }
        public override int Meat { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextCubSummon)
            {
                SummonCub();
                m_NextCubSummon = DateTime.UtcNow + TimeSpan.FromMinutes(3);
            }
        }

        private void SummonCub()
        {
            if (Combatant == null)
                return;

            int cubCount = Utility.RandomMinMax(1, 3); // Summon 1 to 3 cubs
            for (int i = 0; i < cubCount; i++)
            {
                EtherealTigerCub cub = new EtherealTigerCub();
                cub.MoveToWorld(Location, Map);
                cub.Combatant = Combatant;
                cub.Controlled = true;
                cub.ControlMaster = this.ControlMaster;
                cub.Summoned = true;
                cub.SummonMaster = this;
                cub.Delete();
            }
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summons ethereal tiger cubs! *");
            PlaySound(0x482);
            FixedEffect(0x3728, 10, 15);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextCubSummon);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextCubSummon = reader.ReadDateTime();
        }
    }

    public class EtherealTigerCub : BaseCreature
    {
        [Constructable]
        public EtherealTigerCub()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Ethereal Tiger Cub";
            Body = 0xD6;
            Hue = 1153; // Ethereal hue
            BaseSoundID = 0x463;

            SetStr(150, 170);
            SetDex(120, 140);
            SetInt(60, 80);

            SetHits(100, 120);

            SetDamage(10, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Parry, 60.0, 70.0);
            SetSkill(SkillName.Tactics, 70.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 80.0);
            SetSkill(SkillName.MagicResist, 50.0);

            Fame = 4500;
            Karma = -4500;

            Tamable = false;
            ControlSlots = 1;
        }

        public EtherealTigerCub(Serial serial)
            : base(serial)
        {
        }

        public override int GetIdleSound() { return 0x671; }
        public override int GetAngerSound() { return 0x670; }
        public override int GetHurtSound() { return 0x672; }
        public override int GetDeathSound() { return 0x671; }

        public override int Hides { get { return 2; } }
        public override HideType HideType { get { return HideType.Spined; } }
        public override int Meat { get { return 2; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

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
