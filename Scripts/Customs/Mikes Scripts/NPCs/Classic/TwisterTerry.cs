using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Twister Terry")]
    public class TwisterTerry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TwisterTerry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Twister Terry";
            Body = 0x190; // Human male body

            // Stats
            Str = 160;
            Dex = 60; // Adjusted for typical male stats
            Int = 110;
            Hits = 110;

            // Appearance
            AddItem(new LongPants() { Hue = 44 });
            AddItem(new Tunic() { Hue = 44 });
            AddItem(new Sandals() { Hue = 1156 });
            AddItem(new LeatherGloves() { Name = "Terry's Twisting Gloves" });
            
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
                Say("I am Twister Terry, master of the mat!");
            }
            else if (speech.Contains("health"))
            {
                Say("Fighting fit and ready to twist!");
            }
            else if (speech.Contains("job"))
            {
                Say("I wrestle foes both in and out of the ring, testing their honor!");
            }
            else if (speech.Contains("wrestle"))
            {
                Say("Honor and humility often grapple on the mat of life. Can one truly be humble and yet stand tall in victory?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Aye, for in accepting defeat with grace, one's honor shines brightest. Remember this, adventurer.");
            }
            else if (speech.Contains("terry"))
            {
                Say("Aye, that's me! Trained in the ancient arts of twist wrestling. I've traveled many lands, but none taught me humility as much as the mantra I once learned.");
            }
            else if (speech.Contains("fit"))
            {
                Say("Each bruise, each scar, is a testament to a lesson learned on the mat. But always, I remember the mantra that gives me strength.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is not just in victory, but in recognizing one's own limits. In the quiet moments, I meditate on the mantra of humility to remind me.");
            }
            else if (speech.Contains("humility"))
            {
                Say("The delicate balance between them is what defines us. If you ever seek humility's essence, remember the second syllable of its mantra: MUH.");
            }
            else if (speech.Contains("lands"))
            {
                Say("From the frigid tundras to the scorching deserts, every land has its own lessons. But the mantra's syllable, MUH, remained a constant source of humility.");
            }
            else if (speech.Contains("scar"))
            {
                Say("Each scar carries a story, a bout I've faced. Yet, in my journey, nothing etched as deeply as the mantra's syllable: MUH.");
            }
            else if (speech.Contains("meditate"))
            {
                Say("Meditation clears the mind, allowing the essence of humility to fill the void. Always, the mantra with the syllable MUH centers me.");
            }
            else if (speech.Contains("ponder"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new Gold(1000)); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public TwisterTerry(Serial serial) : base(serial) { }

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
