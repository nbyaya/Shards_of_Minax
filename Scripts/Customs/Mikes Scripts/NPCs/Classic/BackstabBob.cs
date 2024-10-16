using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Backstab Bob")]
    public class BackstabBob : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BackstabBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Backstab Bob";
            Body = 0x190; // Human male body

            // Stats
            Str = 115;
            Dex = 100;
            Int = 60;
            Hits = 85;

            // Appearance
            AddItem(new ShortPants() { Hue = 1153 });
            AddItem(new BoneArms() { Hue = 1153 });
            AddItem(new Dagger { Name = "Bob's Dagger" });

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
                Say("Greetings, stranger. I am Backstab Bob, the rogue.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm always in top shape, my friend.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Well, let's just say I specialize in acquiring hard-to-get items.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("True rogues like us must always consider the balance of virtue and vice. Have you thought about that?");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Are you more inclined toward honesty or deceit in your life?");
            }
            else if (speech.Contains("items"))
            {
                Say("Oh, the items I've acquired over the years have stories of their own. Some treasures, some trinkets, and some... well, let's just say they're best left unspoken of. Would you like to hear about one of my most prized possessions?");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is the essence of our existence. In our line of work, leaning too far to virtue makes one vulnerable, while excess vice can be... costly. It's a dance on the razor's edge, and mastering it is an art.");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty has its merits. I respect those who can walk their path with a clear conscience. But in our world, sometimes a little dishonesty can open doors honesty can't.");
            }
            else if (speech.Contains("deceit"))
            {
                Say("Deceit is a tool, much like a dagger or a lockpick. It's not inherently evil; it's all about how and when you use it. And sometimes, it can save your life or get you that which is unattainable by other means.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, you're interested in rewards, are you? Tell you what, since you've shown genuine interest in my tales, I'll gift you something from my collection. It's not much, but it may come in handy for someone like you.");
                    from.AddToBackpack(new FireHitAreaCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BackstabBob(Serial serial) : base(serial) { }

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
