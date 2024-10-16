using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a dairy wraith corpse")]
    public class DairyWraith : BaseCreature
    {
        private DateTime m_NextWraithsTouch;
        private DateTime m_NextEtherealShift;
        private DateTime m_NextGhostlyHowl;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized
        private bool m_IsEthereal;

        [Constructable]
        public DairyWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Dairy Wraith";
            Body = 0xE8; // Bull body
            BaseSoundID = 0x482; // Ghost sound
            Hue = 1287; // Translucent white hue

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public DairyWraith(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextWraithsTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEtherealShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGhostlyHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWraithsTouch)
                {
                    DoWraithsTouch();
                }
                if (DateTime.UtcNow >= m_NextEtherealShift && !m_IsEthereal)
                {
                    DoEtherealShift();
                }
                if (DateTime.UtcNow >= m_NextGhostlyHowl)
                {
                    DoGhostlyHowl();
                }
            }

            if (m_IsEthereal && DateTime.UtcNow >= m_NextEtherealShift)
            {
                EndEtherealShift();
            }
        }

        private void DoWraithsTouch()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Wraith's Touch! *");
            PlaySound(0x1FB);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1070831); // The cold touch of the wraith chills your soul!
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                    m.PlaySound(0x1ED);

                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this);

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Curse, 1075835, 1075836, TimeSpan.FromSeconds(10), m, "25\t25\t25\t25"));
                }
            }

            m_NextWraithsTouch = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DoEtherealShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ethereal Shift! *");
            PlaySound(0x1FF);
            FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

            m_IsEthereal = true;
            Hidden = true;

            Point3D newLocation = GetSpawnPosition(3);
            if (newLocation != Point3D.Zero)
            {
                Location = newLocation;
                ProcessDelta();
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(RevealWraith));

            m_NextEtherealShift = DateTime.UtcNow + TimeSpan.FromSeconds(5); // Adjusted cooldown to avoid conflict
        }

        private void RevealWraith()
        {
            Hidden = false;
            FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
        }

        private void EndEtherealShift()
        {
            m_IsEthereal = false;
        }

        private void DoGhostlyHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ghostly Howl! *");
            PlaySound(0x482);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1070842); // The howl chills your blood!
                    m.FixedParticles(0x3728, 10, 15, 5013, EffectLayer.Head);
                    m.PlaySound(0x5C6);

                    int duration = 10; // 10 seconds
                    m.AddStatMod(new StatMod(StatType.Str, "GhostlyHowl_Str", -10, TimeSpan.FromSeconds(duration)));
                    m.AddStatMod(new StatMod(StatType.Dex, "GhostlyHowl_Dex", -10, TimeSpan.FromSeconds(duration)));
                    m.AddStatMod(new StatMod(StatType.Int, "GhostlyHowl_Int", -10, TimeSpan.FromSeconds(duration)));

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Curse, 1075835, 1075836, TimeSpan.FromSeconds(duration), m, "10\t10\t10"));
                }
            }

            m_NextGhostlyHowl = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
            m_NextWraithsTouch = DateTime.UtcNow;
            m_NextEtherealShift = DateTime.UtcNow;
            m_NextGhostlyHowl = DateTime.UtcNow;
            m_IsEthereal = false;
        }
    }
}
