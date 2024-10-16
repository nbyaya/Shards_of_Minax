using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lorenzo the Lorekeeper")]
    public class LorenzoTheLorekeeper : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LorenzoTheLorekeeper() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lorenzo the Lorekeeper";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new LongPants() { Hue = 1109 });
            AddItem(new FancyShirt() { Hue = 1109 });
            AddItem(new Cloak() { Hue = 1109 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Lute() { Name = "Lorenzo's Lute" });

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
                Say("Greetings, traveler. I am Lorenzo the Lorekeeper, a bard.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job as a lorekeeper is to collect and share stories and knowledge. I believe in the power of storytelling.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor lies not in the strength of one's arm, but in the strength of one's character. Do you possess valor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then you understand that courage is not the absence of fear, but the ability to act despite it.");
            }
            else if (speech.Contains("lorenzo"))
            {
                Say("Ah, you have heard of me. I've traveled far and wide, gathering tales from every corner of the world. My favorite story is of the Lost City. Have you heard of it?");
            }
            else if (speech.Contains("thank"))
            {
                Say("Your kindness warms my heart. Many in this land are too busy with their own pursuits to extend a simple courtesy. By the way, have you come across the Enchanted Lyre in your travels?");
            }
            else if (speech.Contains("storytelling"))
            {
                Say("Stories have the power to inspire, to warn, and to teach. One of my cherished tales is of the Whispering Forest. It's said to hold secrets only a few have discovered. Are you interested?");
            }
            else if (speech.Contains("city"))
            {
                Say("The Lost City is a legend. It's said that deep within its walls is a treasure beyond imagination. But many who sought it never returned. If you ever decide to search for it, remember: the journey itself is the real treasure.");
            }
            else if (speech.Contains("lyre"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The Enchanted Lyre is said to have the power to soothe even the fiercest of beasts. I've been searching for it to add its story to my collection. If you ever find it, bring it to me, and I'll reward you generously.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with the actual item you want to reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("forest"))
            {
                Say("The Whispering Forest is named so because the trees themselves murmur ancient tales. Those who listen closely might learn forgotten wisdom. Tread carefully though; not all whispers are friendly.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Many have been lured by the promise of gold and jewels, but the true treasure lies in the friendships forged and lessons learned along the way. Seek wisdom, not just wealth.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Ah, you're interested in rewards. Well, for those who aid me in my quest for knowledge and stories, I offer something invaluable. A piece of lore unknown to many. Will you assist me?");
            }
            else if (speech.Contains("murmur"))
            {
                Say("It is said that the murmurs of the forest are voices of the ancients, sharing their wisdom. However, it takes a keen ear and a pure heart to truly understand them. Are you up for the challenge?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that guides us through the darkest nights. It's more precious than any jewel. I've dedicated my life to seeking it. Will you join me on this quest? Take this to light your path.");
                from.AddToBackpack(new MaxxiaScroll()); // Replace with the actual item you want to reward
            }

            base.OnSpeech(e);
        }

        public LorenzoTheLorekeeper(Serial serial) : base(serial) { }

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
