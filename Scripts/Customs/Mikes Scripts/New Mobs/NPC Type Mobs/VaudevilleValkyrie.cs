using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of a valkyrie performer")]
    public class VaudevilleValkyrie : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between valkyrie speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public VaudevilleValkyrie() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            Body = 0x191;  // Female body type
            Name = "Vaudeville Valkyrie";
            Title = "the Warrior Performer";
            
            // Valkyrie armor and attire
            AddItem(new ChainChest());
            AddItem(new ChainLegs());
            AddItem(new VikingSword());
            AddItem(new BronzeShield());
            AddItem(new Sandals(Utility.RandomRedHue()));
            AddItem(new FeatheredHat(Utility.RandomRedHue()));
            
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x204A));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

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
                        case 0: this.Say(true, "Behold the power of a Valkyrie!"); break;
                        case 1: this.Say(true, "You dare challenge a performer?"); break;
                        case 2: this.Say(true, "My skills are unmatched!"); break;
                        case 3: this.Say(true, "For honor and glory!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }  
        }

        public override void GenerateLoot()
        {
            PackGold(400, 450);
            AddLoot(LootPack.Rich);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "Is that all you've got?"); break;
                        case 1: this.Say(true, "You'll need more than that!"); break;
                        case 2: this.Say(true, "My performance has just begun!"); break;
                        case 3: this.Say(true, "I've faced tougher foes on stage!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
            
            return base.Damage(amount, from);
        }

        public VaudevilleValkyrie(Serial serial) : base(serial)
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
