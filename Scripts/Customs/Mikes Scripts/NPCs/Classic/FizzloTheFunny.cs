using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Fizzlo the Funny")]
    public class FizzloTheFunny : BaseCreature
    {
        [Constructable]
        public FizzloTheFunny() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Fizzlo the Funny";
            Body = 0x190; // Human male body

            // Stats
            Str = 92;
            Dex = 60;
            Int = 90;
            Hits = 92;

            // Appearance
            AddItem(new JesterHat() { Hue = 1924 });
            AddItem(new JesterSuit() { Hue = 1254 });
            AddItem(new Boots() { Hue = 1166 });

            // Initialize the NPC
            SpeechHue = 0; // Default speech hue

        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ho, ho, ho! I am Fizzlo the Funny, the jester of cryptic mirth!");
            }
            else if (speech.Contains("health"))
            {
                Say("Ah, health, my friend! A jest can heal the soul. As for my body, it's as lively as ever!");
            }
            else if (speech.Contains("job"))
            {
                Say("My jest is my job, and laughter is my weapon. What about you, traveler? What's your calling?");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Speaking of jests, what virtue brings a smile to your face? Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, or Humility?");
            }
            else if (speech.Contains("intriguing"))
            {
                Say("Ah, the virtues, a tapestry of life's mysteries! They intertwine like jests in a grand performance. Tell me, which virtues do you find most intriguing?");
            }
            else if (speech.Contains("harmonious"))
            {
                Say("Ah, you ponder the enigma of virtue! Remember, my friend, in life's grand jest, the true virtue is found not in isolation but in their harmonious dance.");
            }
            else if (speech.Contains("cryptic"))
            {
                Say("Cryptic, you say? Ah, my riddles and jests often are! Would you like to hear one and test your wit?");
            }
            else if (speech.Contains("lively"))
            {
                Say("Indeed! I owe my energy to a special brew I concoct. It keeps me jumping, tumbling, and jesting all day!");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Laughter, they say, is the best medicine. It's my mission to spread it. Have you witnessed its healing power?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion, a heart that feels and understands. I've seen it heal the deepest of wounds. Ever done a deed of pure compassion?");
            }
            else if (speech.Contains("tapestry"))
            {
                Say("The tapestry of life is woven with many threads, each telling a tale. I've gathered many tales in my travels. Care to hear one?");
            }
            else if (speech.Contains("dance"))
            {
                Say("The dance of virtues is a spectacle, a ballet of balance. To master one, one must understand the dance. Ever danced with destiny?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Ah, a seeker of wisdom! Here's one for you: 'I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.' What am I? Solve it, and a reward awaits!");
            }
            else if (speech.Contains("echo"))
            {
                Say("Correct! Here is your prize!");
                // Add the reward logic here
                from.AddToBackpack(new MaxxiaScroll()); // Example reward item
            }

            base.OnSpeech(e);
        }

        public FizzloTheFunny(Serial serial) : base(serial) { }

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
