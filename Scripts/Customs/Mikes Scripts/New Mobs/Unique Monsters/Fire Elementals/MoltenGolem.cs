using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a molten golem corpse")]
    public class MoltenGolem : BaseCreature
    {
        private DateTime m_NextMoltenBlast;
        private DateTime m_NextLavaSurge;
        private DateTime m_NextHeatAbsorption;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public MoltenGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a molten golem";
            Body = 15; // Fire Elemental body
            Hue = 1597; // Unique hue, reddish with molten effects
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

        public MoltenGolem(Serial serial)
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
                    m_NextMoltenBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLavaSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextHeatAbsorption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMoltenBlast)
                {
                    PerformMoltenBlast();
                }

                if (DateTime.UtcNow >= m_NextLavaSurge)
                {
                    PerformLavaSurge();
                }

                if (DateTime.UtcNow >= m_NextHeatAbsorption)
                {
                    PerformHeatAbsorption();
                }
            }
        }

        private void PerformMoltenBlast()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    Point3D location = target.Location;
                    Map map = target.Map;

                    Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 5013);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Molten Blast! *");

                    foreach (Mobile m in map.GetMobilesInRange(location, 3))
                    {
                        if (m != this && m.Player)
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                        }
                    }

                    m_NextMoltenBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update to fixed cooldown
                }
            }
        }

        private void PerformLavaSurge()
        {
            if (Combatant != null)
            {
                Point3D location = this.Location;
                Map map = this.Map;

                Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 5013);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lava Surge! *");

                foreach (Mobile m in map.GetMobilesInRange(location, 5))
                {
                    if (m != this && m.Player)
                    {
                        AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    }
                }

                m_NextLavaSurge = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Update to fixed cooldown
            }
        }

        private void PerformHeatAbsorption()
        {
            if (DateTime.UtcNow >= m_NextHeatAbsorption)
            {
                int healAmount = 20;
                Hits += healAmount;

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Heat Absorption! *");
                m_NextHeatAbsorption = DateTime.UtcNow + TimeSpan.FromSeconds(120); // Update to fixed cooldown
            }
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
