using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a lava crab corpse")]
    public class LavaCrab : BaseMount
    {
        private DateTime m_NextMoltenPull;
        private DateTime m_NextEruptionSlam;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LavaCrab()
            : base("Lava Crab", 1510, 16081, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Body = 1510; // Coconut Crab body
            Hue = 1457; // Lava hue
			BaseSoundID = 0x4F2;

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
            SetResistance(ResistanceType.Poison, 100);
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public LavaCrab(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                    m_NextMoltenPull = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEruptionSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMoltenPull)
                {
                    MoltenPull();
                    m_NextMoltenPull = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown for Molten Pull
                }

                if (DateTime.UtcNow >= m_NextEruptionSlam)
                {
                    EruptionSlam();
                    m_NextEruptionSlam = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Set cooldown for Eruption Slam
                }
            }
        }

        private void MoltenPull()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Lava Crab performs a Molten Pull! *");
            Effects.SendLocationEffect(Combatant.Location, Combatant.Map, 0x3709, 10, 30, 0x9C, 0);

            AOS.Damage(Combatant, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                if (Combatant != null && !Combatant.Deleted && Combatant.Alive)
                {
                    AOS.Damage(Combatant, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                }
            });
        }

        private void EruptionSlam()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Lava Crab slams the ground with fiery eruption! *");
            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 30, 0x9C, 0);

            int fireDamage = Utility.RandomMinMax(15, 25);
            AOS.Damage(Combatant, this, fireDamage, 0, 100, 0, 0, 0);
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
        }
    }
}
