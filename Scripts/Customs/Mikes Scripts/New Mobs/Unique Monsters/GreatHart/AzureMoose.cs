using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an azure moose corpse")]
    public class AzureMoose : BaseCreature
    {
        private DateTime m_NextIcyAntlers;
        private DateTime m_NextGlacialStomp;
        private DateTime m_NextFrostBreath;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public AzureMoose()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an azure moose";
            Body = 0xEA; // GreatHart body
            Hue = 1991; // Azure hue

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

        public AzureMoose(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextIcyAntlers = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextGlacialStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextIcyAntlers)
                {
                    IcyAntlers();
                }

                if (DateTime.UtcNow >= m_NextGlacialStomp)
                {
                    GlacialStomp();
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }
            }
        }

        private void IcyAntlers()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Azure Moose's antlers freeze you in place! *");
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are frozen by the Azure Moose's icy antlers!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                }
            }
            m_NextIcyAntlers = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void GlacialStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Azure Moose stomps the ground, creating icy spikes! *");
            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    m.SendMessage("Sharp ice spikes pierce you!");
                }
            }
            m_NextGlacialStomp = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void FrostBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Azure Moose exhales a freezing breath! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are hit by the Azure Moose's freezing breath!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.PlaySound(0x2D5); // Frost breath sound
                }
            }
            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
