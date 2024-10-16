using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;
using Server.Regions;

namespace Server.Mobiles
{
    [CorpseName("a cosmic bouncer corpse")]
    public class CosmicBouncer : BaseCreature
    {
        private DateTime m_NextStarfall;
        private DateTime m_NextGravitationalPull;
        private DateTime m_NextCosmicShield;
        private DateTime m_NextTeleport;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CosmicBouncer()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Cosmic Bouncer";
            Body = 205; // Rabbit body
            Hue = 2258; // Cosmic hue for the starry pattern

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

            m_AbilitiesInitialized = false;
        }

        public CosmicBouncer(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextStarfall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));
                    m_NextGravitationalPull = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 30));
                    m_NextCosmicShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(25, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextStarfall)
                {
                    Starfall();
                }

                if (DateTime.UtcNow >= m_NextGravitationalPull)
                {
                    GravitationalPull();
                }

                if (DateTime.UtcNow >= m_NextCosmicShield)
                {
                    CosmicShield();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }
            }
        }

        private void Starfall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cosmic Bouncer calls down a rain of dark stars! *");
            PlaySound(0x20A); // Celestial sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 0, 100, 0, 0);
                    m.SendMessage("You are struck by dark stars that pierce the heavens!");
                }
            }

            m_NextStarfall = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void GravitationalPull()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cosmic Bouncer creates a powerful gravitational pull! *");
            PlaySound(0x20B); // Gravity well sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                    m.SendMessage("You are caught in a swirling gravitational vortex!");
                    m.MoveToWorld(new Point3D(m.X, m.Y, m.Z - 5), Map); // Pull down effect
                }
            }

            m_NextGravitationalPull = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void CosmicShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cosmic Bouncer activates a cosmic shield! *");
            PlaySound(0x20C); // Shield activation sound

            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Shield effect

            this.VirtualArmor += 30;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => DeactivateCosmicShield());

            m_NextCosmicShield = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void DeactivateCosmicShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cosmic Shield fades away! *");
            this.VirtualArmor -= 30;
        }

        private void Teleport()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cosmic Bouncer disappears in a swirl of stardust! *");
            PlaySound(0x20D); // Teleport sound effect

            // Teleport to a random location within a certain range
            Point3D newLocation = new Point3D(
                Utility.RandomMinMax(Location.X - 10, Location.X + 10),
                Utility.RandomMinMax(Location.Y - 10, Location.Y + 10),
                Location.Z
            );

            if (Map.CanSpawnMobile(newLocation) && Map != null)
            {
                this.Location = newLocation;
                this.ProcessDelta();
                SendLocalizedMessage(1060637); // The creature teleports away!
            }

            m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(60);
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
            m_AbilitiesInitialized = false;
        }
    }
}
