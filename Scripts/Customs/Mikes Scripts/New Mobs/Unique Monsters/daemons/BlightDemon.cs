using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a blight demon corpse")]
    public class BlightDemon : BaseCreature
    {
        private DateTime m_NextBlightWave;
        private DateTime m_NextPlagueSpit;
        private DateTime m_NextCorruptedGround;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public BlightDemon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blight demon";
            Body = 9; // Daemon body
            Hue = 1472; // Unique green hue
            BaseSoundID = 357;

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

        public BlightDemon(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override double DispelDifficulty => 125.0;
        public override double DispelFocus => 45.0;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextBlightWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPlagueSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextCorruptedGround = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextBlightWave)
                {
                    BlightWave();
                }

                if (DateTime.UtcNow >= m_NextPlagueSpit)
                {
                    PlagueSpit();
                }

                if (DateTime.UtcNow >= m_NextCorruptedGround)
                {
                    CorruptedGround();
                }
            }
        }

        private void BlightWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Blight Demon unleashes a wave of decay! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is PlayerMobile)
                {
                    Mobile target = (Mobile)m;
                    target.Damage(20, this);
                    target.SendMessage("You are hit by a wave of decay!");
                    target.AddStatMod(new StatMod(StatType.Str, "BlightWave", -10, TimeSpan.FromSeconds(30)));
                }
            }

            m_NextBlightWave = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for BlightWave
        }

        private void PlagueSpit()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Blight Demon spits a corrosive plague! *");

            if (Combatant != null && Combatant is PlayerMobile)
            {
                Mobile target = (Mobile)Combatant;
                target.Damage(10, this);
                target.SendMessage("You are afflicted by a corrosive plague!");
                target.AddStatMod(new StatMod(StatType.Str, "PlagueSpit", -5, TimeSpan.FromMinutes(1)));
            }

            m_NextPlagueSpit = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for PlagueSpit
        }

        private void CorruptedGround()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Blight Demon corrupts the ground beneath it! *");

            Point3D loc = this.Location;
            Map map = this.Map;

            for (int i = 0; i < 10; i++)
            {
                int x = loc.X + Utility.RandomMinMax(-5, 5);
                int y = loc.Y + Utility.RandomMinMax(-5, 5);
                int z = map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (map.CanSpawnMobile(p))
                {
                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 0x3B2);
                    foreach (Mobile m in map.GetMobilesInRange(p, 2))
                    {
                        if (m != this && m is PlayerMobile)
                        {
                            Mobile target = (Mobile)m;
                            target.Damage(15, this);
                            target.SendMessage("You are standing in corrupted ground!");
                            target.AddStatMod(new StatMod(StatType.Str, "CorruptedGround", -10, TimeSpan.FromSeconds(30)));
                        }
                    }
                }
            }

            m_NextCorruptedGround = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for CorruptedGround
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
