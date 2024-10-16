using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of DJ Rhyme Jones")]
    public class DJRhymeJones : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DJRhymeJones() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "DJ Rhyme Jones";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyDress() { Hue = Utility.RandomBrightHue() }); // Stylish attire
            AddItem(new Bandana() { Hue = Utility.RandomMetalHue() }); // A DJ's signature bandana
            AddItem(new Boots() { Hue = Utility.RandomBrightHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomBrightHue() }); // A shield that represents DJ equipment
            AddItem(new FancyShirt() { Hue = Utility.RandomBrightHue() });

            Hue = Utility.RandomSkinHue(); // Random skin hue for variety
            HairItemID = 0x203B; // Short hair
            HairHue = Utility.RandomHairHue(); // Random hair color
            FacialHairItemID = 0; // No facial hair

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
                Say("Yo! I'm DJ Rhyme Jones, spinning tracks and dropping beats.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as fresh as ever! The beats keep me in prime condition.");
            }
            else if (speech.Contains("job"))
            {
                Say("My gig is all about laying down the hottest tracks and keeping the party live.");
            }
            else if (speech.Contains("beats"))
            {
                Say("The beats are always tight. You gotta feel the rhythm to understand the groove.");
            }
            else if (speech.Contains("vault"))
            {
                Say("You've heard about the vault, huh? It's got the sickest loot in town. Want to know more?");
            }
            else if (speech.Contains("sickest") || speech.Contains("loot"))
            {
                Say("Yeah, it's packed with all kinds of hip-hop swag. But you'll have to prove yourself first.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove yourself, you gotta show you know your hip-hop. Ready for a challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Alright, here's the deal. Answer these questions and you'll earn a special reward.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Great! First question: Who's known as the 'King of Pop'? Hint: Not a rapper.");
            }
            else if (speech.Contains("michael jackson"))
            {
                Say("Correct! Next, what's the name of the legendary East Coast rapper who was known for his deep voice and storytelling?");
            }
            else if (speech.Contains("notorious b.i.g.") || speech.Contains("biggie"))
            {
                Say("Right on! Last question: Which city is famously known for its hip-hop scene and the birth of many rap legends?");
            }
            else if (speech.Contains("new york"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You already got your reward! Come back later for more.");
                }
                else
                {
                    Say("You aced the challenge! Here's the 'Hip-Hop & Rap Vault' chest as a token of your hip-hop knowledge.");
                    from.AddToBackpack(new HipHopRapVault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("no"))
            {
                Say("No worries. If you change your mind, just let me know.");
            }
            else if (speech.Contains("pop"))
            {
                Say("The 'King of Pop' is Michael Jackson. He's got moves that make the world groove.");
            }
            else if (speech.Contains("east coast"))
            {
                Say("The East Coast rapper I'm talking about is Notorious B.I.G., also known as Biggie Smalls. His rhymes are legendary.");
            }
            else if (speech.Contains("west coast"))
            {
                Say("Ah, the West Coast! It’s known for its own style and legends, like Tupac Shakur. But that's a different story.");
            }
            else if (speech.Contains("tupac"))
            {
                Say("Tupac Shakur is a West Coast legend, known for his powerful lyrics and impactful presence.");
            }
            else if (speech.Contains("legends"))
            {
                Say("Legends like Biggie and Tupac have shaped the hip-hop scene with their unique styles and messages.");
            }
            else if (speech.Contains("message"))
            {
                Say("Messages in hip-hop often reflect life experiences, struggles, and triumphs. It's all about expressing what's real.");
            }
            else if (speech.Contains("express"))
            {
                Say("Hip-hop is a form of self-expression, whether through beats, rhymes, or dance. It’s about showcasing your true self.");
            }
            else if (speech.Contains("dance"))
            {
                Say("Dance is a key element of hip-hop culture. It’s not just about moving your body, but about feeling the music.");
            }
            else if (speech.Contains("music"))
            {
                Say("Music drives the soul of hip-hop. It sets the tone, the rhythm, and the message. Without it, there’s no hip-hop.");
            }
            else if (speech.Contains("tone"))
            {
                Say("The tone of a track can change everything – it can be hard-hitting, smooth, or reflective. It's all about setting the mood.");
            }
            else if (speech.Contains("mood"))
            {
                Say("The mood of a track can influence how you feel and react. It’s all part of the vibe.");
            }
            else if (speech.Contains("vibe"))
            {
                Say("Vibe is everything in hip-hop. It’s the essence of the music, the energy it brings, and how it connects with you.");
            }

            base.OnSpeech(e);
        }

        public DJRhymeJones(Serial serial) : base(serial) { }

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
