using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a dreaded creeper corpse")]
    public class DreadedCreeper : BaseCreature
    {
        private DateTime m_NextDreadWave;
        private DateTime m_NextNightmareRoots;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public DreadedCreeper()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dreaded creeper";
            Body = 8; // Corpser body
            Hue = 1389; // Unique dark green hue
			BaseSoundID = 684;

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

            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public DreadedCreeper(Serial serial)
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

            if (Combatant is Mobile target)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextDreadWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextNightmareRoots = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to true to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDreadWave)
                {
                    DreadWave(target);
                }

                if (DateTime.UtcNow >= m_NextNightmareRoots)
                {
                    NightmareRoots(target);
                }
            }
        }

        public void DreadWave(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 10))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Dreaded Creeper sends you into a state of dread!*");
            Effects.SendTargetEffect(target, 0x3709, 10, 20, 0x3B2, 0);

            // Apply fear effect (e.g., lower combat skills, damage)
            AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 100);

            m_NextDreadWave = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
        }

        public void NightmareRoots(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 10))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Dreaded Creeper's nightmare roots weaken you!*");
            Effects.SendTargetEffect(target, 0x3709, 10, 20, 0x3B2, 0);

            // Apply nightmare effect (e.g., reduce attack and defense)
            target.Damage(Utility.RandomMinMax(10, 20), this);
            target.SendMessage("You feel weakened and disoriented!");

            m_NextNightmareRoots = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset cooldown
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

            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
