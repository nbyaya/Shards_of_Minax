using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Scientist Galileo")]
    public class ScientistGalileo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ScientistGalileo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Scientist Galileo";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 110;
            Hits = 60;

            // Appearance
            AddItem(new LongPants() { Hue = 1154 });
            AddItem(new FancyShirt() { Hue = 1156 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new WideBrimHat() { Hue = 1103 });
            AddItem(new LeatherArms() { Name = "Galileo's Arms" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Scientist Galileo.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in excellent health, thank you for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation? I'm a scientist, dedicated to unraveling the mysteries of the universe.");
            }
            else if (speech.Contains("stars secrets"))
            {
                Say("Scientific inquiry is the key to understanding the world. Have you ever gazed at the stars and wondered about their secrets?");
            }
            else if (speech.Contains("night sky"))
            {
                Say("Fascinating! Tell me, what do you see when you look up at the night sky?");
            }
            else if (speech.Contains("universe"))
            {
                Say("The universe is vast and enigmatic. Its sheer vastness sometimes keeps me awake at night. There's one particular constellation that intrigues me the most. Do you know of the Celestial Serpent?");
            }
            else if (speech.Contains("serpent"))
            {
                Say("Ah, the Celestial Serpent! It's a constellation that is said to hold a hidden message. I've been trying to decipher it for years. If you ever come across any ancient scrolls or texts about it, do let me know. I might reward you for your help.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Yes, if you can bring me any valuable information about the Celestial Serpent, I will gladly compensate you for your effort. Knowledge is the greatest treasure, but I understand that material rewards can be motivating too.");
                    from.AddToBackpack(new BagOfBombs()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the beacon that dispels the darkness of ignorance. In my studies, I've often come across ancient tales of mystical artifacts. Have you heard of the Luminous Orb?");
            }
            else if (speech.Contains("orb"))
            {
                Say("The Luminous Orb is said to be a powerful artifact that can amplify one's understanding of the cosmos. Legends say it's hidden in a lost temple. Finding it could revolutionize our understanding of the stars.");
            }
            else if (speech.Contains("temple"))
            {
                Say("The temple, as legends describe, is hidden deep within the Whispering Forest. Many have tried to find it, but the forest's illusions have led them astray. Tread carefully if you decide to embark on such a quest.");
            }
            else if (speech.Contains("forest"))
            {
                Say("It's a dense and magical forest located to the north. The trees there are said to whisper secrets to those who listen closely. However, it's easy to get lost, so ensure you're well-prepared if you venture in. Take this!");
                from.AddToBackpack(new BagOfBombs()); // Give an additional item as a hint or aid
            }

            base.OnSpeech(e);
        }

        public ScientistGalileo(Serial serial) : base(serial) { }

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
