using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lily the Lost")]
    public class LilyTheLost : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilyTheLost() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lily the Lost";
            Body = 0x191; // Human female body

            // Stats
            Str = 35;
            Dex = 35;
            Int = 35;
            Hits = 35;

            // Appearance
            AddItem(new PlainDress() { Hue = 1109 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Bonnet() { Hue = 1109 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Lily the Lost, a beggar.");
            }
            else if (speech.Contains("health"))
            {
                Say("Life's a constant struggle, but I manage.");
            }
            else if (speech.Contains("job"))
            {
                Say("Begging for coin and food is my job, kind stranger.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Compassion, a virtue I cling to in my hardship, is about understanding the suffering of others. Do you know this virtue?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Indeed, it's about showing kindness and helping those in need, even when it's tough for you. Will you help me with some coin today?");
            }
            else if (speech.Contains("lily"))
            {
                Say("Lily was once a name of happiness. I was a florist in the city before I fell upon hard times.");
            }
            else if (speech.Contains("struggle"))
            {
                Say("It's not just hunger, but also the cold nights and occasional cruelty of passersby that I face. Yet, some still show kindness.");
            }
            else if (speech.Contains("coin"))
            {
                Say("Most days, I'm able to gather enough to eat a small meal. On good days, I might find a trinket or two, which I sometimes trade.");
            }
            else if (speech.Contains("suffering"))
            {
                Say("Suffering isn't just physical. The loneliness can be even more painful. But conversations like this, they give me hope.");
            }
            else if (speech.Contains("kindness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your kindness reminds me of a pendant I once owned. A symbol of hope. I'd like to give you something in return for your compassion.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("florist"))
            {
                Say("Yes, I once had a shop filled with roses, lilies, and tulips. The scent was heavenly. Now, I can only dream of those days.");
            }
            else if (speech.Contains("cold"))
            {
                Say("The nights are the harshest. Sometimes, kind souls offer me blankets or firewood. It's what keeps me going.");
            }
            else if (speech.Contains("trinket"))
            {
                Say("These trinkets often carry stories. Once, I found a locket with a faded picture inside. I wonder about its story.");
            }
            else if (speech.Contains("loneliness"))
            {
                Say("Loneliness is a silent battle. Sometimes a simple hello or a shared meal can make a world of difference.");
            }
            else if (speech.Contains("pendant"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Here, take this small token. It's not much, but it has been with me for some time. A reminder that even in darkness, there's light.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LilyTheLost(Serial serial) : base(serial) { }

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
