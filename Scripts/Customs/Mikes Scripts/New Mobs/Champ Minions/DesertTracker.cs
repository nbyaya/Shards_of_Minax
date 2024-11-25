using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a desert tracker")]
    public class DesertTracker : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between tracker speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public DesertTracker() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = Utility.RandomList(0x191, 0x190);
            Name = NameList.RandomName(Body == 0x191 ? "female" : "male");
            Title = " the Desert Tracker";
			Team = 1;

            if (Body == 0x191) // Female
            {
                Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem(hair);
            }
            else // Male
            {
                Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                hair.Hue = Utility.RandomHairHue();
                beard.Hue = hair.Hue;
                hair.Layer = Layer.Hair;
                beard.Layer = Layer.FacialHair;
                hair.Movable = false;
                beard.Movable = false;
                AddItem(hair);
                AddItem(beard);
            }

            Item robe = new Robe();
            robe.Hue = 2406; // Sandy color
            AddItem(robe);

            Item boots = new Sandals();
            boots.Hue = 2406; // Sandy color
            AddItem(boots);

            SetStr(700, 900);
            SetDex(150, 250);
            SetInt(200, 300);

            SetHits(500, 700);

            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 55);

            SetSkill(SkillName.Anatomy, 50.1, 70.0);
            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 85.5, 100.0);
            SetSkill(SkillName.Meditation, 20.1, 40.0);
            SetSkill(SkillName.MagicResist, 90.5, 120.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 70.1, 90.0);

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
                        case 0: this.Say(true, "The sands will swallow you!"); break;
                        case 1: this.Say(true, "You cannot see me through the mirage!"); break;
                        case 2: this.Say(true, "You are lost in the desert!"); break;
                        case 3: this.Say(true, "The desert claims all!"); break;
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
            AddLoot(LootPack.Average);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "I return to the sands..."); break;
                case 1: this.Say(true, "The desert takes me..."); break;
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
                        case 0: this.Say(true, "You cannot harm what you cannot see!"); break;
                        case 1: this.Say(true, "The sands protect me!"); break;
                        case 2: this.Say(true, "Is that the best you can do?"); break;
                        case 3: this.Say(true, "I blend with the dunes!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public DesertTracker(Serial serial) : base(serial)
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
