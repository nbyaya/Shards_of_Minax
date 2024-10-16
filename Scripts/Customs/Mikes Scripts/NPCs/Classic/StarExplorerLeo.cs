using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Star Explorer Leo")]
    public class StarExplorerLeo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public StarExplorerLeo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Star Explorer Leo";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 100;
            Int = 90;
            Hits = 80;

            // Appearance
            AddItem(new ChainLegs() { Hue = 2211 });
            AddItem(new ChainChest() { Hue = 2211 });
            AddItem(new PlateHelm() { Hue = 2211 });
            AddItem(new Boots() { Hue = 2211 });
            AddItem(new FireballWand() { Name = "Leo's Laser Gun" });

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
                Say("Oh, great, another curious mortal...");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Do I look like a healer to you?");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\"? I'm here, talking to you, aren't I?");
            }
            else if (speech.Contains("purpose"))
            {
                Say("True wisdom is not in asking pointless questions, but in making meaningful choices. What's your purpose here?");
            }
            else if (speech.Contains("games"))
            {
                Say("Oh, you think you're clever, huh? Well, good luck with that. Now go on, play your little hero games.");
            }
            else if (speech.Contains("curious"))
            {
                Say("Yes, curiosity has led many mortals astray. But if you're wise enough, it can also guide you to the stars.");
            }
            else if (speech.Contains("healer"))
            {
                Say("No, I am not a healer in the traditional sense. But I've seen things and know tales that might heal one's soul.");
            }
            else if (speech.Contains("talking"))
            {
                Say("Talking, yes, but that's not my true purpose. I've traveled galaxies and seen the secrets of the universe. My real job is to observe.");
            }
            else if (speech.Contains("choices"))
            {
                Say("Every choice leads to a new path. Some paths are bright, others dim. Choose wisely, and you may find something valuable.");
            }
            else if (speech.Contains("hero"))
            {
                Say("Hero or not, everyone has a story. If you show kindness and prove yourself, I might reward you for your deeds.");
            }
            else if (speech.Contains("stars"))
            {
                Say("The stars are not just dots in the sky. They're ancient beings with stories of their own. Some even hold the power of fate.");
            }
            else if (speech.Contains("soul"))
            {
                Say("Souls are like constellations, intricate and connected. Some tales have the power to align them correctly.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Every secret has its time and place. The universe is vast, and its mysteries are countless. Only the truly dedicated can uncover them.");
            }
            else if (speech.Contains("path"))
            {
                Say("Indeed, every path has its own treasure. You've shown interest in mine, so take this as a reward for your curiosity.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    // Reward logic
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("deeds"))
            {
                Say("A hero is defined not by their title, but by their actions. Do something truly noble, and I'll ensure you're appropriately rewarded.");
                // Reward logic
                from.AddToBackpack(new MaxxiaScroll()); // Example reward
            }

            base.OnSpeech(e);
        }

        public StarExplorerLeo(Serial serial) : base(serial) { }

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
