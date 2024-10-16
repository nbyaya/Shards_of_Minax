using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an electro wraith corpse")]
    public class ElectroWraith : BaseCreature
    {
        private DateTime m_NextChainLightning;
        private DateTime m_NextStaticDischarge;

        private bool m_AbilitiesInitialized;
        private Dictionary<string, DateTime> m_Cooldowns;

        [Constructable]
        public ElectroWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Electro Wraith";
            Body = 0x2F4; // ExodusOverseer body
            BaseSoundID = 0x482;
            Hue = 2294; // Electric blue hue

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

            PackItem(new PowerCrystal());
            PackItem(new ArcaneGem());

            m_AbilitiesInitialized = false; // Initialize flag
            m_Cooldowns = new Dictionary<string, DateTime>();
        }

        public ElectroWraith(Serial serial)
            : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0xFD;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x23B;
        }

        public override int GetHurtSound()
        {
            return 0x140;
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

        public override bool BardImmune { get { return !Core.AOS; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random start times for abilities
                    Random rand = new Random();
                    m_NextChainLightning = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStaticDischarge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                // Constant electricity effect
                if (!IsCooldown("ElectricityEffect"))
                {
                    FixedParticles(0x3818, 1, 12, 9963, 31, 7, EffectLayer.Waist);
                    SetCooldown("ElectricityEffect", TimeSpan.FromSeconds(2));
                }

                if (DateTime.UtcNow >= m_NextChainLightning)
                {
                    DoChainLightning();
                }

                if (DateTime.UtcNow >= m_NextStaticDischarge)
                {
                    DoStaticDischarge();
                }
            }
        }

        private void DoChainLightning()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Chain Lightning *");
            PlaySound(0x5CE);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(8);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m) && m.Player)
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            if (targets.Count > 0)
            {
                int damage = Utility.RandomMinMax(30, 40);
                for (int i = 0; i < Math.Min(targets.Count, 3); i++)
                {
                    Mobile target = targets[i];
                    target.BoltEffect(0);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    damage = (int)(damage * 0.75); // 25% less damage for each jump
                }
            }

            m_NextChainLightning = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        private void DoStaticDischarge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Static Discharge *");
            PlaySound(0x201);

            FixedParticles(0x3818, 1, 12, 9963, 31, 7, EffectLayer.Waist);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1072070); // You have been stunned by a colossal blow!
                    m.Freeze(TimeSpan.FromSeconds(3));

                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 100);
                }
            }

            m_NextStaticDischarge = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        private bool IsCooldown(string effectName)
        {
            if (m_Cooldowns.TryGetValue(effectName, out DateTime cooldownEnd))
            {
                return DateTime.UtcNow < cooldownEnd;
            }
            return false;
        }

        private void SetCooldown(string effectName, TimeSpan duration)
        {
            m_Cooldowns[effectName] = DateTime.UtcNow + duration;
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
            m_NextChainLightning = DateTime.UtcNow;
            m_NextStaticDischarge = DateTime.UtcNow;
            m_Cooldowns = new Dictionary<string, DateTime>(); // Ensure this is reinitialized
        }
    }
}
