using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a karate expert")]
    public class KarateExpert : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speeches
        public DateTime m_NextSpeechTime;

        [Constructable]
        public KarateExpert() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Karate Expert";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Karate Expert";
            }

            Item giTop = new FancyShirt();
            giTop.Hue = 1150; // White
            AddItem(giTop);

            Item giPants = new LongPants();
            giPants.Hue = 1150; // White
            AddItem(giPants);

            Item belt = new HalfApron();
            belt.Hue = 0; // Black Belt
            AddItem(belt);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(700, 1000);
            SetDex(200, 300);
            SetInt(100, 150);

            SetHits(600, 900);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0);
            SetSkill(SkillName.MagicResist, 75.1, 95.0);

            Fame = 8000;
            Karma = -8000;

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
                        case 0: this.Say(true, "Feel the power of my strikes!"); break;
                        case 1: this.Say(true, "Your defenses are useless!"); break;
                        case 2: this.Say(true, "You cannot withstand my skill!"); break;
                        case 3: this.Say(true, "Prepare to be defeated!"); break;
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
                case 0: this.Say(true, "My strength... fails me..."); break;
                case 1: this.Say(true, "You... will pay..."); break;
            }

            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
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
                        case 0: this.Say(true, "You think you can hurt me?!"); break;
                        case 1: this.Say(true, "That barely scratched me!"); break;
                        case 2: this.Say(true, "I'm just getting started!"); break;
                        case 3: this.Say(true, "Is that all you've got?!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public KarateExpert(Serial serial) : base(serial)
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
