using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Vincent Vintage")]
    public class VincentVintage : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public VincentVintage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Vincent Vintage";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1789 }); // Wine-themed color
            AddItem(new LongPants() { Hue = 1789 });
            AddItem(new Boots() { Hue = 1789 });
            AddItem(new Cloak() { Hue = 1789 });
            AddItem(new TricorneHat() { Hue = 1789 });
            
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
                Say("Ah, greetings! I am Vincent Vintage, a connoisseur of fine wines.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in splendid health, thanks to the tranquility of the vineyard.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to oversee the cellar and ensure every vintage is perfect.");
            }
            else if (speech.Contains("vintage"))
            {
                Say("Ah, vintage. It speaks of time and quality. Do you appreciate the finer things in life?");
            }
            else if (speech.Contains("finer things"))
            {
                Say("Indeed, the finer things are often hidden from plain sight. Seek them out with patience.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is a virtue, much like the aging of a fine wine. It rewards those who wait.");
            }
            else if (speech.Contains("wine"))
            {
                Say("Wine, a product of patience and craft, holds secrets and stories within its bottles.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the cellar are not easily revealed. They require keen observation.");
            }
            else if (speech.Contains("cellar"))
            {
                Say("The cellar is filled with treasures. Hidden among the bottles is a chest of great worth.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Indeed, there is a chest of great value. To find it, one must understand the essence of the cellar.");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of the cellar is in its quiet corners and aged bottles. Look for clues in the shadows.");
            }
            else if (speech.Contains("clues"))
            {
                Say("Clues often lie in the details. Pay attention to the old labels and dusty corners.");
            }
            else if (speech.Contains("labels"))
            {
                Say("Old labels can tell stories. Each one holds a piece of history that might guide you.");
            }
            else if (speech.Contains("history"))
            {
                Say("History is a guide. It shows us the path taken by those who came before us.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path to the chest is paved with knowledge. Each step you take reveals more about its location.");
            }
            else if (speech.Contains("location"))
            {
                Say("The chest's location is hidden, but understanding the path will bring you closer.");
            }
            else if (speech.Contains("closer"))
            {
                Say("You're getting closer to the chest. Remember, patience and observation are your greatest allies.");
            }
            else if (speech.Contains("allies"))
            {
                Say("Allies in this quest are your skills and insights. Use them wisely to uncover the treasure.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("The treasure you seek is within reach. Show your dedication and you shall be rewarded.");
            }
            else if (speech.Contains("dedication"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your dedication has paid off. For your efforts, accept this Vintner's Vault as a token of my gratitude.");
                    from.AddToBackpack(new VintnersVault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public VincentVintage(Serial serial) : base(serial) { }

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
