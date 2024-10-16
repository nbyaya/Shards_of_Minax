using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cosmo Galileo")]
    public class CosmoGalileo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CosmoGalileo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cosmo Galileo";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomBlueHue() });
            AddItem(new WizardsHat() { Hue = Utility.RandomBlueHue() });
            AddItem(new Sandals() { Hue = Utility.RandomBlueHue() });
            AddItem(new Spellbook() { Name = "Galileo's Observations" });

            // Appearance details
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = Utility.RandomHairHue();

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
                Say("Greetings, I am Cosmo Galileo, stargazer and cosmic explorer. I chart the stars and explore the universe.");
            }
            else if (speech.Contains("cosmo"))
            {
                Say("Indeed, 'Cosmo' is derived from the cosmos, the vast universe that I study.");
            }
            else if (speech.Contains("universe"))
            {
                Say("The universe is an endless expanse full of celestial wonders. Each star tells a story.");
            }
            else if (speech.Contains("celestial"))
            {
                Say("Celestial objects like planets and comets hold mysteries waiting to be discovered.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The mysteries of space are profound. Sometimes, we find answers in the light of distant stars.");
            }
            else if (speech.Contains("stars"))
            {
                Say("Stars are the beacons of the universe, guiding us through the dark expanse of space.");
            }
            else if (speech.Contains("guiding"))
            {
                Say("Stars guide sailors and explorers alike. Their light helps us find our way through the cosmos.");
            }
            else if (speech.Contains("sailors"))
            {
                Say("Sailors have long relied on the stars for navigation. Just as we use them to navigate space.");
            }
            else if (speech.Contains("navigation"))
            {
                Say("Proper navigation is crucial for exploring both the seas and the cosmos. It ensures we stay on course.");
            }
            else if (speech.Contains("exploration"))
            {
                Say("Exploration drives us to seek new horizons. It is a fundamental aspect of discovery, whether on Earth or in space.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery is the pursuit of new knowledge. Every new finding expands our understanding of the universe.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to unlocking the secrets of the universe. With it, we can achieve great things.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the universe are often hidden in plain sight. Sometimes, we need to look deeper.");
            }
            else if (speech.Contains("plain sight"))
            {
                Say("Sometimes the most profound truths are visible, but not always obvious. They require keen observation.");
            }
            else if (speech.Contains("observation"))
            {
                Say("Observation is a powerful tool. It allows us to gather data and make sense of the universe.");
            }
            else if (speech.Contains("data"))
            {
                Say("Data collected from observations helps us form theories and understand cosmic phenomena.");
            }
            else if (speech.Contains("theories"))
            {
                Say("Theories are our best explanations based on the data we collect. They guide further exploration and discovery.");
            }
            else if (speech.Contains("exploration"))
            {
                Say("Exploration is an endless journey. There are always new frontiers to discover and mysteries to solve.");
            }
            else if (speech.Contains("frontiers"))
            {
                Say("Every new frontier presents a challenge and an opportunity for great discoveries.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are what drive us to push beyond our limits and achieve great things in our quest for knowledge.");
            }
            else if (speech.Contains("achieve"))
            {
                Say("To achieve great things, one must be persistent and dedicated to the pursuit of their goals.");
            }
            else if (speech.Contains("goals"))
            {
                Say("Our goals shape our journey and give us direction. With clear goals, we can achieve remarkable feats.");
            }
            else if (speech.Contains("remarkable"))
            {
                Say("Remarkable achievements often start with a simple question or curiosity.");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity drives us to explore and discover new things. It is the spark of innovation and progress.");
            }
            else if (speech.Contains("innovation"))
            {
                Say("Innovation is the result of curiosity and exploration. It leads to breakthroughs and advancements in understanding.");
            }
            else if (speech.Contains("breakthroughs"))
            {
                Say("Breakthroughs in science and exploration often lead to new discoveries and advancements.");
            }
            else if (speech.Contains("advancements"))
            {
                Say("Advancements in our understanding of the universe lead to even more questions and areas of exploration.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding is the goal of our exploration. It helps us make sense of the universe and our place in it.");
            }
            else if (speech.Contains("place"))
            {
                Say("Our place in the universe is a profound question. It drives us to seek knowledge and understanding.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("As a reward for your persistence and curiosity, take this Space Race Cache. It contains wonders from beyond the stars.");
                from.AddToBackpack(new SpaceRaceCache()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }

            base.OnSpeech(e);
        }

        public CosmoGalileo(Serial serial) : base(serial) { }

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
