using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a steel bear corpse")]
    public class SteelBear : BaseCreature
    {
        private DateTime m_NextMetallicRoar;
        private DateTime m_NextIronClaw;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SteelBear()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a steel bear";
            Body = 211; // BlackBear body
            Hue = 1182; // Steel hue

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			BaseSoundID = 0xA3;
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
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

        public SteelBear(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                    m_NextMetallicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextIronClaw = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextMetallicRoar)
                {
                    MetallicRoar();
                }

                if (DateTime.UtcNow >= m_NextIronClaw)
                {
                    IronClaw();
                }
            }
        }

        private void MetallicRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Steel Bear lets out a deafening metallic roar! *");
            PlaySound(0x2D8); // Example roar sound
            FixedEffect(0x3B2, 10, 16);

            // Increase the bear's defense temporarily
            VirtualArmor += 10;

            // Confuse enemies within range
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.Player)
                {
                    m.SendMessage("You are confused by the Steel Bear's roar!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextMetallicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset with a fixed cooldown
        }

        private void IronClaw()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Steel Bear slashes with its glowing iron claws! *");
            PlaySound(0x2D9); // Example claw sound
            FixedEffect(0x2D9, 10, 16);

            // Reduce the target's armor
            Mobile target = Combatant as Mobile;
            if (target != null)
            {
                target.VirtualArmor -= 5;
                target.SendMessage("Your armor is reduced by the Steel Bear's attack!");
            }

            m_NextIronClaw = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset with a fixed cooldown
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

            m_AbilitiesInitialized = false; // Reset the flag on deserialize to reinitialize random intervals
        }
    }
}
