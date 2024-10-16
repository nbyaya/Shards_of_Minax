using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a belding’s ground squirrel corpse")]
    public class BeldingsGroundSquirrel : BaseCreature
    {
        private DateTime m_NextBurrow;
        private DateTime m_NextUndergroundRoar;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BeldingsGroundSquirrel()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Belding’s Ground Squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2440; // Light brown hue

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

        public BeldingsGroundSquirrel(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextBurrow = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextUndergroundRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBurrow)
                {
                    Burrow();
                }

                if (DateTime.UtcNow >= m_NextUndergroundRoar)
                {
                    UndergroundRoar();
                }
            }
        }

        private void Burrow()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Belding’s Ground Squirrel burrows underground! *");
            PlaySound(0x02F); // Digging sound

            // Create dirt effect
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 0, Hue, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (Combatant != null)
                {
                    // Summon lesser squirrels to aid
                    for (int i = 0; i < 2; i++)
                    {
                        BaseCreature summonedSquirrel = new LesserSquirrel(); // Custom class for lesser squirrel
                        summonedSquirrel.MoveToWorld(Location, Map);
                        summonedSquirrel.Combatant = Combatant;
                    }

                    // Burst attack
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Belding’s Ground Squirrel emerges and attacks with a burst of energy! *");
                    PlaySound(0x02F); // Emerging sound

                    Effects.SendLocationEffect(Location, Map, 0x3728, 15, 0, Hue, 0);
                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Alive && CanBeHarmful(m))
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                            m.SendMessage("You are struck by the emerging squirrel's burst!");
                        }
                    }
                }
            });

            m_NextBurrow = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for Burrow
        }

        private void UndergroundRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Belding’s Ground Squirrel lets out an underground roar! *");
            PlaySound(0x2B8); // Roar sound

            // Create tremor effect
            Effects.SendLocationEffect(Location, Map, 0x1F4, 20, 0, Hue, 0);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.SendMessage("You are disoriented by the underground roar!");
                    
                    // Chance to stun
                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.Freeze(TimeSpan.FromSeconds(2));
                        m.SendMessage("You are stunned by the roar!");
                    }
                }
            }

            m_NextUndergroundRoar = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Underground Roar
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

    // Custom lesser squirrel class
    public class LesserSquirrel : BaseCreature
    {
        [Constructable]
        public LesserSquirrel()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a lesser squirrel";
            Body = 0x116; // Squirrel body
            Hue = 1150; // Slightly different hue

            SetStr(20, 30);
            SetDex(20, 30);
            SetInt(10, 20);

            SetHits(30, 40);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 15, 20);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 20.0, 30.0);
            SetSkill(SkillName.Tactics, 30.0, 40.0);
            SetSkill(SkillName.Wrestling, 30.0, 40.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 20;

            Tamable = false;
        }

        public LesserSquirrel(Serial serial)
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
