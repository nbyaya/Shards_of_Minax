using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a silent movie monk")]
    public class SilentMovieMonk : BaseCreature
    {
        private TimeSpan m_GestureDelay = TimeSpan.FromSeconds(10.0); // time between monk gestures
        public DateTime m_NextGestureTime;

        [Constructable]
        public SilentMovieMonk() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // male body
            Name = "Silent Movie Monk";
            Title = "the Silent";

            AddItem(new Robe(Utility.RandomNeutralHue())); // Robe for the monk
            AddItem(new Sandals(Utility.RandomNeutralHue())); // Sandals for a more monk-like appearance

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

            m_NextGestureTime = DateTime.Now + m_GestureDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextGestureTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    // These gestures mimic silent movie text cards
                    int gesture = Utility.Random(4);

                    switch (gesture)
                    {
                        case 0: Emote("*nods slowly*"); break;
                        case 1: Emote("*points accusingly*"); break;
                        case 2: Emote("*waves hands dramatically*"); break;
                        case 3: Emote("*bows respectfully*"); break;
                    }

                    m_NextGestureTime = DateTime.Now + m_GestureDelay;
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
                        case 0: Emote("*nods slowly*"); break;
                        case 1: Emote("*points accusingly*"); break;
                        case 2: Emote("*waves hands dramatically*"); break;
                        case 3: Emote("*bows respectfully*"); break;
					}

					m_NextGestureTime = DateTime.Now + m_GestureDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(50, 100);
			AddLoot(LootPack.Rich);
            // Other potential loot can be added here
        }

        public SilentMovieMonk(Serial serial) : base(serial)
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
