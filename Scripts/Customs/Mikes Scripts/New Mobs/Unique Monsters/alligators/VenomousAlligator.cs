using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a venomous alligator corpse")]
    public class VenomousAlligator : BaseCreature
    {
        private DateTime m_NextVenomousBite;
        private DateTime m_NextPoisonCloud;
        private bool m_AbilitiesInitialized; // New flag to track initial setup

        [Constructable]
        public VenomousAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venomous alligator";
            Body = 0xCA; // Alligator body
            Hue = 1159; // Unique hue for venomous appearance
            BaseSoundID = 660;

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

        public VenomousAlligator(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Spined; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));
                    m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initialization
                }

                if (DateTime.UtcNow >= m_NextVenomousBite)
                {
                    PerformVenomousBite();
                }

                if (DateTime.UtcNow >= m_NextPoisonCloud)
                {
                    CreatePoisonCloud();
                }
            }
        }

        private void PerformVenomousBite()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("You feel a sharp, venomous bite from the alligator!");
                    target.Damage(Utility.RandomMinMax(10, 15), this);

                    // Apply poison
                    Poison poison = Poison.Greater;
                    target.ApplyPoison(this, poison);

                    // Display message above the alligator's head
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Venomous bite! *");
                    m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Reset with a new random interval
                }
            }
        }

        private void CreatePoisonCloud()
        {
            if (Combatant != null)
            {
                Point3D loc = Location;
                Map map = Map;

                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 0x13F5);

                // Apply poison damage over time
                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Player)
                    {
                        m.SendMessage("You are engulfed by a poisonous cloud!");
                        Poison poison = Poison.Greater;
                        m.ApplyPoison(this, poison);
                        m.Damage(Utility.RandomMinMax(5, 10), this);
                    }
                }

                // Display message above the alligator's head
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Poison cloud! *");
                m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset with a new random interval
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

            m_AbilitiesInitialized = false; // Reset flag
        }
    }
}
