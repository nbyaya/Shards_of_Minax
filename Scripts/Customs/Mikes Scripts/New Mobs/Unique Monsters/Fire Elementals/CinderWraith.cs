using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cinder wraith corpse")]
    public class CinderWraith : BaseCreature
    {
        private DateTime m_NextAshCloud;
        private DateTime m_NextFlareBurst;
        private DateTime m_NextEmberWraith;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CinderWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a cinder wraith";
            this.Body = 15; // Fire Elemental body
            this.Hue = 1664; // Unique hue for the wraith
			BaseSoundID = 838;

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

        public CinderWraith(Serial serial)
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
                    m_NextAshCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFlareBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextEmberWraith = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 2));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAshCloud)
                {
                    CreateAshCloud();
                }

                if (DateTime.UtcNow >= m_NextFlareBurst)
                {
                    FlareBurst();
                }

                if (DateTime.UtcNow >= m_NextEmberWraith)
                {
                    SummonEmberWraiths();
                }
            }
        }

        private void CreateAshCloud()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are enveloped in a cloud of ash and embers!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }

            m_NextAshCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void FlareBurst()
        {
            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A burst of fiery energy blinds and burns you!");
                    m.Damage(Utility.RandomMinMax(10, 15), this);
                    m.SendMessage("You are blinded by the flare!");
                }
            }

            m_NextFlareBurst = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void SummonEmberWraiths()
        {
            for (int i = 0; i < 2; i++)
            {
                Point3D loc = GetSpawnPosition(2);
                if (loc != Point3D.Zero)
                {
                    EmberWraith emberWraith = new EmberWraith();
                    emberWraith.MoveToWorld(loc, Map);
                    emberWraith.Combatant = Combatant;
                }
            }

            m_NextEmberWraith = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

    public class EmberWraith : BaseCreature
    {
        [Constructable]
        public EmberWraith()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ember wraith";
            Body = 15; // Fire Elemental body
            Hue = 1151; // Slightly different hue

            SetStr(50, 75);
            SetDex(50, 75);
            SetInt(50, 75);

            SetHits(30, 45);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Fire, 100);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 0);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 30.1, 50.0);
            SetSkill(SkillName.Magery, 30.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 60.0);
            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 60.0);

            VirtualArmor = 20;
        }

        public EmberWraith(Serial serial)
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
