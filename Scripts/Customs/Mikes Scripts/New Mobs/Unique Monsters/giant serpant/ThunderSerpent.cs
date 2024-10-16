using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a thunder serpent corpse")]
    public class ThunderSerpent : GiantSerpent
    {
        private DateTime m_NextElectricBite;
        private DateTime m_NextThunderStrike;
        private DateTime m_NextStormCall;
        private DateTime m_NextLightningBreath;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ThunderSerpent()
            : base()
        {
            Name = "a thunder serpent";
            Hue = 1773; // Thunderstorm blue hue
			BaseSoundID = 219;
			
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

            m_AbilitiesInitialized = false; // Set flag to false initially
        }

        public ThunderSerpent(Serial serial)
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
                    m_NextElectricBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextStormCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60)); // Adjusted for Storm Call
                    m_NextLightningBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 45)); // Adjusted for Lightning Breath
                    m_AbilitiesInitialized = true; // Set flag to true after initialization
                }

                // Electric Bite
                if (DateTime.UtcNow >= m_NextElectricBite)
                {
                    ElectricBite();
                }

                // Thunder Strike
                if (DateTime.UtcNow >= m_NextThunderStrike)
                {
                    ThunderStrike();
                }

                // Storm Call
                if (DateTime.UtcNow >= m_NextStormCall)
                {
                    StormCall();
                }

                // Lightning Breath
                if (DateTime.UtcNow >= m_NextLightningBreath)
                {
                    LightningBreath();
                }

                // Randomly change behavior if below half health
                if (Hits < HitsMax / 2)
                {
                    if (Utility.RandomDouble() < 0.1) // 10% chance
                    {
                        StormCall();
                        LightningBreath();
                    }
                }
            }
        }

        private void ElectricBite()
        {
            if (Combatant != null && !Combatant.Deleted)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Thunder Serpent’s bite shocks you!");
                Combatant.PlaySound(0x1F2); // Electric sound

                // Apply lightning effect
                Combatant.FixedParticles(0x37F4, 1, 30, 9912, EffectLayer.Waist);
                
                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

                // Chance to stun
                if (Utility.RandomDouble() < 0.2) // 20% chance
                {
                    Mobile mobCombatant = Combatant as Mobile; // Cast to Mobile
                    if (mobCombatant != null)
                    {
                        mobCombatant.SendMessage("You are stunned by the electric shock!");
                        mobCombatant.Freeze(TimeSpan.FromSeconds(2));
                    }
                }

                m_NextElectricBite = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        private void ThunderStrike()
        {
            Mobile target = GetRandomMobileInRange(10);

            if (target != null && !target.Deleted)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "Lightning strikes from the sky!");
                target.PlaySound(0x1FE); // Thunder sound

                // Apply lightning effect
                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 10, 30);
                
                int damage = Utility.RandomMinMax(25, 35);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                // Chance to daze
                if (Utility.RandomDouble() < 0.3) // 30% chance
                {
                    target.SendMessage("You are dazed by the lightning strike!");
                    target.Freeze(TimeSpan.FromSeconds(3));
                }

                m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void StormCall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "A storm erupts around the Thunder Serpent!");
            PlaySound(0x65C); // Storm sound

            // Apply storm visual effect
            Effects.SendLocationEffect(Location, Map, 0x36BD, 30, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && m.Alive)
                {
                    m.PlaySound(0x1FD); // Thunder sound
                    m.SendMessage("The storm around the Thunder Serpent confuses you!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }

            m_NextStormCall = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void LightningBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Thunder Serpent breathes a torrent of lightning!");
            PlaySound(0x65C); // Storm sound

            // Apply lightning breath effect
            Effects.SendLocationEffect(Location, Map, 0x373A, 20, 10);
            
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are scorched by the lightning breath!");
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }

            m_NextLightningBreath = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private Mobile GetRandomMobileInRange(int range)
        {
            foreach (Mobile m in GetMobilesInRange(range))
            {
                if (m != this && m.Alive)
                    return m;
            }
            return null;
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

            // Reset the initialization flag to ensure abilities are re-initialized on load
            m_AbilitiesInitialized = false;
        }
    }
}
