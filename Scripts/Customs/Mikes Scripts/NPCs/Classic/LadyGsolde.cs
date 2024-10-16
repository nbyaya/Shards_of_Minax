using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Gsolde")]
    public class LadyGsolde : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyGsolde() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Gsolde";
            Body = 0x191; // Human female body

            // Stats
            Str = 152;
            Dex = 64;
            Int = 24;
            Hits = 107;

            // Appearance
            AddItem(new LeatherSkirt() { Hue = 1300 });
            AddItem(new ChainChest() { Hue = 1300 });
            AddItem(new PlateHelm() { Hue = 1300 });
            AddItem(new PlateGloves() { Hue = 1300 });
            AddItem(new Boots() { Hue = 1300 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Lady Gsolde, a woman of impeccable taste and refinement. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care about my health? It's not as if anyone in this wretched place cares about me.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\"? I'm here, aren't I? Isn't that enough to satisfy your curiosity?");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you think you're valiant? Prove it! Tell me, what would you do if you were stuck in a place like this?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! That's what I thought. No one can truly understand the misery of my existence. Begone!");
            }
            else if (speech.Contains("impeccable"))
            {
                Say("Yes, 'impeccable' isn't a word thrown around lightly. I come from a line of nobles known for their taste and class. Have you heard of the Fallen Keep?");
            }
            else if (speech.Contains("wretched"))
            {
                Say("This place, it's a shadow of the world I once knew. Everything changed after the dark mage cast his curse upon my family. Do you know of this curse?");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Your curiosity reminds me of a bard named Ellion who once wrote tales of our land. He captured stories that many have forgotten. Have you met him?");
            }
            else if (speech.Contains("stuck"))
            {
                Say("Being stuck is a mindset. My great-grandfather once told me about an enchanted amulet that could free a person's spirit. Would you like to hear about it?");
            }
            else if (speech.Contains("misery"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Misery can be a powerful motivator. There's a hidden chamber within the Fallen Keep that holds a treasure. I can tell you more, but only if you prove your loyalty. Bring me a raven's feather, and I shall reveal its secrets. For your effort, you shall be rewarded.");
                    from.AddToBackpack(new TasteIDAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LadyGsolde(Serial serial) : base(serial) { }

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
