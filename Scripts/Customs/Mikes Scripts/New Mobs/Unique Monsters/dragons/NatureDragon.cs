using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a nature dragon corpse")]
    public class NatureDragon : BaseCreature
    {
        private DateTime m_NextBreathAttack;
        private DateTime m_NextForestEmbrace;
        private DateTime m_NextSummonAllies;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public NatureDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a nature dragon";
            Body = 12; // Dragon body
            Hue = 1488; // Unique greenish hue
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

        public NatureDragon(Serial serial)
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

        public override int Meat => 19;

        public override int Hides => 20;

        public override HideType HideType => HideType.Barbed;

        public override int Scales => 7;

        public override ScaleType ScaleType => ScaleType.Green;

        public override FoodType FavoriteFood => FoodType.Meat;

        public override bool CanFly => true;


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextBreathAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextForestEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBreathAttack)
                {
                    NatureBreath();
                }

                if (DateTime.UtcNow >= m_NextForestEmbrace)
                {
                    ForestEmbrace();
                }

                if (DateTime.UtcNow >= m_NextSummonAllies)
                {
                    SummonAllies();
                }
            }
        }

        private void NatureBreath()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
                    target.Damage(20, this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nature's Breath scorches you with a wave of nature energy! *");

                    // Summon vines to entangle enemies (using a dummy effect here)
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vines spring from the ground, entangling you! *");

                    m_NextBreathAttack = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                }
            }
        }

        private void ForestEmbrace()
        {
            // Heal over time and reduce incoming damage
            this.Hits += 20;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The forest surrounds you, healing your wounds and reducing the damage you take! *");

            m_NextForestEmbrace = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void SummonAllies()
        {
            Point3D loc = GetSpawnPosition(10);

            if (loc != Point3D.Zero)
            {
                // Summon wolves or treants (assuming they are predefined elsewhere)
                Wolf wolf = new Wolf();
                wolf.MoveToWorld(loc, Map);

                Treant treant = new Treant();
                treant.MoveToWorld(loc, Map);

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Forest creatures rise from the earth to aid you in battle! *");

                m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
            m_NextBreathAttack = DateTime.UtcNow;
            m_NextForestEmbrace = DateTime.UtcNow;
            m_NextSummonAllies = DateTime.UtcNow;
        }
    }

    public class Wolf : BaseCreature
    {
        [Constructable]
        public Wolf() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wolf";
            Body = 225;
            Hue = 1150; // Grayish hue

            SetStr(66, 80);
            SetDex(66, 85);
            SetInt(26, 40);

            SetHits(40, 50);

            SetDamage(8, 12);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 30.0);
            SetSkill(SkillName.Magery, 30.0);
            SetSkill(SkillName.MagicResist, 30.0);
            SetSkill(SkillName.Tactics, 30.0);
            SetSkill(SkillName.Wrestling, 30.0);

            Fame = 300;
            Karma = 0;
        }

        public Wolf(Serial serial) : base(serial) { }

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

    public class Treant : BaseCreature
    {
        [Constructable]
        public Treant() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a treant";
            Body = 22; // Treant body
            Hue = 0x3B; // Greenish hue

            SetStr(300, 350);
            SetDex(50, 75);
            SetInt(50, 75);

            SetHits(200, 250);

            SetDamage(15, 20);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 50.0);
            SetSkill(SkillName.Magery, 50.0);
            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 50.0);

            Fame = 500;
            Karma = 500;
        }

        public Treant(Serial serial) : base(serial) { }

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
