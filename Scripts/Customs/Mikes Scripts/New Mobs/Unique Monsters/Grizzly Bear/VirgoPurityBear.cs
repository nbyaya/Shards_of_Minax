using System;
using Server;
using Server.Mobiles;
using Server.Network; // Added for MessageType

namespace Server.Mobiles
{
    [CorpseName("a puritybear corpse")]
    public class VirgoPurityBear : BaseCreature
    {
        private DateTime m_NextPurifyingAura;
        private DateTime m_NextPrecisionStrike;
        private DateTime m_NextPurityBeam;
        private DateTime m_NextDefensiveShield;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VirgoPurityBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Virgo PurityBear";
            Body = 212; // GrizzlyBear body
            Hue = 1992; // Pure white hue
			BaseSoundID = 0xA3;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public VirgoPurityBear(Serial serial)
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
                    m_NextPurifyingAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPrecisionStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextPurityBeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextDefensiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPurifyingAura)
                {
                    UsePurifyingAura();
                }

                if (DateTime.UtcNow >= m_NextPrecisionStrike)
                {
                    UsePrecisionStrike();
                }

                if (DateTime.UtcNow >= m_NextPurityBeam)
                {
                    UsePurityBeam();
                }

                if (DateTime.UtcNow >= m_NextDefensiveShield)
                {
                    UseDefensiveShield();
                }
            }
        }

        private void UsePurifyingAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Virgo PurityBear radiates a purifying aura! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature bc && bc.Alive)
                {
                    bc.Hits += 15; // Increased healing effect
                    bc.CurePoison(this); // Fixed to pass the Mobile instance
                    bc.SendMessage("You feel rejuvenated by the PurityBear's aura!");
                }
            }

            m_NextPurifyingAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void UsePrecisionStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Virgo PurityBear delivers a devastating precision strike! *");

            if (Combatant != null)
            {
                Mobile combatantMobile = Combatant as Mobile; // Cast to Mobile
                if (combatantMobile != null)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(Combatant, this, damage, 0, 50, 0, 0, 0);
                    combatantMobile.SendMessage("You are struck with incredible precision!");
                }
            }

            m_NextPrecisionStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void UsePurityBeam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Virgo PurityBear channels a Purity Beam! *");

            if (Combatant != null)
            {
                Mobile combatantMobile = Combatant as Mobile; // Cast to Mobile
                if (combatantMobile != null)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
                    combatantMobile.SendMessage("A blinding beam of purity hits you!");

                    // Handle debuff logic if applicable or remove this part if not supported
                }
            }

            m_NextPurityBeam = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void UseDefensiveShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Virgo PurityBear surrounds itself with a protective shield! *");

            // Increase Virtual Armor temporarily
            VirtualArmor += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(() => {
                VirtualArmor -= 20; // Reset armor after 10 seconds
            }));

            m_NextDefensiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Virgo PurityBear strikes with a holy fury! *");
                int damage = Utility.RandomMinMax(10, 15);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);
            }
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
