using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Judah Goldleaf")]
    public class JudahGoldleaf : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JudahGoldleaf() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Judah Goldleaf";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Tunic() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomMetalHue() });
            AddItem(new FeatheredHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldRing() { Name = "Goldleaf's Ring" });
            AddItem(new GoldNecklace() { Name = "Goldleaf's Necklace" });

            // Random hair and facial hair
            HairItemID = Utility.RandomList(0x204A, 0x203B); // Hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2049; // Mustache

            // Speech Hue
            SpeechHue = Utility.RandomNeutralHue();

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
                Say("Ah, greetings! I am Judah Goldleaf, guardian of treasures and secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in splendid health, my friend. How can I assist you today?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to safeguard the legendary treasures hidden within this land. Many seek them, few find them.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the allure of treasure! It is a quest of great excitement and peril. Many seek it, but only the resolute find it.");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek treasure is to seek adventure. Are you prepared for both? Only those with true courage succeed.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Adventure is the heart of exploration. Only those with true courage succeed. Show me your bravery and I will guide you further.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is more than bravery; it's the willingness to face the unknown. Speak of courage, and you may earn further wisdom.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery often comes with risk. The treasure you seek will test your resolve. What are you willing to risk?");
            }
            else if (speech.Contains("risk"))
            {
                Say("Risk is inherent in all great quests. Show your willingness to face these risks, and I may share with you more of the treasure’s lore.");
            }
            else if (speech.Contains("lore"))
            {
                Say("The lore of the treasure is rich and deep. Knowledge of it can aid you greatly. Are you prepared to learn its secrets?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are not given lightly. Prove your commitment and you will gain access to the greatest secrets of the treasure.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment is shown through actions, not words. Demonstrate your resolve, and you shall be rewarded with greater knowledge.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Your resolve is being tested. Show that you are unwavering, and you may claim the treasure I guard.");
            }
            else if (speech.Contains("claim"))
            {
                Say("To claim the treasure, you must first prove your worth. Return once you have shown your mettle through deeds.");
            }
            else if (speech.Contains("mettle"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The treasure awaits. Return when you have shown your resolve.");
                }
                else
                {
                    Say("You have shown great courage and resolve. For your bravery, accept this treasure chest as a token of your success.");
                    from.AddToBackpack(new JudahsTreasureChest()); // Give the treasure chest as a reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("success"))
            {
                Say("Success is the culmination of effort and persistence. You have proven yourself worthy of the treasure.");
            }
            else if (speech.Contains("effort"))
            {
                Say("Effort is what sets apart the dreamers from the doers. Your effort in this quest has not gone unnoticed.");
            }
            else if (speech.Contains("dreamers"))
            {
                Say("Dreamers may envision the treasure, but only those who act can attain it. You have moved beyond dreams to reality.");
            }
            else if (speech.Contains("reality"))
            {
                Say("Reality often demands more than dreams. You have faced challenges and overcome them. Well done!");
            }

            base.OnSpeech(e);
        }

        public JudahGoldleaf(Serial serial) : base(serial) { }

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
