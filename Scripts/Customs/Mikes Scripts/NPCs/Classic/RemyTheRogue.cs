using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Remy the Rogue")]
    public class RemyTheRogue : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RemyTheRogue() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Remy the Rogue";
            Body = 0x190; // Human male body

            // Stats
            Str = 60;
            Dex = 60;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new LeatherArms() { Name = "Remy's Armguards" });
            AddItem(new LeatherLegs() { Name = "Remy's Leggings" });
            AddItem(new LeatherChest() { Name = "Remy's Vest" });
            AddItem(new LeatherGorget() { Name = "Remy's Collar" });
            AddItem(new LeatherCap() { Name = "Remy's Cap" });
            AddItem(new Boots() { Hue = 1904 });
            AddItem(new Dagger() { Name = "Remy's Dagger" });

            // Random hair and facial hair settings
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Greetings, traveler. They call me Remy the Rogue.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as healthy as the shadows, friend.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession? I dance in the moonlight's embrace, unseen by most. A rogue's life, you might say.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Ah, the virtues, the very essence of Britannia. What virtue do you seek guidance in?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Ah, honesty, the foundation of trust. In shadows, one must be honest with oneself. How else can you navigate the labyrinth of deceit?");
            }
            else if (speech.Contains("remy"))
            {
                Say("Yes, it's a name passed down in my family. It means 'oarsman' in the old tongue. Do you know about my ancestry?");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are my allies, they cloak and protect me. But they also hold many secrets. Would you like to learn some?");
            }
            else if (speech.Contains("moonlight"))
            {
                Say("Moonlight has a special significance for rogues like me. It's not just light, but a symbol of hope in the darkest times. Ever heard of the Moonblade?");
            }
            else if (speech.Contains("ancestry"))
            {
                Say("My ancestors were renowned sailors, navigating treacherous waters with ease. They discovered many islands during their time.");
            }
            else if (speech.Contains("secrets"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Not all secrets should be shared openly. But for you, I'll make an exception. Here's a small token for your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with the correct item class
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("moonblade"))
            {
                Say("The Moonblade is a legendary dagger said to be forged from moonlight itself. Many believe it's just a myth, but I've seen things that make me believe otherwise.");
            }
            else if (speech.Contains("myth"))
            {
                Say("Myths and legends are often grounded in reality, twisted by time and retelling. I've journeyed far and wide, and some tales hold more truth than you might think.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is subjective, shaped by our experiences and beliefs. But one universal truth is the value of friendship. It's more precious than any treasure.");
            }
            else if (speech.Contains("friendship"))
            {
                Say("In this world filled with danger, having allies and friends is essential. Remember, sometimes it's not what you know, but who you know.");
            }

            base.OnSpeech(e);
        }

        public RemyTheRogue(Serial serial) : base(serial) { }

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
