using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the gang overlord")]
    public class GangLeaderBoss : GangLeader
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between gang leader's speech
        private DateTime m_NextSpeechTime;

        [Constructable]
        public GangLeaderBoss() : base()
        {
            Name = "Gang Overlord";
            Title = "the Supreme Leader";

            // Enhanced stats
            SetStr(1200); // Upper range of strength for a boss
            SetDex(255); // Max dexterity for a boss
            SetInt(250); // Max intelligence for a boss

            SetHits(12000); // High health to match the boss tier
            SetDamage(20, 40); // Higher damage to be a challenge

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80); // Higher resistance
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Boss-tier armor

            // Attach a random ability to make it even more unpredictable
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime && Combatant != null)
            {
                int phrase = Utility.Random(3);

                switch (phrase)
                {
                    case 0: Say("Roll out, biker!"); SummonBiker(); break;
                    case 1: Say("Get them, wastelander!"); SummonBiker(); break;
                    case 2: Say("Let's burn rubber!"); SummonBiker(); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        public void SummonBiker()
        {
            WastelandBiker biker = new WastelandBiker();
            biker.MoveToWorld(this.Location, this.Map);
            biker.Combatant = this.Combatant;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            PackGold(500, 1000); // Increased gold drop for a boss
        }

        public GangLeaderBoss(Serial serial) : base(serial)
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
