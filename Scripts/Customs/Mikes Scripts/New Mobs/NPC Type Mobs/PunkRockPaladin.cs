using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a punk rock paladin")]
    public class PunkRockPaladin : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public PunkRockPaladin() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female"); // Assuming punk names can be taken from elf names
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            // Mohawk hair
            Item hair = new Item(0x2044);
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            // Clothing and equipment
            AddItem(new PlateChest());
            AddItem(new PlateLegs());
            AddItem(new PlateGloves());
            AddItem(new PlateLegs());
            AddItem(new Longsword());
            AddItem(hair);

            // Tattoo - for this example, using the tribal mask to represent a tattoo
            AddItem(new TribalMask());

            // Motorcycle steed not represented in base RunUO. You'll need a custom item or mount.
            // For the sake of this example, I'll use an EtherealHorse which might look like a ghostly steed.
            EtherealHorse motorcycle = new EtherealHorse();
            motorcycle.Rider = this;

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

        public override int TreasureMapLevel { get { return 5; } }
        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Punk's not dead!"); break;
                        case 1: this.Say(true, "To the end, my friend!"); break;
                        case 2: this.Say(true, "Justice and punk rock!"); break;
                        case 3: this.Say(true, "Fight the power!"); break;
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
                        case 0: this.Say(true, "Punk's not dead!"); break;
                        case 1: this.Say(true, "To the end, my friend!"); break;
                        case 2: this.Say(true, "Justice and punk rock!"); break;
                        case 3: this.Say(true, "Fight the power!"); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(300, 350);
            AddLoot(LootPack.UltraRich);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(15, 25)));
        }

        public PunkRockPaladin(Serial serial) : base(serial)
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
