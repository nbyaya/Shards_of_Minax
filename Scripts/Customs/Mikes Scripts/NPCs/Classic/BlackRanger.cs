using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Black Ranger")]
    public class BlackRanger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BlackRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Black Ranger";
            Body = 0x190; // Human male body

            // Stats
            Str = 115;
            Dex = 100;
            Int = 60;
            Hits = 85;

            // Appearance
            AddItem(new PlateLegs() { Hue = 1 });
            AddItem(new PlateChest() { Hue = 1 });
            AddItem(new PlateHelm() { Hue = 1 });
            AddItem(new PlateGloves() { Hue = 1 });
            AddItem(new Boots() { Hue = 1 });
            AddItem(new WarAxe() { Name = "Black Ranger's Axe" });

            // Standard hair information
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am the Black Ranger, guardian of the shadows!");
            }
            else if (speech.Contains("health"))
            {
                Say("My essence is shrouded in mystery, and my health remains concealed.");
            }
            else if (speech.Contains("job"))
            {
                Say("I walk the path of shadows, protecting the realm from unseen threats.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Within the darkness, one must find their own light. Do you seek wisdom?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your response reveals much. Seek the hidden paths, for therein lies true strength.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are not merely absence of light, they hold secrets and mysteries of their own. In them, I've found purpose.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("There are secrets that even I, the Black Ranger, do not fully understand. But for your dedication, I shall share one with you. Have you heard of the Whispering Grove?");
            }
            else if (speech.Contains("grove"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the Whispering Grove, a hidden place where trees murmur ancient tales. Seek it out, and you might discover truths long forgotten. And for your curiosity, take this as a token of my appreciation.");
                    from.AddToBackpack(new BanishingRod()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("purpose"))
            {
                Say("Every shadow has its purpose, every step in darkness is a step towards enlightenment. What is your purpose, traveler?");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Enlightenment is the culmination of wisdom and experience. It's not just about knowledge, but understanding one's place in the vast tapestry of existence.");
            }
            else if (speech.Contains("tapestry"))
            {
                Say("Life is a complex tapestry of interwoven fates and destinies. Every thread, no matter how insignificant, plays a part in the grand design.");
            }
            else if (speech.Contains("destinies"))
            {
                Say("Destinies are not set in stone, but shaped by our choices. Even in shadows, one can forge their own destiny.");
            }

            base.OnSpeech(e);
        }

        public BlackRanger(Serial serial) : base(serial) { }

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
