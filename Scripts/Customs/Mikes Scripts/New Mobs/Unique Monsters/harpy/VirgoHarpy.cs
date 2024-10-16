using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Virgo Harpy corpse")]
    public class VirgoHarpy : Harpy
    {
        private DateTime m_NextPurification;
        private DateTime m_NextPrecisionStrike;
        private DateTime m_NextCelestialStorm;
        private DateTime m_NextShieldOfLight;
        private bool m_AbilitiesActivated; // Flag to track if abilities have been initialized with random times

        [Constructable]
        public VirgoHarpy()
            : base()
        {
            Name = "Virgo Harpy";
            Body = 30; // Harpy body
            Hue = 2066; // Elegant, pristine white feathers
			BaseSoundID = 402; // Harpy sound

            // Set custom stats
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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public VirgoHarpy(Serial serial)
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
                if (!m_AbilitiesActivated)
                {
                    // Initialize random intervals for abilities
                    Random rand = new Random();
                    m_NextPurification = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextPrecisionStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCelestialStorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 80));
                    m_NextShieldOfLight = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextPurification)
                {
                    Purification();
                }

                if (DateTime.UtcNow >= m_NextPrecisionStrike)
                {
                    PrecisionStrike();
                }

                if (DateTime.UtcNow >= m_NextCelestialStorm)
                {
                    CelestialStorm();
                }

                if (DateTime.UtcNow >= m_NextShieldOfLight)
                {
                    ShieldOfLight();
                }
            }
        }

        private void Purification()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Virgo Harpy emits a purifying light! *");
            FixedEffect(0x376A, 10, 16); // Purification effect

            // Heal and remove harmful effects from allies within range
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature creature && !creature.Deleted && creature.Alive)
                {
                    creature.Heal(30); // Example healing value
                    m.SendMessage("You feel a calming presence and your wounds heal.");
                }
            }

            // Temporary buff to resistances
            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 40, 50);
            this.SetResistance(ResistanceType.Cold, 40, 50);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                // Revert resistances to normal after 30 seconds
                this.SetResistance(ResistanceType.Physical, 40, 50);
                this.SetResistance(ResistanceType.Fire, 30, 40);
                this.SetResistance(ResistanceType.Cold, 30, 40);
                this.SetResistance(ResistanceType.Poison, 40, 50);
                this.SetResistance(ResistanceType.Energy, 30, 40);
            });

            m_NextPurification = DateTime.UtcNow + TimeSpan.FromSeconds(120); // Set interval based on random time
        }

        private void PrecisionStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Virgo Harpy performs a precision strike! *");
            FixedEffect(0x376A, 10, 16); // Precision strike effect

            if (Combatant != null)
            {
                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

                if (Utility.RandomDouble() < 0.25) // 25% chance to disarm
                {
                    Mobile combatantMobile = Combatant as Mobile;
                    if (combatantMobile != null)
                    {
                        combatantMobile.SendMessage("You have been disarmed by the Virgo Harpy!");
                        combatantMobile.SendMessage("You feel your weapon slipping from your grasp!");
                    }
                }
            }

            m_NextPrecisionStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set interval based on random time
        }

        private void CelestialStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Virgo Harpy summons a celestial storm! *");
            FixedEffect(0x376A, 10, 16); // Celestial storm effect

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are caught in the Virgo Harpy's celestial storm!");
                }
            }

            m_NextCelestialStorm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set interval based on random time
        }

        private void ShieldOfLight()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Virgo Harpy shields itself with a barrier of light! *");
            FixedEffect(0x376A, 10, 16); // Shield of light effect

            // Shield effect: temporarily reduce damage taken
            this.VirtualArmor += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                // Revert VirtualArmor to normal after 30 seconds
                this.VirtualArmor -= 20;
            });

            m_NextShieldOfLight = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set interval based on random time
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

            m_NextPurification = DateTime.UtcNow;
            m_NextPrecisionStrike = DateTime.UtcNow;
            m_NextCelestialStorm = DateTime.UtcNow;
            m_NextShieldOfLight = DateTime.UtcNow;
            m_AbilitiesActivated = false; // Reset flag
        }
    }
}
