using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a caramel conjurer corpse")]
    public class CaramelConjurer : BaseCreature
    {
        private DateTime m_NextCaramelConjure;
        private DateTime m_NextStickySweet;
        private DateTime m_NextStickyTrap;
        private DateTime m_NextShield;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CaramelConjurer()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Caramel Conjurer";
            Body = 0xCF; // Sheep body
            Hue = 2355; // Unique caramel hue
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

        public CaramelConjurer(Serial serial)
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
                    Random rand = new Random();
                    m_NextCaramelConjure = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStickySweet = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStickyTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCaramelConjure)
                {
                    CaramelConjure();
                }

                if (DateTime.UtcNow >= m_NextStickySweet)
                {
                    StickySweet();
                }

                if (DateTime.UtcNow >= m_NextStickyTrap)
                {
                    StickyTrap();
                }

                if (DateTime.UtcNow >= m_NextShield)
                {
                    ActivateShield();
                }
            }
        }

        private void CaramelConjure()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Caramel Conjurer casts a sticky spell of caramel!*");
            this.PlaySound(0x20D); // Spell sound

            // Create a caramel puddle
            Point3D loc = Location;
            CaramelPuddle puddle = new CaramelPuddle();
            puddle.MoveToWorld(loc, Map);

            m_NextCaramelConjure = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for CaramelConjure
        }

        private void StickySweet()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Caramel Conjurer hurls a sticky blob of caramel!*");
                this.PlaySound(0x20D); // Spell sound

                // Throw a sticky sweet blob
                Point3D loc = Combatant.Location;
                CaramelBlob blob = new CaramelBlob();
                blob.MoveToWorld(loc, Map);
                blob.Hit((Mobile)Combatant); // Explicit cast to Mobile

                m_NextStickySweet = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for StickySweet
            }
        }

        private void StickyTrap()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Caramel Conjurer sets a sticky trap!*");
                this.PlaySound(0x20D); // Spell sound

                // Place a sticky trap
                Point3D loc = Location;
                StickyTrapItem trap = new StickyTrapItem();
                trap.MoveToWorld(loc, Map);

                m_NextStickyTrap = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for StickyTrap
            }
        }

        private void ActivateShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Caramel Conjurer activates a caramel shield!*");
            this.PlaySound(0x20D); // Shield sound

            // Temporary shield to reduce damage
            VirtualArmor = 60;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => DeactivateShield());

            m_NextShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for ActivateShield
        }

        private void DeactivateShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The caramel shield fades away!*");
            VirtualArmor = 40; // Restore original armor value
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

    public class CaramelPuddle : Item
    {
        public CaramelPuddle() : base(0x1D3B)
        {
            Movable = false;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => Delete()); // Puddle disappears after 10 seconds
        }

        public CaramelPuddle(Serial serial) : base(serial)
        {
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            base.OnLocationChange(oldLocation);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != null && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are slowed by a caramel puddle!");
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

	public class CaramelBlob : Item
	{
		public CaramelBlob() : base(0x122A)
		{
			Movable = false;
		}

		public CaramelBlob(Serial serial) : base(serial)
		{
		}

		public void Hit(Mobile target)
		{
			Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyDamage(target));
		}

		private void ApplyDamage(Mobile target)
		{
			if (target != null && target.Alive)
			{
				target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The caramel blob burns with sticky sweetness! *");
				target.PlaySound(0x1F4); // Damage sound
				AOS.Damage(target, 25, 0, 0, 0, 0, 0); // Apply 25 damage with no specific resistance type
				target.SendMessage("You are covered in sticky caramel, taking damage over time!");
			}

			Delete(); // Remove the blob after application
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

	public class StickyTrapItem : Item
	{
		public StickyTrapItem() : base(0x122A)
		{
			Movable = false;
			Timer.DelayCall(TimeSpan.FromSeconds(5), () => Delete()); // Trap disappears after 5 seconds
		}

		public StickyTrapItem(Serial serial) : base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			base.OnLocationChange(oldLocation);

			foreach (Mobile m in GetMobilesInRange(1))
			{
				if (m != null && m.Alive && !m.IsDeadBondedPet)
				{
					m.SendMessage("You triggered a sticky trap and are caught in a sticky mess!");
					m.SendMessage("You are slowed and take damage over time!");

					Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyDamage(m));
				}
			}
		}

		private void ApplyDamage(Mobile target)
		{
			if (target != null && target.Alive)
			{
				target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The sticky trap burns with sticky sweetness! *");
				target.PlaySound(0x1F4); // Damage sound
				AOS.Damage(target, 15, 0, 0, 0, 0, 0); // Apply 15 damage with no specific resistance type
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
