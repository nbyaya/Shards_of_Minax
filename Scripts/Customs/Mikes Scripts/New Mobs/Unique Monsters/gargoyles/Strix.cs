using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a strix corpse")]
    public class Strix : BaseCreature
    {
        private DateTime m_NextEchoLocation;
        private DateTime m_NextWingSlash;
        private DateTime m_NextShadowyDive;
        private DateTime m_NextDarkEmbrace;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Strix()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Strix the Winged Horror";
            Body = 4; // Gargoyle body
            Hue = 1668; // Dark hue
            BaseSoundID = 372;

            this.SetStr(250);
            this.SetDex(140);
            this.SetInt(120);

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

        public Strix(Serial serial)
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
                    m_NextEchoLocation = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextWingSlash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowyDive = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextDarkEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEchoLocation)
                {
                    EchoLocation();
                }

                if (DateTime.UtcNow >= m_NextWingSlash)
                {
                    WingSlash();
                }

                if (DateTime.UtcNow >= m_NextShadowyDive)
                {
                    ShadowyDive();
                }

                if (DateTime.UtcNow >= m_NextDarkEmbrace)
                {
                    DarkEmbrace();
                }
            }
        }

        private void EchoLocation()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Echo Location! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Hidden)
                {
                    m.RevealingAction();
                    m.SendMessage("Strix reveals your position with its echo location!");
                }

                // Blind effect for a short duration
                if (m.Alive && !m.Hidden)
                {
                    m.SendMessage("You are temporarily blinded by Strix's echo location!");
                    m.FixedEffect(0x376A, 10, 16);
                    m.Freeze(TimeSpan.FromSeconds(2)); // Temporary blindness
                }
            }

            m_NextEchoLocation = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown after ability use
        }

        private void WingSlash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Wing Slash! *");
            FixedEffect(0x37B9, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    m.Damage(damage, this);
                    m.SendMessage("You are struck by Strix's wing slash!");

                    // Apply a debuff with a chance of 50%
                    if (Utility.RandomDouble() < 0.5)
                    {
                        m.SendMessage("Strix's wing slash leaves you disoriented!");
                        m.Freeze(TimeSpan.FromSeconds(5)); // Simulated disorientation
                    }
                }
            }

            m_NextWingSlash = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Set cooldown after ability use
        }

        private void ShadowyDive()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadowy Dive! *");
            FixedEffect(0x376A, 10, 16);

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                Point3D newLocation = new Point3D(target.X + Utility.RandomMinMax(-2, 2), target.Y + Utility.RandomMinMax(-2, 2), target.Z);
                if (Map.CanSpawnMobile(newLocation))
                {
                    this.Location = newLocation;
                    this.Combatant = target;
                    target.Damage(40, this);
                    target.SendMessage("Strix appears from the shadows and strikes you!");

                    // AoE damage on reappearance
                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m != target && m.Alive)
                        {
                            int aoeDamage = Utility.RandomMinMax(10, 15);
                            m.Damage(aoeDamage, this);
                            m.SendMessage("You are caught in Strix's shadowy aftermath!");
                        }
                    }
                }
            }

            m_NextShadowyDive = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown after ability use
        }

        private void DarkEmbrace()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Dark Embrace! *");
            FixedEffect(0x37C4, 10, 16);

            // Temporary damage reduction
            this.VirtualArmor += 20;

            // Fear effect
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("Strix's dark aura instills fear in you!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Fear effect
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(() => { this.VirtualArmor -= 20; }));

            m_NextDarkEmbrace = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set cooldown after ability use
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
