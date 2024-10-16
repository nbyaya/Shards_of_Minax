using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a spectral tiger corpse")]
    public class SpectralTiger : BaseCreature
    {
        private DateTime m_NextCubSummon;

        [Constructable]
        public SpectralTiger()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Spectral Tiger";
            Body = 0x592; // Using the body type from the provided example
            Hue = 1154; // Unique hue for Spectral Tiger
            BaseSoundID = 0x3EE;

            SetStr(750, 800);
            SetDex(220, 240);
            SetInt(150, 170);

            SetHits(400, 450);

            SetDamage(18, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 35, 45);
            SetResistance(ResistanceType.Poison, 35, 45);
            SetResistance(ResistanceType.Energy, 25, 45);

            SetSkill(SkillName.Parry, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.DetectHidden, 85.0);

            Fame = 12000;
            Karma = -12000;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextCubSummon = DateTime.UtcNow;
        }

        public SpectralTiger(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextCubSummon)
            {
                SummonSpectralCubs();
                m_NextCubSummon = DateTime.UtcNow + TimeSpan.FromMinutes(3);
            }
        }

        private void SummonSpectralCubs()
        {
            if (Combatant != null)
            {
                int cubsToSummon = Utility.RandomMinMax(1, 3);

                for (int i = 0; i < cubsToSummon; i++)
                {
                    SpectralTigerCub cub = new SpectralTigerCub();
                    cub.MoveToWorld(Location, Map);
                    cub.Combatant = Combatant;
                    cub.Controlled = true;
                    cub.ControlMaster = this;
                }

                PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, true, "* Summons spectral tiger cubs *");
                PlaySound(0x20F);
                FixedEffect(0x376A, 10, 20);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1);
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override bool StatLossAfterTame { get { return true; } }

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

    public class SpectralTigerCub : BaseCreature
    {
        [Constructable]
        public SpectralTigerCub()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Spectral Tiger Cub";
            Body = 0xD6; // Using a smaller feline body type
            Hue = 1154; // Matching hue with Spectral Tiger
            BaseSoundID = 0x69;

            SetStr(150, 200);
            SetDex(150, 180);
            SetInt(80, 100);

            SetHits(100, 150);

            SetDamage(8, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Tactics, 50.0, 60.0);
            SetSkill(SkillName.Wrestling, 50.0, 60.0);

            Fame = 4500;
            Karma = -4500;

            Tamable = false;
            ControlSlots = 1;
        }

        public SpectralTigerCub(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
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
