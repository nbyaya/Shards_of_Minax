using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a leprosy rat corpse")]
    public class LeprosyRat : GiantRat
    {
        private DateTime m_NextFleshRot;
        private DateTime m_NextInfestation;
        private DateTime m_NextDiseaseSpread;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LeprosyRat()
            : base()
        {
            Name = "a leprosy rat";
            Hue = 2266; // Unique hue for the Leprosy Rat
            this.BaseSoundID = 0xCC;
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

        public LeprosyRat(Serial serial)
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
                    m_NextFleshRot = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextInfestation = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDiseaseSpread = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFleshRot)
                {
                    ApplyFleshRot();
                }

                if (DateTime.UtcNow >= m_NextInfestation)
                {
                    SummonInfestation();
                }

                if (DateTime.UtcNow >= m_NextDiseaseSpread)
                {
                    SpreadDisease();
                }
            }
        }

        private void ApplyFleshRot()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                target.SendMessage("*Your flesh begins to decay under the Leprosy Rat's bite!*");
                target.FixedEffect(0x376A, 10, 16);

                // Start custom debuff
                new FleshRotDebuff(target).Start();

                m_NextFleshRot = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for the ability
            }
        }

        private void SummonInfestation()
        {
            if (Utility.RandomDouble() < 0.2) // 20% chance to summon infestation
            {
                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    InfestationRat rat = new InfestationRat();
                    rat.MoveToWorld(loc, Map);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*A swarm of infestation rats emerges!*");
                    m_NextInfestation = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for infestation
                }
            }
        }

        private void SpreadDisease()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.Player)
                {
                    m.SendMessage("*You feel a sudden wave of nausea!*");
                    m.SendMessage("You are afflicted with a disease!");

                    // Apply disease effect
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => m.Damage(3, this)); // Disease damage
                    m_NextDiseaseSpread = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for disease spread
                    break; // Only spread disease to one player per interval
                }
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
            writer.Write(m_AbilitiesInitialized); // Save the initialization flag
            writer.Write(m_NextFleshRot);
            writer.Write(m_NextInfestation);
            writer.Write(m_NextDiseaseSpread);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = reader.ReadBool(); // Load the initialization flag
            m_NextFleshRot = reader.ReadDateTime();
            m_NextInfestation = reader.ReadDateTime();
            m_NextDiseaseSpread = reader.ReadDateTime();
        }
    }

    public class InfestationRat : GiantRat
    {
        [Constructable]
        public InfestationRat()
        {
            Name = "an infestation rat";
            Body = 0xD7;
            Hue = 0x400; // Unique hue for infestation rats
            SetStr(10, 20);
            SetDex(20, 30);
            SetInt(10, 20);
            SetHits(10, 20);
            SetDamage(1, 3);
            SetResistance(ResistanceType.Physical, 10);
        }

        public InfestationRat(Serial serial)
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

    public class FleshRotDebuff : Timer
    {
        private Mobile m_Target;

        public FleshRotDebuff(Mobile target)
            : base(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), 3) // Repeats 3 times
        {
            m_Target = target;
        }

        protected override void OnTick()
        {
            if (m_Target != null && m_Target.Alive)
            {
                m_Target.Damage(5); // Damage applied over time
                m_Target.SendMessage("*The rot worsens, causing more pain!*");
            }
        }
    }
}
