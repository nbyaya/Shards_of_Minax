using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bearded goat corpse")]
    public class BeardedGoat : BaseCreature
    {
        private DateTime m_NextBeardWhip;
        private DateTime m_NextProtectiveStance;
        private DateTime m_NextGroundQuake;
        private DateTime m_NextSummonGoats;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BeardedGoat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bearded goat";
            Body = 0xD1; // Goat body
            Hue = 1921; // Dark brown hue
			BaseSoundID = 0x99;

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

            // Initialize the abilities with random start times
            m_AbilitiesInitialized = false;
        }

        public BeardedGoat(Serial serial)
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
                    m_NextBeardWhip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextProtectiveStance = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextGroundQuake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSummonGoats = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBeardWhip)
                {
                    BeardWhip();
                }

                if (DateTime.UtcNow >= m_NextProtectiveStance)
                {
                    ProtectiveStance();
                }

                if (DateTime.UtcNow >= m_NextGroundQuake)
                {
                    GroundQuake();
                }

                if (DateTime.UtcNow >= m_NextSummonGoats)
                {
                    SummonGoats();
                }
            }
        }

        private void BeardWhip()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bearded Goat lashes out with its thick beard! *");

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(12, 18), 0, 100, 0, 0, 0);

                    if (Utility.RandomDouble() < 0.30) // 30% chance to stun
                    {
                        m.SendMessage("You are stunned by the Bearded Goat's beard!");
                        m.Freeze(TimeSpan.FromSeconds(2));
                    }
                }
            }

            m_NextBeardWhip = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void ProtectiveStance()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bearded Goat takes a defensive stance, ready to resist magic! *");

            VirtualArmor += 15;
            // Magic resistance functionality is removed

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(RemoveProtectiveStance));
            m_NextProtectiveStance = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void RemoveProtectiveStance()
        {
            VirtualArmor -= 15;
            // Magic resistance functionality is removed
        }

        private void GroundQuake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bearded Goat stomps the ground with a mighty quake! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(8, 12), 0, 100, 0, 0, 0);

                    if (Utility.RandomDouble() < 0.20) // 20% chance to knock down
                    {
                        KnockDown(m);
                    }
                }
            }

            m_NextGroundQuake = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void KnockDown(Mobile m)
        {
            m.SendMessage("You are knocked down by the Bearded Goat's ground quake!");
            // Implement additional effects if necessary
        }

        private void SummonGoats()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bearded Goat summons its kin to assist in battle! *");

            for (int i = 0; i < 2; i++)
            {
                BeardedGoatKid kid = new BeardedGoatKid();
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                kid.MoveToWorld(loc, Map);
                kid.Combatant = this;
            }

            m_NextSummonGoats = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

            // Reset the initialization flag and set random times
            m_AbilitiesInitialized = false;
        }
    }

    public class BeardedGoatKid : BaseCreature
    {
        [Constructable]
        public BeardedGoatKid()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bearded goat kid";
            Body = 0xD1; // Goat body
            Hue = 1121; // Dark brown hue

            SetStr(50);
            SetDex(30);
            SetInt(10);

            SetHits(30);
            SetMana(0);

            SetDamage(5, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 30.0);
            SetSkill(SkillName.Wrestling, 30.0);

            Fame = 200;
            Karma = -200;

            VirtualArmor = 10;

            Tamable = false;
        }

        public BeardedGoatKid(Serial serial)
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
