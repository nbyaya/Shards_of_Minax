using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a starborn predator corpse")]
    public class StarbornPredator : BaseCreature
    {
        private DateTime m_NextStellarSurge;
        private DateTime m_NextMeteorStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StarbornPredator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a starborn predator";
            Body = 0xD6; // Panther body
            Hue = 2179;  // Cosmic, starry hue
            BaseSoundID = 0x462;

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

        public StarbornPredator(Serial serial)
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
                    m_NextStellarSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMeteorStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextStellarSurge)
                {
                    StellarSurge();
                }

                if (DateTime.UtcNow >= m_NextMeteorStrike)
                {
                    MeteorStrike();
                }
            }
        }

        private void StellarSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashing Stellar Surge! *");
            PlaySound(0x20F); // Cosmic sound
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are struck by a surge of cosmic energy!");
                }
            }

            m_NextStellarSurge = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void MeteorStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Calling down Meteor Strike! *");
            PlaySound(0x20F); // Cosmic sound
            FixedEffect(0x373A, 10, 16);

            Point3D targetLoc = GetSpawnPosition(5);

            if (targetLoc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(targetLoc, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 9925);
                Effects.PlaySound(targetLoc, Map, 0x21F); // Meteor strike sound

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        int damage = Utility.RandomMinMax(50, 70);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        m.SendMessage("A meteor crashes down upon you!");
                    }
                }
            }

            m_NextMeteorStrike = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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
            m_NextStellarSurge = DateTime.UtcNow;
            m_NextMeteorStrike = DateTime.UtcNow;
        }
    }
}
