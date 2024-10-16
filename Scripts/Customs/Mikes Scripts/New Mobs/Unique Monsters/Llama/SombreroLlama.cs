using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a sombrero llama corpse")]
    public class SombreroLlama : BaseCreature
    {
        private DateTime m_NextElGrito;
        private DateTime m_NextDesertMirage;
        private DateTime m_NextSummonMinions;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SombreroLlama()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Sombrero Llama";
            Body = 0xDC; // Llama body
            Hue = 2152; // Unique hue for Sombrero Llama
			this.BaseSoundID = 0x3F3;
            Item sombrero = new Item(0x1714) // Sombrero item ID
            {
                Layer = Layer.Helm,
                Hue = 1155
            };
            AddItem(sombrero);

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

        public SombreroLlama(Serial serial)
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
                    m_NextElGrito = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDesertMirage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextElGrito)
                {
                    ElGrito();
                }

                if (DateTime.UtcNow >= m_NextDesertMirage)
                {
                    DesertMirage();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }
            }
        }

        private void ElGrito()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero Llama lets out a powerful war cry! *");
            PlaySound(0x1F6); // War cry sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m is BaseCreature creature && creature.Controlled && creature.ControlMaster != null)
                {
                    creature.Str += 15;
                    creature.Dex += 15;
                    creature.Int += 15;
                    creature.SendMessage("You feel a surge of strength and defense!");
                }

                if (m != this && m.Alive && Utility.RandomDouble() < 0.2) // 20% chance to stun
                {
                    m.SendMessage("You are stunned by the llama's war cry!");
                    m.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => m.Frozen = false);
                }
            }

            m_NextElGrito = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for ElGrito
        }

        private void DesertMirage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero Llama creates a series of illusions! *");
            PlaySound(0x227); // Illusion sound

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 2), () =>
                {
                    Point3D loc = Location;
                    BaseCreature illusion = new IllusionLlama();
                    illusion.MoveToWorld(loc, Map);
                    illusion.Say("I am the real Sombrero Llama!");
                });
            }

            m_NextDesertMirage = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DesertMirage
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero Llama summons lesser llamas to aid it in battle! *");
            PlaySound(0x227); // Summoning sound

            for (int i = 0; i < 2; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 2), () =>
                {
                    BaseCreature minion = new LesserLlama();
                    minion.MoveToWorld(Location, Map);
                    minion.Combatant = Combatant;
                });
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for SummonMinions
        }

		public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
		{
			// Sombrero Shield - Reflect 25% of physical damage back and a chance to disarm
			if (from == null || damage <= 0) // Check if 'from' is null and if damage is greater than 0
				return;

			if (Utility.RandomDouble() < 0.25) // 25% chance to reflect damage
			{
				PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero Llama's sombrero reflects some of the damage back at you! *");
				from.SendMessage("You are hit by the reflected damage!");
				int reflectedDamage = (int)(damage * 0.25);
				AOS.Damage(from, this, reflectedDamage, 0, 0, 100, 0, 0);
				damage -= reflectedDamage;
			}

			if (Utility.RandomDouble() < 0.15) // 15% chance to disarm
			{
				from.SendMessage("You are disarmed by the llama's sombrero!");
				from.SendMessage("You lose your weapon!");
				from.SendMessage("Your weapon is disarmed!");

				// Check for weapon on either layer
				Item weapon = from.FindItemOnLayer(Layer.OneHanded) ?? from.FindItemOnLayer(Layer.TwoHanded);
				if (weapon != null)
				{
					from.Backpack.DropItem(weapon);
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class IllusionLlama : BaseCreature
    {
        [Constructable]
        public IllusionLlama() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "an illusion of the Sombrero Llama";
            Body = 0xDC; // Llama body
            Hue = 1155; // Same hue for consistency

            SetStr(150, 200);
            SetDex(80, 120);
            SetInt(30, 50);

            SetHits(100, 150);
            SetMana(0);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);

            SetSkill(SkillName.MagicResist, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 30;

            Tamable = false;
        }

        public IllusionLlama(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            // Illusions should not act beyond being distractions
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

    public class LesserLlama : BaseCreature
    {
        [Constructable]
        public LesserLlama() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a lesser llama";
            Body = 0xDC; // Llama body
            Hue = 1155; // Same hue for consistency

            SetStr(200, 250);
            SetDex(100, 150);
            SetInt(40, 60);

            SetHits(200, 250);
            SetMana(0);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 40;

            Tamable = false;
        }

        public LesserLlama(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            // Lesser llamas will fight to assist their master
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
