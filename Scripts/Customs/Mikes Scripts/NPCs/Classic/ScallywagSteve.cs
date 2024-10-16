using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Scallywag Steve")]
    public class ScallywagSteve : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ScallywagSteve() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Scallywag Steve";
            Body = 0x190; // Human male body

            // Stats
            Str = 135;
            Dex = 65;
            Int = 20;
            Hits = 92;

            // Appearance
            AddItem(new TricorneHat() { Hue = 2122 });
            AddItem(new FancyShirt() { Hue = 1154 });
            AddItem(new ShortPants() { Hue = 1158 });
            AddItem(new Boots() { Hue = 38 });
            AddItem(new Longsword() { Name = "Steve's Sneaky Saber" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Arrr, ye be talkin' to Scallywag Steve, the saltiest pirate this side of the Seven Seas!");
            }
            else if (speech.Contains("health"))
            {
                Say("Me health be as sturdy as a ship's hull!");
            }
            else if (speech.Contains("job"))
            {
                Say("Me job? I be a pirate, laddin' the high seas and plunderin' like there's no tomorrow!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Ye think ye have what it takes to sail with Scallywag Steve, do ye?");
            }
            else if (speech.Contains("yes") && LastSpeechWas("battles"))
            {
                Say("Har har har! Ye've got spunk, I'll give ye that! But remember, on me ship, I be the captain, and I expect nothin' less than absolute loyalty. Can ye handle that?");
            }
            else if (speech.Contains("scallywag"))
            {
                Say("Aye, that be me moniker! But most just call me Steve. Got it from a particularly wild adventure involving a ghost ship and a cursed treasure.");
            }
            else if (speech.Contains("hull"))
            {
                Say("Aye, the ship's hull! Reminds me of the time me crew and I battled the kraken. That be a tale of courage and cunning!");
            }
            else if (speech.Contains("plunder"))
            {
                Say("Ah, plunderin' be the best part of bein' a pirate. Especially when ye come across a ship full of gold doubloons and rare gems. But it ain't always easy; sometimes ye run into fierce competitors!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Aye, that cursed treasure... Hidden on the Isle of the Dead Man's Chest. Many have tried to find it, but none have returned. If ye be brave enough, I might share its location for a price.");
            }
            else if (speech.Contains("kraken"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("That kraken was a mighty beast, nearly tore our ship apart! But with some quick thinking, a barrel of rum, and a lit torch, we sent it back to the depths from whence it came. A memory I'll never forget, and for your interest, here's a token of appreciation.");
                    from.AddToBackpack(new ItemIdentificationAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("competitors"))
            {
                Say("Competitors like Blackbeard and Calico Jack. Fierce pirates, but none match the cunning of Scallywag Steve! They've tried to best me many a time, but I always come out on top!");
            }

            base.OnSpeech(e);
        }

        private bool LastSpeechWas(string keyword)
        {
            // This method checks if the previous speech contained the given keyword.
            // Implement this method based on your specific needs and logic.
            return false; // Placeholder; needs actual implementation.
        }

        public ScallywagSteve(Serial serial) : base(serial) { }

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
