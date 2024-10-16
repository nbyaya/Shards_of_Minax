using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bubble ferret corpse")]
    public class BubbleFerret : BaseCreature
    {
        private DateTime m_NextBubbleBurst;
        private DateTime m_NextBubbleShield;
        private DateTime m_BubbleShieldEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BubbleFerret()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bubble ferret";
            Body = 0x117; // Ferret body
            Hue = 1578; // Unique blue hue
			BaseSoundID = 0xCF;

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

        public BubbleFerret(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish; } }

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
                    m_NextBubbleShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBubbleBurst)
                {
                    DoBubbleBurst();
                }

                if (DateTime.UtcNow >= m_NextBubbleShield && DateTime.UtcNow >= m_BubbleShieldEnd)
                {
                    ActivateBubbleShield();
                }
            }

            if (DateTime.UtcNow >= m_BubbleShieldEnd && m_BubbleShieldEnd != DateTime.MinValue)
            {
                DeactivateBubbleShield();
            }

            // Constantly blow bubbles
            if (Utility.RandomDouble() < 0.1) // 10% chance each think cycle
            {
                BlowBubbles();
            }
        }

        private void DoBubbleBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bubble Burst *");
            PlaySound(0x026); // Bubble pop sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0); // Pure cold damage
                    m.SendLocalizedMessage(1114727); // The burst of bubbles knocks you back!
                    m.MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0); // Spray of water particles
                }
            }

            m_NextBubbleBurst = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void ActivateBubbleShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bubble Shield *");
            PlaySound(0x1E3); // Spell sound

            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

            m_BubbleShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextBubbleShield = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DeactivateBubbleShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bubble Shield Fades *");
            m_BubbleShieldEnd = DateTime.MinValue;
        }

        private void BlowBubbles()
        {
            MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_BubbleShieldEnd > DateTime.UtcNow)
            {
                damage = 0; // No melee damage when bubble shield is active
                from.SendLocalizedMessage(1114728); // Your attack is deflected by the bubble shield!
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
