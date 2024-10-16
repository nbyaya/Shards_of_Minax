using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a candy corn creep corpse")]
    public class CandyCornCreep : BaseCreature
    {
        private DateTime m_NextCornfieldCover;
        private DateTime m_NextStickyCorn;
        private DateTime m_NextCornStorm;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CandyCornCreep()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Candy Corn Creep";
            Body = 0xCF; // Sheep body
            Hue = 2356; // Unique hue
			BaseSoundID = 0xD6;

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

        public CandyCornCreep(Serial serial) : base(serial) { }

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
                    m_NextCornfieldCover = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStickyCorn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCornStorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCornfieldCover)
                {
                    CornfieldCover();
                }

                if (DateTime.UtcNow >= m_NextStickyCorn)
                {
                    StickyCorn();
                }

                if (DateTime.UtcNow >= m_NextCornStorm)
                {
                    CornStorm();
                }
            }
        }

        private void CornfieldCover()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Candy Corn Creep hides in a field of sticky, colorful corn!*");
            PlaySound(0x1F5); // Sound for effect

            Point3D loc = Location;
            for (int x = -3; x <= 3; x++)
            {
                for (int y = -3; y <= 3; y++)
                {
                    Point3D loc2 = new Point3D(loc.X + x, loc.Y + y, loc.Z);
                    if (Map.CanFit(loc2, 16, false, false))
                    {
                        Item candyCorn = new Item(0x0C7D); // Placeholder graphic, use an appropriate one
                        candyCorn.MoveToWorld(loc2, Map);
                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => candyCorn.Delete());
                    }
                }
            }

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("Your vision is obscured by the field of candy corn!");
                }
            }

            m_NextCornfieldCover = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown
        }

        private void StickyCorn()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Candy Corn Creep throws sticky candy corn!*");
            PlaySound(0x1F5); // Throw sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, 10, 0, 0, 0, 0, 0); // Apply damage
                    m.SendMessage("You are slowed down by sticky candy corn!");
                    m.SendMessage("Your movement speed is reduced!");

                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        if (m.Alive)
                        {
                            m.SendMessage("The sticky candy corn's effect wears off.");
                        }
                    });
                }
            }

            m_NextStickyCorn = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown
        }

        private void CornStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Candy Corn Creep summons a storm of candy corn!*");
            PlaySound(0x307); // Storm sound effect

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 2), () =>
                {
                    Point3D loc = Location;
                    Item candyCorn = new Item(0x0C7D); // Placeholder graphic, use an appropriate one
                    candyCorn.MoveToWorld(loc, Map);

                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Alive && CanBeHarmful(m))
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 0); // Random damage
                            m.SendMessage("You are hit by a storm of candy corn!");
                        }
                    }

                    Timer.DelayCall(TimeSpan.FromSeconds(5), () => candyCorn.Delete()); // Remove after 5 seconds
                });
            }

            m_NextCornStorm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown
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
            m_AbilitiesInitialized = false;
        }
    }
}
