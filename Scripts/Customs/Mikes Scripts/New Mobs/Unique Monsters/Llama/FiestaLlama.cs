using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fiesta llama corpse")]
    public class FiestaLlama : BaseCreature
    {
        private DateTime m_NextConfettiBurst;
        private DateTime m_NextDanceOfTheLlama;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FiestaLlama()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a fiesta llama";
            Body = 0xDC; // Llama body
            Hue = 2155; // Unique hue
			this.BaseSoundID = 0x3F3;

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

        public FiestaLlama(Serial serial)
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
                    m_NextConfettiBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextDanceOfTheLlama = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextConfettiBurst)
                {
                    ConfettiBurst();
                }

                if (DateTime.UtcNow >= m_NextDanceOfTheLlama)
                {
                    DanceOfTheLlama();
                }
            }
        }

		private void ConfettiBurst()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fiesta Llama throws a dazzling burst of confetti! *");
			PlaySound(0x2D4); // Confetti sound (substitute with an appropriate sound ID)

			foreach (Mobile m in GetMobilesInRange(5))
			{
				if (m != this && m.Alive && CanBeHarmful(m))
				{
					DoHarmful(m);

					// Apply debuffs and effects
					m.SendMessage("You are blinded and your vision is obscured by the confetti burst!");
					m.SendMessage("You feel dizzy and disoriented!");

					// You can use other effects or methods here, e.g., reduce skill or stats temporarily
					// Example: Apply a temporary debuff (custom logic needed)
				}
			}

			m_NextConfettiBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for ConfettiBurst
		}


		private void DanceOfTheLlama()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fiesta Llama performs an extravagant dance! *");
			PlaySound(0x2D5); // Dance sound (substitute with an appropriate sound ID)

			// Apply a burst of color and sound effects
			FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			PlaySound(0x2D5); // Celebration sound effect

			Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
			{
				PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fiesta Llama finishes its dance and calms down. *");
			});

			m_NextDanceOfTheLlama = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for DanceOfTheLlama
		}


        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Drop random candies, small treasures, and unique items
            c.DropItem(new Candy()); // Assumes you have a Candy item defined
            c.DropItem(new SmallTreasure()); // Assumes you have a SmallTreasure item defined
            c.DropItem(new PiñataItem()); // Custom item that represents a pinata

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fiesta Llama bursts into a shower of candies, treasures, and confetti! *");
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }

    // Sample Candy, SmallTreasure, and PiñataItem items (substitute with your actual items)
    public class Candy : Item
    {
        public Candy() : base(0x1F7) // Example ID for a candy item
        {
            Movable = true;
            Name = "a piece of candy";
        }

        public Candy(Serial serial) : base(serial)
        {
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

    public class SmallTreasure : Item
    {
        public SmallTreasure() : base(0x1F5) // Example ID for a small treasure item
        {
            Movable = true;
            Name = "a small treasure";
        }

        public SmallTreasure(Serial serial) : base(serial)
        {
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

    public class PiñataItem : Item
    {
        public PiñataItem() : base(0x1F8) // Example ID for a pinata item
        {
            Movable = true;
            Name = "a piñata";
        }

        public PiñataItem(Serial serial) : base(serial)
        {
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
