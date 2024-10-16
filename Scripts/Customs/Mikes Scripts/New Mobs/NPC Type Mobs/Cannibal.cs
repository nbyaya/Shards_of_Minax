using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a cannibal's corpse")]
    public class Cannibal : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(15.0); // time between speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Cannibal() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Cannibal";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Cannibal";
            }

            AddItem(new Robe(Utility.RandomRedHue())); // Change robe hue to something suitable for a cannibal
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x204A));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Archery, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.ArmsLore, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Bushido, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Chivalry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Fencing, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Lumberjacking, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Ninjitsu, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Parry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Swords, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Tactics, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Wrestling, Utility.RandomMinMax(50, 100));

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 58;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } } // Cannibals are always murderers
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
                        case 0: this.Say(true, "I hunger!"); break;
                        case 1: this.Say(true, "Your flesh will be my feast!"); break;
                        case 2: this.Say(true, "You'll be part of my stew!"); break;
                        case 3: this.Say(true, "I crave your meat!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
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
                        case 0: this.Say(true, "I hunger!"); break;
                        case 1: this.Say(true, "Your flesh will be my feast!"); break;
                        case 2: this.Say(true, "You'll be part of my stew!"); break;
                        case 3: this.Say(true, "I crave your meat!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        public override void GenerateLoot()
        {
            PackGold(250, 500);
            AddLoot(LootPack.Rich);  // Richer loot compared to Brigand
        }

        public Cannibal(Serial serial) : base(serial)
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
