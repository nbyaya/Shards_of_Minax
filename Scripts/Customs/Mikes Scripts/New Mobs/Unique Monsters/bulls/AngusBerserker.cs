using System;
using Server;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an angus berserker corpse")]
    public class AngusBerserker : BaseCreature
    {
        private DateTime m_NextBerserkFury;
        private DateTime m_NextRageWave;
        private DateTime m_NextBloodlust;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AngusBerserker()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an angus berserker";
            Body = 0xE8; // Using bull body
            Hue = 1290; // Unique hue, e.g., dark red or purple
			BaseSoundID = 0x64;

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

            m_AbilitiesInitialized = false; // Set the initialization flag to false
        }

        public AngusBerserker(Serial serial)
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
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextBerserkFury = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextRageWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextBloodlust = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBerserkFury)
                {
                    ActivateBerserkFury();
                }

                if (DateTime.UtcNow >= m_NextRageWave)
                {
                    CastRageWave();
                }

                if (DateTime.UtcNow >= m_NextBloodlust)
                {
                    ActivateBloodlust();
                }
            }
        }

        private void ActivateBerserkFury()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Berserk Fury! *");
            PlaySound(0x14F);
            FixedEffect(0x373A, 10, 16);

            SetDamage(DamageMin + 10, DamageMax + 10);
            VirtualArmor -= 10;

            m_NextBerserkFury = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void CastRageWave()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Rage Wave! *");
            PlaySound(0x2D6);
            FixedEffect(0x37B9, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant)
                {
                    m.Damage(10, this);
                    m.SendMessage("You are hit by a shockwave of energy!");
                }
            }

            m_NextRageWave = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ActivateBloodlust()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Bloodlust Activated! *");
            PlaySound(0x1F1);
            FixedEffect(0x376A, 10, 16);

            Hits = Math.Min(Hits + 20, HitsMax);

            m_NextBloodlust = DateTime.UtcNow + TimeSpan.FromSeconds(15);
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
