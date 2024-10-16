using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a katana duelist")]
    public class KatanaDuelist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between duelist speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public KatanaDuelist() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Katana Duelist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Katana Duelist";
            }

            Item kimono = new FancyShirt();
            Item hakama = new LongPants();
            Item boots = new Boots();
            Item katana = new Katana();

            kimono.Hue = Utility.RandomNeutralHue();
            hakama.Hue = Utility.RandomNeutralHue();
            boots.Hue = Utility.RandomNeutralHue();
            katana.Movable = false;

            AddItem(kimono);
            AddItem(hakama);
            AddItem(boots);
            AddItem(katana);

            if (!this.Female)
            {
                Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem(hair);
            }

            SetStr(500, 800);
            SetDex(200, 250);
            SetInt(50, 100);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.Fencing, 95.1, 120.0);
            SetSkill(SkillName.Tactics, 95.1, 120.0);
            SetSkill(SkillName.MagicResist, 75.1, 100.0);
            SetSkill(SkillName.Bushido, 95.1, 120.0);

            Fame = 10000;
            Karma = -10000;

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
                        case 0: this.Say(true, "Prepare yourself!"); break;
                        case 1: this.Say(true, "You will fall by my blade!"); break;
                        case 2: this.Say(true, "Your technique is weak!"); break;
                        case 3: this.Say(true, "Feel the sharpness of my katana!"); break;
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
                case 0: this.Say(true, "My honor... shattered..."); break;
                case 1: this.Say(true, "I... have failed..."); break;
            }

            PackItem(new BlackPearl(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "Is that all you have?"); break;
                        case 1: this.Say(true, "You cannot defeat me!"); break;
                        case 2: this.Say(true, "You will regret that!"); break;
                        case 3: this.Say(true, "Your strikes are futile!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public KatanaDuelist(Serial serial) : base(serial)
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
