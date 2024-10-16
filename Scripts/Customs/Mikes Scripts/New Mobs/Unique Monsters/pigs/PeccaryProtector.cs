using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a peccary protector corpse")]
    public class PeccaryProtector : BaseCreature
    {
        private DateTime m_NextDefensiveShield;
        private DateTime m_NextRallyCry;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PeccaryProtector()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a peccary protector";
            Body = 0xCB; // Pig body
            Hue = 2189; // Gray-Brown hue
            BaseSoundID = 0xC4;

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

        public PeccaryProtector(Serial serial)
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
                    m_NextDefensiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRallyCry = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDefensiveShield)
                {
                    DefensiveShield();
                }

                if (DateTime.UtcNow >= m_NextRallyCry)
                {
                    RallyCry();
                }
            }
        }

        private void DefensiveShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a protective shield! *");
            // Add effects for the defensive shield here (e.g., visual effects)

            // You can add logic to absorb damage or apply any other effects here

            m_NextDefensiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for DefensiveShield
        }

        private void RallyCry()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Boosts the attack power of nearby allies! *");
            
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature bc && bc != this && bc.Hits > 0)
                {
                    // Increase the damage output of nearby allies
                    bc.SetDamage(bc.DamageMin + 5, bc.DamageMax + 10);
                }
            }

            m_NextRallyCry = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for RallyCry
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
