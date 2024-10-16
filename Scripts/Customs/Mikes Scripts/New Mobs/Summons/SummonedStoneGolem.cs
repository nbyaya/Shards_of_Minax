using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a golem corpse")]
    public class SummonedStoneGolem : BaseCreature
    {
        private DateTime m_NextRockSlam;
        private DateTime m_NextStoneSkin;
        private DateTime m_NextEarthquake;
        private DateTime m_NextGraniteShield;
        
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedStoneGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "Stone Golem";
            Body = 752; // Golem body
            Hue = 1924; // Unique hue for Stone Golem
			BaseSoundID = 357;

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
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SummonedStoneGolem(Serial serial) : base(serial)
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
                    m_NextRockSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextGraniteShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRockSlam)
                {
                    RockSlam();
                }

                if (DateTime.UtcNow >= m_NextStoneSkin)
                {
                    StoneSkin();
                }

                if (DateTime.UtcNow >= m_NextEarthquake)
                {
                    Earthquake();
                }

                if (DateTime.UtcNow >= m_NextGraniteShield)
                {
                    GraniteShield();
                }
            }
        }

        private void RockSlam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Golem slams the ground with a thunderous crash! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are hit by a shockwave and knocked down!");
                    m.PlaySound(0x65D); // Shockwave sound
                    m.FixedEffect(0x374A, 10, 16); // Shockwave effect
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2)); // Knockdown effect
                }
            }
            m_NextRockSlam = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void StoneSkin()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Golem’s skin hardens into unyielding stone! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You feel a sudden chill as the Golem’s stone skin hardens!");
                    m.PlaySound(0x1E8); // Stone skin sound
                    m.FixedParticles(0x374A, 10, 16, 5012, EffectLayer.Waist); // Stone skin effect
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0); // Apply damage
                }
            }
            m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void Earthquake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground shakes violently as the Golem stomps! *");
            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are stunned by the earthquake!");
                    m.PlaySound(0x65F); // Earthquake sound
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist); // Earthquake effect
                    m.Freeze(TimeSpan.FromSeconds(3)); // Stun effect
                }
            }
            m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void GraniteShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A granite shield forms around the Golem, reflecting incoming blows! *");
            this.VirtualArmor += 20; // Increase armor
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You hit the Golem, and your attack is partially reflected!");
                    m.PlaySound(0x1E4); // Granite shield sound
                    m.FixedParticles(0x373A, 10, 30, 9922, EffectLayer.Waist); // Granite shield effect
                    int reflectDamage = Utility.RandomMinMax(5, 15);
                    AOS.Damage(m, this, reflectDamage, 0, 100, 0, 0, 0); // Reflect damage
                }
            }
            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(RemoveGraniteShield)); // Shield duration
            m_NextGraniteShield = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void RemoveGraniteShield()
        {
            this.VirtualArmor -= 20; // Reset armor
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
