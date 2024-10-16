using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a plague lich corpse")]
    public class PlagueLich : Lich
    {
        private DateTime m_NextPlagueWave;
        private DateTime m_NextContagion;
        private DateTime m_NextVirulentTouch;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PlagueLich()
            : base()
        {
            Name = "a plague lich";
            Hue = 2137; // Dark, greenish hue
			BaseSoundID = 0x3E9;
			
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

            // Initialize the random intervals in the constructor
            m_AbilitiesInitialized = false;
        }

        public PlagueLich(Serial serial)
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
                // Initialize random intervals if not already done
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPlagueWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120)); // Random between 30s and 2m
                    m_NextContagion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90)); // Random between 20s and 1.5m
                    m_NextVirulentTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90)); // Random between 20s and 1.5m
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPlagueWave)
                {
                    PlagueWave();
                }

                if (DateTime.UtcNow >= m_NextContagion)
                {
                    Contagion();
                }

                if (DateTime.UtcNow >= m_NextVirulentTouch)
                {
                    VirulentTouch();
                }
            }
        }

        private void PlagueWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Plague Lich releases a wave of disease! *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2023);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed in a wave of disease!");
                    m.ApplyPoison(this, Poison.Lethal); // Apply poison
                    m.Damage(10, this); // Adjust damage as needed
                }
            }

            // Reset the next time for this ability with a new random interval
            Random rand = new Random();
            m_NextPlagueWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(120, 300)); // Random between 2m and 5m
        }

        private void Contagion()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Player)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Plague Lich infects you with a contagious disease! *");
                Effects.SendTargetParticles(target, 0x374A, 10, 30, 0x8E2, 0, 0, 0, 0);
                target.SendMessage("You feel the disease spreading within you!");

                foreach (Mobile m in GetMobilesInRange(2))
                {
                    if (m != this && m != target && m.Player)
                    {
                        m.SendMessage("You feel a shiver as you catch the disease from another!");
                        m.ApplyPoison(this, Poison.Lethal); // Apply poison
                    }
                }

                // Reset the next time for this ability with a new random interval
                Random rand = new Random();
                m_NextContagion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 180)); // Random between 1m and 3m
            }
        }

        private void VirulentTouch()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Player)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Plague Lich touches you with its virulent hand! *");
                Effects.SendTargetParticles(target, 0x374A, 10, 30, 0x8E2, 0, 0, 0, 0);
                target.SendMessage("You feel a debilitating disease taking hold!");

                target.ApplyPoison(this, Poison.Lethal); // Apply poison
                target.Damage(5, this); // Adjust damage as needed

                // Reset the next time for this ability with a new random interval
                Random rand = new Random();
                m_NextVirulentTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 180)); // Random between 1m and 3m
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

            // Reset the initialization flag and random intervals on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
