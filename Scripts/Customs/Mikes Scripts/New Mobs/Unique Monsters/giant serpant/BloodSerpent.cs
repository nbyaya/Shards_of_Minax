using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blood serpent corpse")]
    public class BloodSerpent : BaseCreature
    {
        private DateTime m_NextBloodDrain;
        private DateTime m_NextSanguineRage;
        private DateTime m_NextBloodRain;
        private DateTime m_NextSummonLesserSerpents;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public BloodSerpent()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blood serpent";
            Body = 0x15; // Giant Serpent body
            Hue = 1780; // Blood red hue
            BaseSoundID = 219;

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

        public BloodSerpent(Serial serial) : base(serial)
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
                    // Initialize random start times
                    Random rand = new Random();
                    m_NextBloodDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSanguineRage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextBloodRain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSummonLesserSerpents = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 90));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextBloodDrain)
                {
                    BloodDrain();
                }

                if (DateTime.UtcNow >= m_NextSanguineRage)
                {
                    SanguineRage();
                }

                if (DateTime.UtcNow >= m_NextBloodRain)
                {
                    BloodRain();
                }

                if (DateTime.UtcNow >= m_NextSummonLesserSerpents)
                {
                    SummonLesserSerpents();
                }
            }
        }

        private void BloodDrain()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(15, 25);
                int healAmount = (int)(damage * 0.5);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 0);
                Hits += healAmount;

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Blood Serpent drains your life force!");
                m_NextBloodDrain = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }
        }

        private void SanguineRage()
        {
            if (Hits < HitsMax * 0.5)
            {
                int damageBoost = Utility.RandomMinMax(15, 25);
                int attackSpeedBoost = 2; // Simulate attack speed boost by shorter delay

                DamageMin += damageBoost;
                DamageMax += damageBoost;

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Blood Serpent’s rage grows with each wound!");
                m_NextSanguineRage = DateTime.UtcNow + TimeSpan.FromSeconds(35);
            }
        }

        private void BloodRain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "Blood rains down from the Blood Serpent!");
            FixedEffect(0x36BD, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 0);
                    m.SendMessage("You are struck by a shower of blood!");
                }
            }

            m_NextBloodRain = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void SummonLesserSerpents()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Blood Serpent summons lesser serpents to aid it!");
            FixedEffect(0x3728, 10, 16);

            for (int i = 0; i < 2; i++)
            {
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                if (Map.CanSpawnMobile(loc))
                {
                    LesserSerpent lesser = new LesserSerpent();
                    lesser.MoveToWorld(loc, Map);
                }
            }

            m_NextSummonLesserSerpents = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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

    public class LesserSerpent : BaseCreature
    {
        [Constructable]
        public LesserSerpent()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a lesser serpent";
            Body = 0x15; // Same body as Blood Serpent
            Hue = 1152; // Dark red hue
            BaseSoundID = 219;

            SetStr(100, 150);
            SetDex(40, 60);
            SetInt(50, 70);

            SetHits(80, 120);
            SetMana(0);

            SetDamage(8, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Poisoning, 50.1, 70.0);
            SetSkill(SkillName.MagicResist, 30.1, 50.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 40.1, 60.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 20;
        }

        public LesserSerpent(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
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
