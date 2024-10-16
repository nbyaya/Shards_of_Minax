using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a magma golem corpse")]
    public class MagmaGolem : BaseCreature
    {
        private DateTime m_NextMagmaShield;
        private DateTime m_NextLavaWave;
        private bool m_AbilitiesInitialized;
        private bool m_MagmaShieldActive;

        [Constructable]
        public MagmaGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a magma golem";
            Body = 14;
            BaseSoundID = 268;
            Hue = 1496; // Fiery red-orange hue

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

            PackItem(new SulfurousAsh(Utility.RandomMinMax(4, 10)));
            PackItem(new IronIngot(Utility.RandomMinMax(8, 16)));

            m_AbilitiesInitialized = false;
        }

        public MagmaGolem(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override bool BardImmune { get { return false; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextMagmaShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLavaWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextMagmaShield)
                {
                    ActivateMagmaShield();
                }

                if (DateTime.UtcNow >= m_NextLavaWave)
                {
                    LavaWave();
                }
            }
        }

        private void ActivateMagmaShield()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Magma swirls around the golem *");
            PlaySound(0x10B);
            FixedEffect(0x376A, 10, 15);

            m_MagmaShieldActive = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                m_MagmaShieldActive = false;
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The magma shield dissipates *");
            });

            m_NextMagmaShield = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void LavaWave()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Unleashes a wave of lava *");
            PlaySound(0x108);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    Point3D loc = new Point3D(X + Utility.RandomMinMax(-1, 1), Y + Utility.RandomMinMax(-1, 1), Z);
                    Effects.SendLocationEffect(loc, Map, 0x36B0, 20, 10, 0, 5);

                    foreach (Mobile m in GetMobilesInRange(1))
                    {
                        if (m != this && CanBeHarmful(m))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                            m.SendLocalizedMessage(1008112); // The intense heat is damaging you!

                            // Start burning effect
                            new BurningTimer(m, TimeSpan.FromSeconds(5)).Start();
                        }
                    }
                });
            }

            m_NextLavaWave = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            base.AlterMeleeDamageFrom(from, ref damage);

            if (m_MagmaShieldActive)
            {
                int reflect = damage / 2;
                from.Damage(reflect, this);
                this.Hits += reflect;
                this.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The magma shield reflects damage *");
            }
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            base.AlterSpellDamageFrom(from, ref damage);

            if (m_MagmaShieldActive)
            {
                int absorb = damage / 2;
                damage -= absorb;
                this.Hits += absorb;
                this.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The magma shield absorbs spell damage *");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset initialization flag and ability times
            m_AbilitiesInitialized = false;
            m_NextMagmaShield = DateTime.UtcNow;
            m_NextLavaWave = DateTime.UtcNow;
        }
    }

    public class BurningTimer : Timer
    {
        private Mobile m_Mobile;
        private DateTime m_StartTime;
        private const double BurnDamage = 5.0; // Damage per second

        public BurningTimer(Mobile m, TimeSpan duration) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
        {
            m_Mobile = m;
            m_StartTime = DateTime.UtcNow;
            Priority = TimerPriority.OneSecond;
        }

        protected override void OnTick()
        {
            if (m_Mobile.Deleted || m_Mobile.Map == null)
            {
                Stop();
                return;
            }

            // Apply damage
            AOS.Damage(m_Mobile, m_Mobile, (int)BurnDamage, 0, 100, 0, 0, 0);
            m_Mobile.SendLocalizedMessage(1008112); // The intense heat is damaging you!

            // Stop timer if duration has elapsed
            if (DateTime.UtcNow - m_StartTime > TimeSpan.FromSeconds(5))
            {
                Stop();
            }
        }
    }
}
