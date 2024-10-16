using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cursed toad corpse")]
    public class CursedToad : BaseCreature
    {
        private DateTime m_NextCurseTouch;
        private DateTime m_NextCursedSpawn;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CursedToad()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cursed toad";
            Body = 80; // Giant toad body
            Hue = 2450; // Dark magical hue
            BaseSoundID = 0x26B;

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

        public CursedToad(Serial serial)
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
                    m_NextCurseTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Randomize initial cooldown
                    m_NextCursedSpawn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45)); // Randomize initial cooldown
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCurseTouch)
                {
                    CurseTouch();
                }

                if (DateTime.UtcNow >= m_NextCursedSpawn)
                {
                    CursedSpawn();
                }
            }
        }

        private void CurseTouch()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed Toad touches you with a dark curse! *");

                target.SendMessage("You feel a dark curse affecting your strength, dexterity, and intelligence!");

                target.Damage(Utility.RandomMinMax(5, 15), this);

                // Apply debuffs to stats
                target.RawStr = Math.Max(1, target.RawStr - Utility.RandomMinMax(5, 10));
                target.RawDex = Math.Max(1, target.RawDex - Utility.RandomMinMax(5, 10));
                target.RawInt = Math.Max(1, target.RawInt - Utility.RandomMinMax(5, 10));

                m_NextCurseTouch = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for CurseTouch
            }
        }

        private void CursedSpawn()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed Toad summons cursed minions! *");

            for (int i = 0; i < 2; i++)
            {
                CursedToadling toadling = new CursedToadling();
                Point3D spawnLocation = GetSpawnPosition(2);

                if (spawnLocation != Point3D.Zero)
                {
                    toadling.MoveToWorld(spawnLocation, Map);
                }
            }

            m_NextCursedSpawn = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for CursedSpawn
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class CursedToadling : BaseCreature
    {
        [Constructable]
        public CursedToadling()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cursed toadling";
            Body = 80; // Giant toad body
            Hue = 1157; // Same dark magical hue as parent

            SetStr(40, 60);
            SetDex(20, 30);
            SetInt(10, 20);

            SetHits(30, 50);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 25);

            SetSkill(SkillName.MagicResist, 20.0, 40.0);
            SetSkill(SkillName.Tactics, 20.0, 40.0);
            SetSkill(SkillName.Wrestling, 20.0, 40.0);

            Fame = 250;
            Karma = -250;

            VirtualArmor = 10;
        }

        public CursedToadling(Serial serial)
            : base(serial)
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
