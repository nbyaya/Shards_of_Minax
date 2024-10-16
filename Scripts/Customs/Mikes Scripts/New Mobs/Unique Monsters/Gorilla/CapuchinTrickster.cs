using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a capuchin trickster corpse")]
    public class CapuchinTrickster : BaseCreature
    {
        private DateTime m_NextTrickDecoy;
        private DateTime m_NextTrap;
        private DateTime m_NextTricksterGambit;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CapuchinTrickster()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Capuchin Trickster";
            Body = 0x1D; // Gorilla body
            Hue = 1968; // Unique hue for Capuchin Trickster
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

        public CapuchinTrickster(Serial serial)
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
                    m_NextTrickDecoy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 30));
                    m_NextTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 40));
                    m_NextTricksterGambit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTrickDecoy)
                {
                    CreateTrickDecoy();
                }

                if (DateTime.UtcNow >= m_NextTrap)
                {
                    LayTrap();
                }

                if (DateTime.UtcNow >= m_NextTricksterGambit)
                {
                    TricksterGambit();
                }
            }
        }

        private void CreateTrickDecoy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capuchin Trickster creates a deceptive duplicate to confuse its foes!*");
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 0x21, 0, 0, 0);
                TrickDecoy decoy = new TrickDecoy(this);
                decoy.MoveToWorld(loc, Map);
            }

            m_NextTrickDecoy = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for CreateTrickDecoy
        }

        private void LayTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capuchin Trickster sets a devious trap!*");
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x36BD, 10, 16, 0x21, 0, 0, 0);
                Trap trap = new Trap(this);
                trap.MoveToWorld(loc, Map);
            }

            m_NextTrap = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for LayTrap
        }

        private void TricksterGambit()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capuchin Trickster performs a Trickster's Gambit, boosting its abilities!*");
            this.AddStatMod(new StatMod(StatType.Str, "Trickster's Gambit", 20, TimeSpan.FromSeconds(30)));
            this.AddStatMod(new StatMod(StatType.Dex, "Trickster's Gambit", 20, TimeSpan.FromSeconds(30)));

            m_NextTricksterGambit = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for TricksterGambit
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

    public class TrickDecoy : BaseCreature
    {
        private Mobile m_Master;

        public TrickDecoy(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = "a decoy";

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);

            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public TrickDecoy(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;

            if (DateTime.UtcNow >= m_NextTrickDecoyAttack)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The decoy explodes on destruction!*");
                Delete();
            }
        }

        private DateTime m_NextTrickDecoyAttack = DateTime.UtcNow + TimeSpan.FromSeconds(15);

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }

    public class Trap : Item
    {
        private Mobile m_Owner;
        private DateTime m_TriggerTime;

        public Trap(Mobile owner)
            : base(0x9B5)
        {
            m_Owner = owner;
            Movable = false;
            Hue = 1155; // Unique hue for Trap

            m_TriggerTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);
        }

        public Trap(Serial serial)
            : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.UtcNow >= m_TriggerTime)
            {
                Trigger();
            }
        }

        private void Trigger()
        {
            if (Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The trap is triggered!*");
            Effects.PlaySound(GetWorldLocation(), Map, 0x307);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != m_Owner && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 30);
                    AOS.Damage(m, m_Owner, damage, 0, 100, 0, 0, 0);

                    m.PlaySound(0x1DD);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Owner);
            writer.Write(m_TriggerTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_TriggerTime = reader.ReadDateTime();
        }
    }
}
