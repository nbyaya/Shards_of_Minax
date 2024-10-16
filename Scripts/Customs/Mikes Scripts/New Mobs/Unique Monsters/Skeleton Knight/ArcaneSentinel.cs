using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an arcane sentinel corpse")]
    public class ArcaneSentinel : BaseCreature
    {
        private DateTime m_NextArcaneBlast;
        private DateTime m_NextManaShield;
        private bool m_HasManaShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ArcaneSentinel()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an arcane sentinel";
            Body = 57; // BoneKnight body
            Hue = 2377; // Unique hue for Arcane Sentinel (light blue/purple)
			BaseSoundID = 451;

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

        public ArcaneSentinel(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextArcaneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextManaShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextArcaneBlast)
                {
                    ArcaneBlast();
                }

                if (DateTime.UtcNow >= m_NextManaShield && !m_HasManaShield)
                {
                    ActivateManaShield();
                }
            }
        }

        private void ArcaneBlast()
        {
            if (Combatant != null && Combatant.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Arcane Blast! *");
                PlaySound(0x2D6);
                FixedEffect(0x373A, 10, 16);

                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Player)
                    {
                        int damage = Utility.RandomMinMax(10, 20);
                        m.Damage(damage, this);
                        m.SendMessage("You are struck by a burst of arcane energy!");
                        if (Utility.RandomDouble() < 0.3) // 30% chance to confuse
                        {
                            m.Freeze(TimeSpan.FromSeconds(2));
                            m.SendMessage("You feel confused by the arcane energy!");
                        }
                    }
                }

                m_NextArcaneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ArcaneBlast
            }
        }

        private void ActivateManaShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mana Shield activated! *");
            PlaySound(0x2D7);
            FixedEffect(0x375A, 10, 16);

            m_HasManaShield = true;
            this.VirtualArmor += 30; // Increase armor for the duration of the shield

            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(DeactivateManaShield));
            m_NextManaShield = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for ManaShield
        }

        private void DeactivateManaShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mana Shield deactivated. *");
            this.VirtualArmor -= 30; // Reset armor to original value
            m_HasManaShield = false;
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
