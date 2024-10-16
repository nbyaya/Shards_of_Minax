using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a typhus rat corpse")]
    public class TyphusRat : GiantRat
    {
        private DateTime m_NextLiceInfestation;
        private DateTime m_NextLiceSwarm;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TyphusRat()
            : base()
        {
            Name = "a typhus rat";
            Hue = 2262; // Unique hue for Typhus Rat
			this.BaseSoundID = 0xCC;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public TyphusRat(Serial serial)
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
                    m_NextLiceInfestation = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLiceSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLiceInfestation)
                {
                    LiceInfestation();
                }

                if (DateTime.UtcNow >= m_NextLiceSwarm)
                {
                    LiceSwarm();
                }
            }
        }

        private void LiceInfestation()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                target.SendMessage("*Lice crawl all over you as the Typhus Rat bites!*");
                target.FixedEffect(0x376A, 10, 16); // Small insect particles

                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (target != null && target.Alive)
                    {
                        target.RawStr -= 5;
                        target.RawDex -= 5;
                        target.SendMessage("You feel significantly weaker as lice infest you!");
                        target.Damage(10, this); // More damage for higher impact
                    }
                });

                m_NextLiceInfestation = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for LiceInfestation
            }
        }

        private void LiceSwarm()
        {
            Point3D location = Location;
            Map map = Map;

            if (map != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Typhus Rat summons a swarm of lice! *");

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are surrounded by a swarm of lice! The bite is excruciating!");
                        m.Damage(5, this); // Area damage
                        m.SendMessage("You are confused by the swarm!");
                        m.Paralyze(TimeSpan.FromSeconds(3)); // Confusion effect
                    }
                }

                m_NextLiceSwarm = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for LiceSwarm
            }
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
