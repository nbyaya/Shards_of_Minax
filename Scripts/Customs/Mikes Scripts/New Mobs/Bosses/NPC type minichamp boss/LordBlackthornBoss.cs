using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of Lord Blackthorn, the Overlord")]
    public class LordBlackthornBoss : LordBlackthorn
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between Lord Blackthorn's speech
        private DateTime m_NextSpeechTime;

        [Constructable]
        public LordBlackthornBoss() : base()
        {
            Name = "Lord Blackthorn, the Overlord";
            Title = "the Supreme Chaos Master";
            Hue = 0x455; // Same shade of grey for Blackthorn

            // Update stats to match or exceed the boss tier
            SetStr(1200); // Increased strength
            SetDex(255); // Increased dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Higher health for a boss-tier fight
            SetDamage(30, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 140.0); // Enhanced magic skills
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Increased resist skill
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            
            Fame = 22500; // Higher fame value
            Karma = -22500; // Negative karma for boss-tier character

            VirtualArmor = 80; // Increased virtual armor for toughness

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime && Combatant != null)
            {
                int phrase = Utility.Random(3);

                switch (phrase)
                {
                    case 0: Say("Embrace the chaos!"); SummonChaosGuard(); break;
                    case 1: Say("Rise and defend your master!"); SummonChaosGuard(); break;
                    case 2: Say("To my side, guardian!"); SummonChaosGuard(); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Add some additional loot, if needed
            PackGold(500, 750);
        }

        public LordBlackthornBoss(Serial serial) : base(serial)
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
