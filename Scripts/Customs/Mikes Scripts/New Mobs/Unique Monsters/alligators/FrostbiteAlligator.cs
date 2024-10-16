using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frostbite alligator corpse")]
    public class FrostbiteAlligator : BaseCreature
    {
        private DateTime m_NextFrostBite;
        private DateTime m_NextIceShards;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostbiteAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frostbite alligator";
            Body = 0xCA; // Alligator body
            BaseSoundID = 660;

            Hue = 1170; // Unique hue for the frostbite alligator

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

        public FrostbiteAlligator(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextFrostBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextIceShards = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostBite)
                {
                    FrostBite();
                }

                if (DateTime.UtcNow >= m_NextIceShards)
                {
                    IceShards();
                }
            }
        }

        private void FrostBite()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("You are bitten by the frostbite alligator and feel a chilling freeze!");
                    target.Freeze(TimeSpan.FromSeconds(3)); // Freezing effect for 3 seconds
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Frostbite attack slows you down! *");
                    m_NextFrostBite = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for FrostBite
                }
            }
        }

        private void IceShards()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("Ice shards pierce through you, causing cold damage!");
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100); // Cold damage
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ice shards attack inflicts cold damage! *");
                    m_NextIceShards = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for IceShards
                }
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
