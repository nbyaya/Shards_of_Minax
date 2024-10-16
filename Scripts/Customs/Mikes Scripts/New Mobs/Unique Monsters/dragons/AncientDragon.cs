using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ancient dragon corpse")]
    public class AncientDragon : BaseCreature
    {
        private DateTime m_NextAncientBreath;
        private DateTime m_NextDragonsRoar;
        private DateTime m_NextTemporalShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AncientDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ancient dragon";
            Body = 12; // Dragon body
            Hue = 1490; // Unique hue for ancient dragon
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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public AncientDragon(Serial serial)
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
                    // Initialize random intervals
                    Random rand = new Random();
                    m_NextAncientBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDragonsRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextTemporalShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAncientBreath)
                {
                    AncientBreath();
                }

                if (DateTime.UtcNow >= m_NextDragonsRoar)
                {
                    DragonsRoar();
                }

                if (DateTime.UtcNow >= m_NextTemporalShield)
                {
                    TemporalShield();
                }
            }
        }

        private void AncientBreath()
        {
            if (Combatant != null && Combatant.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ancient Dragon exhales a powerful breath! *");
                PlaySound(0x5A5);
                Effects.SendTargetEffect(Combatant, 0x36D4, 20);

                int damage = Utility.RandomMinMax(50, 70);
                Combatant.Damage(damage, this);

                // Cast Combatant to Mobile to use SendMessage and Freeze
                Mobile mobileTarget = Combatant as Mobile;
                if (mobileTarget != null)
                {
                    if (Utility.RandomDouble() < 0.2) // 20% chance to petrify
                    {
                        mobileTarget.SendMessage("You feel yourself turning to stone!");
                        mobileTarget.Freeze(TimeSpan.FromSeconds(5));
                    }
                }

                m_NextAncientBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset with a fixed cooldown
            }
        }

        private void DragonsRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ancient Dragon roars fiercely! *");
            PlaySound(0x5A4);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).AIObject != null)
                {
                    ((BaseCreature)m).SetStr(((BaseCreature)m).Str + 20);
                    ((BaseCreature)m).SetDex(((BaseCreature)m).Dex + 20);
                    ((BaseCreature)m).SetInt(((BaseCreature)m).Int + 20);
                }
            }

            m_NextDragonsRoar = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Fixed cooldown
        }

        private void TemporalShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ancient Dragon surrounds itself with a temporal shield! *");
            PlaySound(0x5A2);

            VirtualArmor += 50; // Temporarily increase armor
            Hits += 100; // Temporarily increase hits

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate()
            {
                VirtualArmor -= 50;
                Hits -= 100;
            }));

            m_NextTemporalShield = DateTime.UtcNow + TimeSpan.FromMinutes(5); // Fixed cooldown
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
            // Reinitialize abilities
            Random rand = new Random();
            m_NextAncientBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
            m_NextDragonsRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
            m_NextTemporalShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
        }
    }
}
