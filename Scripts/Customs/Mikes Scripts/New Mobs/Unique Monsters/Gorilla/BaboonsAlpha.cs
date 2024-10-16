using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a baboon's alpha corpse")]
    public class BaboonsAlpha : BaseCreature
    {
        private DateTime m_NextRallyingRoar;
        private DateTime m_NextSavageStrike;
        private DateTime m_NextSummonBaboons;
        private DateTime m_NextFrenzy;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BaboonsAlpha()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Baboons' Alpha";
            Body = 0x1D; // Gorilla body
            Hue = 1969; // Unique hue for the Baboons' Alpha
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

        public BaboonsAlpha(Serial serial)
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
                    // Initialize random intervals for abilities
                    Random rand = new Random();
                    m_NextRallyingRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSavageStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonBaboons = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRallyingRoar)
                {
                    RallyingRoar();
                }

                if (DateTime.UtcNow >= m_NextSavageStrike)
                {
                    SavageStrike();
                }

                if (DateTime.UtcNow >= m_NextSummonBaboons)
                {
                    SummonBaboons();
                }

                if (DateTime.UtcNow >= m_NextFrenzy)
                {
                    Frenzy();
                }
            }
        }

        private void RallyingRoar()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Alpha's roar inspires its allies to fight harder!*");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && m != this)
                {
                    BaseCreature ally = m as BaseCreature;
                    if (ally.Body == 0x1D) // Example for baboons
                    {
                        ally.Hits += 15; // Example effect: healing
                        ally.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Alpha's roar has invigorated you!");
                    }
                }
            }

            m_NextRallyingRoar = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Rallying Roar
        }

        private void SavageStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null)
            {
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Alpha strikes with savage fury!*");
                int damage = Utility.RandomMinMax(25, 35);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "You are bleeding from the Alpha's savage strike!");

                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(() => BleedEffect(target)));

                m_NextSavageStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Savage Strike
            }
        }

        private void BleedEffect(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "You are bleeding from the Alpha's attack!");
                Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(() => BleedDamage(target)));
            }
        }

        private void BleedDamage(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                int damage = Utility.RandomMinMax(10, 15);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The bleeding from your wounds continues!");
            }
        }

        private void SummonBaboons()
        {
            if (Combatant != null)
            {
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Alpha summons reinforcements!*");

                for (int i = 0; i < 3; i++)
                {
                    BaboonsMinion minion = new BaboonsMinion();
                    minion.MoveToWorld(Location, Map);
                }

                m_NextSummonBaboons = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for Summon Baboons
            }
        }

        private void Frenzy()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Alpha enters a frenzied state!*");
            FixedEffect(0x376A, 10, 16); // Visual effect

            SetDamage(20, 30);
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    target.Damage(Utility.RandomMinMax(10, 20), this);
                }
            }
            m_NextFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Frenzy
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

            m_NextRallyingRoar = DateTime.UtcNow;
            m_NextSavageStrike = DateTime.UtcNow;
            m_NextSummonBaboons = DateTime.UtcNow;
            m_NextFrenzy = DateTime.UtcNow;
            m_AbilitiesInitialized = false; // Reset initialization flag on deserialization
        }
    }

    public class BaboonsMinion : BaseCreature
    {
        [Constructable]
        public BaboonsMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a baboon minion";
            Body = 0x1D; // Gorilla body
            Hue = 0x9C2; // Same hue as Baboons' Alpha

            SetStr(70);
            SetDex(60);
            SetInt(30);

            SetHits(50);
            SetMana(0);

            SetDamage(8, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 15);

            SetSkill(SkillName.MagicResist, 40.0, 55.0);
            SetSkill(SkillName.Tactics, 50.0, 65.0);
            SetSkill(SkillName.Wrestling, 50.0, 65.0);

            Fame = 300;
            Karma = -300;

            VirtualArmor = 25;

            Tamable = false;
        }

        public BaboonsMinion(Serial serial)
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
