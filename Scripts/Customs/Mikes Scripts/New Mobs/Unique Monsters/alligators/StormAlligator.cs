using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a storm alligator corpse")]
    public class StormAlligator : BaseCreature
    {
        private DateTime m_NextLightningStrike;
        private DateTime m_NextStormAura;
        private bool m_AbilitiesActivated; // Flag to track if abilities have been initialized

        [Constructable]
        public StormAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm alligator";
            Body = 0xCA; // Alligator body
            Hue = 1162; // Unique stormy hue
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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public StormAlligator(Serial serial)
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
        public override int Hides { get { return 20; } }
        public override HideType HideType { get { return HideType.Spined; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesActivated)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));
                    m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 30));
                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLightningStrike)
                {
                    LightningStrike();
                }

                if (DateTime.UtcNow >= m_NextStormAura)
                {
                    StormAura();
                }
            }
        }

        private void LightningStrike()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.Damage(Utility.RandomMinMax(15, 25), this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The storm alligator calls down lightning! *");
                    m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

        private void StormAura()
        {
            if (Combatant != null)
            {
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive && m != Combatant)
                    {
                        m.Damage(Utility.RandomMinMax(5, 10), this);
                        m.SendMessage("You are struck by the storm aura of the alligator!");
                    }
                }

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The storm alligator creates a stormy aura around it! *");
                m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(60);
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

            m_AbilitiesActivated = false; // Reset flag on deserialize
        }
    }
}
