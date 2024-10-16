using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a spectral warden corpse")]
    public class SpectralWarden : BaseCreature
    {
        private DateTime m_NextPhantomGuard;
        private DateTime m_NextWraithStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SpectralWarden()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spectral warden";
            Body = 57; // BoneKnight body
            Hue = 2366; // Ghostly blue hue
            BaseSoundID = 451;

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

        public SpectralWarden(Serial serial)
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
                    m_NextPhantomGuard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWraithStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPhantomGuard)
                {
                    PhantomGuard();
                }

                if (DateTime.UtcNow >= m_NextWraithStrike)
                {
                    WraithStrike();
                }
            }
        }

        private void PhantomGuard()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summoning ghostly allies! *");

            // Adjust the SendLocationParticles method according to the correct signature
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x374A, 10, 30, 0x21, 0);

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
                GhostlyAlly ally = new GhostlyAlly();
                ally.MoveToWorld(loc, Map);
            }

            m_NextPhantomGuard = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void WraithStrike()
        {
            if (Combatant != null && Combatant.Alive)
            {
                int damage = Utility.RandomMinMax(10, 20);
                Combatant.Damage(damage, this);
                Hits = Math.Min(Hits + (damage / 2), HitsMax); // Heal the Spectral Warden

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Wraith Strike! *");
                Combatant.PlaySound(0x1F1);
                Combatant.FixedEffect(0x376A, 10, 16);
                
                m_NextWraithStrike = DateTime.UtcNow + TimeSpan.FromSeconds(15);
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

    public class GhostlyAlly : BaseCreature
    {
        public GhostlyAlly()
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Body = 0x3B2; // Ghostly body
            Hue = 1153; // Ghostly blue hue
            Name = "a ghostly ally";

            SetStr(50);
            SetDex(50);
            SetInt(50);

            SetHits(40);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            VirtualArmor = 20;
        }

        public GhostlyAlly(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
