using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dexter Goldleaf")]
    public class DexterGoldleaf : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DexterGoldleaf() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dexter Goldleaf";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new LeatherCap() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Bandana() { Hue = Utility.RandomMetalHue() });

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Short hair
            HairHue = Utility.RandomHairHue();

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
                Say("Ah, you’ve discovered Dexter Goldleaf, at your service. But tell me, do you know what my job is?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I specialize in acquiring the rarest treasures and keeping them well-hidden. Speaking of treasures, have you heard about the legendary Smuggler's Cache?");
            }
            else if (speech.Contains("cache"))
            {
                Say("The Smuggler's Cache is a trove of hidden wonders. But to unlock its secrets, one must prove their worth. Are you familiar with the art of deception?");
            }
            else if (speech.Contains("deception"))
            {
                Say("Ah, deception is a skill many need. But to truly understand it, one must first grasp the concept of treasure. What do you know about rare finds?");
            }
            else if (speech.Contains("rare"))
            {
                Say("Rare finds are what make the hunt exciting. They hold secrets and stories. But speaking of stories, have you ever heard the tale of the hidden cache?");
            }
            else if (speech.Contains("tale"))
            {
                Say("Indeed, tales of hidden caches are the stuff of legends. But remember, every legend has its key. Do you know what a key symbolizes?");
            }
            else if (speech.Contains("key"))
            {
                Say("A key is not just a tool; it's a symbol of access and opportunity. To gain access to the Smuggler's Cache, you must show patience. Do you have the patience of a true adventurer?");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is crucial in our line of work. But patience alone won't get you far. You must also have a keen mind. How sharp is your mind when it comes to solving puzzles?");
            }
            else if (speech.Contains("puzzle"))
            {
                Say("Puzzles challenge our intellect. Speaking of challenges, I have a riddle for you. Solve it, and you may be closer to the Smuggler's Cache. Are you ready for a riddle?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Here is the riddle: I can be cracked, made, told, and played. What am I?");
            }
            else if (speech.Contains("joke"))
            {
                Say("Correct! A joke is the answer. You have a sharp mind indeed. But sharpness alone is not enough; you must also be clever. Are you clever enough to claim the Smuggler's Cache?");
            }
            else if (speech.Contains("clever"))
            {
                Say("Cleverness is a trait many admire. However, it’s also about understanding when to act. Are you prepared to act on what you've learned today?");
            }
            else if (speech.Contains("act"))
            {
                Say("Excellent! Your preparation shows. Now, to claim the Smuggler's Cache, you need to demonstrate your understanding. Are you ready to receive your reward?");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Ah, patience, my friend. Come back later for a chance at the prize.");
                }
                else
                {
                    Say("Congratulations on solving the puzzle! For your cleverness and patience, accept this Smuggler’s Cache.");
                    from.AddToBackpack(new SmugglersCash()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("congratulations"))
            {
                Say("Thank you! May the treasures of the Smuggler’s Cache serve you well.");
            }

            base.OnSpeech(e);
        }

        public DexterGoldleaf(Serial serial) : base(serial) { }

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
