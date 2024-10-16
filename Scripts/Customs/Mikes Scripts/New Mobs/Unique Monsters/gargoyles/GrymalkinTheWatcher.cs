using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells; // For spell effects, if needed

namespace Server.Mobiles
{
    [CorpseName("a gryfalkin corpse")]
    public class GrymalkinTheWatcher : BaseCreature
    {
        private static readonly string[] m_Messages = new string[]
        {
            "*Grymalkin’s eyes flare with intensity!*",
            "*Grymalkin’s gaze pierces through the shadows!*",
            "*Grymalkin pulses with an omniscient aura!*"
        };

        private DateTime m_NextGazeOfDisdain;
        private DateTime m_NextOmniscientPulse;
        private DateTime m_NextPhaseShift;
        private bool m_AbilitiesInitialized;
        private bool m_PhaseShiftActive;

        [Constructable]
        public GrymalkinTheWatcher()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Grymalkin the Watcher";
            Body = 4; // Gargoyle body
            Hue = 1669; // Unique hue for Grymalkin
            BaseSoundID = 372;

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
            m_PhaseShiftActive = false; // Ensure PhaseShiftActive is false initially
        }

        public GrymalkinTheWatcher(Serial serial)
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
                    // Initialize random intervals for abilities
                    Random rand = new Random();
                    m_NextGazeOfDisdain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextOmniscientPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set flag to prevent reinitializing
                }

                if (DateTime.UtcNow >= m_NextGazeOfDisdain)
                {
                    GazeOfDisdain();
                }

                if (DateTime.UtcNow >= m_NextOmniscientPulse)
                {
                    OmniscientPulse();
                }

                if (DateTime.UtcNow >= m_NextPhaseShift)
                {
                    PhaseShift();
                }
            }
        }

        private void GazeOfDisdain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Gaze of Disdain!*");
            PlaySound(0x20E); // Sound effect for the ability
            FixedEffect(0x376A, 10, 16); // Visual effect

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You feel paralyzed by Grymalkin's piercing gaze!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.Damage(20, this); // Apply damage
                }
            }

            m_NextGazeOfDisdain = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void OmniscientPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Omniscient Pulse!*");
            PlaySound(0x20F); // Sound effect for the ability
            FixedEffect(0x37C4, 10, 36); // Visual effect

            foreach (Mobile m in GetMobilesInRange(15))
            {
                if (m.Hidden) // Use Hidden instead of Invisible
                {
                    // Custom revealing code can be implemented here
                    // For example, sending a message or using a spell effect
                    m.SendMessage("Grymalkin's pulse reveals your hidden presence!");
                }
            }

            m_NextOmniscientPulse = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void PhaseShift()
        {
            if (!m_PhaseShiftActive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Grymalkin shifts through dimensions!*");
                PlaySound(0x20E); // Sound effect for the ability
                FixedEffect(0x376A, 10, 16); // Visual effect

                Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                if (Map.CanSpawnMobile(newLocation))
                {
                    MoveToWorld(newLocation, Map);
                    m_PhaseShiftActive = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () => { m_PhaseShiftActive = false; });
                }

                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Grymalkin the Watcher falls!*");
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

            m_NextGazeOfDisdain = DateTime.UtcNow;
            m_NextOmniscientPulse = DateTime.UtcNow;
            m_NextPhaseShift = DateTime.UtcNow;
            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
            m_PhaseShiftActive = false;
        }
    }
}
