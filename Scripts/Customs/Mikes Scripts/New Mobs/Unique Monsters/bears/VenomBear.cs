using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a venom bear corpse")]
    public class VenomBear : BaseCreature
    {
        private DateTime m_NextToxicSpit;
        private DateTime m_NextPoisonousAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VenomBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venom bear";
            Body = 211; // BlackBear body
            Hue = 1177; // Unique hue for visual distinction
			BaseSoundID = 0xA3;

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

            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public VenomBear(Serial serial)
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
                    m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPoisonousAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextToxicSpit)
                {
                    ToxicSpit();
                }

                if (DateTime.UtcNow >= m_NextPoisonousAura)
                {
                    PoisonousAura();
                }
            }
        }

        private void ToxicSpit()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile;
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                target.SendMessage("You have been hit by the Venom Bear's toxic spit!");
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Venom Bear spits toxic poison *");
                target.FixedEffect(0x376A, 10, 16); // Green toxic visual effect

                m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ToxicSpit
            }
        }

        private void PoisonousAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Venom Bear emits a poisonous aura *");
            FixedEffect(0x376A, 10, 16); // Green mist visual effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(5, 15);
                    m.Damage(damage, this);
                    m.SendMessage("You are poisoned by the Venom Bear's aura!");
                }
            }

            m_NextPoisonousAura = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for PoisonousAura
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
