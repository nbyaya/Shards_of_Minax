using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;

namespace Server.Mobiles
{
    [CorpseName("a black widow queen corpse")]
    public class BlackWidowQueen : BaseCreature
    {
        private DateTime m_NextDeadlyKiss;
        private DateTime m_NextCloakOfShadows;
        private DateTime m_NextSummonSpiders;
        private DateTime m_NextWebTrap;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BlackWidowQueen()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Black Widow Queen";
            Body = 28; // GiantSpider body
            Hue = 1796; // Unique hue
            BaseSoundID = 0x388;

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

        public BlackWidowQueen(Serial serial)
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
                    m_NextDeadlyKiss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCloakOfShadows = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonSpiders = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDeadlyKiss)
                {
                    DeadlyKiss();
                }

                if (DateTime.UtcNow >= m_NextCloakOfShadows)
                {
                    CloakOfShadows();
                }

                if (DateTime.UtcNow >= m_NextSummonSpiders)
                {
                    SummonSpiders();
                }

                if (DateTime.UtcNow >= m_NextWebTrap)
                {
                    WebTrap();
                }
            }
        }

        private void DeadlyKiss()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Black Widow Queen delivers a deadly kiss! *");
            Combatant.PlaySound(0x1F6);
            AOS.Damage(Combatant, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);

            // Apply a poison effect if the target is a creature
            if (Combatant is BaseCreature creature)
            {
                creature.Poison = Poison.Lethal; // Apply a lethal poison
            }

            m_NextDeadlyKiss = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void CloakOfShadows()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Black Widow Queen cloaks in shadows, becoming less visible! *");
            Hue = 1153; // Change hue to indicate invisibility
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => Hue = 1109); // Revert hue

            SetResistance(ResistanceType.Physical, 60, 70); // Increase physical resistance
            m_NextCloakOfShadows = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void SummonSpiders()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Black Widow Queen summons smaller spiders! *");
            
            for (int i = 0; i < 5; i++)
            {
                SmallSpiderMite spider = new SmallSpiderMite();
                spider.MoveToWorld(GetSpawnPosition(3), Map);
            }

            m_NextSummonSpiders = DateTime.UtcNow + TimeSpan.FromMinutes(3);
        }

        private void WebTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Black Widow Queen casts a web trap! *");
            
            // Create web trap effect
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are ensnared in a web trap!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Web trap effect
                }
            }

            m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

    public class SmallSpiderMite : BaseCreature
    {
        [Constructable]
        public SmallSpiderMite()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a small spider";
            Body = 28; // GiantSpider body
            Hue = 0x8A; // Default hue for small spiders

            SetStr(30, 50);
            SetDex(40, 60);
            SetInt(20, 40);

            SetHits(30, 50);
            SetMana(0);

            SetDamage(8, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);

            SetSkill(SkillName.Poisoning, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 30.1, 50.0);
            SetSkill(SkillName.Tactics, 30.0);
            SetSkill(SkillName.Wrestling, 40.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 20;
        }

        public SmallSpiderMite(Serial serial)
            : base(serial)
        {
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
        }
    }
}
