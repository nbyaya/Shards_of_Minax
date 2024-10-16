using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a plasma juggernaut corpse")]
    public class PlasmaJuggernaut : BaseCreature
    {
        private DateTime m_NextPlasmaCannon;
        private DateTime m_NextEnergyShield;
        private bool m_ShieldActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PlasmaJuggernaut()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a plasma juggernaut";
            Body = 0x2F4; // Exodus Overseer body
            Hue = 2285; // Unique hue

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
        }

        public PlasmaJuggernaut(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextPlasmaCannon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEnergyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPlasmaCannon)
                {
                    PlasmaCannon();
                }

                if (DateTime.UtcNow >= m_NextEnergyShield && !m_ShieldActive)
                {
                    EnergyShield();
                }
            }
        }

        private void PlasmaCannon()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Fires Plasma Cannon! *");
                this.FixedParticles(0x3709, 10, 15, 0x1F4, EffectLayer.Head);
                this.PlaySound(0x2F4);

                // Deal high damage and apply burn effect
                AOS.Damage(Combatant, this, Utility.RandomMinMax(50, 70), 0, 0, 0, 0, 100);

                // Chance to burn target
                if (Utility.RandomDouble() < 0.3) // 30% chance to burn
                {
                    Mobile target = Combatant as Mobile; // Cast Combatant to Mobile
                    if (target != null)
                    {
                        target.SendMessage("You are burning from the plasma cannon!");
                        target.SendMessage(0x22, "You are burning from the plasma cannon!");
                        target.ApplyPoison(this, Poison.Lethal); // Apply a burning effect
                    }
                }

                // Set the next plasma cannon time
                m_NextPlasmaCannon = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Adjust the interval as needed
            }
        }

        private void EnergyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Activates Energy Shield! *");
            this.FixedParticles(0x376A, 10, 16, 0x2B2, EffectLayer.Waist);
            this.PlaySound(0x1F2);

            m_ShieldActive = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                m_ShieldActive = false;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Energy Shield Deactivated! *");
                this.FixedParticles(0x3735, 10, 30, 0x251F, EffectLayer.Waist);
            });

            // Set the next energy shield time
            m_NextEnergyShield = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Adjust the interval as needed
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_ShieldActive)
            {
                // Absorb a percentage of incoming damage while shield is active
                amount = (int)(amount * 0.5); // Example: reduce damage by 50%
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_ShieldActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ShieldActive = reader.ReadBool();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
