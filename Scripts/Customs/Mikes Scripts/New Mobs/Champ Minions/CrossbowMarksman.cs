using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a crossbow marksman")]
    public class CrossbowMarksman : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between marksman speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public CrossbowMarksman() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Marksman";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Marksman";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new HeavyCrossbow();
            AddItem(hair);
            AddItem(pants);
            AddItem(boots);
            AddItem(weapon);
            weapon.Movable = false;

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr( 600, 800 );
            SetDex( 177, 255 );
            SetInt( 151, 250 );

            SetHits( 500, 700 );

            SetDamage( 15, 25 );

            SetDamageType( ResistanceType.Physical, 75 );

            SetResistance( ResistanceType.Physical, 50, 65 );
            SetResistance( ResistanceType.Fire, 40, 55 );
            SetResistance( ResistanceType.Cold, 30, 45 );
            SetResistance( ResistanceType.Poison, 60, 75 );
            SetResistance( ResistanceType.Energy, 40, 55 );

            SetSkill( SkillName.Archery, 100.1, 120.0 );
            SetSkill( SkillName.Anatomy, 75.1, 100.0 );
            SetSkill( SkillName.MagicResist, 85.1, 100.0 );
            SetSkill( SkillName.Tactics, 75.1, 100.0 );

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 50;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
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
                        case 0: this.Say(true, "I'll pierce your armor!"); break;
                        case 1: this.Say(true, "You can't hide from my bolts!"); break;
                        case 2: this.Say(true, "My aim is deadly!"); break;
                        case 3: this.Say(true, "You're in my sights!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My aim... failed me..."); break;
                case 1: this.Say(true, "You'll pay for this..."); break;
            }

            PackItem(new Bolt(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "You think that'll stop me?!"); break;
                        case 1: this.Say(true, "Is that all you got?"); break;
                        case 2: this.Say(true, "I've taken on worse than you!"); break;
                        case 3: this.Say(true, "You're in over your head!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public CrossbowMarksman(Serial serial) : base(serial)
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
