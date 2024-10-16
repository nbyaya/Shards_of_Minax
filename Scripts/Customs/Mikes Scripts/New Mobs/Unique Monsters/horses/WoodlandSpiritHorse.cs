using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a woodland spirit horse corpse")]
    public class WoodlandSpiritHorse : BaseMount
    {
        private static readonly int[] m_IDs = new int[]
        {
            0xC8, 0x3E9F,
            0xE2, 0x3EA0,
            0xE4, 0x3EA1,
            0xCC, 0x3EA2
        };

        private DateTime m_NextSpiritSurge;
        private DateTime m_NextNaturesEmbrace;
        private DateTime m_NextSpiritHealing;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public WoodlandSpiritHorse()
            : base("a woodland spirit horse", 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            int random = Utility.Random(4);

            Body = m_IDs[random * 2];
            ItemID = m_IDs[random * 2 + 1];
            BaseSoundID = 0xA8;

            Hue = 2082; // Greenish hue

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

            m_AbilitiesInitialized = false;
        }

        public WoodlandSpiritHorse(Serial serial)
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
                    m_NextSpiritSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextNaturesEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextSpiritHealing = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextSpiritSurge)
                {
                    SpiritSurge();
                }

                if (DateTime.UtcNow >= m_NextNaturesEmbrace)
                {
                    NaturesEmbrace();
                }

                if (DateTime.UtcNow >= m_NextSpiritHealing)
                {
                    SpiritHealing();
                }
            }
        }

        private void SpiritSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Woodland spirits surge forth!*");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(10, this);
                    m.SendMessage("You are engulfed by woodland spirits!");
                }
            }

            m_NextSpiritSurge = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset cooldown
        }

        private void NaturesEmbrace()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Nature’s embrace shields the Woodland Spirit Horse!*");
            this.SetResistance(ResistanceType.Physical, 50, 60);

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(() =>
            {
                this.SetResistance(ResistanceType.Physical, 30, 40);
            }));

            m_NextNaturesEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(90); // Reset cooldown
        }

        private void SpiritHealing()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Woodland Spirit Horse heals itself and nearby allies!*");
            this.Hits = Math.Min(this.HitsMax, this.Hits + 30);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature creature && creature != this && creature.Alive)
                {
                    creature.Hits = Math.Min(creature.HitsMax, creature.Hits + 20);
                }
            }

            m_NextSpiritHealing = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextSpiritSurge);
            writer.Write(m_NextNaturesEmbrace);
            writer.Write(m_NextSpiritHealing);
            writer.Write(m_AbilitiesInitialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextSpiritSurge = reader.ReadDateTime();
            m_NextNaturesEmbrace = reader.ReadDateTime();
            m_NextSpiritHealing = reader.ReadDateTime();
            m_AbilitiesInitialized = reader.ReadBool();
        }
    }
}
