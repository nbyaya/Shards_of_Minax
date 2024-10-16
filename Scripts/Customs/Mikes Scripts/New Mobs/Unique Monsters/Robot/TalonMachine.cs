using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a talon machine corpse")]
    public class TalonMachine : BaseCreature
    {
        private DateTime m_NextRapidAssault;
        private DateTime m_NextAmbushProtocol;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TalonMachine()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a talon machine";
            Body = 0x2F5; // ExodusMinion body
            Hue = 2272; // Unique hue

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

        public TalonMachine(Serial serial)
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
        public override int GetIdleSound()
        {
            return 0x218;
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
            return 0x232;
        }

        public override int GetHurtSound()
        {
            return 0x140;
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
                    m_NextRapidAssault = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextAmbushProtocol = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRapidAssault)
                {
                    PerformRapidAssault();
                }

                if (DateTime.UtcNow >= m_NextAmbushProtocol && this.Hidden)
                {
                    ActivateAmbushProtocol();
                }
            }
        }

        private void PerformRapidAssault()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Rapid Assault! *");
            FixedEffect(0x3735, 10, 30);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    if (Combatant != null)
                    {
                        // Custom logic to deal damage to the combatant or simulate an attack
                        Combatant.Damage(10, this); // Example to manually deal damage
                    }
                });
            }

            m_NextRapidAssault = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set fixed cooldown for next use
        }

        private void ActivateAmbushProtocol()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ambush Protocol Activated! *");
            FixedEffect(0x376A, 10, 16);

            SetDamage(DamageMin + 10, DamageMax + 10);

            // Set fixed cooldown for next use
            m_NextAmbushProtocol = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (this.Hidden)
            {
                // Show the effect of ambush protocol when the monster gets attacked
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ambushed! *");
                FixedEffect(0x3735, 10, 30);
                this.Hidden = false;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (0.2 > Utility.RandomDouble())
            {
                c.DropItem(new MechanicalComponent());
            }
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
        }
    }
}
