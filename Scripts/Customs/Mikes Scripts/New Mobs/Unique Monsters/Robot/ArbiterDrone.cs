using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an arbiter drone's remains")]
    public class ArbiterDrone : BaseCreature
    {
        private DateTime m_NextJudgmentBeam;
        private DateTime m_NextSeekerMissiles;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ArbiterDrone()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Arbiter Drone";
            Body = 0x2F5; // ExodusMinion body
            Hue = 2500; // Unique purple hue

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public ArbiterDrone(Serial serial)
            : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0x218;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x232;
        }

        public override int GetHurtSound()
        {
            return 0x140;
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
        public override Poison PoisonImmune => Poison.Lethal;


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextJudgmentBeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSeekerMissiles = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextJudgmentBeam)
                {
                    DoJudgmentBeam();
                }

                if (DateTime.UtcNow >= m_NextSeekerMissiles)
                {
                    DoSeekerMissiles();
                }
            }
        }

        private void DoJudgmentBeam()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Charging Judgment Beam *");
                PlaySound(0x20A);
                FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

                Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Judgment Beam Released *");
                    PlaySound(0x208);
                    MovingParticles(target, 0x36D4, 7, 0, false, true, 0, 0, 9502, 4019, 0x160, 0);

                    int damage = Utility.RandomMinMax(50, 80);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                });

                m_NextJudgmentBeam = DateTime.UtcNow + TimeSpan.FromSeconds(35);
            }
        }

        private void DoSeekerMissiles()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Launching Seeker Missiles *");
            PlaySound(0x1E5);

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), () =>
                {
                    Mobile target = Combatant as Mobile;
                    if (target != null && target.Alive)
                    {
                        MovingParticles(target, 0x36E4, 5, 0, false, true, 0x304, 0, 9502, 4019, 0x160, 0);
                        PlaySound(0x11B);

                        int damage = Utility.RandomMinMax(15, 25);
                        AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    }
                });
            }

            m_NextSeekerMissiles = DateTime.UtcNow + TimeSpan.FromSeconds(25);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
            m_NextJudgmentBeam = DateTime.UtcNow;
            m_NextSeekerMissiles = DateTime.UtcNow;
        }
    }
}
