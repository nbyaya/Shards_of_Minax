using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an azure mirage corpse")]
    public class AzureMirage : BaseCreature
    {
        private DateTime m_NextMirageWave;
        private DateTime m_NextPrismaticSpray;
        private DateTime m_NextHaze;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AzureMirage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an azure mirage";
            this.Body = 16; // Water Elemental body
            this.BaseSoundID = 278;
			Hue = 2533; // Blue hue for storm effect

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
            this.CanSwim = true;

            this.PackItem(new BlackPearl(5));
            this.PackItem(new Bottle(1));
            this.PackItem(new GreaterHealPotion());
            this.PackItem(new GreaterCurePotion());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public AzureMirage(Serial serial)
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
                    m_NextMirageWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPrismaticSpray = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextHaze = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 3));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMirageWave)
                {
                    CastMirageWave();
                }

                if (DateTime.UtcNow >= m_NextPrismaticSpray)
                {
                    CastPrismaticSpray();
                }

                if (DateTime.UtcNow >= m_NextHaze)
                {
                    CastHaze();
                }
            }
        }

        private void CastMirageWave()
        {
            // Create illusory duplicates
            for (int i = 0; i < 3; i++)
            {
                BaseCreature illusion = new Illusion();
                illusion.MoveToWorld(GetSpawnPosition(5), Map);
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Illusory duplicates appear! *");

            m_NextMirageWave = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void CastPrismaticSpray()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Damage(Utility.RandomMinMax(20, 30), this);
                    m.SendMessage("You are blinded by a spray of refracted light!");
                    m.SendLocalizedMessage(1072078); // "You are blinded by the light!"
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A prismatic spray of light hits the area! *");

            m_NextPrismaticSpray = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void CastHaze()
        {
            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A thick mist envelops the area, reducing your visibility!");
                    m.VirtualArmor -= 10; // Reducing accuracy temporarily
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A thick mist reduces visibility in the area! *");

            m_NextHaze = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

            return Point3D.Zero;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(m_NextMirageWave);
            writer.Write(m_NextPrismaticSpray);
            writer.Write(m_NextHaze);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_NextMirageWave = reader.ReadDateTime();
                    m_NextPrismaticSpray = reader.ReadDateTime();
                    m_NextHaze = reader.ReadDateTime();
                    break;
            }

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }

        public class Illusion : BaseCreature
        {
            public Illusion()
                : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
            {
                Body = 16; // Water Elemental body
                Hue = 1150; // Light blue hue

                SetStr(1);
                SetDex(1);
                SetInt(1);

                SetHits(1);
                SetDamage(0);

                SetResistance(ResistanceType.Physical, 100);
                SetResistance(ResistanceType.Fire, 100);
                SetResistance(ResistanceType.Cold, 100);
                SetResistance(ResistanceType.Poison, 100);
                SetResistance(ResistanceType.Energy, 100);

                VirtualArmor = 100;
            }

            public Illusion(Serial serial)
                : base(serial)
            {
            }

            public override bool DeleteCorpseOnDeath { get { return true; } }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
