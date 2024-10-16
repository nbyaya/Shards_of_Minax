using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("corpse of a flutist")]
    public class Flutist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between flutist actions
        public DateTime m_NextSpeechTime;
        private DateTime m_NextMelodyTime;
        private TimeSpan m_MelodyDelay = TimeSpan.FromSeconds(20.0);

        [Constructable]
        public Flutist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x190; // human male
            Hue = Utility.RandomSkinHue();
            Name = NameList.RandomName("male");
            Title = "the Flutist";

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item sandals = new Sandals();

            AddItem(robe);
            AddItem(sandals);
            
            SetStr(100, 200);
            SetDex(150, 200);
            SetInt(250, 350);

            SetHits(200, 300);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Musicianship, 100.0, 120.0);
            SetSkill(SkillName.Provocation, 100.0, 120.0);
            SetSkill(SkillName.Discordance, 100.0, 120.0);
            SetSkill(SkillName.Peacemaking, 100.0, 120.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 30;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            m_NextMelodyTime = DateTime.Now + m_MelodyDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                int phrase = Utility.Random(3);
                switch (phrase)
                {
                    case 0: this.Say(true, "Let the music soothe your soul."); break;
                    case 1: this.Say(true, "Feel the power of my melody."); break;
                    case 2: this.Say(true, "You can't resist my tune."); break;
                }
                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            if (DateTime.Now >= m_NextMelodyTime)
            {
                PlayMelody();
                m_NextMelodyTime = DateTime.Now + m_MelodyDelay;
            }

            base.OnThink();
        }

        private void PlayMelody()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && this.CanBeHarmful(m, false) && !m.Blessed && m.Player)
                {
                    targets.Add(m);
                }
            }

            foreach (Mobile target in targets)
            {
                if (Utility.RandomDouble() < 0.5)
                {
                    this.Say(true, "Behold the power of my melody!");
                }
                else
                {
                    target.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6)));
                    this.Say(true, "You are entranced by my tune!");
                }
            }
        }

        public Flutist(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 2);
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
