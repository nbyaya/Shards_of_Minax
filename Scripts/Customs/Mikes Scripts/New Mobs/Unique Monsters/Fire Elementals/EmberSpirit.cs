using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an ember spirit corpse")]
    public class EmberSpirit : BaseCreature
    {
        private DateTime m_NextEtherealFlame;
        private DateTime m_NextFireWalk;
        private DateTime m_NextPhantomBurn;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EmberSpirit()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ember spirit";
            Body = 15; // Fire elemental body
            Hue = 1660; // Unique hue for the ember spirit
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

        public EmberSpirit(Serial serial)
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
                    m_NextEtherealFlame = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFireWalk = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPhantomBurn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEtherealFlame)
                {
                    CastEtherealFlame();
                }

                if (DateTime.UtcNow >= m_NextFireWalk)
                {
                    PerformFireWalk();
                }

                if (DateTime.UtcNow >= m_NextPhantomBurn)
                {
                    CastPhantomBurn();
                }
            }
        }

        private void CastEtherealFlame()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The Ember Spirit launches a fiery projectile towards you!");
                    Effects.SendTargetParticles(m, 0x36D4, 1, 30, 9920, 1153, 0, 0, 0);
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ember Spirit shoots out ethereal fireballs! *");
            m_NextEtherealFlame = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for Ethereal Flame
        }

        private void PerformFireWalk()
        {
            Point3D loc = GetSpawnPosition(5);

            if (loc != Point3D.Zero)
            {
                this.Location = loc;
                this.Map = Map;

                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ember Spirit teleports with a burst of flames! *");

                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are scorched by the flames left behind by the Ember Spirit!");
                        m.Damage(Utility.RandomMinMax(10, 20), this);
                    }
                }

                m_NextFireWalk = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for Fire Walk
            }
        }

        private void CastPhantomBurn()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                target.SendMessage("You feel intense pain as the Ember Spirit's phantom burn affects you!");
                target.SendMessage("Your attacks are weakened by the phantom burn!");

                // Apply damage over time
                Timer.DelayCall(TimeSpan.FromSeconds(0), delegate { target.Damage(10, this); });
                Timer.DelayCall(TimeSpan.FromSeconds(1), delegate { target.Damage(10, this); });
                Timer.DelayCall(TimeSpan.FromSeconds(2), delegate { target.Damage(10, this); });

                // Optionally, apply a debuff to the target
                // target.AddToBackpack(new PhantomBurnDebuff()); 

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ember Spirit inflicts a phantom burn upon its target! *");
                m_NextPhantomBurn = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for Phantom Burn
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
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class PhantomBurnDebuff : Item
    {
        [Constructable]
        public PhantomBurnDebuff()
            : base(0x1BD4) // Custom item ID
        {
            Name = "Phantom Burn Debuff";
        }

        public PhantomBurnDebuff(Serial serial)
            : base(serial)
        {
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
        }
    }
}
