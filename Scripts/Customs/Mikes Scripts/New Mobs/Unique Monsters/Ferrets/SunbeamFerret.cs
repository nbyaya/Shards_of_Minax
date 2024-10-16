using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a sunbeam ferret corpse")]
    public class SunbeamFerret : BaseCreature
    {
        private DateTime m_NextSolarFlare;
        private DateTime m_NextSunlitHeal;
        private DateTime m_NextRadiantShield;
        private DateTime m_NextSolarBurst;

        private bool m_HasRadiantShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SunbeamFerret()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a sunbeam ferret";
            Body = 0x117; // Ferret body
            Hue = 1567; // Golden hue
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

        public SunbeamFerret(Serial serial)
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
                    m_NextSolarFlare = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSunlitHeal = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextRadiantShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_NextSolarBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSolarFlare)
                {
                    SolarFlare();
                }

                if (DateTime.UtcNow >= m_NextSunlitHeal)
                {
                    SunlitHeal();
                }

                if (DateTime.UtcNow >= m_NextRadiantShield)
                {
                    RadiantShield();
                }

                if (DateTime.UtcNow >= m_NextSolarBurst)
                {
                    SolarBurst();
                }
            }
        }

        private void SolarFlare()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Solar Flare! *");
            FixedEffect(0x37B9, 10, 16); // Light ray effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are blinded by the burst of sunlight!");
                    m.FixedEffect(0x376A, 10, 16); // Blinding effect
                    m.Damage(20, this); // Damage
                    if (Utility.RandomDouble() < 0.25) // 25% chance to stun
                    {
                        m.Freeze(TimeSpan.FromSeconds(2));
                        m.SendMessage("The sunlight stuns you!");
                    }
                }
            }

            m_NextSolarFlare = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for SolarFlare
        }

        private void SunlitHeal()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sunlit Heal! *");
            FixedEffect(0x376A, 10, 16); // Healing effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature creature && creature.Alive)
                {
                    creature.Hits = Math.Min(creature.HitsMax, creature.Hits + 30); // Heal
                    creature.SendMessage("You feel revitalized by the sun's power!");

                    // Optional: Apply a simple effect or message instead of a custom buff
                    if (m != this && Utility.RandomDouble() < 0.5)
                    {
                        creature.SendMessage("You are blessed with a protective aura!");
                        creature.FixedEffect(0x37B9, 10, 16); // Buff effect

                        // Increase virtual armor directly
                        creature.VirtualArmor += 10; // Temporary defense boost

                        Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                        {
                            creature.VirtualArmor -= 10; // Revert defense boost
                        });
                    }
                }
            }

            m_NextSunlitHeal = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for SunlitHeal
        }

        private void RadiantShield()
        {
            if (m_HasRadiantShield)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Radiant Shield Activated! *");
            FixedEffect(0x373A, 10, 16); // Shield effect

            m_HasRadiantShield = true;
            this.VirtualArmor += 30; // Increase defense

            Timer.DelayCall(TimeSpan.FromSeconds(20), () => 
            {
                m_HasRadiantShield = false;
                this.VirtualArmor -= 30; // Revert defense
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Radiant Shield Deactivated! *");
            });

            m_NextRadiantShield = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for RadiantShield
        }

        private void SolarBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Solar Burst! *");
            FixedEffect(0x37B9, 10, 16); // Large burst effect

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are caught in a powerful burst of sunlight!");
                    m.Damage(30, this); // High damage
                }
            }

            m_NextSolarBurst = DateTime.UtcNow + TimeSpan.FromMinutes(3); // Cooldown for SolarBurst
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
