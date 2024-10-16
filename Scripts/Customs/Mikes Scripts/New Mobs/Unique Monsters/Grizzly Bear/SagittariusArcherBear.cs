using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a sagittarius archerbear corpse")]
    public class SagittariusArcherBear : GrizzlyBear
    {
        private static readonly TimeSpan PierceCooldown = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan AgilityCooldown = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan ArrowRainCooldown = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan MysticRoarCooldown = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan ShadowBearCloneCooldown = TimeSpan.FromMinutes(2);

        private DateTime m_NextPierceTime;
        private DateTime m_NextAgilityTime;
        private DateTime m_NextArrowRainTime;
        private DateTime m_NextMysticRoarTime;
        private DateTime m_NextShadowBearCloneTime;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public SagittariusArcherBear() : base()
        {
            Name = "Sagittarius ArcherBear";
            Hue = 1998; // Unique hue for the Sagittarius ArcherBear
            Body = 212; // GrizzlyBear body
			BaseSoundID = 0xA3;
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

            m_AbilitiesInitialized = false; // Set initialization flag to false
        }

        public SagittariusArcherBear(Serial serial) : base(serial)
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
                    m_NextPierceTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAgilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextArrowRainTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextMysticRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextShadowBearCloneTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set flag to true after initialization
                }

                if (DateTime.UtcNow >= m_NextPierceTime)
                {
                    PierceArrow();
                    m_NextPierceTime = DateTime.UtcNow + PierceCooldown;
                }

                if (DateTime.UtcNow >= m_NextAgilityTime)
                {
                    HuntressAgility();
                    m_NextAgilityTime = DateTime.UtcNow + AgilityCooldown;
                }

                if (DateTime.UtcNow >= m_NextArrowRainTime)
                {
                    ArrowRain();
                    m_NextArrowRainTime = DateTime.UtcNow + ArrowRainCooldown;
                }

                if (DateTime.UtcNow >= m_NextMysticRoarTime)
                {
                    MysticRoar();
                    m_NextMysticRoarTime = DateTime.UtcNow + MysticRoarCooldown;
                }

                if (DateTime.UtcNow >= m_NextShadowBearCloneTime)
                {
                    CreateShadowBearClone();
                    m_NextShadowBearCloneTime = DateTime.UtcNow + ShadowBearCloneCooldown;
                }
            }
        }

        private void PierceArrow()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Sagittarius ArcherBear takes aim!*");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m.InLOS(this))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are struck by a piercing arrow!");
                }
            }

            PlaySound(0x1F2); // Example sound effect
        }

        private void HuntressAgility()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Sagittarius ArcherBear moves with incredible speed!*");

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(() =>
            {
            }));
        }

        private void ArrowRain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Sagittarius ArcherBear unleashes a rain of arrows!*");

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), new TimerCallback(() =>
                {
                    foreach (Mobile m in GetMobilesInRange(10))
                    {
                        if (m != this && m.Alive && m.InLOS(this))
                        {
                            int damage = Utility.RandomMinMax(10, 20);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                            m.SendMessage("You are pelted by a barrage of arrows!");
                        }
                    }
                }));
            }
        }

        private void MysticRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Sagittarius ArcherBear lets out a mystic roar!*");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are stunned by the mystic roar!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            PlaySound(0x1F5); // Example sound effect
        }

        private void CreateShadowBearClone()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Sagittarius ArcherBear creates shadow clones!*");

            for (int i = 0; i < 2; i++)
            {
                Point3D loc = GetSpawnPosition(5);
                if (loc != Point3D.Zero)
                {
                    ShadowBearClone clone = new ShadowBearClone(this);
                    clone.MoveToWorld(loc, Map);
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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class ShadowBearClone : BaseCreature
    {
        private Mobile m_Master;

        public ShadowBearClone(Mobile master) : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = "Shadow Clone";

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

        public ShadowBearClone(Serial serial) : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

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
}
