using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a venomous ivy corpse")]
    public class VenomousIvy : BaseCreature
    {
        private DateTime m_NextPoisonSpray;
        private DateTime m_NextToxicTouch;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VenomousIvy() : this("Venomous Ivy")
        {
        }

        [Constructable]
        public VenomousIvy(string name) : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 684;
            Hue = 1382; // Dark green hue
            this.Body = 8;
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

        public VenomousIvy(Serial serial) : base(serial)
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
                    m_NextPoisonSpray = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20)); // Random interval between 5 and 20 seconds
                    m_NextToxicTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30)); // Random interval between 10 and 30 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPoisonSpray)
                {
                    PoisonSpray();
                }

                if (DateTime.UtcNow >= m_NextToxicTouch)
                {
                    ToxicTouch();
                }
            }
        }

        private void PoisonSpray()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Venomous Ivy releases a toxic mist!*");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    m.SendMessage("You are poisoned by the toxic mist of the Venomous Ivy!");
                    m.ApplyPoison(this, Poison.Lesser);
                }
            }

            m_NextPoisonSpray = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for PoisonSpray
        }

        private void ToxicTouch()
        {
            if (Combatant == null || !InRange(Combatant, 2))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Venomous Ivy poisons you with its touch!*");
            PlaySound(0x20B);

            int poisonDamage = Utility.RandomMinMax(10, 20);
            Combatant.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
            Combatant.PlaySound(0x231);

            AOS.Damage(Combatant, this, poisonDamage, 0, 0, 0, 0, 100);

            m_NextToxicTouch = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for ToxicTouch
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 5; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

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
