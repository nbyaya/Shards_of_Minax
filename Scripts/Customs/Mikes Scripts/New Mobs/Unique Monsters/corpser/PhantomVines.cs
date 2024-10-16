using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a phantom vines corpse")]
    public class PhantomVines : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextSpectralGrasp;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized
        private bool m_IsPhased;

        [Constructable]
        public PhantomVines()
            : this("Phantom Vines")
        {
        }

        [Constructable]
        public PhantomVines(string name)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 684;
            Hue = 1385; // Ethereal blue hue
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

            // Initialize ability timers
            m_AbilitiesInitialized = false;
        }

        public PhantomVines(Serial serial)
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
        public override int Hides { get { return 7; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                // Initialize ability timers with random intervals if not already initialized
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSpectralGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPhaseShift)
                {
                    PhaseShift();
                }

                if (DateTime.UtcNow >= m_NextSpectralGrasp && !m_IsPhased)
                {
                    SpectralGrasp(Combatant as Mobile);
                }
            }
        }

        public void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Phantom Vines phase in and out, making them hard to hit!*");
            PlaySound(0x37D);

            m_IsPhased = true;
            FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                m_IsPhased = false;
                FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
            });

            // Set the next PhaseShift time
            Random rand = new Random();
            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
        }

        public void SpectralGrasp(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 2))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Phantom Vines reach out with a spectral grasp!*");
            PlaySound(0x1FA);

            target.FixedParticles(0x376A, 10, 15, 5013, EffectLayer.Waist);
            target.PlaySound(0x1F8);

            int manaDrain = Utility.RandomMinMax(10, 20);
            target.Mana -= manaDrain;
            Mana += manaDrain;

            target.SendLocalizedMessage(1070848); // You feel the life force being drawn out of you!

            // Set the next SpectralGrasp time
            Random rand = new Random();
            m_NextSpectralGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 45));
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_IsPhased && Utility.RandomDouble() < 0.5)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The attack phases through Phantom Vines!*");
                PlaySound(0x37D);
            }
        }

        public override int GetIdleSound()
        {
            return 443;
        }

        public override int GetAngerSound()
        {
            return 442;
        }

        public override int GetHurtSound()
        {
            return 445;
        }

        public override int GetDeathSound()
        {
            return 447;
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

            // Reset the initialization flag and ability timers
            m_AbilitiesInitialized = false;
        }
    }
}
