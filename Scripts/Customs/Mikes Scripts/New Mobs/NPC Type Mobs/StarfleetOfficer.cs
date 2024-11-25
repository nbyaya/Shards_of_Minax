using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "corpse of a Starfleet officer" )]
    public class StarfleetOfficer : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds( 10.0 );
        public DateTime m_NextSpeechTime;

        [Constructable]
        public StarfleetOfficer() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if ( Female = Utility.RandomBool() )
            {
                Body = 0x191;
                Name = "Officer " + NameList.RandomName("female"); // Modify as needed for a Starfleet naming list
            }
            else
            {
                Body = 0x190;
                Name = "Officer " + NameList.RandomName("male"); // Modify as needed for a Starfleet naming list
            }
            
            Item shirt = new Shirt();

            // Assign shirt color based on the Starfleet division
            switch( Utility.Random( 3 ) )
            {
                case 0: 
                    shirt.Hue = 0x21; // Red
                    break;
                case 1: 
                    shirt.Hue = 0x5C; // Blue
                    break;
                default: 
                    shirt.Hue = 0x35; // Yellow
                    break;
            }
            AddItem( shirt );
			// Black pants
			Item pants = new LongPants();
			pants.Hue = 0x0001; // Black color
			AddItem( pants );
			Item boots = new Boots();
			boots.Hue = 0x0001; // Black color
			AddItem( boots );

            // Additional attire and equipment here
            // E.g. Starfleet badge, pants, boots, etc.

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
            if ( DateTime.Now >= m_NextSpeechTime )
            {
                Mobile combatant = this.Combatant as Mobile;

                if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 8 ) )
                {
                    int phrase = Utility.Random( 4 );

                    switch ( phrase )
                    {
                        case 0: this.Say( true, "Engage!" ); break;
                        case 1: this.Say( true, "Set phasers to stun!" ); break;
                        case 2: this.Say( true, "Live long and prosper." ); break;
                        case 3: this.Say( true, "We come in peace." ); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }

                base.OnThink();
            }            
            
        }
		
		public override int Damage(int amount, Mobile from)
		{
			Mobile combatant = this.Combatant as Mobile;

			if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
			{
				if (Utility.RandomBool())
				{
					int phrase = Utility.Random(4);

					switch (phrase)
					{
                        case 0: this.Say( true, "Engage!" ); break;
                        case 1: this.Say( true, "Set phasers to stun!" ); break;
                        case 2: this.Say( true, "Live long and prosper." ); break;
                        case 3: this.Say( true, "We come in peace." ); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        public override void GenerateLoot()
        {
            PackGold( 50, 100 );
			AddLoot(LootPack.Rich);
            // Add additional loot as required
        }

        public StarfleetOfficer( Serial serial ) : base( serial )
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
