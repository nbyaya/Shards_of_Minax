using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherdess Anna")]
    public class ShepherdessAnna : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdessAnna() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherdess Anna";
            Body = 0x191; // Human female body

            // Stats
            Str = 78;
            Dex = 58;
            Int = 52;
            Hits = 68;

            // Appearance
            AddItem(new PlainDress() { Hue = 1133 });
            AddItem(new Boots() { Hue = 1152 });
            AddItem(new ShepherdsCrook() { Name = "Shepherdess Anna's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            
            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Shepherdess Anna.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I'm a shepherdess, tending to my flock and the land.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("True virtue lies in the heart, where compassion and humility flourish. What virtues do you hold dear?");
            }
            else if (speech.Contains("anna"))
            {
                Say("Anna is a name passed down in my family for generations. My grandmother was named Anna, and she taught me many tales of old.");
            }
            else if (speech.Contains("good"))
            {
                Say("I believe my good health is a result of the fresh air here and the exercise I get from tending to the sheep. The land has a way of healing.");
            }
            else if (speech.Contains("shepherdess"))
            {
                Say("Being a shepherdess is more than just a job, it's a calling. The sheep depend on me, just as I depend on them for warmth and sustenance. Do you know the importance of wool?");
            }
            else if (speech.Contains("tales"))
            {
                Say("Ah, the tales my grandmother told me! Stories of knights, dragons, and hidden treasures. Once, she gave me a mysterious amulet after sharing a particular tale. If you're interested, I might share its story and reward you for your keen interest.");
            }
            else if (speech.Contains("land"))
            {
                Say("This land has a rich history. Many battles were fought here, and it's said that the spirits of fallen warriors still roam at night. I've felt their presence on more than one occasion.");
            }
            else if (speech.Contains("wool"))
            {
                Say("Wool is a vital resource for us. It provides warmth during the cold winters. I also trade it in the village for other necessities. It's amazing how something so simple can be so valuable.");
            }
            else if (speech.Contains("amulet"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("This amulet was given to my grandmother by a wandering mage. She never told anyone its true power, but she mentioned it held a secret. Since you're interested, here, take it as a gift. Perhaps you can uncover its mystery.");
                    from.AddToBackpack(new HerdingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("spirits"))
            {
                Say("Some nights, when the moon is full, you can hear the spirits whispering. They are not to be feared, but respected. They remind us of the history and the sacrifices made for this land.");
            }
            else if (speech.Contains("trade"))
            {
                Say("Trading is essential for survival. I often exchange wool for food, tools, and sometimes stories from travelers. It's through trade that I learn about the world beyond these hills.");
            }

            base.OnSpeech(e);
        }

        public ShepherdessAnna(Serial serial) : base(serial) { }

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
