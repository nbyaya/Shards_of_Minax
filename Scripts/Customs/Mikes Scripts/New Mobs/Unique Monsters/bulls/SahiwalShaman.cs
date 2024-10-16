using System;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a Sahiwal Shaman corpse")]
    public class SahiwalShaman : BaseCreature
    {
        private DateTime m_NextNaturesGrasp;
        private DateTime m_NextHealingRain;
        private DateTime m_NextSummonSpirit;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SahiwalShaman()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Sahiwal Shaman";
            Body = 0xE8; // Bull body
            BaseSoundID = 0x64;
            Hue = 1271; // Unique hue (you can adjust this)

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

            // Add a staff to the creature
            AddItem(new WildStaff());
        }

        public SahiwalShaman(Serial serial)
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

        public override int Meat { get { return 10; } }
        public override int Hides { get { return 15; } }
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
                    m_NextNaturesGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextHealingRain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                    m_NextSummonSpirit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNaturesGrasp)
                {
                    DoNaturesGrasp();
                }

                if (DateTime.UtcNow >= m_NextHealingRain)
                {
                    DoHealingRain();
                }

                if (DateTime.UtcNow >= m_NextSummonSpirit)
                {
                    DoSummonSpirit();
                }
            }
        }

        private void DoNaturesGrasp()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Nature's Grasp! *");
            PlaySound(0x218);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1075154); // The vines tangle your feet!
                    m.Freeze(TimeSpan.FromSeconds(5));
                    m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                }
            }

            m_NextNaturesGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DoHealingRain()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Healing Rain! *");
            PlaySound(0x10);
            FixedParticles(0x376A, 10, 15, 5052, EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 5, new TimerCallback(HealAllies));

            m_NextHealingRain = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void HealAllies()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Controlled)))
                {
                    int healAmount = Utility.RandomMinMax(10, 20);
                    m.Heal(healAmount);
                    m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                }
            }
        }

        private void DoSummonSpirit()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Summon Nature Spirit! *");
            PlaySound(0x218);

            Map map = this.Map;

            if (map != null)
            {
                int newNatureSpirits = 0;

                foreach (Mobile m in this.GetMobilesInRange(10))
                {
                    if (m is NatureSpirit)
                        newNatureSpirits++;
                }

                if (newNatureSpirits < 3)
                {
                    BaseCreature spirit = new NatureSpirit();
                    spirit.Team = this.Team;

                    Point3D loc = this.Location;
                    bool validLocation = false;

                    for (int j = 0; !validLocation && j < 10; ++j)
                    {
                        int x = X + Utility.Random(3) - 1;
                        int y = Y + Utility.Random(3) - 1;
                        int z = map.GetAverageZ(x, y);

                        if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                            loc = new Point3D(x, y, Z);
                        else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                            loc = new Point3D(x, y, z);
                    }

                    spirit.MoveToWorld(loc, map);
                    spirit.Combatant = this.Combatant;
                }
            }

            m_NextSummonSpirit = DateTime.UtcNow + TimeSpan.FromSeconds(90);
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

    [CorpseName("a nature spirit corpse")]
    public class NatureSpirit : BaseCreature
    {
        [Constructable]
        public NatureSpirit() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a nature spirit";
            Body = 0x33;
            Hue = 1272;

            SetStr(100, 120);
            SetDex(80, 100);
            SetInt(60, 80);

            SetHits(80, 100);

            SetDamage(8, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 50.0, 65.0);
            SetSkill(SkillName.Tactics, 50.0, 65.0);
            SetSkill(SkillName.Wrestling, 50.0, 65.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 30;
        }

        public NatureSpirit(Serial serial) : base(serial)
        {
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
        }
    }
}
