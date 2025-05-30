using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "corpse of a mischievous witch" )]
    public class MischievousWitch : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds( 10.0 ); // time between witch speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public MischievousWitch() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Hue = Utility.RandomSkinHue();
            
            Female = true;  // Assuming witches are always female.
            Body = 0x191;
            Name = NameList.RandomName("female");
            Title = " the Witch";
			Team = Utility.RandomMinMax(1, 5);

            // Witch outfit
            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            
            Item hat = new WizardsHat();
            hat.Hue = Utility.RandomBlueHue();
            AddItem(hat);

            Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D ) );
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem( hair );

            // Witch attributes
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

        public override bool AlwaysMurderer{ get{ return true; } }
        public override bool CanRummageCorpses{ get{ return true; } }

        public override void OnThink()
        {
            if ( DateTime.Now >= m_NextSpeechTime )
            {
                Mobile combatant = this.Combatant as Mobile;

                if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 8 ) )
                {
                    int phrase = Utility.Random( 4 );

                    switch ( phrase )
                    {
                        case 0: this.Say( true, "I'll turn you into a frog!" ); break;
                        case 1: this.Say( true, "Fly, my pretties!" ); break;
                        case 2: this.Say( true, "You'll make a nice ingredient for my brew!" ); break;
                        case 3: this.Say( true, "Double, double, toil and trouble!" ); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;               
                }

                base.OnThink();
            }           
        }

        public override void GenerateLoot()
        {
            PackGold( 250, 300 );
            AddLoot( LootPack.HighScrolls );

            // Additional specific witch loot can be added here
        }

        public MischievousWitch( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}
