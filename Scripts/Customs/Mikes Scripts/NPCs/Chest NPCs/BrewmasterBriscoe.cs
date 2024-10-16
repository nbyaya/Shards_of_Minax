using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class BrewmasterBriscoe : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BrewmasterBriscoe() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Brewmaster Briscoe";
            Body = 0x190; // Male body
            Title = "the Brewmaster";
            Hue = Utility.RandomSkinHue();

            // Equip Briscoe
            AddItem(new FancyShirt(Utility.RandomRedHue()));
            AddItem(new Kilt(Utility.RandomGreenHue()));
            AddItem(new Sandals(Utility.RandomYellowHue()));
            AddItem(new TricorneHat(Utility.RandomMetalHue()));
            AddItem(new LeatherGloves() { Hue = Utility.RandomMetalHue() });

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
                Say("Ah, welcome! I am Brewmaster Briscoe, the finest brewer in these parts.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as healthy as a barrel of fresh ale! Thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Oh, I brew the finest ales and spirits this side of the realm.");
            }
            else if (speech.Contains("brew"))
            {
                Say("Brewing is an art and a science. It takes patience and skill. My brews are legendary.");
            }
            else if (speech.Contains("ale"))
            {
                Say("My ale is brewed with the finest ingredients. I’m always perfecting my recipes.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("Ah, spirits! They can warm the coldest of hearts and bring joy to any gathering.");
            }
            else if (speech.Contains("recipe"))
            {
                Say("Recipes are a brewer’s secret! But I’ll share this: always use fresh ingredients.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Come back later.");
                }
                else
                {
                    Say("You've shown great interest in the art of brewing. For your enthusiasm, please accept this Brewmaster's Chest.");
                    from.AddToBackpack(new BrewmastersChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public BrewmasterBriscoe(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
