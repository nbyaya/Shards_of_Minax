using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bison brute corpse")]
    public class BisonBrute : BaseCreature
    {
        private DateTime m_NextStampede;
        private DateTime m_NextEarthquakeStomp;
        private DateTime m_NextBisonStrength;
        private DateTime m_BisonStrengthEnd;

        // Add fields to track damage values
        private int m_DamageMin;
        private int m_DamageMax;

        // Add fields for random starting intervals
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BisonBrute()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bison brute";
            Body = 0xE8; // Bull body
            BaseSoundID = 0x64;
            Hue = 1288; // Unique hue (you can adjust this)

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
            m_BisonStrengthEnd = DateTime.MinValue;
        }

        public BisonBrute(Serial serial)
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

        public override int Meat { get { return 20; } }
        public override int Hides { get { return 30; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bull; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextStampede = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextBisonStrength = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextStampede)
                {
                    DoStampede();
                }

                if (DateTime.UtcNow >= m_NextEarthquakeStomp)
                {
                    DoEarthquakeStomp();
                }

                if (DateTime.UtcNow >= m_NextBisonStrength && DateTime.UtcNow >= m_BisonStrengthEnd)
                {
                    DoBisonStrength();
                }
            }

            if (DateTime.UtcNow >= m_BisonStrengthEnd && m_BisonStrengthEnd != DateTime.MinValue)
            {
                DeactivateBisonStrength();
            }
        }

        private void DoStampede()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bison Brute begins a stampede! *");
            PlaySound(0x3E9);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            foreach (Mobile target in targets)
            {
                Direction = GetDirectionTo(target);
                Move(Direction);

                if (InRange(target, 1))
                {
                    int damage = Utility.RandomMinMax(30, 40);
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                    target.PlaySound(0x14E);
                    target.FixedEffect(0x376A, 10, 16);
                }
            }

            m_NextStampede = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DoEarthquakeStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bison Brute stomps the ground! *");
            PlaySound(0x2F3);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    m.SendLocalizedMessage(1072215); // The earth shakes beneath your feet!
                }
            }

            m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void DoBisonStrength()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bison Brute's muscles bulge with power! *");
            PlaySound(0x1F7);
            FixedEffect(0x376A, 10, 16);

            SetStr(Str + 50);

            // Increase damage values
            m_DamageMin += 5;
            m_DamageMax += 10;
            SetDamage(m_DamageMin, m_DamageMax);

            m_BisonStrengthEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextBisonStrength = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void DeactivateBisonStrength()
        {
            // Decrease damage values
            m_DamageMin -= 5;
            m_DamageMax -= 10;
            SetDamage(m_DamageMin, m_DamageMax);

            SetStr(Str - 50);
            m_BisonStrengthEnd = DateTime.MinValue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_DamageMin);
            writer.Write(m_DamageMax);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_DamageMin = reader.ReadInt();
            m_DamageMax = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset initialization flag
            m_BisonStrengthEnd = DateTime.MinValue;
        }
    }
}
