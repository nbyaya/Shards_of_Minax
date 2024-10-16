using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a sandstorm elemental corpse")]
    public class SandstormElemental : BaseCreature
    {
        private DateTime m_NextSandstorm;
        private DateTime m_NextSandBlast;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SandstormElemental()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a sandstorm elemental";
            Body = 14; // Sand Elemental Body
            BaseSoundID = 268;
            Hue = 1494; // Mud color

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

        public SandstormElemental(Serial serial)
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

        public override double DispelDifficulty
        {
            get { return 120.0; }
        }

        public override double DispelFocus
        {
            get { return 50.0; }
        }

        public override bool BleedImmune
        {
            get { return true; }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20)); // Random start between 5 and 20 seconds
                    m_NextSandBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30)); // Random start between 10 and 30 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSandstorm)
                {
                    CastSandstorm();
                }

                if (DateTime.UtcNow >= m_NextSandBlast)
                {
                    SandBlast();
                }
            }
        }

        private void CastSandstorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A sandstorm swirls around the Sandstorm Elemental! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are caught in the sandstorm! Your vision is obscured.");
                    m.SendMessage(-1, "You have a 40% reduced accuracy for 10 seconds.");

                    Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(() =>
                    {
                        if (m != null && !m.Deleted)
                        {
                            m.SendMessage("The sandstorm's effect wears off.");
                        }
                    }));
                }
            }

            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown period
        }

        private void SandBlast()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.Damage(10, this);
                    target.SendMessage("A blast of sand blinds you for a few seconds.");
                    target.FixedParticles(0x36D4, 9, 32, 0x2588, 0, 0, EffectLayer.Head);
                    target.SendMessage("You are blinded!");

                    Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(() =>
                    {
                        if (target != null && !target.Deleted)
                        {
                            target.SendMessage("The blindness wears off.");
                        }
                    }));
                }
            }

            m_NextSandBlast = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown period
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
