using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a corrosive toad corpse")]
    public class CorrosiveToad : BaseCreature
    {
        private DateTime m_NextAcidicSpray;
        private DateTime m_NextCorrosiveTouch;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CorrosiveToad()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a corrosive toad";
            Body = 80; // Giant toad body
            BaseSoundID = 0x26B;
            Hue = 2463; // Unique hue for the corrosive toad

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

        public CorrosiveToad(Serial serial)
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
                    m_NextAcidicSpray = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCorrosiveTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAcidicSpray)
                {
                    AcidicSpray();
                }

                if (DateTime.UtcNow >= m_NextCorrosiveTouch)
                {
                    CorrosiveTouch();
                }
            }
        }

        private void AcidicSpray()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Corrosive Toad spits out a spray of acid! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are hit by a spray of corrosive acid!");
                    m.Damage(Utility.RandomMinMax(10, 20), this);
                }
            }

            m_NextAcidicSpray = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown period
        }

        private void CorrosiveTouch()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Corrosive Toad's touch burns with corrosive acid! *");
                FixedEffect(0x376A, 10, 16);

                if (target != null && target.Alive)
                {
                    target.SendMessage("You are burned by the Corrosive Toad's corrosive touch!");
                    target.Damage(Utility.RandomMinMax(5, 15), this);
                }

                m_NextCorrosiveTouch = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown period
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
