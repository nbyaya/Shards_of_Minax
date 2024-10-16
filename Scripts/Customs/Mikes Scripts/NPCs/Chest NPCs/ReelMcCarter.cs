using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Reel McCarter")]
    public class ReelMcCarter : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ReelMcCarter() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Reel McCarter";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 60;
            Int = 95;
            Hits = 70;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomGreenHue() });
            AddItem(new LongPants() { Hue = Utility.RandomRedHue() });
            AddItem(new Shoes() { Hue = Utility.RandomBlueHue() });
            AddItem(new FeatheredHat() { Hue = Utility.RandomBrightHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomPinkHue() });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(true);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(true);

            // Speech Hue
            SpeechHue = Utility.RandomNeutralHue();

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
                Say("Greetings, film buff! I'm Reel McCarter, the curator of cinematic curiosities. Tell me, do you know about the magic of cinema?");
            }
            else if (speech.Contains("magic"))
            {
                Say("Ah, the magic of cinema! It's like a spellbinding reel of stories. Speaking of reels, do you understand the essence of a classic tale?");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of a classic tale is timeless. But before you delve into tales, do you know the significance of a classic director?");
            }
            else if (speech.Contains("director"))
            {
                Say("A classic director shapes the narrative with flair. Yet, even the most skilled directors need a good script. Have you ever pondered the value of a good script?");
            }
            else if (speech.Contains("script"))
            {
                Say("Indeed, a good script is the foundation of any great film. But what's a script without the essence of cinema itself? Tell me, what is the essence of cinema?");
            }
            else if (speech.Contains("tale"))
            {
                Say("A tale in cinema can capture the imagination. Do you know what makes a tale unforgettable? It’s the characters. But more importantly, it's the story behind them.");
            }
            else if (speech.Contains("characters"))
            {
                Say("Characters drive the story, each with their own journey. Speaking of journeys, every film has a journey of its own. Do you know the journey of a classic film?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every classic film's journey is a path filled with twists and turns. But the most important part of any journey is the destination. What do you think is the destination of a classic tale?");
            }
            else if (speech.Contains("destination"))
            {
                Say("The destination of a classic tale is often a revelation. Speaking of revelations, have you ever wondered about the reward of discovering a classic film?");
            }
            else if (speech.Contains("revelation"))
            {
                Say("Revelations in cinema can be profound. For your curiosity and understanding, there's a special reward. But first, tell me, do you understand the value of a good film reel?");
            }
            else if (speech.Contains("reel"))
            {
                Say("A good film reel holds the magic of cinema. To earn the chest of technicolor tales, you must truly appreciate the magic and essence of films.");
            }
            else if (speech.Contains("value"))
            {
                Say("The value of a good film reel lies in its ability to transport us to another world. You've done well to explore these dialogues. Are you ready to receive the chest?");
            }
            else if (speech.Contains("receive"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Patience, my friend. The chest can only be awarded once in a while.");
                }
                else
                {
                    Say("You've explored the world of cinema with great understanding! For your dedication and insight, accept this chest of technicolor tales.");
                    from.AddToBackpack(new TechnicolorTalesChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ReelMcCarter(Serial serial) : base(serial) { }

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
