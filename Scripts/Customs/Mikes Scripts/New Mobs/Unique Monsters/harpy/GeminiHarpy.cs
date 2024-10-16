using System;
using System.Linq; // Make sure to include this for LINQ operations
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Gemini Harpy corpse")]
    public class GeminiHarpy : BaseCreature
    {
        private DateTime m_NextMirrorImage;
        private DateTime m_NextDoppelgangerStrike;
        private DateTime m_NextEnrage;
        private bool m_Enraged;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public GeminiHarpy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Gemini Harpy";
            Body = 30; // Harpy body
            Hue = 2076; // Unique hue for Gemini Harpy
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

        public GeminiHarpy(Serial serial)
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

        public override bool CanRummageCorpses => true;
        public override int Meat => 6;
        public override MeatType MeatType => MeatType.Bird;
        public override int Feathers => 75;
        public override bool CanFly => true;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDoppelgangerStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEnrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextMirrorImage)
                {
                    CreateMirrorImage();
                }

                if (DateTime.UtcNow >= m_NextDoppelgangerStrike)
                {
                    DoppelgangerStrike();
                }

                if (DateTime.UtcNow >= m_NextEnrage && !m_Enraged && Hits < HitsMax * 0.3)
                {
                    Enrage();
                }
            }
        }

        private void CreateMirrorImage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini Harpy creates a mirrored image! *");
            FixedEffect(0x376A, 10, 16);

            // Create a mirrored image clone
            GeminiHarpyMirrorImage clone = new GeminiHarpyMirrorImage(this);
            Point3D loc = GetSpawnPosition(2);
            if (loc != Point3D.Zero)
            {
                clone.MoveToWorld(loc, Map);
            }

            m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset the time for the next mirror image
        }

        private void DoppelgangerStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini Harpy strikes with dual fury! *");

            // Convert IPooledEnumerable<Mobile> to a list
            var targets = GetMobilesInRange(5).ToList()
                            .Where(m => m != this && m.Alive && !m.IsDeadBondedPet)
                            .Take(2)
                            .ToArray();

            foreach (Mobile target in targets)
            {
                AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                target.SendMessage("You feel weakened by the Gemini Harpy's strike!");
                AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0); // Weakening damage
                target.Damage(5); // Reduce damage dealt for a short time
            }

            m_NextDoppelgangerStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset the time for the next doppelganger strike
        }

        private void Enrage()
        {
            m_Enraged = true;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini Harpy becomes enraged! *");
            PlaySound(0x30F); // Roar sound
            FixedEffect(0x376A, 10, 16);

            // Increase damage and attack speed
            SetDamage(25, 30); // Adjust damage as needed
            // Adjust attack speed through other available means if necessary

            m_NextEnrage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset the time for the next enrage
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
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesActivated = false; // Reset flag on deserialize
        }
    }

    public class GeminiHarpyMirrorImage : BaseCreature
    {
        private Mobile m_Master;
        private DateTime m_NextMirrorImageAttack;

        public GeminiHarpyMirrorImage(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(50);
            SetDex(50);
            SetInt(50);

            SetHits(50);
            SetDamage(5, 10);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public GeminiHarpyMirrorImage(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;

            if (DateTime.UtcNow >= m_NextMirrorImageAttack)
            {
                PerformMirrorImageAttack();
                m_NextMirrorImageAttack = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Reset the attack time
            }
        }

        private void PerformMirrorImageAttack()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The mirror image attacks! *");
                AOS.Damage(Combatant, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
