using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Merry the Mischievous")]
    public class MerryTheMischievous : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MerryTheMischievous() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Merry the Mischievous";
            Body = 0x190; // Human male body

            // Stats
            Str = 128;
            Dex = 100;
            Int = 100;
            Hits = 86;

            // Appearance
            AddItem(new JesterHat() { Hue = 54 });
            AddItem(new JesterSuit() { Hue = 1930 });
            AddItem(new Shoes() { Hue = 1915 });

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
                Say("Greetings, traveler! I am Merry the Mischievous, a jester by trade.");
            }
            else if (speech.Contains("health"))
            {
                Say("As for my health, I'm as lively as a jester can be!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I bring laughter and amusement to this world!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Ah, but humor and merriment are the greatest battles of all! Do you appreciate a good jest?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ah, a connoisseur of humor! Tell me, what's your favorite jest?");
            }
            else if (speech.Contains("mischievous"))
            {
                Say("Ah, you picked up on that! Indeed, they call me Mischievous because I've played pranks on nearly every citizen in this town.");
            }
            else if (speech.Contains("lively"))
            {
                Say("Indeed, being lively keeps me young at heart. But it's not always about jests, I've had my fair share of adventures!");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Laughter is the best medicine, they say. But sometimes, I find myself in need of a good chuckle. If you bring me a joke that truly tickles me, I might reward you.");
            }
            else if (speech.Contains("pranks"))
            {
                Say("One of my most memorable pranks was when I replaced the blacksmith's iron with rubber. The look on his face was priceless! Still, some didn't find it as amusing as I did.");
            }
            else if (speech.Contains("adventures"))
            {
                Say("Ah, my adventures. There was this one time I ended up in a faraway land, just because I was chasing a butterfly. It led me to a hidden treasure.");
            }
            else if (speech.Contains("joke"))
            {
                Say("Oh, you have a joke for me? Let's hear it! If it truly amuses me, I'll give you something special.");
            }
            else if (speech.Contains("special"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("You've truly made my day! For that, I shall reward you. Check your pockets, there might be something extra there now.");
                    from.AddToBackpack(new CampingAugmentCrystal()); // Replace with the actual item you want to reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MerryTheMischievous(Serial serial) : base(serial) { }

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
