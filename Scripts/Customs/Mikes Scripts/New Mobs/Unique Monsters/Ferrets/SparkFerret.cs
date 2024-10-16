using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a spark ferret corpse")]
    public class SparkFerret : BaseCreature
    {
        private DateTime m_NextStaticShock;
        private DateTime m_NextElectrifyAura;
        private DateTime m_NextElectricalSurge;
        private bool m_ElectrifyAuraActive;
        private bool m_IsSurging;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SparkFerret()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a spark ferret";
            Body = 0x117; // Ferret body
            Hue = 1568; // Unique hue for Spark Ferret
            BaseSoundID = 0xCF;

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

            // Initialize ability timers and flags
            m_AbilitiesInitialized = false;
            m_ElectrifyAuraActive = false;
            m_IsSurging = false;
        }

        public SparkFerret(Serial serial)
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
                    m_NextStaticShock = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextElectrifyAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextElectricalSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextStaticShock)
                {
                    StaticShock();
                    m_NextStaticShock = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set cooldown after use
                }

                if (DateTime.UtcNow >= m_NextElectrifyAura)
                {
                    ToggleElectrifyAura();
                    m_NextElectrifyAura = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown after use
                }

                if (DateTime.UtcNow >= m_NextElectricalSurge)
                {
                    ElectricalSurge();
                    m_NextElectricalSurge = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set cooldown after use
                }
            }

            if (m_ElectrifyAuraActive)
            {
                ApplyElectrifyAura();
            }

            if (m_IsSurging)
            {
                ApplySurgeEffects();
            }
        }

        private void StaticShock()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Discharges a powerful burst of static electricity! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(5, 15);
                    m.Damage(damage, this);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    if (Utility.RandomDouble() < 0.5)
                    {
                        m.SendMessage("You are stunned by the electric shock!");
                        m.Freeze(TimeSpan.FromSeconds(1));
                    }
                }
            }
        }

        private void ToggleElectrifyAura()
        {
            m_ElectrifyAuraActive = !m_ElectrifyAuraActive;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, m_ElectrifyAuraActive ? "* Electrify Aura activated! *" : "* Electrify Aura deactivated! *");
        }

        private void ApplyElectrifyAura()
        {
            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are shocked by the electrified aura!");
                    m.Damage(3, this);
                }
            }
        }

        private void ElectricalSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Electrical Surge! *");
            FixedEffect(0x376A, 10, 16);
            PlaySound(0x29F);

            m_IsSurging = true;
            SetDamage(DamageMin + 10, DamageMax + 10);
            SetStr(Str + 30);
            SetDex(Dex + 20);
            SetInt(Int + 10);

            Timer.DelayCall(TimeSpan.FromSeconds(15), EndElectricalSurge);
        }

        private void EndElectricalSurge()
        {
            m_IsSurging = false;
            SetDamage(DamageMin - 10, DamageMax - 10);
            SetStr(Str - 30);
            SetDex(Dex - 20);
            SetInt(Int - 10);
        }

        private void ApplySurgeEffects()
        {
            // Additional effects during the surge
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.SendMessage("The Spark Ferret's surge of electricity makes your skin tingle!");
                        m.Damage(5, this);
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // No need to save state of random timers as they will be reinitialized on load
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset initialization flag
            m_ElectrifyAuraActive = false;
            m_IsSurging = false;
        }
    }
}
