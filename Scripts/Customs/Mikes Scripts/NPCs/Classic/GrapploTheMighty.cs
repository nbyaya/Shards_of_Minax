using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grapplo the Mighty")]
    public class GrapploTheMighty : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrapploTheMighty() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grapplo the Mighty";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 100;
            Int = 60;
            Hits = 100;

            // Appearance
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            
            // Equipment
            AddItem(new FancyShirt() { Hue = 2126 });
            AddItem(new ShortPants() { Hue = 2126 });
            AddItem(new ThighBoots() { Hue = 1904 });
            AddItem(new PlateGloves() { Name = "Grapplo's Gloves" });
            
            // Optional: Add extra details to the NPC like Speech or other items
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            if (e.Speech.Contains("name"))
                e.Mobile.Say("I am Grapplo the Mighty, the undisputed champion of the wrestling ring!");

            if (e.Speech.Contains("health"))
                e.Mobile.Say("My health is always at its peak, ready for a challenge!");

            if (e.Speech.Contains("job"))
                e.Mobile.Say("I am a professional wrestler, and I live for the thrill of the fight!");

            if (e.Speech.Contains("wrestle"))
                e.Mobile.Say("Do you have what it takes to step into the ring with me, adventurer?");

            if (e.Speech.Contains("yes"))
                e.Mobile.Say("Excellent! Prepare yourself for a showdown of epic proportions!");

            if (e.Speech.Contains("no"))
                e.Mobile.Say("Very well, if you ever change your mind, I'll be here waiting for a worthy opponent!");

            if (e.Speech.Contains("mighty"))
                e.Mobile.Say("Ah, 'mighty' is not just a title, but a legacy. Passed down from my ancestors, who were all champions in their own right!");

            // Add other responses based on the keywords from the XML file
            
            // Handling rewards
            if (e.Speech.Contains("ancestors") && (DateTime.Now - lastRewardTime).TotalMinutes >= 10)
            {
                e.Mobile.AddToBackpack(new StatCapDeed()); // Reward item
                lastRewardTime = DateTime.Now;
                e.Mobile.Say("My ancestors were warriors, gladiators, and champions of old. Their legacy lives within me. They've left behind a treasure, a reward from their battles. Would you like to have it?");
            }
            
            if (e.Speech.Contains("reward") && (DateTime.Now - lastRewardTime).TotalMinutes < 10)
            {
                e.Mobile.Say("Please wait a bit longer before asking for another reward.");
            }

            if (e.Speech.Contains("reward") && (DateTime.Now - lastRewardTime).TotalMinutes >= 10)
            {
                e.Mobile.AddToBackpack(new TwinPendantOfDespise()); // Reward item
                lastRewardTime = DateTime.Now;
                e.Mobile.Say("Ah, you are indeed interested! Here, take this, a token from the past, a gift from the legacy of Grapplo the Mighty. May it serve you well on your journey.");
            }
        }

        public GrapploTheMighty(Serial serial) : base(serial)
        {
        }

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
