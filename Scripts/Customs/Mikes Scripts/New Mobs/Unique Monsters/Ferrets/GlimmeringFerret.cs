using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a glimmering ferret corpse")]
    public class GlimmeringFerret : BaseCreature
    {
        private DateTime m_NextSparkleBlast;
        private DateTime m_NextGlitterShield;
        private DateTime m_NextStardustBeam;
        private DateTime m_NextEvasiveManeuver;
        private DateTime m_NextSparklingAura;
        private bool m_HasShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public GlimmeringFerret()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a glimmering ferret";
            Body = 0x117; // Using the ferret body
            Hue = 1573; // Sparkling hue for magical effect
			BaseSoundID = 0xCF;

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

        public GlimmeringFerret(Serial serial)
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
                    m_NextSparkleBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGlitterShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextStardustBeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_NextEvasiveManeuver = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSparklingAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSparkleBlast)
                {
                    SparkleBlast();
                }

                if (DateTime.UtcNow >= m_NextGlitterShield && !m_HasShield)
                {
                    GlitterShield();
                }

                if (DateTime.UtcNow >= m_NextStardustBeam)
                {
                    StardustBeam();
                }

                if (DateTime.UtcNow >= m_NextEvasiveManeuver)
                {
                    EvasiveManeuver();
                }

                if (DateTime.UtcNow >= m_NextSparklingAura)
                {
                    SparklingAura();
                }
            }
        }

        private void SparkleBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Emits a burst of glittery particles! *");
            FixedEffect(0x3728, 10, 16); // Glitter effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are dazzled and confused by the glimmering blast!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                    m.SendMessage("You feel slowed down!");
                    // Apply damage over time
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => m.Damage(5, this));
                }
            }

            m_NextSparkleBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void GlitterShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A shimmering shield surrounds the ferret! *");
            FixedEffect(0x373A, 10, 16); // Shield effect

            this.VirtualArmor += 20;
            m_HasShield = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), RemoveGlitterShield);

            m_NextGlitterShield = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void RemoveGlitterShield()
        {
            this.VirtualArmor -= 20;
            m_HasShield = false;
        }

        private void StardustBeam()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Fires a dazzling stardust beam! *");
                FixedEffect(0x373A, 10, 16); // Beam effect

                Mobile combatant = Combatant as Mobile; // Ensure Combatant is of type Mobile
                combatant.SendMessage("You are struck by a powerful beam of stardust!");
                combatant.Damage(30, this); // High damage
                combatant.Freeze(TimeSpan.FromSeconds(2)); // Stun effect
            }

            m_NextStardustBeam = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void EvasiveManeuver()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Performs an evasive maneuver! *");
            this.Dex += 20; // Temporarily increase dexterity for better evasion
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => this.Dex -= 20);

            m_NextEvasiveManeuver = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void SparklingAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A sparkling aura surrounds the ferret! *");
            FixedEffect(0x3728, 10, 16); // Aura effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The sparkling aura around the ferret burns your skin!");
                    m.Damage(5, this); // Minor damage
                    m.SendMessage("Your attacks feel sluggish!");
                    m.SendMessage("Your attack speed is reduced!");
                }
            }

            m_NextSparklingAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);
            if (Combatant == null)
            {
                // Leave a trail of sparkling particles as it moves
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
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

            // Reset initialization flag and random intervals on deserialization
            m_AbilitiesInitialized = false;
            m_NextSparkleBlast = DateTime.UtcNow;
            m_NextGlitterShield = DateTime.UtcNow;
            m_NextStardustBeam = DateTime.UtcNow;
            m_NextEvasiveManeuver = DateTime.UtcNow;
            m_NextSparklingAura = DateTime.UtcNow;
        }
    }
}
