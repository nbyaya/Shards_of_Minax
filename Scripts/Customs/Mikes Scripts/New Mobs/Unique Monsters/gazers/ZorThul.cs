using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a zor'thul corpse")]
    public class ZorThul : BaseCreature
    {
        private DateTime m_NextWhisperOfMadness;
        private DateTime m_NextHallucinogenicGaze;
        private DateTime m_NextRealityWarp;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ZorThul()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Zor'Thul the Whispering";
            Body = 22; // Elder Gazer body
            Hue = 1758; // Unique hue for Zor'Thul
			BaseSoundID = 377;

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

        public ZorThul(Serial serial)
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
                    m_NextWhisperOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHallucinogenicGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextRealityWarp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWhisperOfMadness)
                {
                    WhisperOfMadness();
                }

                if (DateTime.UtcNow >= m_NextHallucinogenicGaze)
                {
                    HallucinogenicGaze();
                }

                if (DateTime.UtcNow >= m_NextRealityWarp)
                {
                    RealityWarp();
                }
            }
        }

        private void WhisperOfMadness()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Zor'Thul whispers maddening secrets *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m is PlayerMobile)
                {
                    int randomEffect = Utility.Random(2);
                    if (randomEffect == 0)
                    {
                        m.SendMessage("You are suddenly filled with a sense of confusion!");
                        m.Paralyze(TimeSpan.FromSeconds(5));
                        m.SendMessage("You begin attacking your allies in a fit of madness!");
                    }
                    else
                    {
                        m.SendMessage("You are overcome with fear and flee uncontrollably!");
                        m.Freeze(TimeSpan.FromSeconds(5));
                        // Optionally move the player randomly
                        Point3D newLocation = new Point3D(
                            m.X + Utility.RandomMinMax(-5, 5),
                            m.Y + Utility.RandomMinMax(-5, 5),
                            m.Z);
                        m.MoveToWorld(newLocation, Map);
                    }
                }
            }

            m_NextWhisperOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void HallucinogenicGaze()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Zor'Thul's gaze induces disturbing hallucinations *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m is PlayerMobile)
                {
                    m.SendMessage("You are seeing terrifying illusions!");
                    m.SendMessage("Your vision is distorted, making it difficult to fight!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }

            m_NextHallucinogenicGaze = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void RealityWarp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Zor'Thul warps reality around him *");
            Point3D newLocation = GetSpawnPosition(10);
            MoveToWorld(newLocation, Map);

            FixedEffect(0x373A, 10, 16);
            PlaySound(0x1FE);

            m_NextRealityWarp = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Location; // Fallback in case no valid location is found
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

            m_AbilitiesInitialized = false; // Reset initialization flag to set random intervals on next OnThink
        }
    }
}
