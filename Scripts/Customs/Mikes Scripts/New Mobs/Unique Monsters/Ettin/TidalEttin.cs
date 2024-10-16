using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a tidal ettin corpse")]
    public class TidalEttin : BaseCreature
    {
        private DateTime m_NextTidalWave;
        private DateTime m_NextAquaShield;
        private DateTime m_NextDrowningGrip;
        private Timer m_AquaShieldTimer;
        private List<Mobile> m_DrowningVictims;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public TidalEttin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tidal ettin";
            Body = 18;
            BaseSoundID = 367;
            Hue = 1556; // Blue-green hue

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

            m_AbilitiesInitialized = false; // Initialize flag
            m_DrowningVictims = new List<Mobile>();
        }

        public TidalEttin(Serial serial)
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

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 4; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextTidalWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAquaShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextDrowningGrip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextTidalWave)
                {
                    TidalWave();
                }

                if (DateTime.UtcNow >= m_NextAquaShield)
                {
                    AquaShield();
                }

                if (DateTime.UtcNow >= m_NextDrowningGrip)
                {
                    DrowningGrip();
                }
            }

            // Process drowning damage
            for (int i = m_DrowningVictims.Count - 1; i >= 0; i--)
            {
                Mobile victim = m_DrowningVictims[i];
                if (victim.Alive && victim.InRange(this, 12))
                {
                    victim.Damage(Utility.RandomMinMax(5, 10), this);
                    victim.SendLocalizedMessage(1072121); // Gurgle gurgle... You're drowning!
                }
                else
                {
                    m_DrowningVictims.RemoveAt(i);
                }
            }
        }

        private void TidalWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tidal Ettin summons a massive wave! *");
            PlaySound(0x15E);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.SendLocalizedMessage(1072122); // A huge wave of water washes over you, knocking you off your feet!

                    m.FixedParticles(0x3779, 10, 15, 5052, EffectLayer.Waist);
                }
            }

            m_NextTidalWave = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown
        }

        private void AquaShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tidal Ettin surrounds itself with a shield of water! *");
            PlaySound(0x64);

            FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);

            m_AquaShieldTimer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 15, new TimerCallback(AquaShieldEffect));

            m_NextAquaShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set cooldown
        }

        private void AquaShieldEffect()
        {
            Hits = Math.Min(Hits + 10, HitsMax);
            FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
        }

        private void DrowningGrip()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tidal Ettin's watery grasp constricts its enemies! *");
            PlaySound(0x026);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    m.SendLocalizedMessage(1072123); // You feel the crushing grip of watery tentacles constricting you!

                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);

                    if (!m_DrowningVictims.Contains(m))
                    {
                        m_DrowningVictims.Add(m);
                    }

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075635, 1075636, TimeSpan.FromSeconds(10), m));
                }
            }

            m_NextDrowningGrip = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set cooldown
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (m_AquaShieldTimer != null)
            {
                m_AquaShieldTimer.Stop();
            }

            m_DrowningVictims.Clear();
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
            m_NextTidalWave = DateTime.UtcNow;
            m_NextAquaShield = DateTime.UtcNow;
            m_NextDrowningGrip = DateTime.UtcNow;
            m_DrowningVictims = new List<Mobile>();
        }
    }
}
