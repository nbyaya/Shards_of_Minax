using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a venomous toad corpse")]
    public class VenomousToad : GiantToad
    {
        private DateTime m_NextVenomousBite;
        private DateTime m_NextPoisonousBreath;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VenomousToad()
            : base()
        {
            Name = "a venomous toad";
            Hue = 2443; // Unique hue for the Venomous Toad
            BaseSoundID = 0x26B;
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

            // Define additional stats or changes if needed
            m_AbilitiesInitialized = false; // Initialize flag
        }

        public VenomousToad(Serial serial)
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
                    m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPoisonousBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVenomousBite)
                {
                    PerformVenomousBite();
                }

                if (DateTime.UtcNow >= m_NextPoisonousBreath)
                {
                    PerformPoisonousBreath();
                }
            }
        }

        private void PerformVenomousBite()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int poisonDamage = Utility.RandomMinMax(5, 10);
                target.ApplyPoison(this, Poison.Lethal); // Inflict lethal poison
                target.SendMessage("The Venomous Toad bites you, inflicting a potent toxin!");

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Toad strikes with a venomous bite! *");

                m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Set next bite time
            }
        }

        private void PerformPoisonousBreath()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Toad breathes a cloud of poisonous gas! *");
            FixedEffect(0x373A, 10, 16); // Poison breath effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    int poisonDamage = Utility.RandomMinMax(5, 15);
                    m.ApplyPoison(this, Poison.Lethal); // Apply poison
                    m.Damage(poisonDamage, this);
                    m.SendMessage("The poisonous gas from the Venomous Toad burns you!");
                }
            }

            m_NextPoisonousBreath = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set next breath time
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
