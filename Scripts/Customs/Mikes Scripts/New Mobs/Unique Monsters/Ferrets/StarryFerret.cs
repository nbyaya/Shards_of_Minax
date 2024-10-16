using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a starry ferret corpse")]
    public class StarryFerret : BaseCreature
    {
        private DateTime m_NextStarShower;
        private DateTime m_NextCosmicCloak;
        private DateTime m_NextGravityWarp;
        private DateTime m_NextStarlightBurst;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public StarryFerret()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a starry ferret";
            Body = 0x117; // Using ferret body as base
            Hue = 1569; // Starry hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public StarryFerret(Serial serial)
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

        public override int Meat { get { return 1; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextStarShower = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCosmicCloak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGravityWarp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextStarlightBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextStarShower)
                {
                    StarShower();
                }

                if (DateTime.UtcNow >= m_NextCosmicCloak)
                {
                    CosmicCloak();
                }

                if (DateTime.UtcNow >= m_NextGravityWarp)
                {
                    GravityWarp();
                }

                if (DateTime.UtcNow >= m_NextStarlightBurst)
                {
                    StarlightBurst();
                }
            }
        }

        private void StarShower()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summons a shower of stars *");
            FixedEffect(0x376A, 10, 16); // Particle effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The Starry Ferret's star shower dazzles you!");
                    m.Damage(10, this);

                    // Chance to stun
                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.Freeze(TimeSpan.FromSeconds(3));
                    }
                }
            }

            m_NextStarShower = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Fixed cooldown
        }

        private void CosmicCloak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Envelops itself in a cloak of starlight *");
            FixedEffect(0x376A, 10, 16); // Particle effect

            // Temporary invisibility
            this.Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => this.Hidden = false);

            m_NextCosmicCloak = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown
        }

        private void GravityWarp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Warps the gravity around you *");
            FixedEffect(0x376A, 10, 16); // Particle effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The Starry Ferret's gravity warp throws you off balance!");
                    m.Damage(15, this);

                    // Chance to slow
                    if (Utility.RandomDouble() < 0.25)
                    {
                        m.Dex -= 10;
                        if (m.Dex < 1) m.Dex = 1;
                    }
                }
            }

            m_NextGravityWarp = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Fixed cooldown
        }

        private void StarlightBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a burst of starlight *");
            FixedEffect(0x376A, 10, 16); // Particle effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The Starry Ferret's starlight burst blinds you!");
                    m.Damage(20, this);

                    // Chance to blind
                    if (Utility.RandomDouble() < 0.25)
                    {
                        m.SendMessage("You are blinded by the starlight!");
                        m.Freeze(TimeSpan.FromSeconds(3));
                    }
                }
            }

            m_NextStarlightBurst = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown
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
