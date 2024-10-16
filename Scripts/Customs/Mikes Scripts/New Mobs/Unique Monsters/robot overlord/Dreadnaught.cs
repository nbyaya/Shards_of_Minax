using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a dreadnaught corpse")]
    public class Dreadnaught : BaseCreature
    {
        private DateTime m_NextPowerSlam;
        private DateTime m_NextOverloadPulse;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Dreadnaught()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dreadnaught";
            Body = 0x2F4; // ExodusOverseer body
            Hue = 2295; // Unique hue

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

        public Dreadnaught(Serial serial)
            : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0xFD;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x23B;
        }

        public override int GetHurtSound()
        {
            return 0x140;
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
                    m_NextPowerSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextOverloadPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPowerSlam)
                {
                    PowerSlam();
                }

                if (DateTime.UtcNow >= m_NextOverloadPulse)
                {
                    OverloadPulse();
                }
            }
        }

        private void PowerSlam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dreadnaught performs a ground-shaking slam! *");
            FixedEffect(0x376A, 10, 16);
            PlaySound(0x20E);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(30, this);
                    m.SendMessage("You are stunned by the Dreadnaught's power slam!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            // Reset the cooldown for PowerSlam
            Random rand = new Random();
            m_NextPowerSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
        }

        private void OverloadPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dreadnaught emits a powerful overload pulse! *");
            FixedEffect(0x373A, 10, 16);
            PlaySound(0x20F);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    m.Damage(damage, this);
                    m.Mana -= Utility.RandomMinMax(20, 40);
                    if (m.Mana < 0) m.Mana = 0;
                }
            }

            // Reset the cooldown for OverloadPulse
            Random rand = new Random();
            m_NextOverloadPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
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