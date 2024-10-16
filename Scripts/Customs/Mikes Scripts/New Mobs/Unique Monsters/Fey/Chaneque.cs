using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a chaneque corpse")]
    public class Chaneque : BaseCreature
    {
        private DateTime m_NextForestStealth;
        private DateTime m_NextTrapSet;
        private DateTime m_NextEcho;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Chaneque()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a chaneque";
            Body = 723; // GreenGoblin body
            BaseSoundID = 0x600;
            Hue = 1591; // Forest green hue

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

        public Chaneque(Serial serial)
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

        public override int GetAngerSound() { return 0x600; }
        public override int GetIdleSound() { return 0x600; }
        public override int GetAttackSound() { return 0x5FD; }
        public override int GetHurtSound() { return 0x5FF; }
        public override int GetDeathSound() { return 0x5FE; }

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 1; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextForestStealth = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTrapSet = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextEcho = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextForestStealth)
                {
                    ForestStealth();
                }

                if (DateTime.UtcNow >= m_NextTrapSet)
                {
                    TrapSet();
                }

                if (DateTime.UtcNow >= m_NextEcho)
                {
                    Echo();
                }
            }
        }

        private void ForestStealth()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The Chaneque blends into the forest *");
            Hidden = true;
            Frozen = true;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                Hidden = false;
                Frozen = false;
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The Chaneque reappears *");
            });

            m_NextForestStealth = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for ForestStealth
        }

        private void TrapSet()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The Chaneque sets a magical trap *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You've been caught in the Chaneque's trap!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 100, 0, 0, 0, 0);
                }
            }

            m_NextTrapSet = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for TrapSet
        }

        private void Echo()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The Chaneque creates echoing sounds *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("Echoing sounds disorient you!");
                    m.Animate(32, 5, 1, true, false, 0);
                }
            }

            m_NextEcho = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Echo
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
