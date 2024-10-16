using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a sifaka warrior corpse")]
    public class SifakaWarrior : BaseCreature
    {
        private DateTime m_NextGracefulLeap;
        private DateTime m_NextCamouflage;
        private DateTime m_NextCounterAttack;
        private DateTime m_NextSummonAllies;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SifakaWarrior()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Sifaka Warrior";
            Body = 0x1D; // Gorilla body
            Hue = 1957; // Unique hue
			this.BaseSoundID = 0x9E;

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

        public SifakaWarrior(Serial serial)
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
                    m_NextGracefulLeap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextCounterAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextGracefulLeap)
                {
                    GracefulLeap();
                }

                if (DateTime.UtcNow >= m_NextCamouflage)
                {
                    Camouflage();
                }

                if (DateTime.UtcNow >= m_NextCounterAttack)
                {
                    CounterAttack();
                }

                if (DateTime.UtcNow >= m_NextSummonAllies)
                {
                    SummonAllies();
                }
            }
        }

        private void GracefulLeap()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sifaka Warrior leaps gracefully, attacking from above!*");
            PlaySound(0x3D6);
            FixedEffect(0x376A, 10, 16);

            int damage = Utility.RandomMinMax(20, 30);
            AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

            // AoE Effect
            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != Combatant && m != this && m.Alive)
                {
                    m.SendMessage("The Sifaka Warrior lands nearby with a thunderous impact!");
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 15), 0, 100, 0, 0, 0);
                }
            }

            m_NextGracefulLeap = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Graceful Leap
        }

        private void Camouflage()
        {
            Mobile target = Combatant as Mobile;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sifaka Warrior blends into its surroundings, becoming harder to detect!*");

            // Increase evasion
            VirtualArmor += 30;

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(EndCamouflage));

            m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for Camouflage
        }

        private void EndCamouflage()
        {
            Hidden = false;
            VirtualArmor -= 30;
        }

        private void CounterAttack()
        {
            if (Combatant == null) return;

            // Random chance to perform a counter attack
            if (Utility.RandomDouble() < 0.25)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sifaka Warrior deflects an attack and strikes back!*");
                PlaySound(0x3D6);

                int damage = Utility.RandomMinMax(10, 20);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
            }

            m_NextCounterAttack = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Counter Attack
        }

        private void SummonAllies()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sifaka Warrior summons reinforcements!*");
            PlaySound(0x3D6);

            // Summon allies (e.g., lesser creatures or clones)
            int numberOfAllies = Utility.RandomMinMax(1, 2);
            for (int i = 0; i < numberOfAllies; i++)
            {
                BaseCreature ally = new LesserSifakaWarrior();
                Point3D loc = GetSpawnPosition(5);
                if (loc != Point3D.Zero)
                {
                    ally.MoveToWorld(loc, Map);
                }
            }

            m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for Summon Allies
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

            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }

    public class LesserSifakaWarrior : BaseCreature
    {
        [Constructable]
        public LesserSifakaWarrior()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a lesser Sifaka Warrior";
            Body = 0x1D; // Gorilla body
            Hue = 1151; // Slightly different hue for variety

            SetStr(100);
            SetDex(100);
            SetInt(50);

            SetHits(80);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);

            SetSkill(SkillName.MagicResist, 50.0, 60.0);
            SetSkill(SkillName.Tactics, 60.0, 70.0);
            SetSkill(SkillName.Wrestling, 60.0, 70.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 30;

            Tamable = false;
        }

        public LesserSifakaWarrior(Serial serial)
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
