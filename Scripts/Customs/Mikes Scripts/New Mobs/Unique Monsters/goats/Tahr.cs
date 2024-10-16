using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Engines.Craft;

namespace Server.Mobiles
{
    [CorpseName("a tahr corpse")]
    public class Tahr : BaseCreature
    {
        private DateTime m_NextShaggyDefense;
        private DateTime m_NextHornSweep;
        private DateTime m_NextRagingCharge;
        private bool m_IsEnraged;
        private bool m_AbilitiesInitialized; // Flag to check if abilities have been initialized

        [Constructable]
        public Tahr()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a tahr";
            Body = 0xD1; // Using goat body
            Hue = 1797; // Dark reddish-brown hue
			BaseSoundID = 0x99;

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

        public Tahr(Serial serial)
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
                    m_NextShaggyDefense = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHornSweep = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRagingCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (Hits < HitsMax / 2 && !m_IsEnraged)
                {
                    Enrage();
                }

                if (DateTime.UtcNow >= m_NextShaggyDefense)
                {
                    ShaggyDefense();
                }

                if (DateTime.UtcNow >= m_NextHornSweep)
                {
                    HornSweep();
                }

                if (DateTime.UtcNow >= m_NextRagingCharge)
                {
                    RagingCharge();
                }
            }
        }

        private void ShaggyDefense()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tahr's shaggy fur absorbs the brunt of the attack! *");
            PlaySound(0x3F5); // Sound effect for defense

            // Increase physical resistance
            SetResistance(ResistanceType.Physical, 50, 60);

            // Visual effect for defense
            Effects.SendLocationEffect(Location, Map, 0x376A, 10, 16);

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                SetResistance(ResistanceType.Physical, 40, 50); // Revert resistance
            });

            m_NextShaggyDefense = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void HornSweep()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tahr sweeps its horns in a powerful arc! *");
            PlaySound(0x3D3); // Sound effect for horn sweep

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    
                    // Knockback effect
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z);
                    
                    // Chance to inflict a debuff (e.g., slowing down)
                    if (Utility.RandomDouble() < 0.3) // 30% chance
                    {
                        m.SendMessage("You are dazed by the Tahr's sweeping horns!");
                        m.SendLocalizedMessage(1070797); // "You feel dazed and confused!"
                    }
                }
            }

            m_NextHornSweep = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown
        }

        private void RagingCharge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tahr charges forward with great fury! *");
            PlaySound(0x1E0); // Sound effect for charge

            if (Combatant != null)
            {
                // Charge forward
                Point3D chargeLocation = new Point3D(Location.X + (Utility.RandomMinMax(-5, 5)), Location.Y + (Utility.RandomMinMax(-5, 5)), Location.Z);
                MoveToWorld(chargeLocation, Map);

                // Damage and stun effect
                foreach (Mobile m in GetMobilesInRange(2))
                {
                    if (m != this && m.Alive && !m.IsDeadBondedPet)
                    {
                        int damage = Utility.RandomMinMax(20, 30);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        m.Freeze(TimeSpan.FromSeconds(2)); // Stun effect
                    }
                }
            }

            m_NextRagingCharge = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown
        }

        private void Enrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tahr becomes enraged! *");
            PlaySound(0x2F0); // Sound effect for enraging

            // Increase damage output
            SetDamage(15, 20);

            // Additional special behavior or effects can be added here

            m_IsEnraged = true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_IsEnraged);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsEnraged = reader.ReadBool();
            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialization
        }
    }
}
