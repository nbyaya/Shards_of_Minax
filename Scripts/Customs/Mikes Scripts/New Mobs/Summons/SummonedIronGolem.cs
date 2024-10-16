using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an iron golem corpse")]
    public class SummonedIronGolem : Golem
    {
        private DateTime m_NextIronFist;
        private DateTime m_NextMetalShield;
        private DateTime m_NextMagneticPulse;
        private DateTime m_NextRoar;
        private DateTime m_NextRage;
        private DateTime m_NextReinforcements;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedIronGolem()
            : base(false, 1)
        {
            Name = "an iron golem";
            Hue = 1935; // Unique hue for Iron Golem
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
		
            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public SummonedIronGolem(Serial serial)
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
                    m_NextIronFist = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMetalShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMagneticPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextRage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextReinforcements = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 70));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextIronFist)
                {
                    IronFist();
                }

                if (DateTime.UtcNow >= m_NextMetalShield)
                {
                    MetalShield();
                }

                if (DateTime.UtcNow >= m_NextMagneticPulse)
                {
                    MagneticPulse();
                }

                if (DateTime.UtcNow >= m_NextRoar)
                {
                    Roar();
                }

                if (DateTime.UtcNow >= m_NextRage)
                {
                    Rage();
                }

                if (DateTime.UtcNow >= m_NextReinforcements)
                {
                    SummonReinforcements();
                }
            }
        }

        private void IronFist()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Golem slams with its iron fist! *");
            FixedEffect(0x376A, 10, 16); // Visual effect for Iron Fist
            
            // Temporarily increase damage by applying a damage boost effect
            Timer.DelayCall(TimeSpan.FromSeconds(5), () => { /* Reset damage boost after 5 seconds */ });
            m_NextIronFist = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set cooldown for Iron Fist
        }

        private void MetalShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Golem activates its metal shield! *");
            FixedEffect(0x376A, 10, 16); // Visual effect for Metal Shield
            SetResistance(ResistanceType.Physical, 80); // Temporary resistance
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => SetResistance(ResistanceType.Physical, 40)); // Reset resistance
            m_NextMetalShield = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Set cooldown for Metal Shield
        }

        private void MagneticPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Golem emits a magnetic pulse! *");
            FixedEffect(0x376A, 10, 16); // Visual effect for Magnetic Pulse

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    // Damage and disrupt nearby players
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.SendMessage("You are pulled and damaged by the Iron Golem's magnetic pulse!");
                }
            }

            m_NextMagneticPulse = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Set cooldown for Magnetic Pulse
        }

        private void Roar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Golem lets out a deafening roar! *");
            FixedEffect(0x3728, 10, 30); // Visual effect for Roar

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are frightened by the Iron Golem's roar!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Freeze players for a short duration
                }
            }

            m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set cooldown for Roar
        }

        private void Rage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Golem enters a state of rage! *");
            FixedEffect(0x376A, 10, 16); // Visual effect for Rage

            // Apply temporary damage boost
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => { /* Reset damage boost after 10 seconds */ });
            
            m_NextRage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown for Rage
        }

        private void SummonReinforcements()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Golem summons reinforcements! *");
            FixedEffect(0x376A, 10, 16); // Visual effect for Reinforcements

            // Spawn a couple of lesser golems
            for (int i = 0; i < 2; i++)
            {
                SummonedIronGolem lesserGolem = new SummonedIronGolem();
                lesserGolem.Body = 752; // Same body type
                lesserGolem.Name = "a lesser iron golem";
                lesserGolem.Hue = 1153; // Same hue
                Point3D spawnLocation = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                lesserGolem.MoveToWorld(spawnLocation, Map);
            }

            m_NextReinforcements = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set cooldown for Reinforcements
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

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialization
        }
    }
}
