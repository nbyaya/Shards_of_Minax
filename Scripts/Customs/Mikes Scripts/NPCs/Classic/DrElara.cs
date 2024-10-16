using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Elara")]
    public class DrElara : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrElara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Elara";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new Skirt() { Hue = 1150 });
            AddItem(new FancyShirt() { Hue = 1133 });
            AddItem(new HalfApron() { Hue = 0 });
            AddItem(new ThighBoots() { Hue = 1103 });
            AddItem(new HalfApron() { Hue = 1910 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(true); // true for female
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            string speech = e.Speech.ToLower();

            if (!from.InRange(this, 3))
                return;

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler! I am Dr. Elara, a scientist.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in perfect health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation is that of a scientist. I study the mysteries of the world.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("The pursuit of knowledge is a noble endeavor. It aligns with the virtue of spirituality. Do you value knowledge?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Knowledge is indeed valuable. It is the key to unlocking the mysteries of our world.");
            }
            else if (speech.Contains("elara"))
            {
                Say("I come from a long line of scholars. My family has always been devoted to the pursuit of knowledge. Have you met other members of my family in your travels?");
            }
            else if (speech.Contains("perfect"))
            {
                Say("Staying active with my experiments and taking occasional breaks to explore the outdoors keeps me in good shape. Do you engage in activities to maintain your health?");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("I've recently been researching a rare herb that is said to have unique properties. If you happen to come across this herb and bring it to me, I might have a reward for you. Are you familiar with the herb I speak of?");
            }
            else if (speech.Contains("spirituality"))
            {
                Say("Spirituality and the quest for understanding go hand in hand. In my studies, I've often turned to ancient tomes for guidance. Have you ever read ancient tomes?");
            }
            else if (speech.Contains("family"))
            {
                Say("Yes, the Elara lineage is known for its intellectual pursuits. My grandfather was a famed historian in his time. Ever heard tales of his exploits?");
            }
            else if (speech.Contains("activities"))
            {
                Say("Activities like meditation, reading, and even the occasional adventure into the wilds help keep my mind and body in sync. Do you have a favorite activity that helps you relax?");
            }
            else if (speech.Contains("herb"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, I see. If you do come across it, remember its unique blue hue and sweet scent. Bring it to me and you'll have my gratitude, as well as a little reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("tomes"))
            {
                Say("Ancient tomes contain wisdom from ages past. I have a collection in my lab, but I'm always on the lookout for more. If you find any, I would be interested in acquiring them.");
            }

            base.OnSpeech(e);
        }

        public DrElara(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
