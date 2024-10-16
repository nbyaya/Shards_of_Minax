using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a flare imp corpse")]
    public class FlareImp : BaseCreature
    {
        private DateTime m_NextFireball;
        private DateTime m_NextBlazingSpeed;
        private DateTime m_NextFieryTrickster;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FlareImp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flare imp";
            Body = 15; // Fire elemental body
            Hue = 1659; // Unique hue
			BaseSoundID = 838;

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

        public FlareImp(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextFireball = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextBlazingSpeed = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFieryTrickster = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFireball)
                {
                    CastFireball();
                }

                if (DateTime.UtcNow >= m_NextBlazingSpeed)
                {
                    UseBlazingSpeed();
                }

                if (DateTime.UtcNow >= m_NextFieryTrickster)
                {
                    CreateFieryTrickster();
                }
            }
        }

        private void CastFireball()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    // Create and send fireball effects
                    Effects.SendTargetEffect(target, 0x36D4, 16, 4, 0, 1153);
                    target.SendMessage("A fireball explodes on you!");
                    target.Damage(Utility.RandomMinMax(15, 25), this);

                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flare Imp hurls a fireball! *");
                    m_NextFireball = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

        private void UseBlazingSpeed()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flare Imp moves in a blazing trail of fire! *");
                Point3D newLocation = new Point3D(this.X + Utility.RandomMinMax(-5, 5), this.Y + Utility.RandomMinMax(-5, 5), this.Z);
                this.Location = newLocation;

                // Create fire trail effects
                Effects.SendLocationParticles(EffectItem.Create(newLocation, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);

                foreach (Mobile m in GetMobilesInRange(1))
                {
                    if (m != this && m != Combatant)
                    {
                        m.SendMessage("You are burned by the blazing trail!");
                        m.Damage(Utility.RandomMinMax(5, 10), this);
                    }
                }

                m_NextBlazingSpeed = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        private void CreateFieryTrickster()
        {
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flare Imp creates a fiery illusion! *");
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);

                FieryIllusion illusion = new FieryIllusion();
                illusion.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerCallback(delegate()
                {
                    if (!illusion.Deleted)
                    {
                        illusion.Delete();
                    }
                }));

                m_NextFieryTrickster = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class FieryIllusion : BaseCreature
    {
        public FieryIllusion()
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Body = 15; // Fire elemental body
            Hue = 1266; // Match the flare imp's hue

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

        public FieryIllusion(Serial serial)
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
