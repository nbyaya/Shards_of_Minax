using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a vile blossom corpse")]
    public class VileBlossom : BaseCreature
    {
        private DateTime m_NextBlossomBurst;
        private DateTime m_NextPollinateFear;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VileBlossom()
            : this("Vile Blossom")
        {
        }

        [Constructable]
        public VileBlossom(string name)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 684;
            Hue = 1381; // Dark green hue
            this.Body = 8;
			BaseSoundID = 0x4F2;

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

        public VileBlossom(Serial serial)
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
        public override int Hides { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.None; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextBlossomBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Randomize initial cooldown
                    m_NextPollinateFear = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Randomize initial cooldown
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBlossomBurst)
                {
                    BlossomBurst();
                }

                if (DateTime.UtcNow >= m_NextPollinateFear)
                {
                    PollinateFear();
                }
            }
        }

        private void BlossomBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Vile Blossom unleashes a toxic bloom!*");
            FixedEffect(0x36D4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Head);
                    m.PlaySound(0x231);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                }
            }

            m_NextBlossomBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown after ability use
        }

        private void PollinateFear()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Vile Blossom releases a wave of fear!*");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You feel a wave of fear wash over you!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextPollinateFear = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Set cooldown after ability use
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
