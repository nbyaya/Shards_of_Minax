using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of Florida Man")]
    public class FloridaMan : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // Reduced time for quicker summons
        private DateTime m_NextSpeechTime;

        [Constructable]
        public FloridaMan() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = Utility.RandomMinMax(1, 5);

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = "Florida Woman";
            }
            else
            {
                Body = 0x190;
                Name = "Florida Man";
            }
            
            // Clothing
            Item shorts = new ShortPants(Utility.RandomBlueHue());

            AddItem(shorts);
            
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
                    case 0: Say("Watch out for gators!"); SummonAlligators(); break;
                    case 1: Say("Here come the swamp lizards!"); SummonAlligators(); break;
                    case 2: Say("From the swamps, I summon thee!"); SummonAlligators(); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        public void SummonAlligators()
        {
            for (int i = 0; i < 3; i++) // Summons 3 alligators quickly
            {
                Alligator alligator = new Alligator();
                alligator.MoveToWorld(this.Location, this.Map);
                alligator.Combatant = this.Combatant;
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
        }

        public FloridaMan(Serial serial) : base(serial)
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
