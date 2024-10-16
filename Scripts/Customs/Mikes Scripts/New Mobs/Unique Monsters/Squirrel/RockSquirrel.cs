using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a rock squirrel corpse")]
    public class RockSquirrel : BaseCreature
    {
        private DateTime m_NextRockToss;
        private DateTime m_NextRockShield;
        private DateTime m_NextRockQuake;
        private DateTime m_NextRockSlide;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RockSquirrel()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Rock Squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2400; // Gray with rugged patches hue

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

        public RockSquirrel(Serial serial)
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
                    m_NextRockToss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextRockShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextRockQuake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_NextRockSlide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRockToss)
                {
                    RockToss();
                }

                if (DateTime.UtcNow >= m_NextRockShield)
                {
                    RockShield();
                }

                if (DateTime.UtcNow >= m_NextRockQuake)
                {
                    RockQuake();
                }

                if (DateTime.UtcNow >= m_NextRockSlide)
                {
                    RockSlide();
                }
            }
        }

        private void RockToss()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rock Squirrel hurls a massive rock! *");
            PlaySound(0x2D8); // Rock toss sound

            Point3D loc = Location;
            RockItem rock = new RockItem();
            rock.MoveToWorld(loc, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () => ExplodeRock(rock));

            m_NextRockToss = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for RockToss
        }

        private void ExplodeRock(RockItem rock)
        {
            if (rock.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The rock crashes into the ground, causing debris to fly! *");
            PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(rock.Location, Map, 0x36BD, 20, 10); // Boulder effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are struck by the falling rock and feel the impact!");
                    }
                }
            }

            rock.Delete();
        }

        private void RockShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rock Squirrel creates a protective shield of rocks! *");
            PlaySound(0x1E3); // Shield sound

            FixedParticles(0x3709, 9, 32, 5030, EffectLayer.Waist);

            VirtualArmor += 25; // Increase virtual armor for the shield

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => DeactivateRockShield());

            m_NextRockShield = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for RockShield
        }

        private void DeactivateRockShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rock Squirrel's rock shield fades away. *");
            VirtualArmor -= 25; // Reset virtual armor
        }

        private void RockQuake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rock Squirrel causes the ground to quake violently! *");
            PlaySound(0x308); // Earthquake sound

            Effects.SendLocationEffect(Location, Map, 0x2D9, 30, 10); // Earthquake effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are shaken by a violent quake!");

                    // Knockback effect
                    m.MoveToWorld(m.Location, Map);
                    m.PlaySound(0x308); // Quake sound
                }
            }

            m_NextRockQuake = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for RockQuake
        }

        private void RockSlide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Rock Squirrel summons a landslide of boulders! *");
            PlaySound(0x1B2); // Landslide sound

            Effects.SendLocationEffect(Location, Map, 0x36BD, 30, 10); // Landslide effect

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are buried under a landslide of boulders!");

                    // Knockback effect
                    m.MoveToWorld(m.Location, Map);
                    m.PlaySound(0x1B2); // Landslide sound
                }
            }

            m_NextRockSlide = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for RockSlide
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

    public class RockItem : Item
    {
        public RockItem() : base(0x1BF2) // Rock item ID
        {
            Movable = false;
        }

        public RockItem(Serial serial) : base(serial)
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
