using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a warthog warrior corpse")]
    public class WarthogWarrior : BaseCreature
    {
        private DateTime m_NextTuskCharge;
        private DateTime m_NextSandstorm;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public WarthogWarrior()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a warthog warrior";
            Body = 0xCB; // Pig body
            Hue = 2187; // Brown hue
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

        public WarthogWarrior(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextTuskCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTuskCharge)
                {
                    TuskCharge();
                }

                if (DateTime.UtcNow >= m_NextSandstorm)
                {
                    Sandstorm();
                }
            }
        }

        private void TuskCharge()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile;
                target.Damage(30, this);
                target.SendMessage("The Warthog Warrior charges at you with its tusks!");
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Tusk Charge! *");
                PlaySound(0x1D4);
                m_NextTuskCharge = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set cooldown after use
            }
        }

        private void Sandstorm()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Player)
                {
                    m.SendMessage("You are caught in the Warthog Warrior's sandstorm!");
                    m.FixedEffect(0x373A, 10, 16);
                    // Reduces accuracy by applying a debuff or similar effect here
                }
            }
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sandstorm! *");
            PlaySound(0x2B4);
            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown after use
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
