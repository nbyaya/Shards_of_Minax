using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Greco-Roman wrestler")]
    public class GrecoRomanWrestler : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between wrestler speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public GrecoRomanWrestler() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Wrestler";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Wrestler";
            }

            Item tunic = new Tunic();
            Item sandals = new Sandals();
            tunic.Hue = Utility.RandomNeutralHue();
            sandals.Hue = Utility.RandomNeutralHue();

            AddItem(tunic);
            AddItem(sandals);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(900, 1200);
            SetDex(150, 250);
            SetInt(100, 200);

            SetHits(800, 1000);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 85.5, 100.0);
            SetSkill(SkillName.Tactics, 95.1, 120.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 60;

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
                        case 0: this.Say(true, "Feel the power of a true wrestler!"); break;
                        case 1: this.Say(true, "You cannot escape my grip!"); break;
                        case 2: this.Say(true, "Prepare to be pinned!"); break;
                        case 3: this.Say(true, "This is my arena!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(300, 400);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "I... cannot be defeated..."); break;
                case 1: this.Say(true, "This... is not the end..."); break;
            }

            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "Is that all you've got?!"); break;
                        case 1: this.Say(true, "You'll need to try harder!"); break;
                        case 2: this.Say(true, "I've endured worse!"); break;
                        case 3: this.Say(true, "You cannot overpower me!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public GrecoRomanWrestler(Serial serial) : base(serial)
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
