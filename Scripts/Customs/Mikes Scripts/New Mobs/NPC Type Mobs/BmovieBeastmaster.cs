using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a B-movie Beastmaster")]
    public class BmovieBeastmaster : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between Beastmaster speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public BmovieBeastmaster() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the B-movie Beastmaster";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the B-movie Beastmaster";
            }

            AddItem(new Robe(Utility.RandomRedHue()));

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

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
                        case 0: this.Say(true, "Behold my monster minions!"); break;
                        case 1: this.Say(true, "You cannot escape the terror of vintage horror!"); break;
                        case 2: this.Say(true, "Feel the wrath of stop-motion!"); break;
                        case 3: this.Say(true, "My rubber suit creatures will devour you!"); break;
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
                        case 0: this.Say(true, "Behold my monster minions!"); break;
                        case 1: this.Say(true, "You cannot escape the terror of vintage horror!"); break;
                        case 2: this.Say(true, "Feel the wrath of stop-motion!"); break;
                        case 3: this.Say(true, "My rubber suit creatures will devour you!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The silver screen shall avenge me!"); break;
                case 1: this.Say(true, "Cut! That's a wrap..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public BmovieBeastmaster(Serial serial) : base(serial)
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
