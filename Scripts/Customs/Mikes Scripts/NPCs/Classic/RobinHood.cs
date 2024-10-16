using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Robin Hood")]
    public class RobinHood : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RobinHood() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Robin Hood";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 120;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 2126 });
            AddItem(new StuddedChest() { Hue = 2126 });
            AddItem(new StuddedGloves() { Hue = 2126 });
            AddItem(new StuddedArms() { Hue = 2126 });
            AddItem(new StuddedGorget() { Hue = 2126 });
            AddItem(new Boots() { Hue = 2126 });
            AddItem(new Bow() { Name = "Sir Robin Hood's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize lastRewardTime
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Robin Hood, the outlaw of Sherwood Forest!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as fit as a fiddle, lad!");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a defender of the oppressed, a protector of the poor.");
            }
            else if (speech.Contains("justice"))
            {
                Say("I take from the rich and give to the poor. Do you share my sense of justice?");
            }
            else if (speech.Contains("yes") && HasTalkedAbout("justice"))
            {
                Say("Then you have a noble heart. We shall work together to bring justice to these lands!");
            }
            else if (speech.Contains("outlaw"))
            {
                Say("Yes, I've been branded an outlaw by the corrupt nobles. They've spread tales of my supposed misdeeds, but it's they who are the real villains.");
            }
            else if (speech.Contains("fiddle"))
            {
                Say("Ah, the fiddle reminds me of the merry tunes my friend Allan often plays around our campfire in Sherwood.");
            }
            else if (speech.Contains("oppressed"))
            {
                Say("The oppressed are many, often suffering at the hands of the Sheriff and his men. We do what we can to aid them.");
            }
            else if (speech.Contains("rich"))
            {
                Say("The wealthy nobles of these lands have long exploited the common folk. I've made it my mission to balance the scales, even if just a little.");
            }
            else if (speech.Contains("villains"))
            {
                Say("They hoard wealth and power, ignoring the pleas of the downtrodden. But every once in a while, someone stands up to them. Have you ever stood against injustice?");
            }
            else if (speech.Contains("campfire"))
            {
                Say("Our campfires in Sherwood are a beacon of hope. There, my band of Merry Men and I strategize and rest. Would you like to visit our camp?");
            }
            else if (speech.Contains("aid"))
            {
                Say("We've set up secret caches of supplies throughout the forest to help those in need. If you're willing, I could show you one such cache. But you must swear to use it for the good of the people.");
            }
            else if (speech.Contains("scales"))
            {
                Say("Balancing the scales is a risky endeavor. It often requires stealth, cunning, and a skilled hand with a bow. Are you skilled in any of these arts?");
            }
            else if (speech.Contains("injustice"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Then you know the weight of the battle we're fighting. As a token of appreciation, take this. It has helped me in many tight situations, and I believe it will serve you well.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("visit"))
            {
                Say("Excellent! But beware, for our camp is hidden deep within Sherwood, protected by traps and secrets to deter unwanted guests.");
            }

            base.OnSpeech(e);
        }

        private bool HasTalkedAbout(string topic)
        {
            // Add logic to check if the topic has been discussed.
            // This can be a complex system, so this is a placeholder.
            return true;
        }

        public RobinHood(Serial serial) : base(serial) { }

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
