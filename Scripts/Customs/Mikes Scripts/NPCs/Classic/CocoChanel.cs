using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Coco Chanel")]
    public class CocoChanel : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CocoChanel() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Coco Chanel";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new FancyDress() { Hue = 1109 });
            AddItem(new FancyShirt() { Hue = 1109 });
            AddItem(new Skirt() { Hue = 1109 });
            AddItem(new Sandals() { Hue = 1109 });
            AddItem(new Dagger() { Name = "Coco's Dagger" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Bonjour! I am Coco Chanel, a purveyor of style and secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Ah, my dear, health is like fashion; it fluctuates.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I am the weaver of destinies, the designer of dreams.");
            }
            else if (speech.Contains("fashion"))
            {
                Say("But fashion, my dear adventurer, is not just about clothes. It's about choices.");
            }
            else if (speech.Contains("colors"))
            {
                Say("Ah, you seek advice? Tell me, adventurer, what colors your dreams?");
            }
            else if (speech.Contains("style"))
            {
                Say("Style, my dear, is an embodiment of one's soul. In this realm, however, there's a particular style that unlocks hidden doors. Would you like to know more?");
            }
            else if (speech.Contains("fluctuates"))
            {
                Say("Just as the seasons change, so does our health. It's all a cycle, much like the moon's phases. Speaking of which, the moon has been peculiar lately, don't you think?");
            }
            else if (speech.Contains("destinies"))
            {
                Say("Every thread I weave into a garment carries a fate, a story waiting to be told. Some of these stories hold great power. Perhaps, for the right price, I might weave a tale just for you.");
            }
            else if (speech.Contains("choices"))
            {
                Say("Every choice you make shapes the world around you. But remember, in fashion and life, it's not just about what you wear, but how you wear it. Confidence, my dear, is your greatest accessory.");
            }
            else if (speech.Contains("dreams"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Dreams are fragments of our deepest desires and fears. If you can decipher the colors of your dreams, I might reward you with a token of appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("hidden"))
            {
                Say("Ah, the hidden doors. Legends speak of a doorway in this very town that leads to treasures untold. But the door only appears to those with a heart true to style.");
            }
            else if (speech.Contains("moon"))
            {
                Say("The moon indeed holds many mysteries. Its glow affects more than just the tides. Some even say it holds the power to reveal hidden truths.");
            }
            else if (speech.Contains("price"))
            {
                Say("Price is not always in gold or silver. Sometimes, it's in favors or knowledge. For now, bring me a rose, and I might share a secret.");
            }
            else if (speech.Contains("confidence"))
            {
                Say("A confident heart can face any challenge. In fact, there's a challenge in this town that only the confident can conquer. Are you up for it?");
            }
            else if (speech.Contains("token"))
            {
                Say("You seem worthy. Here's a token of my appreciation. May it guide your path and choices in fashion and life.");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
            }

            base.OnSpeech(e);
        }

        public CocoChanel(Serial serial) : base(serial) { }

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
