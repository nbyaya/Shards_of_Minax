using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a locksmith")]
    public class Locksmith : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between locksmith speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Locksmith() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Locksmith";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Locksmith";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new Shirt();
            Item pants = new LongPants();
            Item boots = new Shoes();
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(pants);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(400, 600);
            SetDex(300, 450);
            SetInt(200, 300);

            SetHits(300, 500);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Lockpicking, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 80.0, 100.0);
            SetSkill(SkillName.Hiding, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 75.0, 95.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

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
                        case 0: this.Say(true, "Locks are no match for me!"); break;
                        case 1: this.Say(true, "I can pick anything!"); break;
                        case 2: this.Say(true, "Try to keep me out, I dare you!"); break;
                        case 3: this.Say(true, "No lock can keep me out!"); break;
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
                case 0: this.Say(true, "I couldn't unlock my way out of this one..."); break;
                case 1: this.Say(true, "This lock... is... my end..."); break;
            }

            PackItem(new Lockpick(Utility.RandomMinMax(5, 15)));
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
                        case 0: this.Say(true, "You think a few hits can stop me?"); break;
                        case 1: this.Say(true, "I'm just getting started!"); break;
                        case 2: this.Say(true, "Is that all you've got?"); break;
                        case 3: this.Say(true, "I'll unlock your defeat!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public Locksmith(Serial serial) : base(serial)
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
