using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a water alchemist")]
    public class WaterAlchemist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speeches
        public DateTime m_NextSpeechTime;

        [Constructable]
        public WaterAlchemist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Alchemist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Alchemist";
            }

            AddItem(new Robe(Utility.RandomBlueHue())); // Alchemist robe
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2049, 0x204A));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(250, 300);
            SetDex(80, 100);
            SetInt(200, 250);
            SetHits(150, 200);

            VirtualArmor = 30;

            SetDamage(5, 10);

            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);
            SetSkill(SkillName.Alchemy, 80.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);

            Fame = 5000;
            Karma = 5000;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
          
            this.Say(true, "Beware the tides of poison!");
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;

        }

        public override void GenerateLoot()
        {
            PackReg(10, 25); // Packs some reagents
            PackGold(100, 150);
            PackItem(new LesserPoisonPotion());
        }

        public override int TreasureMapLevel { get { return 2; } }
        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return true; } }

        public WaterAlchemist(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
