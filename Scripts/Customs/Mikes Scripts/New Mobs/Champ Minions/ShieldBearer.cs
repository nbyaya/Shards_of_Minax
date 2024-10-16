using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a shield bearer")]
    public class ShieldBearer : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between shield bearer speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public ShieldBearer() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Shield Bearer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Shield Bearer";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item tunic = new PlateChest();
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item shield = new HeaterShield();
            shield.Movable = false;

            AddItem(hair);
            AddItem(tunic);
            AddItem(boots);
            AddItem(shield);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(900, 1200);
            SetDex(90, 120);
            SetInt(60, 80);

            SetHits(800, 1000);

            SetDamage(8, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);
            SetSkill(SkillName.Parry, 90.0, 100.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 70;

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
                        case 0: this.Say(true, "Stay behind me, I'll protect you!"); break;
                        case 1: this.Say(true, "You'll have to get through me first!"); break;
                        case 2: this.Say(true, "My shield will guard us!"); break;
                        case 3: this.Say(true, "I won't let you fall!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 300);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My shield... has failed..."); break;
                case 1: this.Say(true, "I have... fallen..."); break;
            }

            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            if (Utility.RandomBool())
            {
                this.Say(true, "My shield absorbs the blow!");
                amount = (int)(amount * 0.7); // reduce damage by 30%
            }

            return base.Damage(amount, from);
        }

        public ShieldBearer(Serial serial) : base(serial)
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
