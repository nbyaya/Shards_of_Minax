using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a scout leader")]
    public class ScoutLeader : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between scout speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public ScoutLeader() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Scout Leader";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Scout Leader";
            }

            Item cloak = new Cloak(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());
            Item bow = new Bow();

            AddItem(cloak);
            AddItem(boots);
            AddItem(bow);
            bow.Movable = false;

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

            SetStr(400, 600);
            SetDex(300, 400);
            SetInt(200, 300);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.Archery, 100.1, 120.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 70.1, 90.0);

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
                        case 0: this.Say(true, "Enemy movements detected!"); break;
                        case 1: this.Say(true, "Watch for their weak points!"); break;
                        case 2: this.Say(true, "They are vulnerable now!"); break;
                        case 3: this.Say(true, "Strike now while they are weak!"); break;
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
                case 0: this.Say(true, "The scout... mission failed..."); break;
                case 1: this.Say(true, "You will pay for this..."); break;
            }

            PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
        }

        public ScoutLeader(Serial serial) : base(serial)
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
