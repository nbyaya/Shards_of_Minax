using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an Omega Sentinel corpse")]
    public class OmegaSentinel : BaseCreature
    {
        private DateTime m_NextEnergyShield;
        private DateTime m_NextPulseBlast;
        private bool m_EnergyShieldActive;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public OmegaSentinel()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Omega Sentinel";
            Body = 0x2F5; // ExodusMinion body
            BaseSoundID = 0x2F8;
            Hue = 2721; // Unique metallic blue hue

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

            m_AbilitiesInitialized = false;
        }

        public OmegaSentinel(Serial serial)
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
        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }


        public override int GetIdleSound()
        {
            return 0x2FF;
        }

        public override int GetAngerSound()
        {
            return 0x4D8;
        }

        public override int GetDeathSound()
        {
            return 0x4D9;
        }

        public override int GetAttackSound()
        {
            return 0x4D7;
        }

        public override int GetHurtSound()
        {
            return 0x4DA;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEnergyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPulseBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextEnergyShield)
                {
                    ActivateEnergyShield();
                }

                if (DateTime.UtcNow >= m_NextPulseBlast)
                {
                    PulseBlast();
                }
            }
        }

        private void ActivateEnergyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Energy Shield Activated *");
            PlaySound(0x1E8);
            FixedEffect(0x376A, 10, 15, 5052, 0); // Use 0 or the appropriate integer value for the effect layer

            m_EnergyShieldActive = true;

            for (int i = 0; i < 5; i++)
            {
                ResistanceMod mod = new ResistanceMod(GetResistanceType(i), 20);
                AddResistanceMod(mod);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(DeactivateEnergyShield));

            Random rand = new Random();
            m_NextEnergyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60)); // Random cooldown
        }

        private void DeactivateEnergyShield()
        {
            m_EnergyShieldActive = false;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Energy Shield Deactivated *");

            for (int i = 0; i < 5; i++)
            {
                RemoveResistanceMod(new ResistanceMod(GetResistanceType(i), 20));
            }
        }

        private ResistanceType GetResistanceType(int index)
        {
            switch (index)
            {
                case 0: return ResistanceType.Physical;
                case 1: return ResistanceType.Fire;
                case 2: return ResistanceType.Cold;
                case 3: return ResistanceType.Poison;
                case 4: return ResistanceType.Energy;
                default: return ResistanceType.Physical;
            }
        }

        private void PulseBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Pulse Blast Charging *");
            PlaySound(0x2A);
            FixedEffect(0x3709, 10, 30, 5052, 0); // Use 0 or the appropriate integer value for the effect layer

            Timer.DelayCall(TimeSpan.FromSeconds(1.5), new TimerCallback(DoPulseBlast));

            Random rand = new Random();
            m_NextPulseBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60)); // Random cooldown
        }

        private void DoPulseBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Pulse Blast Released *");
            PlaySound(0x665);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(30, 60);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                    m.PlaySound(0x213);

                    if (m.Player)
                    {
                        m.SendLocalizedMessage(1072073); // You have been knocked back by an explosion!
                        m.MoveToWorld(GetSpawnPosition(2), Map);
                    }
                }
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Location;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_EnergyShieldActive)
            {
                PlaySound(0x1E1);
                FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
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

            m_NextEnergyShield = DateTime.UtcNow;
            m_NextPulseBlast = DateTime.UtcNow;
            m_EnergyShieldActive = false;
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
