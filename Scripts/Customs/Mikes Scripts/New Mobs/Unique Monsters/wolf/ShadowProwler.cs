using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shadow prowler corpse")]
    public class ShadowProwler : BaseCreature
    {
        private DateTime m_NextShadowStrike;
        private DateTime m_NextVanishingAct;
        private DateTime m_NextNightmareHowl;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowProwler()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow prowler";
            Body = 23; // DireWolf body
            Hue = 2627; // Dark shadow hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public ShadowProwler(Serial serial)
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
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextVanishingAct = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextNightmareHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextVanishingAct)
                {
                    VanishingAct();
                }

                if (DateTime.UtcNow >= m_NextNightmareHowl)
                {
                    NightmareHowl();
                }
            }
        }

        private void ShadowStrike()
        {
            if (Combatant != null && Combatant.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Strike! *");

                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    target.Damage(Utility.RandomMinMax(20, 30), this);
                    target.SendMessage("You are struck by a shadowy attack!");
                    target.Freeze(TimeSpan.FromSeconds(2));
                }

                m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Reset with a fixed delay
            }
        }

        private void VanishingAct()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vanishing Act! *");
            Hidden = !Hidden; // Toggle visibility

            m_NextVanishingAct = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset with a fixed delay
        }

        private void NightmareHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nightmare Howl! *");
            PlaySound(0x1F2); // Adjust sound ID as needed
            FixedEffect(0x3709, 10, 30); // Adjust effect ID and duration as needed

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("The howl of the Shadow Prowler terrifies you!");

                    // Implement basic fleeing logic
                    Point3D fleePoint = GetFleePoint(m);
                    if (fleePoint != Point3D.Zero && m.Map.CanSpawnMobile(fleePoint))
                    {
                        m.MoveToWorld(fleePoint, m.Map);
                    }
                }
            }

            m_NextNightmareHowl = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset with a fixed delay
        }

        private Point3D GetFleePoint(Mobile m)
        {
            int x = m.X + Utility.RandomMinMax(-10, 10);
            int y = m.Y + Utility.RandomMinMax(-10, 10);
            int z = m.Map.GetAverageZ(x, y);

            return new Point3D(x, y, z);
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
