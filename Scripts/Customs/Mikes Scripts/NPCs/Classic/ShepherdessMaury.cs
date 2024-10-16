using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherdess Mary")]
    public class ShepherdessMaury : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdessMaury() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherdess Maury";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 65;
            Int = 55;
            Hits = 65;

            // Appearance
            AddItem(new Skirt() { Hue = 1115 }); // Skirt with hue 1115
            AddItem(new Shirt() { Hue = 1114 }); // Shirt with hue 1114
            AddItem(new Sandals() { Hue = 0 });  // Sandals with hue 0
            AddItem(new ShepherdsCrook() { Name = "Shepherdess Mary's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Shepherdess Mary, tending to these wretched sheep day in and day out.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Who cares about the health of a lowly shepherdess like me?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? To herd these miserable creatures, that's what! What else could it be?");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you think it takes valor to tend to these wretched beasts all day long?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, you think you're clever, huh? Tell me, what would you do in my miserable shoes?");
            }
            else if (speech.Contains("plan"))
            {
                Say("Well, aren't you a pompous one? Go on then, tell me, what's your grand plan for a shepherdess like me?");
            }
            else if (speech.Contains("wretched"))
            {
                Say("You seem interested in my use of the word 'wretched.' These sheep may look simple and calm, but they can be quite a handful, always finding ways to stray off or get into trouble.");
            }
            else if (speech.Contains("lowly"))
            {
                Say("By 'lowly,' I mean that in this town, being a shepherdess isn't considered a noble or important job. I've always felt overshadowed by others.");
            }
            else if (speech.Contains("miserable"))
            {
                Say("Ah, 'miserable' isn't just about the sheep. It's about the loneliness. Out here in the fields, it's just me and the endless bleating.");
            }
            else if (speech.Contains("trouble"))
            {
                Say("Just the other day, one of them got stuck in a thicket. I had to cut away the branches and free it. Not an easy task, I tell you. But seeing it run back to its mates made it worthwhile.");
            }
            else if (speech.Contains("overshadowed"))
            {
                Say("Many in town have grand roles - blacksmiths, merchants, warriors. Me? I'm just here with my sheep. But a kind traveler once told me that even the humblest roles have value. That gave me a bit of hope.");
            }
            else if (speech.Contains("loneliness"))
            {
                Say("I do find solace in the night sky, though. The stars seem to be my only companions during those long nights. Have you ever gazed upon them and felt their magic?");
            }
            else if (speech.Contains("thicket"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("That thicket? It's cursed, I believe. Strange things happen around it. Once, I found a mysterious amulet nearby. I have no need for it. Perhaps you might find it useful?");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("value"))
            {
                Say("Every role, big or small, has its own importance in the grand scheme of things. Perhaps, if you ever find yourself doubting, remember that even a small light can illuminate the darkest of places.");
            }
            else if (speech.Contains("stars"))
            {
                Say("Gazing at the stars, I often wonder if there's a bigger purpose to all of this. They're so distant, yet so close. Here, take this telescope I've been using. Maybe you'll find your own answers.");
                from.AddToBackpack(new Telescope()); // Give the reward
            }
            else if (speech.Contains("amulet"))
            {
                Say("I never dared to wear it, but it emanates a strange energy. Be cautious with it. Sometimes, things aren't just what they seem.");
            }

            base.OnSpeech(e);
        }

        public ShepherdessMaury(Serial serial) : base(serial) { }

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
