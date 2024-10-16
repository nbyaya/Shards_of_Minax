using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an epee specialist")]
    public class EpeeSpecialist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between epee specialist speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public EpeeSpecialist() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Epee Specialist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Epee Specialist";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new Kryss();
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

            SetStr(500, 700);
            SetDex(300, 400);
            SetInt(100, 200);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Fencing, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Anatomy, 80.0, 100.0);

            Fame = 4500;
            Karma = -4500;

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
                        case 0: this.Say(true, "Feel the precision of my blade!"); break;
                        case 1: this.Say(true, "I strike where you are weakest!"); break;
                        case 2: this.Say(true, "Your defenses are nothing to me!"); break;
                        case 3: this.Say(true, "You can't escape my blade!"); break;
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
                case 0: this.Say(true, "Even in death, my technique is flawless..."); break;
                case 1: this.Say(true, "My skills... are not enough..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 15)));
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
                        case 0: this.Say(true, "You call that an attack?"); break;
                        case 1: this.Say(true, "I've felt worse from a pinprick!"); break;
                        case 2: this.Say(true, "Is that your best?"); break;
                        case 3: this.Say(true, "Pathetic!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public EpeeSpecialist(Serial serial) : base(serial)
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
