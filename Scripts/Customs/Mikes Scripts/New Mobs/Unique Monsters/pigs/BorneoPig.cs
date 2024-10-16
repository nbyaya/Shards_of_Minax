using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a borneo pig corpse")]
    public class BorneoPig : BaseCreature
    {
        private DateTime m_NextPoisonousBreath;
        private DateTime m_NextEarthquakeTrot;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BorneoPig()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Borneo pig";
            Body = 0xCB; // Pig body
            Hue = 2226; // Dark Gray hue
			BaseSoundID = 0xC4;

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

        public BorneoPig(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextPoisonousBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEarthquakeTrot = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPoisonousBreath)
                {
                    PoisonousBreath();
                }

                if (DateTime.UtcNow >= m_NextEarthquakeTrot)
                {
                    EarthquakeTrot();
                }
            }
        }

        private void PoisonousBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Borneo Pig exhales a toxic cloud! *");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are caught in the Borneo Pig's poisonous breath!");
                    m.ApplyPoison(this, Poison.Lethal);
                    m.Damage(10, this);
                }
            }

            m_NextPoisonousBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for PoisonousBreath
        }

        private void EarthquakeTrot()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Borneo Pig stomps the ground with a mighty quake! *");
            FixedEffect(0x3709, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The ground shakes violently beneath you!");
                    m.Damage(15, this);
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextEarthquakeTrot = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for EarthquakeTrot
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
