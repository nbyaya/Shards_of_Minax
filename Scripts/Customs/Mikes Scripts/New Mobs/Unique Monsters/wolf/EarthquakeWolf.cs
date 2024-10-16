using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an earthquake wolf corpse")]
    public class EarthquakeWolf : BaseCreature
    {
        private DateTime m_NextQuakeStomp;
        private DateTime m_NextRockyHide;
        private DateTime m_NextTremorAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EarthquakeWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an earthquake wolf";
            Body = 23; // DireWolf body
            Hue = 2636; // Unique hue
			BaseSoundID = 0xE5;
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

        public EarthquakeWolf(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextQuakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRockyHide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTremorAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextQuakeStomp)
                {
                    QuakeStomp();
                }

                if (DateTime.UtcNow >= m_NextRockyHide)
                {
                    RockyHide();
                }

                if (DateTime.UtcNow >= m_NextTremorAura)
                {
                    TremorAura();
                }
            }
        }

        private void QuakeStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Earthquake Wolf stomps the ground, causing a quake! *");
            PlaySound(0x233);
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.Damage(10, this);
                    m.SendMessage("You are knocked down by the earthquake!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextQuakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void RockyHide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Earthquake Wolf's hide hardens like rock! *");
            PlaySound(0x1F1);
            FixedEffect(0x376A, 10, 16);

            SetResistance(ResistanceType.Physical, 80, 90);
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                SetResistance(ResistanceType.Physical, 60, 70);
            });

            m_NextRockyHide = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void TremorAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Earthquake Wolf emits a powerful tremor! *");
            PlaySound(0x25A);
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("The tremor aura weakens you!");
                    m.Damage(5, this);
                    m.Dex -= 5;
                    m.Str -= 5;
                }
            }

            m_NextTremorAura = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
