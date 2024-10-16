using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a hydrokinetic warden corpse")]
    public class HydrokineticWarden : BaseCreature
    {
        private DateTime m_NextPressureCrush;
        private DateTime m_NextWaveShield;
        private DateTime m_NextCrestingWave;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public HydrokineticWarden()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a hydrokinetic warden";
            this.Body = 16; // Water elemental body
            this.BaseSoundID = 278;
			Hue = 2504; // Blue hue for storm effect

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
            this.CanSwim = true;

            this.PackItem(new BlackPearl(5));

            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public HydrokineticWarden(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPressureCrush = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWaveShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextCrestingWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set the flag to true to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPressureCrush)
                {
                    PressureCrush();
                }

                if (DateTime.UtcNow >= m_NextWaveShield)
                {
                    WaveShield();
                }

                if (DateTime.UtcNow >= m_NextCrestingWave)
                {
                    CrestingWave();
                }
            }
        }

        private void PressureCrush()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    double pressureDamage = Utility.RandomMinMax(20, 30);
                    target.Damage((int)pressureDamage, this);
                    target.SendMessage("You feel an intense pressure crush you!");
                    target.Freeze(TimeSpan.FromSeconds(2));

                    m_NextPressureCrush = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown
                }
            }
        }

        private void WaveShield()
        {
            this.SendMessage("The hydrokinetic warden summons a protective wave shield!");

            // Example shield logic: reduce damage taken and reflect some damage
            this.VirtualArmor += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.VirtualArmor -= 20;
                this.SendMessage("The wave shield dissipates.");
            });

            m_NextWaveShield = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Update cooldown
        }

        private void CrestingWave()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A powerful wave crashes over you!");
                    m.Damage(Utility.RandomMinMax(30, 40), this);
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextCrestingWave = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Update cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_AbilitiesInitialized); // Save the initialization flag
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_AbilitiesInitialized = reader.ReadBool(); // Read the initialization flag
                    break;
            }
        }
    }
}
