using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a breeze phantom corpse")]
    public class BreezePhantom : BaseCreature
    {
        private DateTime m_NextPhantomStrike;
        private DateTime m_NextSilentGale;
        private DateTime m_NextAirborneEscape;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public BreezePhantom()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a breeze phantom";
            Body = 13; // Air elemental body
            Hue = 1072; // Light blue hue
            BaseSoundID = 655;

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
            SetResistance(ResistanceType.Poison, 100);
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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public BreezePhantom(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSilentGale = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextAirborneEscape = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    
                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextPhantomStrike)
                {
                    PhantomStrike();
                }

                if (DateTime.UtcNow >= m_NextSilentGale)
                {
                    SilentGale();
                }

                if (DateTime.UtcNow >= m_NextAirborneEscape)
                {
                    AirborneEscape();
                }
            }
        }

        private void PhantomStrike()
        {
            if (Combatant != null && Combatant is Mobile target)
            {
                Point3D behindLocation = new Point3D(target.X + Utility.RandomMinMax(-1, 1), target.Y + Utility.RandomMinMax(-1, 1), target.Z);
                if (Map.CanSpawnMobile(behindLocation))
                {
                    this.Location = behindLocation;
                    target.SendMessage("You feel a sudden chill as the Breeze Phantom strikes from behind!");
                    this.AggressiveAction(target);
                    target.Damage(Utility.RandomMinMax(20, 30), this);
                    m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Recalculate next activation
                }
            }
        }

        private void SilentGale()
        {
            // Make the phantom harder to detect and target
            this.Skills[SkillName.Stealth].Base = 100.0; // Just an example; adjust as needed
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The breeze phantom moves silently through the air! *");
            m_NextSilentGale = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Recalculate next activation
        }

        private void AirborneEscape()
        {
            if (Combatant != null)
            {
                Point3D newLocation = GetSpawnPosition(10);
                if (newLocation != Point3D.Zero)
                {
                    this.Location = newLocation;
                    this.Map = Map;
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The breeze phantom evades with a swift maneuver! *");
                    Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);
                    m_NextAirborneEscape = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Recalculate next activation
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
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesActivated = false; // Reset flag
        }
    }
}
