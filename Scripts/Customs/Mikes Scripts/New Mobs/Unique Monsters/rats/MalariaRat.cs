using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a malaria rat corpse")]
    public class MalariaRat : GiantRat
    {
        private DateTime m_NextMalariaFever;
        private DateTime m_NextMalariaPlague;
        private DateTime m_FeveredRageEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public MalariaRat()
            : base()
        {
            Name = "a malaria rat";
            Hue = 2265; // Unique hue for the Malaria Rat
            this.BaseSoundID = 0xCC;
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

        public MalariaRat(Serial serial)
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
                    // Initialize random cooldowns for abilities
                    Random rand = new Random();
                    m_NextMalariaFever = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMalariaPlague = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_FeveredRageEnd = DateTime.MinValue; // Set a default value
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMalariaFever)
                {
                    ApplyMalariaFever();
                }

                if (DateTime.UtcNow >= m_NextMalariaPlague)
                {
                    ApplyMalariaPlague();
                }

                if (DateTime.UtcNow >= m_FeveredRageEnd)
                {
                    EnterFeveredRage();
                }
            }
        }

        private void ApplyMalariaFever()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(5, 10);
                target.Damage(damage, this);

                // Show message
                target.SendMessage("*You are wracked with fever as the Malaria Rat bites!*");
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Malaria Fever!*");

                // Apply visual effect
                target.FixedEffect(0x376A, 10, 16);

                // Set next fever application
                m_NextMalariaFever = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        private void ApplyMalariaPlague()
        {
            // Affect nearby players with Malaria Plague
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(3, 6);
                    m.Damage(damage, this);
                    m.SendMessage("*You feel a sudden chill and weakness as the Malaria Plague spreads!*");
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Malaria Plague!*");

                    // Apply visual effect
                    m.FixedEffect(0x376A, 10, 16);
                }
            }

            m_NextMalariaPlague = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void EnterFeveredRage()
        {
            // Entering Fevered Rage phase
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Malaria Rat goes into a fevered rage!*");
            PlaySound(0x1F4); // Sound for Rage

            // Increase damage
            this.SetDamage(this.DamageMin + 5, this.DamageMax + 5);

            m_FeveredRageEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
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
