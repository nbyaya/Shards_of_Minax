using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a gloom wolf corpse")]
    public class GloomWolf : BaseCreature
    {
        private DateTime m_NextGloomStrike;
        private DateTime m_NextShadowCloak;
        private DateTime m_NextDrainingHowl;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public GloomWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a gloom wolf";
            Body = 23; // Dire Wolf body
            Hue = 2628; // Dark hue
			BaseSoundID = 0xE5;
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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public GloomWolf(Serial serial)
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
                    m_NextGloomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextShadowCloak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDrainingHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGloomStrike)
                {
                    GloomStrike();
                }

                if (DateTime.UtcNow >= m_NextShadowCloak)
                {
                    ShadowCloak();
                }

                if (DateTime.UtcNow >= m_NextDrainingHowl)
                {
                    DrainingHowl();
                }
            }
        }

        private void GloomStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Gloom Strike! *");
                target.SendMessage("You feel your strength draining away!");
                target.SendMessage("Your damage output is reduced!");

                int strengthReduction = 5; // Amount to reduce the target's strength
                target.Damage(0, this);
                target.RawStr -= strengthReduction;
                target.RawStr = Math.Max(target.RawStr, 1); // Ensure strength does not go below 1

                m_NextGloomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for GloomStrike
            }
        }

        private void ShadowCloak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Cloak! *");
            PlaySound(0x20B);
            FixedEffect(0x373A, 10, 16);

            // Increase resistance temporarily
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 25);
            SetResistance(ResistanceType.Poison, 15, 20);
            SetResistance(ResistanceType.Energy, 30, 35);

            Timer.DelayCall(TimeSpan.FromSeconds(15), () => 
            {
                // Revert resistance to original values
                SetResistance(ResistanceType.Physical, 30, 40);
                SetResistance(ResistanceType.Fire, 20, 30);
                SetResistance(ResistanceType.Cold, 15, 20);
                SetResistance(ResistanceType.Poison, 10, 15);
                SetResistance(ResistanceType.Energy, 20, 25);
            });

            m_NextShadowCloak = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for ShadowCloak
        }

        private void DrainingHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Draining Howl! *");
            PlaySound(0x16D);
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int drainAmount = 10; // Amount of life drained
                    m.Damage(drainAmount, this);
                    Hits = Math.Min(Hits + drainAmount, HitsMax); // Heal self with drained life
                }
            }

            m_NextDrainingHowl = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DrainingHowl
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
