using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a bubblegum blaster corpse")]
    public class BubblegumBlaster : BaseCreature
    {
        private DateTime m_NextBubbleBurst;
        private DateTime m_NextGumShield;
        private DateTime m_GumShieldEnd;
        private DateTime m_NextBubblegumTrap;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BubblegumBlaster()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Bubblegum Blaster";
            Body = 0xCF; // Sheep body
            Hue = 2359; // Unique pink hue
			BaseSoundID = 0xD6;

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

        public BubblegumBlaster(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
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
                    m_NextBubbleBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextGumShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextBubblegumTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBubbleBurst)
                {
                    BubbleBurst();
                }

                if (DateTime.UtcNow >= m_NextGumShield && DateTime.UtcNow >= m_GumShieldEnd)
                {
                    ActivateGumShield();
                }

                if (DateTime.UtcNow >= m_NextBubblegumTrap)
                {
                    PlaceBubblegumTrap();
                }
            }

            if (DateTime.UtcNow >= m_GumShieldEnd && m_GumShieldEnd != DateTime.MinValue)
            {
                DeactivateGumShield();
            }
        }

        private void BubbleBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Bubblegum Blaster pops a bubble that bursts with sweet damage!*");
            PlaySound(0x1F3); // Bubble pop sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    // Remove CanBeHarmful check and directly apply damage
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0); // Pure damage
                    m.SendLocalizedMessage(1114727); // The bubble burst hits you!

                    // Chance to confuse
                    if (Utility.RandomDouble() < 0.30) // 30% chance
                    {
                        m.SendMessage("You feel disoriented from the bubble burst!");
                        m.SendMessage("You start attacking randomly!");
                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => m.SendMessage("You regain your focus."));
                    }
                }
            }

            m_NextBubbleBurst = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for BubbleBurst
        }

        private void ActivateGumShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Bubblegum Blaster creates a gum shield to protect itself!*");
            PlaySound(0x1E3); // Protective shield sound

            Effects.SendTargetParticles(this, 0x376A, 9, 32, 5030, EffectLayer.Waist);

            m_GumShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextGumShield = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DeactivateGumShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Gum Shield fades away!*");
            m_GumShieldEnd = DateTime.MinValue;
        }

        private void PlaceBubblegumTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Bubblegum Blaster creates sticky bubblegum traps on the ground!*");
            PlaySound(0x1F3); // Sticky sound

            Point3D trapLocation = Location;
            BubblegumTrap trap = new BubblegumTrap();
            trap.MoveToWorld(trapLocation, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(5), () => trap.Delete()); // Trap disappears after 5 seconds

            m_NextBubblegumTrap = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for BubblegumTrap
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_GumShieldEnd > DateTime.UtcNow)
            {
                int reflectedDamage = (int)(damage * 0.20); // Reflect 20% of the damage
                from.SendMessage($"The gum shield reflects {reflectedDamage} damage back to you and slows you down!");
                damage -= reflectedDamage; // Reduce the incoming damage
                from.SendMessage("You feel sluggish as the gum shield slows you down!");
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

	public class BubblegumTrap : Item
	{
		public BubblegumTrap() : base(0x1F3)
		{
			Movable = false;
			// Create a visual effect for the trap
			Effects.SendTargetParticles(this, 0x36BD, 9, 32, 5030, EffectLayer.Waist);
		}

		public BubblegumTrap(Serial serial) : base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			base.OnLocationChange(oldLocation);
			// Trigger effects when entities step on the trap
			foreach (Mobile m in GetMobilesInRange(1))
			{
				if (m != null && m.Alive)
				{
					m.SendMessage("You step on a sticky bubblegum trap!");

					// Corrected AOS.Damage call
					AOS.Damage(m, null, Utility.RandomMinMax(10, 15), 100, 0, 0, 0, 0); // 100% physical damage

					m.SendMessage("You are slowed by the sticky gum!");
					m.SendMessage("You feel stuck and slow!");
				}
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
		}
	}

}
