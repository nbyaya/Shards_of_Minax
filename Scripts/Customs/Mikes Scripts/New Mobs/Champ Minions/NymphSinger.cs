using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a nymph singer")]
    public class NymphSinger : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between nymph songs
        public DateTime m_NextSpeechTime;

        [Constructable]
        public NymphSinger() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            Female = true;
            Body = 0x191;
            Name = NameList.RandomName("female");
            Title = "the Nymph Singer";

            Item dress = new FancyDress();
            AddItem(dress);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(300, 400);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(200, 300);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 50.1, 75.0);
            SetSkill(SkillName.Wrestling, 20.1, 40.0);

            Fame = 4500;
            Karma = 4500;

            VirtualArmor = 28;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
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
                        case 0: this.Say(true, "Let my song soothe your anger."); break;
                        case 1: this.Say(true, "Hear my voice and find peace."); break;
                        case 2: this.Say(true, "You don't want to harm me, do you?"); break;
                        case 3: this.Say(true, "My melody will calm your spirit."); break;
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
            AddLoot(LootPack.Average);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My song... fades..."); break;
                case 1: this.Say(true, "The melody... ends..."); break;
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
                        case 0: this.Say(true, "Why hurt someone so beautiful?"); break;
                        case 1: this.Say(true, "Please, I mean no harm!"); break;
                        case 2: this.Say(true, "Listen to my song, not your anger."); break;
                        case 3: this.Say(true, "Would you strike a melody?"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public NymphSinger(Serial serial) : base(serial)
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
