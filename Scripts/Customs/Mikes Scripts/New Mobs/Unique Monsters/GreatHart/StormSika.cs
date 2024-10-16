using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Engines;

namespace Server.Mobiles
{
    [CorpseName("a storm sika corpse")]
    public class StormSika : GreatHart
    {
        private DateTime m_NextThunderousStomp;
        private DateTime m_NextLightningDash;
        private DateTime m_NextLightningStrike;
        private DateTime m_NextStormAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormSika()
            : base()
        {
            Name = "a Storm Sika";
            Hue = 1971; // Stormy grey hue
			
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

            // Initialize abilities
            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public StormSika(Serial serial)
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
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
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
                    m_NextThunderousStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLightningDash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextThunderousStomp)
                {
                    ThunderousStomp();
                }

                if (DateTime.UtcNow >= m_NextLightningDash)
                {
                    LightningDash();
                }

                if (DateTime.UtcNow >= m_NextLightningStrike)
                {
                    LightningStrike();
                }

                if (DateTime.UtcNow >= m_NextStormAura)
                {
                    StormAura();
                }

                // Storm Phase: Increase intensity if health is below 50%
                if (Hits < HitsMax * 0.5)
                {
                    IncreaseStormIntensity();
                }
            }
        }

        private void ThunderousStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* The Storm Sika's stomp shakes the earth!*");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are struck by a shockwave!");
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 100, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextThunderousStomp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Thunderous Stomp
        }

        private void LightningDash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* The Storm Sika dashes with the speed of lightning!*");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are scorched by lightning!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 100, 0);
                }
            }

            m_NextLightningDash = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Lightning Dash
        }

        private void LightningStrike()
        {
            Mobile target = GetRandomPlayer();
            if (target != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Lightning strikes from the storm!*");
                Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 16, 3, 0xB00B, 0);

                AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0);
                target.SendMessage("You are struck by a lightning bolt!");
            }

            m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for Lightning Strike
        }

        private void StormAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* A storm aura surrounds the Storm Sika!*");
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are affected by the storm's aura!");
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 100, 0);
                    m.Freeze(TimeSpan.FromSeconds(5)); // Using Freeze instead of Slow
                }
            }

            m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Storm Aura
        }

        private Mobile GetRandomPlayer()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is PlayerMobile && m.Alive)
                    return m;
            }

            return null;
        }

        private void IncreaseStormIntensity()
        {
            // Increase damage and frequency of abilities
            m_NextThunderousStomp = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextLightningDash = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(10);
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

            // Reset abilities initialization
            m_AbilitiesInitialized = false;
        }
    }
}
