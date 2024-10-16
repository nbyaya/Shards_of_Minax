using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a cholera rat corpse")]
    public class CholeraRat : BaseCreature
    {
        private DateTime m_NextVomitTime;
        private DateTime m_NextDiseaseTime;
        private DateTime m_NextSwarmTime;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CholeraRat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cholera rat";
            Body = 0xD7; // Same body as GiantRat
            Hue = 2269; // Unique hue for Cholera Rat
            this.BaseSoundID = 0xCC;
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

            m_AbilitiesInitialized = false;
        }

        public CholeraRat(Serial serial)
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
                    m_NextVomitTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDiseaseTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSwarmTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextVomitTime)
                {
                    VomitSpray();
                }

                if (DateTime.UtcNow >= m_NextDiseaseTime)
                {
                    SpreadDisease();
                }

                if (DateTime.UtcNow >= m_NextSwarmTime)
                {
                    SummonSwarm();
                }
            }
        }

        private void VomitSpray()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cholera Rat spews toxic bile, creating deadly puddles! *");
            PlaySound(0x1E5); // Sound of vomiting

            Point3D sprayLocation = Location;
            Map map = Map;

            for (int i = 0; i < 3; i++)
            {
                int x = sprayLocation.X + Utility.RandomMinMax(-2, 2);
                int y = sprayLocation.Y + Utility.RandomMinMax(-2, 2);
                int z = sprayLocation.Z;

                Point3D puddleLocation = new Point3D(x, y, z);
                if (map.CanSpawnMobile(puddleLocation))
                {
                    Effects.SendLocationParticles(EffectItem.Create(puddleLocation, map, EffectItem.DefaultDuration), 0x376A, 10, 30, 0x3F5);
                    
                    foreach (Mobile m in map.GetMobilesInRange(puddleLocation, 1))
                    {
                        if (m != this && m.Player)
                        {
                            m.SendMessage("You are standing in a puddle of toxic bile!");
                            m.SendMessage("Your movement speed and stamina regeneration are reduced!");
                            // Optionally, you can apply a debuff effect here
                        }
                    }
                }
            }

            m_NextVomitTime = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Shorter cooldown for VomitSpray
        }

        private void SpreadDisease()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cholera Rat releases a noxious cloud of disease! *");
            PlaySound(0x1E7); // Sound of disease spreading

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are exposed to a dangerous disease!");
                    // Apply a disease effect (e.g., reduce health over time, slow movement)
                }
            }

            m_NextDiseaseTime = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for SpreadDisease
        }

        private void SummonSwarm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cholera Rat summons a swarm of infected fleas! *");
            PlaySound(0x1E6); // Sound of swarming

            Point3D swarmLocation = Location;
            Map map = Map;

            for (int i = 0; i < 2; i++)
            {
                int x = swarmLocation.X + Utility.RandomMinMax(-2, 2);
                int y = swarmLocation.Y + Utility.RandomMinMax(-2, 2);
                int z = swarmLocation.Z;

                Point3D spawnLocation = new Point3D(x, y, z);
                if (map.CanSpawnMobile(spawnLocation))
                {
                    SwarmFleas swarmFleas = new SwarmFleas();
                    swarmFleas.MoveToWorld(spawnLocation, map);

                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate() 
                    {
                        if (!swarmFleas.Deleted)
                            swarmFleas.Delete(); 
                    }));
                }
            }

            m_NextSwarmTime = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for SummonSwarm
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
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }

    public class SwarmFleas : BaseCreature
    {
        public SwarmFleas() : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Body = 6; // Flea body ID
            Hue = 0x4F5; // Matching hue for consistency
            Name = "a flea";

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

        public SwarmFleas(Serial serial) : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The fleas bite you and spread irritation!");
                    // Apply irritation effect, e.g., slow health regeneration
                }
            }

            base.OnThink();
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
