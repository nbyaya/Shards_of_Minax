using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bloodthirsty vines corpse")]
    public class BloodthirstyVines : BaseCreature
    {
        private DateTime m_NextLifeDrain;
        private bool m_Bloodlust;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BloodthirstyVines()
            : this("Bloodthirsty Vines")
        {
        }

        [Constructable]
        public BloodthirstyVines(string name)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 684;
            Hue = 1388; // Dark red hue
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

        public BloodthirstyVines(Serial serial)
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
        public override int Hides { get { return 5; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20)); // Random start time for LifeDrain
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLifeDrain)
                {
                    LifeDrain(Combatant as Mobile);
                }
            }
        }

        public void LifeDrain(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 2))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Bloodthirsty Vines drain your life!*");
            PlaySound(0x1FA);

            int damage = Utility.RandomMinMax(20, 30);
            int heal = damage / 2;

            target.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
            target.PlaySound(0x231);

            AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
            Hits += heal;

            if (!m_Bloodlust)
            {
                m_Bloodlust = true;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Bloodthirsty Vines enter a frenzy!*");
                DamageMax += 5;
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
                {
                    m_Bloodlust = false;
                    DamageMax -= 5;
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Bloodthirsty Vines calm down*");
                });
            }

            // Update the cooldown for LifeDrain
            m_NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(15);
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
