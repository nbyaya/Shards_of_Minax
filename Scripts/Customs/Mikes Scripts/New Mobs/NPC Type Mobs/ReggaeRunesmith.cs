using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a chilled runesmith")]
    public class ReggaeRunesmith : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public ReggaeRunesmith() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");  // For now using elf names, you can change this
                Title = "the Reggae Enchanter";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");  // For now using elf names, you can change this
                Title = "the Rasta Scribe";
            }

            AddItem(new StrawHat(Utility.RandomGreenHue())); // A straw hat for a rasta look
            AddItem(new Robe(Utility.RandomGreenHue())); // Robe in shades of green

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041));
                beard.Hue = HairHue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

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

        public override int TreasureMapLevel { get { return 3; } }
        public override bool CanRummageCorpses { get { return true; } }

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
                        case 0: this.Say(true, "Feel the rhythm of the runes!"); break;
                        case 1: this.Say(true, "Irie vibes only, mon!"); break;
                        case 2: this.Say(true, "One love, one rune!"); break;
                        case 3: this.Say(true, "Don't kill the vibe, mon!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Rich);
            
            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The vibes shall continue!"); break;
                case 1: this.Say(true, "Reggae never dies!"); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
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
                        case 0: this.Say(true, "That's not very irie of you!"); break;
                        case 1: this.Say(true, "You can't stop the rhythm!"); break;
                        case 2: this.Say(true, "Feel the magic, mon!"); break;
                        case 3: this.Say(true, "Jah protect me!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public ReggaeRunesmith(Serial serial) : base(serial)
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
