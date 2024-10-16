using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a blighted corpse")]
    public class XalRath : BaseCreature
    {
        private DateTime m_NextDecayAura;
        private DateTime m_NextBlightBurst;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public XalRath()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Xal'Rath the Blighted";
            Body = 22; // ElderGazer body
            Hue = 1761; // Unique hue for the Blighted
			BaseSoundID = 377;

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

        public XalRath(Serial serial)
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
                    m_NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30)); // Random start between 10 and 30 seconds
                    m_NextBlightBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 45)); // Random start between 20 and 45 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDecayAura)
                {
                    DecayAura();
                }

                if (DateTime.UtcNow >= m_NextBlightBurst)
                {
                    BlightBurst();
                }
            }
        }

        private void DecayAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A miasma of decay surrounds Xal'Rath! *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You feel your healing abilities weaken in the presence of Xal'Rath.");
                    // Add code to reduce healing effectiveness
                }
            }
            m_NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for DecayAura
        }

        private void BlightBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Xal'Rath releases a burst of toxic blight! *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 9905);
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Damage(Utility.RandomMinMax(20, 30), this);
                    m.SendMessage("You are engulfed in a poisonous cloud!");
                }
            }
            m_NextBlightBurst = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for BlightBurst
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (willKill && from != null && from is PlayerMobile)
            {
                from.SendMessage("You have vanquished Xal'Rath the Blighted. Beware, the land is still tainted.");
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
