using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mire spawner corpse")]
    public class MireSpawner : BaseCreature
    {
        private DateTime m_NextQuagmire;
        private DateTime m_NextMudSling;
        private DateTime m_NextToxicSludge;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public MireSpawner()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mire spawner";
            Body = 16; // Water elemental body
            BaseSoundID = 278;
			Hue = 2503; // Blue hue for storm effect

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

            m_AbilitiesInitialized = false; // Flag to indicate abilities have not been initialized

            PackItem(new BlackPearl(5));
        }

        public MireSpawner(Serial serial)
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
                    m_NextQuagmire = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMudSling = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextToxicSludge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextQuagmire)
                {
                    CastQuagmire();
                }

                if (DateTime.UtcNow >= m_NextMudSling)
                {
                    CastMudSling();
                }

                if (DateTime.UtcNow >= m_NextToxicSludge)
                {
                    CastToxicSludge();
                }
            }
        }

        private void CastQuagmire()
        {
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 5007);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground becomes a quagmire! *");

                foreach (Mobile m in Map.GetMobilesInRange(loc, 2))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are engulfed by a quagmire and slowed!");
                        m.SendLocalizedMessage(1042567); // Your movement is slowed!
                        m.Freeze(TimeSpan.FromSeconds(5));
                        m.Damage(5, this);
                    }
                }

                m_NextQuagmire = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void CastMudSling()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("You are hit by a clump of mud!");
                    target.SendLocalizedMessage(1042548); // You are covered in mud and your vision is impaired!

                    // Decrease accuracy and movement speed
                    target.Dex -= 10;
                    target.SendLocalizedMessage(1042549); // Your movement is reduced!

                    m_NextMudSling = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                }
            }
        }

        private void CastToxicSludge()
        {
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 9502);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A pool of toxic sludge forms! *");

                foreach (Mobile m in Map.GetMobilesInRange(loc, 2))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are poisoned by the toxic sludge!");
                        m.ApplyPoison(this, Poison.Lethal);
                    }
                }

                m_NextToxicSludge = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
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
            writer.Write((int)1); // version

            writer.Write(m_NextQuagmire);
            writer.Write(m_NextMudSling);
            writer.Write(m_NextToxicSludge);
            writer.Write(m_AbilitiesInitialized); // Add the initialization flag to serialization
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_NextQuagmire = reader.ReadDateTime();
                    m_NextMudSling = reader.ReadDateTime();
                    m_NextToxicSludge = reader.ReadDateTime();
                    m_AbilitiesInitialized = reader.ReadBool(); // Read the initialization flag from deserialization
                    break;
            }
        }
    }
}
