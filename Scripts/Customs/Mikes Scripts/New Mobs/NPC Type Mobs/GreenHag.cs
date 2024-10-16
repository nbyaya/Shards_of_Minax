using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a green hag")]
    public class GreenHag : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between hag's speech
        private DateTime m_NextSpeechTime;
        
        [Constructable]
        public GreenHag() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x191; // Using a female model for the hag
            Name = "Green Hag";
            Hue = 0x4001; // A green hue
            
            // Hag attire
            Item raggedDress = new Robe();
            raggedDress.Hue = 0x47E;
            raggedDress.Name = "ragged dress";
            AddItem(raggedDress);
            
            // Abilities and stats
			SetStr( 800, 1200 );
			SetDex( 177, 255 );
			SetInt( 151, 250 );

			SetHits( 600, 1000 );

			SetDamage( 10, 20 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 95.5, 100.0 );
			SetSkill( SkillName.Meditation, 25.1, 50.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
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

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime && Combatant != null)
            {
                int phrase = Utility.Random(3);

                switch (phrase)
                {
                    case 0: Say("Come forth, my minions!"); SummonCreature(); break;
                    case 1: Say("From the shadows, aid me!"); SummonCreature(); break;
                    case 2: Say("Rise and fight for me!"); SummonCreature(); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        public void SummonCreature()
        {
            BaseCreature creature;

            switch (Utility.Random(2))
            {
                default:
                case 0: creature = new Troll(); break;
                case 1: creature = new Ettin(); break;
            }

            creature.MoveToWorld(this.Location, this.Map);
            creature.Combatant = this.Combatant;
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(250, 300);
            PackItem(new MandrakeRoot());
        }

        public GreenHag(Serial serial) : base(serial)
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
