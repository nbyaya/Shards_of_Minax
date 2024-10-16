using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a milking demon corpse")]
    public class MilkingDemon : BaseCreature
    {
        private DateTime m_NextDemonicSiphon;
        private DateTime m_NextInfernalBreath;
        private DateTime m_NextHellishMoos;

        private bool m_AbilitiesInitialized; // Flag to track if random intervals have been initialized

        [Constructable]
        public MilkingDemon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Milking Demon";
            Body = 0xE8; // Bull body
            BaseSoundID = 0x64;
            Hue = 1277; // Demonic red hue (you can adjust this)

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

            m_AbilitiesInitialized = false; // Set the flag to false

            // Add demonic horns to the creature
            AddItem(new BonePile());
        }

        public MilkingDemon(Serial serial)
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

        public override int Meat { get { return 5; } }
        public override int Hides { get { return 15; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bull; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextDemonicSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextInfernalBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHellishMoos = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to true after initialization
                }

                if (DateTime.UtcNow >= m_NextDemonicSiphon)
                {
                    DoDemonicSiphon();
                }
                else if (DateTime.UtcNow >= m_NextInfernalBreath)
                {
                    DoInfernalBreath();
                }
                else if (DateTime.UtcNow >= m_NextHellishMoos)
                {
                    DoHellishMoos();
                }
            }
        }

        private void DoDemonicSiphon()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Demonic Siphon! *");
            PlaySound(0x1FB);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this);
                    Hits = Math.Min(Hits + damage, HitsMax);
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                    m.PlaySound(0x231);
                }
            }

            m_NextDemonicSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DoInfernalBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Infernal Breath! *");
            PlaySound(0x108);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    Direction = GetDirectionTo(m);
                    Effects.SendMovingParticles(this, m, 0x36D4, 7, 0, false, true, 0x494, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);
                    
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                        m.PlaySound(0x208);
                        
                        AOS.Damage(m, this, Utility.RandomMinMax(30, 40), 0, 100, 0, 0, 0);
                        BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075378, 1075379, TimeSpan.FromSeconds(10), m));
                    });
                }
            }

            m_NextInfernalBreath = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void DoHellishMoos()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Hellish Moos! *");
            PlaySound(0x2C7);

            foreach (Mobile m in GetMobilesInRange(12))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1010581); // The beast's roar terrifies you!
                    m.FixedParticles(0x3779, 1, 15, 9905, 32, 7, EffectLayer.Head);
                    m.PlaySound(0x596);

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075826, 1075827, TimeSpan.FromSeconds(10), m));
                    m.ApplyPoison(this, Poison.Regular);
                }
            }

            m_NextHellishMoos = DateTime.UtcNow + TimeSpan.FromSeconds(60);
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
