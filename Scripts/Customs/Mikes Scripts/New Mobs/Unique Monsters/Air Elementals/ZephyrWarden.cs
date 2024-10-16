using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a zephyr warden corpse")]
    public class ZephyrWarden : BaseCreature
    {
        private DateTime m_NextBreezeHeal;
        private DateTime m_NextGustBarrier;
        private DateTime m_NextSoothingWind;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public ZephyrWarden()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a zephyr warden";
            Body = 13; // Air Elemental body
            Hue = 1066; // Light blue hue
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

        public ZephyrWarden(Serial serial)
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

            if (!m_AbilitiesActivated)
            {
                Random rand = new Random();
                m_NextBreezeHeal = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                m_NextGustBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                m_NextSoothingWind = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 180));

                m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
            }

            if (DateTime.UtcNow >= m_NextBreezeHeal)
            {
                CastBreezeHeal();
            }

            if (DateTime.UtcNow >= m_NextGustBarrier)
            {
                CastGustBarrier();
            }

            if (DateTime.UtcNow >= m_NextSoothingWind)
            {
                CastSoothingWind();
            }
        }

        private void CastBreezeHeal()
        {
            if (Alive)
            {
                int healAmount = Utility.RandomMinMax(10, 20);

                // Heal itself
                Hits += healAmount;
                if (Hits > HitsMax)
                    Hits = HitsMax;

                // Heal allies
                foreach (Mobile m in GetMobilesInRange(10))
                {
                    if (m is BaseCreature && ((BaseCreature)m).Team == this.Team && m.Alive)
                    {
                        ((BaseCreature)m).Hits += healAmount;
                        if (((BaseCreature)m).Hits > ((BaseCreature)m).HitsMax)
                            ((BaseCreature)m).Hits = ((BaseCreature)m).HitsMax;

                        m.SendMessage("You feel a healing breeze from the Zephyr Warden.");
                    }
                }

                // Recalculate next Breeze Heal time
                Random rand = new Random();
                m_NextBreezeHeal = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(120, 180));
            }
        }

        private void CastGustBarrier()
        {
            if (Alive)
            {
                foreach (Mobile m in GetMobilesInRange(10))
                {
                    if (m is BaseCreature && ((BaseCreature)m).Team == this.Team)
                    {
                        Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
                        m.SendMessage("A gust barrier surrounds you, reducing incoming damage!");

                        ((BaseCreature)m).VirtualArmor += 20;
                    }
                }

                Timer.DelayCall(TimeSpan.FromMinutes(1), new TimerCallback(() => 
                {
                    foreach (Mobile m in GetMobilesInRange(10))
                    {
                        if (m is BaseCreature && ((BaseCreature)m).Team == this.Team)
                        {
                            ((BaseCreature)m).VirtualArmor -= 20;
                        }
                    }
                }));

                // Recalculate next Gust Barrier time
                Random rand = new Random();
                m_NextGustBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(180, 240));
            }
        }

        private void CastSoothingWind()
        {
            if (Alive)
            {
                foreach (Mobile m in GetMobilesInRange(10))
                {
                    if (m is BaseCreature && ((BaseCreature)m).Team == this.Team)
                    {
                        // Increase speed by modifying Dexterity temporarily
                        ((BaseCreature)m).SetDex(((BaseCreature)m).Dex + 20);

                        m.SendMessage("The Zephyr Warden's soothing wind increases your speed!");
                    }
                }

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(() => 
                {
                    foreach (Mobile m in GetMobilesInRange(10))
                    {
                        if (m is BaseCreature && ((BaseCreature)m).Team == this.Team)
                        {
                            // Revert Dexterity to original value
                            ((BaseCreature)m).SetDex(((BaseCreature)m).Dex - 20);
                        }
                    }
                }));

                // Recalculate next Soothing Wind time
                Random rand = new Random();
                m_NextSoothingWind = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(240, 300));
            }
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
