using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Mimicron corpse")]
    public class Mimicron : BaseCreature
    {
        private DateTime m_NextCopycat;
        private DateTime m_NextDecoyProjection;
        private Mobile m_CopycatTarget;
        private Dictionary<SkillName, double> m_OriginalSkills;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Mimicron()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Mimicron";
            Body = 0x2F5; // ExodusMinion body
            BaseSoundID = 0x2F8;
            Hue = 2500; // Unique silver-blue hue

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

            m_OriginalSkills = new Dictionary<SkillName, double>();
            m_AbilitiesInitialized = false; // Initialize flag
        }

        public Mimicron(Serial serial)
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

        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override int GetIdleSound()
        {
            return 0x2CC;
        }

        public override int GetAttackSound()
        {
            return 0x2C8;
        }

        public override int GetDeathSound()
        {
            return 0x2C9;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextCopycat = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60)); // Random start time for Copycat
                    m_NextDecoyProjection = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60)); // Random start time for DecoyProjection
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCopycat)
                {
                    UseCopycat();
                }

                if (DateTime.UtcNow >= m_NextDecoyProjection)
                {
                    UseDecoyProjection();
                }
            }
        }

        private void UseCopycat()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Copycat Activated *");
                PlaySound(0x5BC);

                FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

                // Store original skills
                m_OriginalSkills.Clear();
                foreach (SkillName skillName in Enum.GetValues(typeof(SkillName)))
                {
                    m_OriginalSkills[skillName] = Skills[skillName].Base;
                }

                // Copy target's skills
                foreach (SkillName skillName in Enum.GetValues(typeof(SkillName)))
                {
                    Skill targetSkill = target.Skills[skillName];
                    Skills[skillName].Base = targetSkill.Base;
                }

                m_CopycatTarget = target;

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    // Restore original skills
                    foreach (SkillName skillName in Enum.GetValues(typeof(SkillName)))
                    {
                        if (m_OriginalSkills.TryGetValue(skillName, out double originalBase))
                        {
                            Skills[skillName].Base = originalBase;
                        }
                    }
                    m_CopycatTarget = null;
                });

                // Reset the next use time with a random interval
                Random rand = new Random();
                m_NextCopycat = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 90)); // Random cooldown between 30 and 90 seconds
            }
        }

        private void UseDecoyProjection()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Decoy Projection *");
            PlaySound(0x208);

            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5042);

                MimicronDecoy decoy = new MimicronDecoy(this);
                decoy.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    if (!decoy.Deleted)
                        decoy.Delete();
                });
            }

            // Reset the next use time with a random interval
            Random rand = new Random();
            m_NextDecoyProjection = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60)); // Random cooldown between 30 and 60 seconds
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
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextCopycat = DateTime.UtcNow;
            m_NextDecoyProjection = DateTime.UtcNow;
            m_OriginalSkills = new Dictionary<SkillName, double>();
            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialization
        }
    }

    public class MimicronDecoy : BaseCreature
    {
        private Mobile m_Master;

        public MimicronDecoy(Mobile master)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(100);
            SetDex(100);
            SetInt(100);

            SetHits(250);

            SetDamage(5, 10);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 90.0);

            VirtualArmor = 50;
        }

        public MimicronDecoy(Serial serial)
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
            writer.Write((int)0);
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
