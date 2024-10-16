using System;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName( "corpse of a pulpy potion purveyor" )]
    public class PulpyPotionPurveyor : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        // Speech Delay and Timing
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds( 20.0 );
        public DateTime m_NextSpeechTime;

        [Constructable]
        public PulpyPotionPurveyor() : base( "the potion purveyor" )
        {
            // Appearance
            Hue = Utility.RandomSkinHue();
            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            // Outfit
            AddItem(new FancyShirt(Utility.RandomBlueHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Shoes());
            AddItem(new StrawHat(Utility.RandomNeutralHue()));

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

        public override void InitSBInfo()
        {
            // Here you can add potions that he sells. For simplicity, I'm adding just a Greater Heal Potion.
            m_SBInfos.Add(new SBMage());  // Adding mage potions for sale.
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                int phrase = Utility.Random(4);
                switch (phrase)
                {
                    case 0: this.Say("Step right up! Elixirs that can cure any ailment!"); break;
                    case 1: this.Say("Magic in every bottle! Get yours today!"); break;
                    case 2: this.Say("Once in a lifetime opportunity to buy the miracle elixir!"); break;
                    case 3: this.Say("Potions brewed from ancient secrets!"); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        public PulpyPotionPurveyor(Serial serial) : base(serial)
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
