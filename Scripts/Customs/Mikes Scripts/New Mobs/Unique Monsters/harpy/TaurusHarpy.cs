using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a taurus harpy corpse")]
    public class TaurusHarpy : BaseCreature
    {
        private DateTime m_NextStampede;
        private DateTime m_NextStoneSkin;
        private DateTime m_NextRockfall;
        private DateTime m_NextSeismicRoar;
        private bool m_StoneSkinActive;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public TaurusHarpy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Taurus Harpy";
            Body = 30; // Harpy body
            Hue = 2068; // Earthy brown hue
			BaseSoundID = 402; // Harpy sound

            // Add horns to Harpy

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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public TaurusHarpy(Serial serial)
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
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextStampede = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRockfall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSeismicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextStampede)
                {
                    Stampede();
                }

                if (DateTime.UtcNow >= m_NextStoneSkin)
                {
                    StoneSkin();
                }

                if (DateTime.UtcNow >= m_NextRockfall)
                {
                    Rockfall();
                }

                if (DateTime.UtcNow >= m_NextSeismicRoar)
                {
                    SeismicRoar();
                }
            }
        }

        private void Stampede()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground trembles as the Taurus Harpy charges with a powerful stampede! *");
            PlaySound(0x11D); // Earthquake sound
            Effects.SendLocationEffect(Location, Map, 0x2A3, 16, 10, Hue, 0); // Ground shaking effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are knocked off balance by the tremors!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);

                    // Additional aftershock damage
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        if (m.Alive)
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                            m.SendMessage("The ground shakes again from the aftershock!");
                        }
                    });
                }
            }

            m_NextStampede = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set the cooldown for the next Stampede
        }

        private void StoneSkin()
        {
            if (m_StoneSkinActive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taurus Harpy's skin turns to stone, greatly increasing its defense! *");
            PlaySound(0x1B8); // Stone transformation sound

            this.VirtualArmor += 30; // Increase defense temporarily

            // Damage reflection effect
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.VirtualArmor -= 30; // Reset defense
                m_StoneSkinActive = false;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taurus Harpy's stone skin crumbles away, returning to normal! *");
            });

            m_StoneSkinActive = true;
            m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set the cooldown for the next StoneSkin
        }

        private void Rockfall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Rocks fall from above, crashing down around the Taurus Harpy! *");
            PlaySound(0x1E4); // Rockfall sound
            Effects.SendLocationEffect(Location, Map, 0x376A, 16, 10, Hue, 0); // Rockfall effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("Falling rocks hit you, dealing damage!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                    m.Location = new Point3D(m.X, m.Y, m.Z - 5); // Knockback effect
                }
            }

            m_NextRockfall = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Set the cooldown for the next Rockfall
        }

        private void SeismicRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taurus Harpy lets out a powerful seismic roar! *");
            PlaySound(0x307); // Roar sound
            Effects.SendLocationEffect(Location, Map, 0x3709, 16, 10, Hue, 0); // Roar effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The roar disorients you, reducing your attack speed!");
                    m.Damage(Utility.RandomMinMax(10, 15), this);
                    m.SendMessage("Your attack speed is reduced by the seismic roar!");
                }
            }

            m_NextSeismicRoar = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set the cooldown for the next SeismicRoar
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
            m_NextStampede = DateTime.UtcNow; // Reset to current time on deserialize
            m_NextStoneSkin = DateTime.UtcNow;
            m_NextRockfall = DateTime.UtcNow;
            m_NextSeismicRoar = DateTime.UtcNow;
            m_StoneSkinActive = false;
            m_AbilitiesActivated = false; // Reset flag
        }
    }
}
