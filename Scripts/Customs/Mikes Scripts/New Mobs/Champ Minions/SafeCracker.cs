using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a safe cracker")]
    public class SafeCracker : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between SafeCracker speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public SafeCracker() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Safe Cracker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Safe Cracker";
            }

            Item cloak = new Cloak(Utility.RandomNeutralHue());
            AddItem(cloak);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new Shirt(Utility.RandomNeutralHue());
            Item shoes = new Shoes(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(shoes);

            Item weapon = new Dagger();
            AddItem(weapon);
            weapon.Movable = false;

            SetStr(400, 600);
            SetDex(200, 300);
            SetInt(100, 150);

            SetHits(300, 500);

            SetDamage(8, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 50);
            SetResistance(ResistanceType.Fire, 20, 35);
            SetResistance(ResistanceType.Cold, 30, 45);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 25, 40);

            SetSkill(SkillName.Lockpicking, 90.1, 100.0);
            SetSkill(SkillName.Stealing, 90.1, 100.0);
            SetSkill(SkillName.Stealth, 90.1, 100.0);
            SetSkill(SkillName.Hiding, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 75.1, 100.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 4000;
            Karma = -4000;

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
                        case 0: this.Say(true, "I'll crack this open!"); break;
                        case 1: this.Say(true, "No lock can stop me."); break;
                        case 2: this.Say(true, "Let's see what's inside."); break;
                        case 3: this.Say(true, "This is too easy."); break;
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
            AddLoot(LootPack.Meager);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "I couldn't unlock..."); break;
                case 1: this.Say(true, "I'll get you next time..."); break;
            }

            PackItem(new Lockpick(Utility.RandomMinMax(5, 15)));
        }

        public SafeCracker(Serial serial) : base(serial)
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
