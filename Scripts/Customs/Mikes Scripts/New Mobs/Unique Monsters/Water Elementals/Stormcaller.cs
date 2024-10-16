using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a stormcaller corpse")]
    public class Stormcaller : BaseCreature
    {
        private DateTime m_NextLightningStrike;
        private DateTime m_NextStormCloud;
        private DateTime m_NextRainOfWrath;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Stormcaller()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a stormcaller";
            Body = 16; // Water elemental body
            BaseSoundID = 278;
            Hue = 2501; // Blue hue for storm effect

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Set the flag to indicate abilities are not initialized
        }

        public Stormcaller(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStormCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextRainOfWrath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLightningStrike)
                {
                    LightningStrike();
                }

                if (DateTime.UtcNow >= m_NextStormCloud)
                {
                    SummonStormCloud();
                }

                if (DateTime.UtcNow >= m_NextRainOfWrath)
                {
                    RainOfWrath();
                }
            }
        }

        private void LightningStrike()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Combatant.Damage(Utility.RandomMinMax(20, 30), this);
                Combatant.FixedEffect(0x3709, 10, 16); // Lightning effect
                Combatant.PlaySound(0x29F); // Lightning sound

                m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set next interval
            }
        }

        private void SummonStormCloud()
        {
            Point3D loc = GetSpawnPosition(5);

            if (loc != Point3D.Zero)
            {
                StormCloudEffect cloud = new StormCloudEffect(this);
                cloud.MoveToWorld(loc, Map);

                m_NextStormCloud = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set next interval
            }
        }

        private void RainOfWrath()
        {
            Point3D loc = GetSpawnPosition(5);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 2023);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A torrential rain of wrath pours down! *");

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.Damage(Utility.RandomMinMax(5, 10), this);
                    }
                }

                m_NextRainOfWrath = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set next interval
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
            m_NextLightningStrike = DateTime.UtcNow;
            m_NextStormCloud = DateTime.UtcNow;
            m_NextRainOfWrath = DateTime.UtcNow;
        }
    }

    public class StormCloudEffect : BaseCreature
    {
        private Mobile m_Master;

        public StormCloudEffect(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = 16; // Water elemental body
            Hue = 1152; // Blue hue

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);

            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public StormCloudEffect(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
