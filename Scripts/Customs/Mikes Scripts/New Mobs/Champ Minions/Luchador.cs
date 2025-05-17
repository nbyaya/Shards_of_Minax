using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a luchador")]
    public class Luchador : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between luchador speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Luchador() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Luchadora";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Luchador";
            }

            Item mask = new Item(Utility.RandomList(0x154B, 0x154C)); // Adding a mask
            Item outfit = new BodySash(Utility.RandomBrightHue());
            Item boots = new Sandals(Utility.RandomBrightHue());
            mask.Hue = Utility.RandomBrightHue();
            mask.Layer = Layer.Helm;
            mask.Movable = false;

            AddItem(mask);
            AddItem(outfit);
            AddItem(boots);

            SetStr(700, 1000);
            SetDex(200, 300);
            SetInt(100, 150);

            SetHits(500, 800);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 50, 65);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 70.1, 90.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);
            SetSkill(SkillName.Ninjitsu, 80.1, 100.0);

            Fame = 5000;
            Karma = -5000;

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
                        case 0: this.Say(true, "Feel the power of my lucha!"); break;
                        case 1: this.Say(true, "Prepare to be pinned!"); break;
                        case 2: this.Say(true, "Witness my high-flying skills!"); break;
                        case 3: this.Say(true, "You're no match for a luchador!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The ring... it fades..."); break;
                case 1: this.Say(true, "My lucha... ends..."); break;
            }

            PackItem(new SpinedLeather(Utility.RandomMinMax(5, 10)));
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
                        case 0: this.Say(true, "Is that your best move?!"); break;
                        case 1: this.Say(true, "You can't outwrestle me!"); break;
                        case 2: this.Say(true, "I have more tricks up my sleeve!"); break;
                        case 3: this.Say(true, "You'll have to do better than that!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public Luchador(Serial serial) : base(serial)
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
