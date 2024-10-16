using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a scorpio harpy corpse")]
    public class ScorpioHarpy : BaseCreature
    {
        private DateTime m_NextVenomCloud;
        private DateTime m_NextTailSpin;
        private DateTime m_NextHiss;
        private bool m_AbilitiesActivated; // Flag to track if abilities have been initialized

        [Constructable]
        public ScorpioHarpy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Scorpio Harpy";
            Body = 30; // Harpy body
            Hue = 2069; // Dark, venomous black and purple hues
			BaseSoundID = 402; // Harpy sound

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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public ScorpioHarpy(Serial serial)
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
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextVenomCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextTailSpin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextHiss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVenomCloud)
                {
                    VenomousCloud();
                }

                if (DateTime.UtcNow >= m_NextTailSpin)
                {
                    TailSpin();
                }

                if (DateTime.UtcNow >= m_NextHiss)
                {
                    HissAndWeaken();
                }
            }
        }

        private void VenomousCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scorpio Harpy unleashes a venomous cloud! *");
            Effects.SendLocationEffect(Location, Map, 0x36D4, 16, 10, 0x3B2); // Poison cloud effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed by a cloud of venom!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.ApplyPoison(this, Poison.Lethal); // Apply lethal poison
                }
            }
            m_NextVenomCloud = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Set next cooldown
        }

        private void TailSpin()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scorpio Harpy spins its tail in a deadly arc! *");
            Effects.SendLocationEffect(Location, Map, 0x3728, 10, 16, 0x3B2); // Spin effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are struck by the Scorpio Harpy’s spinning tail and bleed!");
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);
                    m.PlaySound(0x1F5); // Bleeding sound
                    m.FixedParticles(0x374A, 10, 30, 5008, EffectLayer.Waist); // Bleed effect
                }
            }
            m_NextTailSpin = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set next cooldown
        }

        private void HissAndWeaken()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scorpio Harpy hisses loudly, weakening its enemies! *");
            Effects.PlaySound(Location, Map, 0x4E3); // Hissing sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You feel weakened by the Scorpio Harpy’s hissing!");
                    m.SendMessage("Your combat skills have been reduced!");
                }
            }
            m_NextHiss = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Set next cooldown
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

            // Reset ability initialization state
            m_AbilitiesActivated = false;
        }
    }
}
