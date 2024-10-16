using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a contortionist")]
    public class Contortionist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between contortionist speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Contortionist() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Contortionist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Contortionist";
            }

            Item shirt = new FancyShirt();
            Item pants = new LongPants(Utility.RandomBrightHue());
            Item boots = new Sandals(Utility.RandomBrightHue());
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));

            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(shirt);
            AddItem(pants);
            AddItem(boots);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(300, 400);
            SetDex(500, 600);
            SetInt(200, 300);

            SetHits(350, 450);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 50);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 85.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

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
                        case 0: this.Say(true, "You can't catch me!"); break;
                        case 1: this.Say(true, "Is that all you've got?"); break;
                        case 2: this.Say(true, "I'm too slippery for you!"); break;
                        case 3: this.Say(true, "Try and bind me!"); break;
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
                case 0: this.Say(true, "You won't bind me..."); break;
                case 1: this.Say(true, "I'll slip away..."); break;
            }

            PackItem(new Ginseng(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomDouble() < 0.5) // 50% chance to dodge
                {
                    this.Say(true, "Missed me!");
                    amount = 0; // dodge the attack
                }
            }
                
            return base.Damage(amount, from);
        }

        public Contortionist(Serial serial) : base(serial)
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
