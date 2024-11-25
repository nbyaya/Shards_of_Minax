using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rex Malcontent")]
    public class RexMalcontent : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RexMalcontent() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rex Malcontent";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1159 });
            AddItem(new StuddedChest() { Hue = 1159 });
            AddItem(new Bandana() { Hue = 1159 });
            AddItem(new Boots() { Hue = 1159 });
            AddItem(new LeatherGloves() { Hue = 1159 });
            AddItem(new Longsword() { Name = "Street Warrior's Blade", Hue = 1159 }); // Example weapon to match the theme

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 1159; // Set to match the theme
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ah, you’ve found me! I am Rex Malcontent, a figure of dissent and rebellion.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I lead the underground movement, working to disrupt the status quo and fight for freedom.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, though the battles we fight can be harsh and unforgiving.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The revolution is more than just a battle; it’s a struggle for a new way of life.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is the prize we seek, the right to live without chains and oppression.");
            }
            else if (speech.Contains("cache"))
            {
                Say("You seek the cache? Prove your resolve and the underground’s treasure will be yours.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Your resolve is commendable. Tell me, do you understand the true nature of our cause?");
            }
            else if (speech.Contains("nature"))
            {
                Say("The nature of our cause is complex. It’s about challenging the existing order and pushing for a better future.");
            }
            else if (speech.Contains("order"))
            {
                Say("Yes, the existing order is what we fight against. But tell me, do you believe in change?");
            }
            else if (speech.Contains("change"))
            {
                Say("Change is what drives us. It’s the catalyst for the revolution. Have you seen any signs of change?");
            }
            else if (speech.Contains("signs"))
            {
                Say("Signs of change are everywhere if you know where to look. Sometimes they are subtle, sometimes blatant. What do you make of these signs?");
            }
            else if (speech.Contains("make"))
            {
                Say("What you make of these signs can determine your future. Do you think you are ready to act upon them?");
            }
            else if (speech.Contains("ready"))
            {
                Say("Being ready means understanding the cost and being willing to pay it. Are you prepared to take on this challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("The challenge is not for the faint-hearted. It requires courage and determination. Have you proven your worth?");
            }
            else if (speech.Contains("worth"))
            {
                Say("Proving your worth is about more than just words; it’s about actions. Do you have what it takes?");
            }
            else if (speech.Contains("takes"))
            {
                Say("It takes dedication and bravery. If you’ve proven yourself, then you’re one step closer to the reward.");
            }
            else if (speech.Contains("dedication"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your dedication and bravery, accept this Underground Anarchist's Cache as a token of our gratitude.");
                    from.AddToBackpack(new UndergroundAnarchistsCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                // General response if no recognized keywords
                Say("I'm not sure what you mean. Please ask about my name, job, health, or the revolution.");
            }

            base.OnSpeech(e);
        }

        public RexMalcontent(Serial serial) : base(serial) { }

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
