using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an urban tracker")]
    public class UrbanTracker : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between tracker speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public UrbanTracker() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Urban Tracker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Urban Tracker";
            }

            Item hood = new HoodedShroudOfShadows();
            AddItem(hood);
            
            Item tunic = new LeatherChest();
            AddItem(tunic);
            
            Item leggings = new LeatherLegs();
            AddItem(leggings);
            
            Item boots = new ThighBoots();
            AddItem(boots);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item weapon = new Dagger();
            weapon.Movable = false;
            AddItem(weapon);

            SetStr(500, 800);
            SetDex(300, 450);
            SetInt(150, 250);

            SetHits(400, 700);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 70.1, 100.0);
            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Hiding, 90.1, 100.0);
            SetSkill(SkillName.Stealth, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.5, 100.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 70.1, 100.0);
            SetSkill(SkillName.Fencing, 90.1, 100.0);

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
                        case 0: this.Say(true, "You can't catch what you can't see!"); break;
                        case 1: this.Say(true, "Lost in the maze, aren't you?"); break;
                        case 2: this.Say(true, "I strike from the shadows!"); break;
                        case 3: this.Say(true, "Try to keep up!"); break;
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
                case 0: this.Say(true, "The shadows... they consume me..."); break;
                case 1: this.Say(true, "I will return..."); break;
            }

            PackItem(new BlackPearl(Utility.RandomMinMax(5, 15)));
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
                        case 0: this.Say(true, "Is that your best shot?"); break;
                        case 1: this.Say(true, "You can't touch me!"); break;
                        case 2: this.Say(true, "I'm too quick for you!"); break;
                        case 3: this.Say(true, "You'll never hit me!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public UrbanTracker(Serial serial) : base(serial)
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
