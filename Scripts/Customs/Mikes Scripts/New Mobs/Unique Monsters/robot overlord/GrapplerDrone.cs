using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a grappler drone corpse")]
    public class GrapplerDrone : BaseCreature
    {
        private DateTime m_NextGrapplingHook;
        private DateTime m_NextLockdownProtocol;
        private DateTime m_NextGrapplingSurge;
        private DateTime m_NextLockdownOverride;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized
        private bool m_LockdownActive;

        [Constructable]
        public GrapplerDrone()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a grappler drone";
            Body = 0x2F4; // Using ExodusOverseer body
            Hue = 2292; // Unique hue

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
            m_LockdownActive = false; // Ensure lockdown state is reset
        }

        public GrapplerDrone(Serial serial) : base(serial) { }
        public override int GetIdleSound()
        {
            return 0xFD;
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
            return 0x23B;
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextGrapplingHook = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLockdownProtocol = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextGrapplingSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextLockdownOverride = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 5));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextGrapplingHook)
                {
                    UseGrapplingHook();
                }

                if (DateTime.UtcNow >= m_NextLockdownProtocol)
                {
                    UseLockdownProtocol();
                }

                if (DateTime.UtcNow >= m_NextGrapplingSurge)
                {
                    UseGrapplingSurge();
                }

                if (DateTime.UtcNow >= m_NextLockdownOverride)
                {
                    ActivateLockdownOverride();
                }
            }
        }

        private void UseGrapplingHook()
        {
            if (Combatant == null)
                return;

            Mobile target = Combatant as Mobile;
            Point3D targetLocation = target.Location;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grappling Hook Deploys! *");
            FixedEffect(0x3735, 10, 16, 0x6D, 0); // Visual effect for the hook

            if (InRange(target, 10))
            {
                target.Location = new Point3D(target.X, target.Y, target.Z); // Move target closer
                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 100);
                target.SendMessage("You are pulled closer by the grappling hook!");
            }

            m_NextGrapplingHook = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void UseLockdownProtocol()
        {
            if (Combatant == null)
                return;

            Mobile target = Combatant as Mobile;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lockdown Protocol Activated! *");
            FixedEffect(0x3735, 10, 16, 0x6D, 0); // Visual effect for the lockdown

            if (InRange(target, 5))
            {
                target.Freeze(TimeSpan.FromSeconds(5)); // Immobilize the target
                AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100);
                target.SendMessage("You are caught in the lockdown field!");
            }

            m_LockdownActive = true;
            m_NextLockdownProtocol = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void UseGrapplingSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grappling Surge! *");
            FixedEffect(0x3735, 10, 16, 0x6D, 0); // Visual effect for the surge

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 100);
                    m.SendMessage("The Grappler Drone unleashes a powerful surge!");
                    
                    if (Utility.RandomBool())
                    {
                        // Chance to disarm the target
                        if (m.Weapon != null)
                        {
                            m.SendMessage("You are disarmed by the grappling surge!");
                            
                            // Cast m.Weapon to Item if necessary
                            Item weapon = m.Weapon as Item;
                            if (weapon != null)
                            {
                                weapon.Delete();
                            }
                        }
                    }
                }
            }

            m_NextGrapplingSurge = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void ActivateLockdownOverride()
        {
            if (!m_LockdownActive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lockdown Override Activated! *");
            FixedEffect(0x3735, 10, 16, 0x6D, 0); // Visual effect for the override

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 55, 65);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 45, 55);
            SetResistance(ResistanceType.Energy, 45, 55);

            m_LockdownActive = false;
            m_NextLockdownOverride = DateTime.UtcNow + TimeSpan.FromMinutes(3);
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
