using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a disguise master")]
    public class DisguiseMaster : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speech
        private TimeSpan m_DisguiseDelay = TimeSpan.FromSeconds(30.0); // time between disguises
        public DateTime m_NextSpeechTime;
        public DateTime m_NextDisguiseTime;

        [Constructable]
        public DisguiseMaster() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            ChangeAppearance();

            SetStr(700, 900);
            SetDex(200, 300);
            SetInt(300, 400);

            SetHits(500, 700);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 60.1, 100.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.Meditation, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 100.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 60;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            m_NextDisguiseTime = DateTime.Now + m_DisguiseDelay;
        }

        private void ChangeAppearance()
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Disguise Master";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Disguise Master";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon;
            if (Utility.RandomBool())
                weapon = new Dagger();
            else
                weapon = new Kryss();

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
                        case 0: this.Say(true, "Can you keep up with me?"); break;
                        case 1: this.Say(true, "Try to catch me if you can!"); break;
                        case 2: this.Say(true, "You can't trust your eyes!"); break;
                        case 3: this.Say(true, "I am everywhere and nowhere!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }

            if (DateTime.Now >= m_NextDisguiseTime)
            {
                ChangeAppearance();
                m_NextDisguiseTime = DateTime.Now + m_DisguiseDelay;
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(300, 400);
            AddLoot(LootPack.FilthyRich);

            PackItem(new MandrakeRoot(Utility.RandomMinMax(15, 25)));
        }

        public DisguiseMaster(Serial serial) : base(serial)
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
