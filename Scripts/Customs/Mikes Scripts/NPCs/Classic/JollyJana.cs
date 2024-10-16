using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jolly Jana")]
    public class JollyJana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JollyJana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jolly Jana";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 70;
            Int = 120;
            Hits = 70;

            // Appearance
            AddItem(new FancyDress() { Hue = 2214 }); // FancyDress with hue 2214
            AddItem(new Boots() { Hue = 2214 }); // Boots with hue 2214
            AddItem(new GoldNecklace() { Name = "Jana's Jolly Necklace" });
            AddItem(new ShortSpear() { Name = "Jana's Jolly Spear" });

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
                Say("Greetings, traveler! I am Jolly Jana, the keeper of laughter!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as hearty as a laugh, my friend!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? Why, I spread joy and merriment wherever I go!");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Laughter is a virtue, my dear friend! Can you find humor in life's trials?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then you possess the heart of a true jester! Tell me, what's the funniest thing that's ever happened to you?");
            }
            else if (speech.Contains("goodbye"))
            {
                Say("Oh, what a splendid tale! Laughter truly is the best medicine, my friend. Farewell, and may your days be filled with mirth!");
            }
            else if (speech.Contains("keeper"))
            {
                Say("Ah, yes! As the keeper of laughter, I've traveled many lands and shared countless jokes. Laughter unites us all!");
            }
            else if (speech.Contains("laugh"))
            {
                Say("Every laugh gives me strength and vitality! Heard a good joke lately?");
            }
            else if (speech.Contains("merriment"))
            {
                Say("Merriment is the light in the darkest of times. It's my mission to ensure that no one forgets the joy of a good chuckle.");
            }
            else if (speech.Contains("travel"))
            {
                Say("I've been to bustling cities and quiet hamlets. But no matter where I go, I find that laughter is a universal language.");
            }
            else if (speech.Contains("joke"))
            {
                Say("Ah! I have a good one for you: Why did the scarecrow win an award? Because he was outstanding in his field! Haha!");
            }
            else if (speech.Contains("scarecrow"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The scarecrow is a simple fellow, but even he knows the importance of a good laugh! By the way, for appreciating my joke, here's a little token of gratitude.");
                    from.AddToBackpack(new SnoopingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("guild"))
            {
                Say("Our guild, 'The Laughing Order', believes in the healing power of humor. We gather jokes and tales to share with the world.");
            }
            else if (speech.Contains("villages"))
            {
                Say("Villages, although small, have close-knit communities. It's heartwarming to see an entire village gather around a bonfire, sharing tales and laughing together.");
            }

            base.OnSpeech(e);
        }

        public JollyJana(Serial serial) : base(serial) { }

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
