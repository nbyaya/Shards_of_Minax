using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frostbite wolf corpse")]
    public class FrostbiteWolf : BaseCreature
    {
        private DateTime m_NextIceBreath;
        private DateTime m_NextFrostbite;
        private DateTime m_NextIceShield;
        private bool m_IceShieldActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostbiteWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frostbite wolf";
            Body = 23;
            BaseSoundID = 0xE5;
            Hue = 2595; // Ice blue hue

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

        public FrostbiteWolf(Serial serial)
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
                    m_NextIceBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostbite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextIceShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextIceBreath)
                {
                    IceBreath();
                }

                if (DateTime.UtcNow >= m_NextFrostbite)
                {
                    Frostbite();
                }

                if (DateTime.UtcNow >= m_NextIceShield && !m_IceShieldActive)
                {
                    IceShield();
                }
            }
        }

        private void IceBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frostbite Wolf exhales a chilling frost breath! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && InFront(m))
                {
                    m.Damage(10, this);
                    m.SendMessage("You are hit by the freezing breath of the Frostbite Wolf!");
                    // Apply a debuff to slow the target
                    m.SendMessage("You feel the frost slowing your movements!");
                    // You can use other methods here to slow the target if needed
                }
            }

            m_NextIceBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void Frostbite()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                if (Utility.RandomDouble() < 0.3) // 30% chance to apply Frostbite
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frostbite Wolf bites with icy fury! *");
                    target.SendMessage("You feel a chilling frostbite that weakens you!");
                    // Apply a debuff here to reduce attack speed and damage
                }
            }

            m_NextFrostbite = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void IceShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frostbite Wolf envelops itself in an icy shield! *");
            FixedEffect(0x376A, 10, 16);

            m_IceShieldActive = true;
            // Logic to apply a shield effect here
            // This could include reducing damage taken or absorbing cold damage

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(DeactivateIceShield));
            m_NextIceShield = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void DeactivateIceShield()
        {
            m_IceShieldActive = false;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The icy shield around the Frostbite Wolf dissipates. *");
        }

        private bool InFront(Mobile m)
        {
            // Simple check to determine if the target is in front of the Frostbite Wolf
            // Adjust this method as needed to fit your needs
            return (m.X >= X - 5 && m.X <= X + 5 && m.Y >= Y - 5 && m.Y <= Y + 5);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_IceShieldActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_IceShieldActive = reader.ReadBool();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
