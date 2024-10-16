using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a selkie corpse")]
    public class Selkie : BaseCreature
    {
        private DateTime m_NextWaterManipulation;
        private DateTime m_NextShapeShift;
        private DateTime m_NextDrown;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized
        private bool m_InSealForm;

        [Constructable]
        public Selkie()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a selkie";
            Body = 723; // GreenGoblin body
            BaseSoundID = 0x5A; // Dolphin sound
            Hue = 1584; // Aqua blue hue

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

            PackItem(new Bandage(Utility.RandomMinMax(5, 10)));
            PackItem(new FishSteak(3));

            m_AbilitiesInitialized = false; // Initialize flag
            m_InSealForm = false;
        }

        public Selkie(Serial serial)
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
        public override Poison PoisonImmune { get { return Poison.Greater; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextWaterManipulation = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 25));
                    m_NextShapeShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
                    m_NextDrown = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(25, 35));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWaterManipulation)
                {
                    DoWaterManipulation();
                }

                if (DateTime.UtcNow >= m_NextShapeShift)
                {
                    DoShapeShift();
                }

                if (DateTime.UtcNow >= m_NextDrown)
                {
                    DoDrown();
                }
            }
        }

        private void DoWaterManipulation()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Manipulates water *");
            PlaySound(0x11);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendLocalizedMessage(502361); // You are engulfed by water!
                    m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    m.PlaySound(0x5C1);

                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                }
            }

            m_NextWaterManipulation = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }

        private void DoShapeShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Shape-shifts *");
            PlaySound(0x655);
            FixedEffect(0x3728, 10, 30);

            if (m_InSealForm)
            {
                Body = 723; // GreenGoblin body
                BaseSoundID = 0x5A; // Dolphin sound
                SetDamageType(ResistanceType.Physical, 50);
                SetDamageType(ResistanceType.Cold, 50);
            }
            else
            {
                Body = 0xDD; // Seal body
                BaseSoundID = 0xE0; // Seal sound
                SetDamageType(ResistanceType.Physical, 100);
            }

            m_InSealForm = !m_InSealForm;
            m_NextShapeShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
        }

        private void DoDrown()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Casts a drowning spell *");
            PlaySound(0x658);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player && !m.Paralyzed)
                {
                    m.SendLocalizedMessage(502359); // You feel yourself unable to breathe!
                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Head);
                    m.PlaySound(0x5C);

                    m.Paralyze(TimeSpan.FromSeconds(5));
                }
            }

            m_NextDrown = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }

        public override int GetAngerSound()
        {
            return 0x1C8;
        }

        public override int GetIdleSound()
        {
            return 0x1C9;
        }

        public override int GetAttackSound()
        {
            return 0x1CA;
        }

        public override int GetHurtSound()
        {
            return 0x1CB;
        }

        public override int GetDeathSound()
        {
            return 0x1CC;
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

            m_AbilitiesInitialized = false; // Reset initialization flag
            m_InSealForm = false; // Ensure the form is reset
        }
    }
}
