using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a blues singing gorgon")]
    public class BluesSingingGorgon : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(15.0); // time between gorgon speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public BluesSingingGorgon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x114; // Gorgon's body ID
            Name = "Blues Singing Gorgon";
            Hue = Utility.RandomGreenHue();
            
            // Dressing up the gorgon
            AddItem(new Robe(Utility.RandomBlueHue()));

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Archery, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.ArmsLore, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Bushido, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Chivalry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Fencing, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Lumberjacking, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Ninjitsu, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Parry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Swords, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Tactics, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Wrestling, Utility.RandomMinMax(50, 100));

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 58;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;        
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say(true, "Ah, the blues of the past..."); break;
                    case 1: this.Say(true, "A tragic tale unfolds through my song."); break;
                    case 2: this.Say(true, "Do you hear the sorrow?"); break;
                    case 3: this.Say(true, "Let me serenade you with a tale of woe."); break;
                }
                
                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;    
            }

            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);
        }

        public override int Damage(int amount, Mobile from)
        {
            if (Utility.RandomBool())
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say(true, "Oh! You interrupt my song!"); break;
                    case 1: this.Say(true, "Can't you appreciate the blues?"); break;
                    case 2: this.Say(true, "Why bring more sorrow?"); break;
                    case 3: this.Say(true, "Each hit, a tragic note."); break;
                }
                
                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public BluesSingingGorgon(Serial serial) : base(serial)
        {
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
