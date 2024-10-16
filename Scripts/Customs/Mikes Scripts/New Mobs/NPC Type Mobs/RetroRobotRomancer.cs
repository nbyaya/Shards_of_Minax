using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of a tin robot")]
    public class RetroRobotRomancer : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(15.0); // time between robot's poetic expressions
        public DateTime m_NextSpeechTime;

        [Constructable]
        public RetroRobotRomancer() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0x841; // Silver-ish hue to resemble metal
			Body = 0x191;
            Name = "Retro Robot Romancer";
			
			Item jacket = new LeatherChest();
            Item jeans = new LongPants(Utility.RandomBlueHue());
            Item boots = new Boots();
			
			AddItem( jacket );
            AddItem( jeans );
            AddItem( boots );

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
            if (DateTime.Now >= m_NextSpeechTime)
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say(true, "Roses are red, circuits are blue, my code is binary, but my heart beats for you!"); break;
                    case 1: this.Say(true, "I may be made of metal, but my romantic algorithms are gentle."); break;
                    case 2: this.Say(true, "They say love is a complex code, but with you, it's an open-source mode."); break;
                    case 3: this.Say(true, "If I had a heart, it'd be programmed to love you."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
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
						case 0: this.Say(true, "Roses are red, circuits are blue, my code is binary, but my heart beats for you!"); break;
						case 1: this.Say(true, "I may be made of metal, but my romantic algorithms are gentle."); break;
						case 2: this.Say(true, "They say love is a complex code, but with you, it's an open-source mode."); break;
						case 3: this.Say(true, "If I had a heart, it'd be programmed to love you."); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        public override void GenerateLoot()
        {
            PackGold(100, 150);
            PackItem(new IronIngot(5));  // Dropping some iron ingots as part of its loot
            PackGem();
			AddLoot(LootPack.Rich);  // Even richer loot than before
        }

        public RetroRobotRomancer(Serial serial) : base(serial)
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
