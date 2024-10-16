using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an Abyssinian Tracker corpse")]
    public class AbyssinianTracker : BaseCreature
    {
        private DateTime m_NextBlindingSpeed;
        private DateTime m_NextPredatorsFocus;
        private DateTime m_NextTrackingPounce;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AbyssinianTracker()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Abyssinian Tracker";
            Body = 0xC9; // Cat body
            Hue = 1357; // Unique red hue
			BaseSoundID = 0x69;

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

        public AbyssinianTracker(Serial serial)
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
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextBlindingSpeed = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPredatorsFocus = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextTrackingPounce = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBlindingSpeed)
                {
                    BlindingSpeed();
                }

                if (DateTime.UtcNow >= m_NextPredatorsFocus)
                {
                    PredatorsFocus();
                }

                if (DateTime.UtcNow >= m_NextTrackingPounce)
                {
                    TrackingPounce();
                }
            }
        }

        private void BlindingSpeed()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Abyssinian Tracker moves too fast to follow! *");
            PlaySound(0x47D);
            FixedEffect(0x376A, 9, 32);

            double speedBoost = 2.0; // Double the movement speed
            double oldSpeed = ActiveSpeed;
            ActiveSpeed /= speedBoost;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                ActiveSpeed = oldSpeed;
            });

            m_NextBlindingSpeed = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown after use
        }

        private void PredatorsFocus()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Abyssinian Tracker focuses intently on its prey! *");
            PlaySound(0x634);
            FixedEffect(0x37C4, 10, 19);

            // Apply temporary damage increase
            Timer.DelayCall(TimeSpan.FromSeconds(15), () =>
            {
                // Restore to normal damage
                SetDamage(12, 16);
            });

            // Increase damage temporarily
            SetDamage(20, 30);

            m_NextPredatorsFocus = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown after use
        }

        private void TrackingPounce()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Abyssinian Tracker pounces with deadly precision! *");
                PlaySound(0x73);
                
                Direction = GetDirectionTo(target);
                
                // Adjust movement to the target's direction
                Move(Direction);

                Timer.DelayCall(TimeSpan.Zero, () =>
                {
                    if (InRange(target, 1))
                    {
                        FixedEffect(0x36BD, 10, 16);
                        int damage = Utility.RandomMinMax(20, 30);
                        AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                        target.MovingEffect(this, 0x36D4, 7, 0, false, false);
                        target.MoveToWorld(new Point3D(target.X + Utility.RandomMinMax(-2, 2), target.Y + Utility.RandomMinMax(-2, 2), target.Z), target.Map);
                    }
                });
            }

            m_NextTrackingPounce = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown after use
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
