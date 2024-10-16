using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an acidic alligator corpse")]
    public class AcidicAlligator : BaseCreature
    {
        private DateTime m_NextAcidicBite;
        private DateTime m_NextAcidSpray;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AcidicAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an acidic alligator";
            Body = 0xCA; // Alligator body
            Hue = 1174; // Unique hue for the acidic alligator
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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public AcidicAlligator(Serial serial)
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
        public override int Hides { get { return 15; } }
        public override HideType HideType { get { return HideType.Regular; } } // Use a valid HideType here
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Initialize random start times for abilities
                    Random rand = new Random();
                    m_NextAcidicBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextAcidSpray = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAcidicBite)
                {
                    PerformAcidicBite();
                }

                if (DateTime.UtcNow >= m_NextAcidSpray)
                {
                    PerformAcidSpray();
                }
            }
        }

        private void PerformAcidicBite()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile; // Cast to Mobile

                if (target != null)
                {
                    target.SendMessage("You feel a burning sensation as the acidic alligator bites you!");
                    target.Damage(Utility.RandomMinMax(5, 10), this);
                    ApplyAcidDamage(target, 10);

                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The acidic alligator bites viciously, causing lingering damage! *");
                    m_NextAcidicBite = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for Acidic Bite
                }
            }
        }

        private void PerformAcidSpray()
        {
            if (Combatant != null)
            {
                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Player)
                    {
                        m.SendMessage("You are engulfed in a cloud of acidic spray!");
                        m.Damage(Utility.RandomMinMax(10, 15), this);
                        ApplyAcidDamage(m, 15);
                        m.SendMessage("Your resistance to attacks has been reduced!");

                        PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The acidic alligator sprays a corrosive cloud, weakening its foes! *");
                        break; // Only spray once per action
                    }
                }

                m_NextAcidSpray = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Acid Spray
            }
        }

        private void ApplyAcidDamage(Mobile target, int duration)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(1), delegate
            {
                if (target != null && target.Alive)
                {
                    target.SendMessage("The acid continues to burn!");
                    target.Damage(Utility.RandomMinMax(2, 5), this);

                    Timer.DelayCall(TimeSpan.FromSeconds(1), delegate { ApplyAcidDamage(target, duration - 1); });
                }
            });
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize to reinitialize abilities
            m_NextAcidicBite = DateTime.UtcNow; // Reset to current time
            m_NextAcidSpray = DateTime.UtcNow; // Reset to current time
        }
    }
}
