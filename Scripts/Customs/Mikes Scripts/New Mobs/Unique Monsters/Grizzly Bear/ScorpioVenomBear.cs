using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a scorpio venom bear corpse")]
    public class ScorpioVenomBear : BaseCreature
    {
        private const int PoisonMessageID = 101; // Example ID for "The Scorpio VenomBear's venom burns fiercely!"
        private const int AmbushMessageID = 102; // Example ID for "Beware the Scorpio VenomBear's deadly ambush!"
        private const int RoarMessageID = 103; // Example ID for "The Scorpio VenomBear roars with a scalding fury!"

        private DateTime m_NextPoisonAttack;
        private DateTime m_NextShadowAmbush;
        private DateTime m_NextScorpioRoar;
        private bool m_IsRoaring;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ScorpioVenomBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Scorpio VenomBear";
            Body = 212; // GrizzlyBear body
            Hue = 1997; // Unique dark hue for venomous appearance
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public ScorpioVenomBear(Serial serial)
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
                    m_NextPoisonAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextShadowAmbush = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextScorpioRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPoisonAttack)
                {
                    PerformPoisonSting();
                }

                if (DateTime.UtcNow >= m_NextShadowAmbush)
                {
                    PerformShadowAmbush();
                }

                if (DateTime.UtcNow >= m_NextScorpioRoar && !m_IsRoaring)
                {
                    PerformScorpioRoar();
                }
            }
        }

        private void PerformPoisonSting()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0, PoisonMessageID);

            int damage = Utility.RandomMinMax(10, 15);
            if (Combatant is Mobile mob)
            {
                mob.ApplyPoison(this, Poison.Greater);
            }

            AOS.Damage(Combatant, this, damage, 0, 0, 0, 100, 0);

            m_NextPoisonAttack = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
        }

        private void PerformShadowAmbush()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0, AmbushMessageID);

            this.Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Reappear));

            Point3D newLocation = GetSpawnPosition(5);
            if (newLocation != Point3D.Zero)
            {
                this.MoveToWorld(newLocation, Map);
                this.Direction = GetDirectionTo(Combatant);
                this.Combatant = Combatant;
                this.OnDamage(0, Combatant as Mobile, false);
            }

            m_NextShadowAmbush = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
        }

        private void PerformScorpioRoar()
        {
            if (Combatant == null)
                return;

            m_IsRoaring = true;
            PublicOverheadMessage(MessageType.Regular, 0, RoarMessageID);
            PlaySound(0x5C6); // Roar sound effect

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Scorpio VenomBear's roar fills you with fear!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10);

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(() => m_IsRoaring = false));
            m_NextScorpioRoar = DateTime.UtcNow + TimeSpan.FromMinutes(Utility.RandomMinMax(1, 2));
        }

        private void Reappear()
        {
            this.Hidden = false;
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
}
