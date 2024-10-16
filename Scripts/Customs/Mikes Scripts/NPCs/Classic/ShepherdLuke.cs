using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherd Luke")]
    public class ShepherdLuke : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdLuke() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherd Luke";
            Body = 0x190; // Human male body

            // Stats
            Str = 82;
            Dex = 62;
            Int = 48;
            Hits = 73;

            // Appearance
            AddItem(new ShortPants() { Hue = 1132 });
            AddItem(new Shirt() { Hue = 1131 });
            AddItem(new Sandals() { Hue = 0 });
            AddItem(new ShepherdsCrook() { Name = "Shepherd Luke's Crook" });

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
                Say("I'm Shepherd Luke, the miserable shepherd.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Do I look like a healer? I'm as healthy as a shepherd can be.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Oh, it's just to tend to these wretched sheep day in and day out.");
            }
            else if (speech.Contains("bitter"))
            {
                Say("Think you're any better off? Hah! What's your lot in life, adventurer?");
            }
            else if (speech.Contains("sarcastic"))
            {
                Say("Ah, so you've got a bit of snark in you. I like that. What's your take on the world, then?");
            }
            else if (speech.Contains("miserable"))
            {
                Say("Miserable, you ask? It's a long tale of lost love and betrayal. Do you really wish to know?");
            }
            else if (speech.Contains("healer"))
            {
                Say("Healers? Pah! All they've given me are remedies for my body. None can heal the scars of my soul.");
            }
            else if (speech.Contains("sheep"))
            {
                Say("These sheep might be wretched, but they're all I have left after the incident. Have you ever lost something dear to you?");
            }
            else if (speech.Contains("betrayal"))
            {
                Say("It was my brother, Jacob. He stole my love, my inheritance, and left me with these sheep. Ever met a man named Jacob?");
            }
            else if (speech.Contains("scars"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The deepest wounds are the ones unseen. My heart has known its share of sorrow. If you bring me a healing potion, I might part with something for your trouble.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("incident"))
            {
                Say("Years ago, a wolf pack attacked. Lost half my herd and almost lost my life. But a mysterious traveler saved me. Do you believe in fate?");
            }
            else if (speech.Contains("fate"))
            {
                Say("Some days I think everything is predestined, other days I think we make our own path. What's your stance on destiny?");
            }
            else if (speech.Contains("jacob"))
            {
                Say("If you ever come across Jacob, tell him I haven't forgotten. And if he ever sets foot on this land, the sheep won't be the only ones bleating.");
            }

            base.OnSpeech(e);
        }

        public ShepherdLuke(Serial serial) : base(serial) { }

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
