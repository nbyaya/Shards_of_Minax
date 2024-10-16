using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a venomous dragon corpse")]
    public class VenomousDragon : BaseCreature
    {
        private DateTime m_NextVenomBreath;
        private DateTime m_NextToxicAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VenomousDragon()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venomous dragon";
            Body = 59; // Dragon body
            Hue = 1473; // Custom hue for venomous appearance
			BaseSoundID = 362;

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

            // Set the abilities to be uninitialized initially
            m_AbilitiesInitialized = false;
        }

        public VenomousDragon(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                    m_NextVenomBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random initial time for VenomBreath
                    m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random initial time for ToxicAura
                    m_AbilitiesInitialized = true; // Set flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVenomBreath)
                {
                    VenomBreath();
                    m_NextVenomBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown after ability use
                }

                if (DateTime.UtcNow >= m_NextToxicAura)
                {
                    ToxicAura();
                    m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown after ability use
                }
            }
        }

        private void VenomBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Dragon breathes poisonous gas! *");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(10, this); // Poison damage
                    m.ApplyPoison(this, Poison.Lethal); // Apply poison debuff
                }
            }
        }

        private void ToxicAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Dragon emits a toxic aura! *");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(5, this); // Aura damage
                    m.ApplyPoison(this, Poison.Lethal); // Apply poison debuff
                }
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (willKill)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Dragon lets out a final toxic roar! *");
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
