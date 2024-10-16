using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of an assassin")]
    public class Assassin : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Assassin() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Assassin";
            }
            else
            {
                Body = 0x190;
                if (Utility.RandomBool())
                {
                    Name = NameList.RandomName("male");
                    Title = " the Assassin";
                }
                else
                {
                    Name = NameList.RandomName("pirate_male") + " " + NameList.RandomName("pirate_color") + NameList.RandomName("pirate_part");
                }
            }

            Item hood = new HoodedShroudOfShadows();
            AddItem(hood);

            Item shirt = new Shirt(Utility.RandomRedHue());
            AddItem(shirt);

            Item sash = new BodySash(0x85);
            Item pants = new LongPants(Utility.RandomRedHue());
            Item boots = new Boots(Utility.RandomRedHue());

            Item dagger = new Dagger();
            dagger.Movable = false;

            AddItem(sash);
            AddItem(pants);
            AddItem(boots);
            AddItem(dagger);

            sash.Hue = pants.Hue = boots.Hue = Utility.RandomNondyedHue();
            hood.Hue = Utility.RandomHairHue();

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D));
                beard.Hue = hood.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
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

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override int TreasureMapLevel { get { return 4; } }
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
                        case 0: this.Say(true, "You won't see me coming!"); break;
                        case 1: this.Say(true, "Prepare for a quick death!"); break;
                        case 2: this.Say(true, "Silent as a shadow, deadly as a dagger!"); break;
                        case 3: this.Say(true, "Your fate is sealed."); break;
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
                        case 0: this.Say(true, "You won't see me coming!"); break;
                        case 1: this.Say(true, "Prepare for a quick death!"); break;
                        case 2: this.Say(true, "Silent as a shadow, deadly as a dagger!"); break;
                        case 3: this.Say(true, "Your fate is sealed."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My work is done here."); break;
                case 1: this.Say(true, "Fading into the shadows..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public Assassin(Serial serial) : base(serial)
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
