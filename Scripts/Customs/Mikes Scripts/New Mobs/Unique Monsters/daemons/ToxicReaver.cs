using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a toxic reaver corpse")]
    public class ToxicReaver : BaseCreature
    {
        private DateTime m_NextPoisonCloud;
        private DateTime m_NextToxicBite;
        private DateTime m_NextVenomousAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ToxicReaver()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a toxic reaver";
            Body = 9; // Daemon body
            Hue = 1463; // Unique hue
			BaseSoundID = 357;

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

        public ToxicReaver(Serial serial)
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

        public override double DispelDifficulty => 125.0;
        public override double DispelFocus => 45.0;
        public override int Meat => 1;
        public override bool CanFly => true;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextToxicBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextVenomousAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPoisonCloud)
                {
                    CastPoisonCloud();
                }

                if (DateTime.UtcNow >= m_NextToxicBite)
                {
                    PerformToxicBite();
                }

                if (DateTime.UtcNow >= m_NextVenomousAura)
                {
                    ActivateVenomousAura();
                }
            }
        }

        private void CastPoisonCloud()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed by a poisonous cloud!");
                    m.ApplyPoison(this, Poison.Lethal);
                    m.Damage(10, this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Reaver releases a cloud of poison! *");

            m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown
        }

        private void PerformToxicBite()
        {
            if (Combatant != null && Combatant is Mobile target)
            {
                if (target.Alive)
                {
                    target.SendMessage("The Toxic Reaver bites you with venomous fangs!");
                    target.ApplyPoison(this, Poison.Lethal);
                    target.Damage(15, this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Reaver bites with deadly venom! *");

            m_NextToxicBite = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Fixed cooldown
        }

        private void ActivateVenomousAura()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You feel a burning sensation as the venomous aura affects you!");
                    m.ApplyPoison(this, Poison.Regular);
                    m.Damage(5, this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Reaver radiates a poisonous aura! *");

            m_NextVenomousAura = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
