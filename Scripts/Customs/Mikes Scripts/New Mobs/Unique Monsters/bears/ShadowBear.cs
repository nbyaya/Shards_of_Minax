using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a shadow bear corpse")]
    public class ShadowBear : BaseCreature
    {
        private DateTime m_NextShadowClaw;
        private DateTime m_NextDarkVeil;
        private DateTime m_DarkVeilEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ShadowBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow bear";
            Body = 211; // BlackBear body
            BaseSoundID = 0xA3;
            Hue = 1184; // Dark purple hue

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

        public ShadowBear(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextShadowClaw = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowClaw)
                {
                    ShadowClaw();
                }

                if (DateTime.UtcNow >= m_NextDarkVeil && DateTime.UtcNow >= m_DarkVeilEnd)
                {
                    DarkVeil();
                }
            }

            if (DateTime.UtcNow >= m_DarkVeilEnd && m_DarkVeilEnd != DateTime.MinValue)
            {
                DeactivateDarkVeil();
            }
        }

        private void ShadowClaw()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Claw *");
            PlaySound(0x165);
            FixedEffect(0x37CC, 10, 15, 1166, 0); // Dark shadow claw visual effect

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    if (Utility.RandomDouble() < 0.3) // 30% chance to inflict fear
                    {
                        m.SendLocalizedMessage(1062506); // You are frozen with fear.
                        m.Frozen = true;
                        Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(delegate { m.Frozen = false; }));
                    }
                }
            }

            m_NextShadowClaw = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void DarkVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Dark Veil *");
            PlaySound(0x15E);
            FixedEffect(0x376A, 10, 15, 2718, 0); // Dark mist visual effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1062507); // A dark mist surrounds you, making it hard to see.
                    m.AddStatMod(new StatMod(StatType.Dex, "DarkVeil", -20, TimeSpan.FromSeconds(20)));
                }
            }

            m_DarkVeilEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void DeactivateDarkVeil()
        {
            m_DarkVeilEnd = DateTime.MinValue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // Save state of ability initialization flag
            writer.Write(m_AbilitiesInitialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Load state of ability initialization flag
            m_AbilitiesInitialized = reader.ReadBool();

            if (!m_AbilitiesInitialized)
            {
                // Reinitialize the random intervals if necessary
                Random rand = new Random();
                m_NextShadowClaw = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
            }
        }
    }
}
