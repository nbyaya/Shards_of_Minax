using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Persian Shade corpse")]
    public class PersianShade : BaseCreature
    {
        private DateTime m_NextGloomCloud;
        private DateTime m_NextMysticVeil;
        private DateTime m_NextShadowStrike;
        private bool m_AbilitiesInitialized;
        private bool m_InStealth;

        [Constructable]
        public PersianShade()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Persian Shade";
            Body = 0xC9; // Cat body
            Hue = 1299; // Dark purple hue
            BaseSoundID = 0x69;

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

            m_AbilitiesInitialized = false; // Initialize the flag
            m_InStealth = false;
        }

        public PersianShade(Serial serial)
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

        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextGloomCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGloomCloud)
                {
                    GloomCloud();
                }

                if (DateTime.UtcNow >= m_NextMysticVeil)
                {
                    MysticVeil();
                }

                if (DateTime.UtcNow >= m_NextShadowStrike && m_InStealth)
                {
                    ShadowStrike();
                }

                if (!m_InStealth && Utility.RandomDouble() < 0.05)
                {
                    m_InStealth = true;
                    Hidden = true;
                    FixedEffect(0x37B9, 10, 20);
                }
            }
        }

        private void GloomCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Persian Shade envelops the area in darkness! *");
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are enveloped in a gloomy cloud!");
                    m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Head);
                    m.PlaySound(0x1E4);

                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                }
            }

            m_NextGloomCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void MysticVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Persian Shade is surrounded by a mystic veil! *");
            FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            PlaySound(0x1ED);

            AddRes(ResistanceType.Physical, 100);
            AddRes(ResistanceType.Fire, 100);
            AddRes(ResistanceType.Cold, 100);
            AddRes(ResistanceType.Poison, 100);
            AddRes(ResistanceType.Energy, 100);

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(RemoveMysticVeil));

            m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void RemoveMysticVeil()
        {
            AddRes(ResistanceType.Physical, -100);
            AddRes(ResistanceType.Fire, -100);
            AddRes(ResistanceType.Cold, -100);
            AddRes(ResistanceType.Poison, -100);
            AddRes(ResistanceType.Energy, -100);
        }

        private void ShadowStrike()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Persian Shade strikes from the shadows! *");
                FixedEffect(0x37B9, 10, 20);
                PlaySound(0x1E5);

                m_InStealth = false;
                Hidden = false;

                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void AddRes(ResistanceType type, int value)
        {
            SetResistance(type, GetResistance(type) + value);
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

            // Reset the initialization flag and cooldowns on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
