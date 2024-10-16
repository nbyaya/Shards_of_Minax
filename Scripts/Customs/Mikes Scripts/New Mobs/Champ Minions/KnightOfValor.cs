using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a knight of valor")]
    public class KnightOfValor : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between knight speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public KnightOfValor() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Knight of Valor";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Knight of Valor";
            }

            Item armor = new PlateChest();
            AddItem(armor);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new PlateLegs();
            Item boots = new PlateArms();
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new Broadsword();
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

            SetStr(1000, 1500);
            SetDex(150, 250);
            SetInt(100, 200);

            SetHits(1000, 1500);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.Swords, 100.1, 120.0);
            SetSkill(SkillName.Tactics, 100.1, 120.0);
            SetSkill(SkillName.Parry, 100.1, 120.0);

            Fame = 10000;
            Karma = 10000;

            VirtualArmor = 70;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return true; } }
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
                        case 0: this.Say(true, "Valor shall triumph!"); break;
                        case 1: this.Say(true, "To the front, my comrades!"); break;
                        case 2: this.Say(true, "I will protect you!"); break;
                        case 3: this.Say(true, "Stand strong, we fight for justice!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(500, 750);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My duty is done..."); break;
                case 1: this.Say(true, "Honor... always..."); break;
            }

            PackItem(new GreaterHealPotion(Utility.RandomMinMax(2, 5)));
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
                        case 0: this.Say(true, "You shall not harm my allies!"); break;
                        case 1: this.Say(true, "I will endure!"); break;
                        case 2: this.Say(true, "Your efforts are in vain!"); break;
                        case 3: this.Say(true, "For valor and honor!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public KnightOfValor(Serial serial) : base(serial)
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
