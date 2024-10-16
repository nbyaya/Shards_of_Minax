using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Finnegan O'Lucky")]
    public class FinneganOLucky : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FinneganOLucky() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Finnegan O'Lucky";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 0x4F3 }); // Emerald green
            AddItem(new Kilt() { Hue = 0x4F3 }); // Matching kilt
            AddItem(new Sandals() { Hue = 0x4F3 });
            AddItem(new FeatheredHat() { Hue = 0x4F3 });

            // Custom item to make him look more leprechaun-like
            AddItem(new GoldBracelet() { Hue = 0x4F3 });
            AddItem(new GoldRing() { Hue = 0x4F3 });
            AddItem(new Mace() { Name = "Lucky Shamrock", Hue = 0x4F3 });

            Hue = 0x203B;
            HairItemID = 0x203B; // Green hair
            HairHue = 0x48D;

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

            // Start the conversation with the name
            if (speech.Contains("name"))
            {
                Say("Ah, ye've found ol' Finnegan O'Lucky! What's the matter, looking for a bit o' luck, are ye?");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, I'm as fine as a fiddle and twice as jolly! Lucky me, eh? But tell me, what do ye think of the treasure?");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Aye, treasure! I've got a grand chest right here, but you'll need to prove yer wit to earn it. Ready for a challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Good! Tell me, what do you seek most in life?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is a treasure in itself! But a true challenge lies ahead. Do you have the patience of a saint?");
            }
            else if (speech.Contains("patience"))
            {
                Say("Ah, patience! The true mark of a wise soul. Prove it, and the chest is yours. Do you know the tale of the three leprechauns?");
            }
            else if (speech.Contains("three leprechauns"))
            {
                Say("Indeed, a classic! The first leprechaun hid his gold in plain sight, the second in a magical forest, and the third... well, the third was always the trickiest. Can you guess where the gold lies?");
            }
            else if (speech.Contains("magical forest"))
            {
                Say("Ye've got it! The magical forest holds many secrets, but not all are as they seem. Tell me, do you believe in luck?");
            }
            else if (speech.Contains("luck"))
            {
                Say("Ah, luck! It's a funny thing, isn't it? Some say it’s all chance, while others believe it's a gift. What do you believe?");
            }
            else if (speech.Contains("gift"))
            {
                Say("Aye, some call it a gift from the fates. But what about hard work? Can it change one's fortune?");
            }
            else if (speech.Contains("hard work"))
            {
                Say("Indeed, hard work can shape one's destiny. But tell me, what is the most important lesson you’ve learned?");
            }
            else if (speech.Contains("lesson"))
            {
                Say("Lessons learned are treasures unto themselves. Yet, sometimes, it's the simple things that matter most. Do you value simplicity?");
            }
            else if (speech.Contains("simplicity"))
            {
                Say("Simplicity is the essence of elegance. It’s often overlooked but holds great truth. Speaking of truth, do you seek honesty in others?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the foundation of trust. Without it, we have nothing. But let's not digress. What do you think of fate?");
            }
            else if (speech.Contains("fate"))
            {
                Say("Fate and fortune are intertwined. Some say we make our own destiny, while others believe it’s written in the stars. What do you believe?");
            }
            else if (speech.Contains("stars"))
            {
                Say("The stars are a wondrous sight, full of mystery and wonder. They guide us in times of need. But enough pondering. Have you figured out where the gold is hidden?");
            }
            else if (speech.Contains("hidden"))
            {
                Say("Aye, the gold is hidden well. But if you’re clever and true, the treasure shall be yours. Have you solved the riddle yet?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("A good riddle is worth its weight in gold. But remember, it's not always about the answer, but the journey. If you’ve done well, the chest is yours. Do you wish to claim your reward?");
            }
            else if (speech.Contains("claim"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've already given ye a reward. Come back later for another chance!");
                }
                else
                {
                    Say("Congratulations, ye’ve solved the riddle and proven yer wit! Here’s the Leprechaun's Loot Chest as promised!");
                    from.AddToBackpack(new LeprechaunsLootChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I’m not quite sure what ye mean. Maybe you can try again?");
            }

            base.OnSpeech(e);
        }

        public FinneganOLucky(Serial serial) : base(serial) { }

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
