using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Celes Chere")]
    public class CelesChere : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CelesChere() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Celes Chere";
            Body = 0x191; // Human female body

            // Stats
            Str = 95;
            Dex = 85;
            Int = 110;
            Hits = 80;

            // Appearance
            AddItem(new ChainLegs() { Hue = 1910 });
            AddItem(new ChainChest() { Hue = 1910 });
            AddItem(new ChainCoif() { Hue = 1910 });
            AddItem(new Longsword() { Name = "Celes's Rapier" });

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
                Say("Greetings, traveler. I am Celes Chere.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a former general of the Empire, skilled in the ways of combat.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("I believe in the virtues of Honesty, Compassion, and Justice. They guide my actions.");
            }
            else if (speech.Contains("believe"))
            {
                Say("Do you believe in the virtues, traveler?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Very well. The path of virtue is not always clear, but it is worth striving for.");
            }
            else if (speech.Contains("celes"))
            {
                Say("Ah, you know of me? Perhaps from tales of old. I have ventured through many lands and faced countless challenges.");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, despite the wounds of battle and the trials of time, I have been fortunate. Though, there was a time when a special potion saved me from a dire fate.");
            }
            else if (speech.Contains("empire"))
            {
                Say("The Empire was once a mighty force, ruling vast territories and commanding respect. But it also had its shadows and secrets. I have seen both its glory and its fall.");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the foundation of trust. Without it, societies crumble and friendships fade. It's more than just telling the truth; it's living a life of transparency and integrity.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the light that shines in the darkest of times. It is the act of understanding and caring for others, even when it's challenging or inconvenient.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the balance that ensures fairness. It's not just about punishment, but about ensuring that the scales are even for all. In my travels, I've seen justice both served and denied.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Indeed, from battling fierce monsters to navigating political intrigues, I've seen it all. But one particular challenge stands out. Would you like to hear about it?");
            }
            else if (speech.Contains("potion"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, that potion was a rare elixir given to me by a skilled alchemist when I was gravely wounded. In gratitude, I can share a vial with you. Consider it a gift.");
                    from.AddToBackpack(new ResistingSpellsAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows of the Empire were deep and many. Espionage, betrayal, and dark magic. It was a time of uncertainty, where trust was a rare commodity.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is staying true to one's values, even when faced with temptations or threats. It's a virtue that I hold dear and strive to uphold in all situations.");
            }
            else if (speech.Contains("caring"))
            {
                Say("True caring is selfless, expecting nothing in return. It's a strength that can move mountains and bring warmth to the coldest hearts.");
            }
            else if (speech.Contains("scales"))
            {
                Say("Just like the scales of justice, life requires balance. Too much of one thing can tip the balance and lead to chaos. It's a lesson I've learned through hard experience.");
            }
            else if (speech.Contains("monsters"))
            {
                Say("Monsters are not always creatures of fang and claw. Sometimes, they wear the faces of men. I've fought both kinds in my time.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Dark magic is a perilous path. It promises power but often at a terrible price. I've seen many consumed by its allure, losing their way and their souls.");
            }

            base.OnSpeech(e);
        }

        public CelesChere(Serial serial) : base(serial) { }

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
