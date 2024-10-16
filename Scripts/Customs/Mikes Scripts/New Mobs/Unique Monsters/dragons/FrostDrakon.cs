using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frost drakon corpse")]
    public class FrostDrakon : BaseCreature
    {
        private static readonly string[] AbilityMessages = new string[]
        {
            "The Frost Drakon breathes a chilling frost!",
            "An ice barrier surrounds the Frost Drakon!",
            "Glacial shards pierce through the air!"
        };

        private DateTime m_NextFrostBreath;
        private DateTime m_NextIceBarrier;
        private DateTime m_NextGlacialWards;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostDrakon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frost drakon";
            Body = 59; // Standard dragon body
            Hue = 1481; // Icy hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FrostDrakon(Serial serial)
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
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextIceBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGlacialWards = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }

                if (DateTime.UtcNow >= m_NextIceBarrier)
                {
                    IceBarrier();
                }

                if (DateTime.UtcNow >= m_NextGlacialWards)
                {
                    GlacialWards();
                }
            }
        }

        private void FrostBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, AbilityMessages[0]);
            Effects.SendTargetEffect(this, 0x3F1C, 16); // Frost breath effect
            PlaySound(0x227); // Frost breath sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(30, this); // Adjust damage as needed
                    m.SendMessage("You are hit by a blast of icy breath!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Slow down movement
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset with fixed cooldown
        }

        private void IceBarrier()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, AbilityMessages[1]);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 1150, 0, 5022, 0); // Ice barrier effect
            PlaySound(0x1F2); // Barrier sound

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate 
            {
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("The ice barrier fades away.");
                    }
                }
            }));

            m_NextIceBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset with fixed cooldown
        }

        private void GlacialWards()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, AbilityMessages[2]);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 1150, 0, 5022, 0); // Glacial wards effect
            PlaySound(0x1F3); // Shard sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(10, this); // Adjust damage as needed
                    m.SendMessage("You are struck by icy shards!");
                    m.Freeze(TimeSpan.FromSeconds(1)); // Freeze effect
                }
            }

            m_NextGlacialWards = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Reset with fixed cooldown
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
