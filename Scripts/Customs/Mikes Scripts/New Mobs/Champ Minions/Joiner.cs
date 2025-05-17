using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a joiner")]
    public class Joiner : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between joiner speech
        public DateTime m_NextSpeechTime;
        private DateTime m_NextWallTime;

        [Constructable]
        public Joiner() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Joiner";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Joiner";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item pants = new LongPants(Utility.RandomNeutralHue());
            AddItem(pants);

            Item boots = new Boots(Utility.RandomNeutralHue());
            AddItem(boots);

            Item apron = new FullApron();
            AddItem(apron);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 800);
            SetDex(150, 200);
            SetInt(50, 100);

            SetHits(300, 500);

            SetDamage(8, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Carpentry, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            m_NextWallTime = DateTime.Now;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextWallTime)
            {
                ConstructWoodenWall();
                m_NextWallTime = DateTime.Now + TimeSpan.FromSeconds(30.0); // 30 seconds cooldown for wall construction
            }

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Let's see you get past this!"); break;
                        case 1: this.Say(true, "A little obstacle for you!"); break;
                        case 2: this.Say(true, "Try and get through now!"); break;
                        case 3: this.Say(true, "Block them off!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        private void ConstructWoodenWall()
        {
            if (Combatant != null && InRange(Combatant, 10))
            {
                Point3D wallLocation = Combatant.Location;
                Map map = this.Map;

                if (map != null)
                {
                    Item wall = new Item(0x1BDC); // Wooden wall item ID
                    wall.MoveToWorld(wallLocation, map);
                    Timer.DelayCall(TimeSpan.FromSeconds(20.0), new TimerStateCallback(RemoveWoodenWall), wall); // Wall lasts for 20 seconds

                    this.Say(true, "Building a wall here!");
                }
            }
        }

        private void RemoveWoodenWall(object state)
        {
            Item wall = state as Item;
            if (wall != null && !wall.Deleted)
            {
                wall.Delete();
            }
        }

        public Joiner(Serial serial) : base(serial)
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
