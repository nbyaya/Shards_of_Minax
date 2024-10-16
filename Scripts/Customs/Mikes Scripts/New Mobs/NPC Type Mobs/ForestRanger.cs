using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a forest ranger")]
    public class ForestRanger : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(30.0); // time between ranger's speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public ForestRanger() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Forest Ranger's appearance
            Hue = Utility.RandomSkinHue(); // More human-like skin tones

            // Adjust the body value for humans if your server has different body values
            if (Female = Utility.RandomBool())
            {
                Body = 0x191; // Female body
                Name = NameList.RandomName("female");
                Title = "the Forest Ranger";
            }
            else
            {
                Body = 0x190; // Male body
                Name = NameList.RandomName("male");
                Title = "the Forest Ranger";
            }

            // Forest Rangers' equipment and clothing
            AddItem(new Boots(0x1BB)); // Standard leather boots
            AddItem(new FancyShirt(GetRandomHue())); // A shirt with a natural color
            AddItem(new LongPants(GetRandomHue())); // Pants of a neutral hue
            AddItem(new Cloak(0x59)); // Green cloak

            // Give the ranger a simple hairstyle with a natural hue
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2048)); // Human hair styles
            hair.Hue = 0x3B2; // Brown hair color
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            // Forest Rangers' stats and skills
            SetStr(350, 500);
            SetDex(350, 500);
            SetInt(350, 500);
            SetHits(500, 900);

            SetDamage(100, 200);

            SetSkill(SkillName.Magery, 120.0, 200.0);
			SetSkill(SkillName.Fencing, 120.0, 200.0);
            SetSkill(SkillName.Macing, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 120.0, 200.0);
            SetSkill(SkillName.Swords, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 200.0);
            SetSkill(SkillName.Wrestling, 120.0, 200.0);

            // Rangers would typically have a neutral to positive karma
            Fame = 5000;
            Karma = 5000;

            // Ranger's speech patterns would reflect their guardianship of nature
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        // Implementing the AI for speech
        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say(true, "The forest is under my watch."); break;
                    case 1: this.Say(true, "Respect the woods and they will respect you."); break;
                    case 2: this.Say(true, "I am the shield that guards the realms of trees."); break;
                    case 3: this.Say(true, "Beware the dangers that lurk in the shadows of the forest."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                
                base.OnThink();
            }           
        }

        // Implementing the loot generation
        public override void GenerateLoot()
        {
            // Forest Rangers might carry useful tools for survival, natural remedies, or arrows
            PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
            PackGold(250, 500);
        }

        // ... Remaining methods can remain unaltered unless further customization is desired ...

        public ForestRanger(Serial serial) : base(serial)
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

        private static int GetRandomHue()
        {
            int[] hues = new int[] { 0x89C, 0x8A2, 0x8A8, 0x8AE, 0x8B4, 0x8B8, 0x8BE };
            return hues[Utility.Random(hues.Length)];
        }
    }
}
