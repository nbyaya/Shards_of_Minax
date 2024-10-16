using System;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a tsunami titan corpse")]
    public class TsunamiTitan : BaseCreature
    {
        private DateTime m_NextTsunamiWave;
        private DateTime m_NextWaveformStrike;
        private DateTime m_NextOceanicGuard;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TsunamiTitan()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tsunami titan";
            Body = 16; // Water Elemental body
            BaseSoundID = 278;
			Hue = 2471; // Blue hue for storm effect

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
            CanSwim = true;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public TsunamiTitan(Serial serial)
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
                    m_NextTsunamiWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWaveformStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextOceanicGuard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTsunamiWave)
                {
                    CastTsunamiWave();
                }

                if (DateTime.UtcNow >= m_NextWaveformStrike)
                {
                    PerformWaveformStrike();
                }

                if (DateTime.UtcNow >= m_NextOceanicGuard)
                {
                    CastOceanicGuard();
                }
            }
        }

        private void CastTsunamiWave()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.Player)
                {
                    m.Damage(Utility.RandomMinMax(50, 70), this);
                    m.SendMessage("A massive wave crashes over you!");
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x2D0C, 10, 30, 1153);
                }
            }

            m_NextTsunamiWave = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown
        }

        private void PerformWaveformStrike()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    target.Damage(damage, this);
                    target.SendMessage("You are hit by a powerful water strike!");

                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x2D0C, 10, 30, 1153);
                    Effects.PlaySound(target, target.Map, 0x308);
                }

                m_NextWaveformStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown
            }
        }

        private void CastOceanicGuard()
        {
            this.VirtualArmor += 40;
            this.SendMessage("The Tsunami Titan is surrounded by an oceanic shield!");

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                this.VirtualArmor -= 40;
                this.SendMessage("The oceanic shield dissipates.");
            });

            m_NextOceanicGuard = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Fixed cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.Write(m_NextTsunamiWave);
            writer.Write(m_NextWaveformStrike);
            writer.Write(m_NextOceanicGuard);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextTsunamiWave = reader.ReadDateTime();
            m_NextWaveformStrike = reader.ReadDateTime();
            m_NextOceanicGuard = reader.ReadDateTime();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
