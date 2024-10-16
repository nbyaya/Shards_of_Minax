using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a goral corpse")]
    public class Goral : BaseCreature
    {
        private DateTime m_NextGoralsGaze;
        private DateTime m_NextStoneSkin;
        private DateTime m_NextQuake;
        private DateTime m_NextRecklessCharge;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Goral()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a goral";
            Body = 0xD1; // Goat body
            Hue = 1910; // Grayish-brown hue
			BaseSoundID = 0x99;

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

        public Goral(Serial serial)
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
                    m_NextGoralsGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextQuake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextRecklessCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGoralsGaze)
                {
                    GoralGaze();
                }

                if (DateTime.UtcNow >= m_NextStoneSkin)
                {
                    StoneSkin();
                }

                if (DateTime.UtcNow >= m_NextQuake)
                {
                    Earthquake();
                }

                if (DateTime.UtcNow >= m_NextRecklessCharge)
                {
                    RecklessCharge();
                }
            }
        }

        private void GoralGaze()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Goral's gaze instills fear in its enemies!*");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    if (Utility.RandomDouble() < 0.30) // 30% chance
                    {
                        m.SendMessage("You are struck with fear by the Goral's gaze!");
                        // Implement simple fleeing logic
                        Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                        {
                            if (m != null && m.Alive)
                            {
                                Point3D newLocation = new Point3D(
                                    m.X + Utility.RandomMinMax(-5, 5),
                                    m.Y + Utility.RandomMinMax(-5, 5),
                                    m.Z
                                );
                                m.MoveToWorld(newLocation, m.Map);
                            }
                        });
                    }
                }
            }

            m_NextGoralsGaze = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void StoneSkin()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Goral's skin turns to stone, enhancing its defenses!*");

            VirtualArmor += 25;

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(ResetStoneSkin));

            m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void ResetStoneSkin()
        {
            VirtualArmor -= 25;
        }

        private void Earthquake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Goral stomps the ground, causing an earthquake!*");

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);
            PlaySound(0x307);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are knocked off balance by the earthquake!");
                }
            }

            m_NextQuake = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void RecklessCharge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Goral charges recklessly at its foe!*");

            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;

                Effects.SendMovingEffect(this, target, 0x36BD, 10, 0, false, false);
                PlaySound(0x666);

                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                target.SendMessage("You are struck by the Goral's reckless charge!");
            }

            m_NextRecklessCharge = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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
