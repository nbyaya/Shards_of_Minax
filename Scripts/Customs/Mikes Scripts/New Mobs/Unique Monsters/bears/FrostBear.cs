using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frost bear corpse")]
    public class FrostBear : BaseCreature
    {
        private DateTime m_NextFrostBreath;
        private DateTime m_NextGlacialShield;
        private bool m_HasShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frost bear";
            Body = 211; // BlackBear body
            Hue = 1195; // Icy blue hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FrostBear(Serial serial) : base(serial) { }

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
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextGlacialShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }

                if (DateTime.UtcNow >= m_NextGlacialShield && !m_HasShield)
                {
                    ActivateGlacialShield();
                }
            }
        }

        private void FrostBreath()
        {
            if (Combatant != null && Combatant.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Frost Breath! *");
                FixedEffect(0x376A, 10, 16); // Icy effect
                Combatant.FixedEffect(0x376A, 10, 16); // Icy effect on target

                Mobile target = Combatant as Mobile; // Cast Combatant to Mobile
                if (target != null)
                {
                    target.SendMessage("You are hit by the frost bear's chilling breath!");
                    target.Damage(Utility.RandomMinMax(10, 20), this);

                    // Slow down the target
                    target.SendMessage("You feel your movements slowing down!");
                    // You may need to replace or remove the Freeze method if it doesn't exist
                    // target.Freeze(TimeSpan.FromSeconds(2));
                }

                m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown for FrostBreath
            }
        }

        private void ActivateGlacialShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Glacial Shield Activated! *");
            FixedEffect(0x376A, 10, 16); // Ice shield effect

            m_HasShield = true;
            this.VirtualArmor += 40; // Increase armor

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(() =>
            {
                if (Combatant != null && Combatant.Alive)
                {
                    Combatant.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }));

            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(() =>
            {
                if (m_HasShield)
                {
                    DeactivateGlacialShield();
                }
            }));

            m_NextGlacialShield = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set cooldown for GlacialShield
        }

        private void DeactivateGlacialShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Glacial Shield Deactivated! *");
            this.VirtualArmor -= 40;
            m_HasShield = false;
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
