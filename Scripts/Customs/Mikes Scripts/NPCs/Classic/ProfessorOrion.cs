using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Orion")]
    public class ProfessorOrion : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorOrion() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Orion";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 55;
            Int = 110;
            Hits = 65;

            // Appearance
            AddItem(new LongPants() { Hue = 1103 });
            AddItem(new Doublet() { Hue = 1109 });
            AddItem(new Cloak() { Hue = 1105 });
            AddItem(new Sandals() { Hue = 0 });

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
                Say("I am Professor Orion, a brilliant scientist, or so they say...");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Pah! What does it matter in the grand scheme of things?");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' is to toil away in obscurity, uncovering the mysteries of the universe for the ungrateful masses.");
            }
            else if (speech.Contains("genius"))
            {
                Say("Do you have the intellect to appreciate the work of a true genius like me?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Well, at least you're not a complete imbecile. Perhaps there's hope for you yet.");
            }
            else if (speech.Contains("brilliant"))
            {
                Say("Most think being 'brilliant' is a gift, but it often comes with its own set of challenges. Many don't understand the thoughts that plague my mind.");
            }
            else if (speech.Contains("scheme"))
            {
                Say("Everything in life has its own scheme, a pattern, a rhythm. The cosmos, the elements, even our very existence. I've dedicated my life to understanding these.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The mysteries of the universe are vast and endless. Just when you think you've understood one, a thousand more appear.");
            }
            else if (speech.Contains("work"))
            {
                Say("My work is not just experiments and equations. It's about seeking the truth in a world full of deception.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Every challenge I've faced has only honed my skills further. But sometimes, I wonder if there's more to life than just relentless pursuit of knowledge.");
            }
            else if (speech.Contains("cosmos"))
            {
                Say("Ah, the cosmos! The final frontier. I've been working on a device that might allow us to gaze deeper into its expanse. If you help me gather some rare materials, there might be a reward for you.");
            }
            else if (speech.Contains("endless"))
            {
                Say("The vastness of our universe is both endless and overwhelming. I sometimes find solace in the little wonders, like the twinkling of a distant star.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is a double-edged sword. Once you know it, there's no going back. But isn't the quest for truth what drives all great minds?");
            }
            else if (speech.Contains("pursuit"))
            {
                Say("The pursuit of knowledge is noble, but it's equally important to share that knowledge. That's why I often hold lectures, though not many attend.");
            }
            else if (speech.Contains("device"))
            {
                Say("My device is revolutionary, but it's missing a few key components. If you manage to find them for me, I'll be most grateful and ensure you're suitably rewarded.");
            }
            else if (speech.Contains("star"))
            {
                Say("Stars are not just balls of gas. They're history keepers, guides, and sometimes, predictors of fate.");
            }
            else if (speech.Contains("quest"))
            {
                Say("Many have embarked on quests, seeking fame or fortune. But my quest? It's to leave a lasting legacy in the annals of science.");
            }
            else if (speech.Contains("lectures"))
            {
                Say("My lectures might be considered dull by some, but for those truly interested, they can open a world of possibilities. Care to attend one?");
            }
            else if (speech.Contains("components"))
            {
                Say("These components are rare, but I've heard tales of a merchant in the East who might have them. If you fetch them for me, your efforts won't go unrewarded.");
            }
            else if (speech.Contains("fate"))
            {
                Say("Fate is an intriguing concept. Is our path preordained, or do we have the power to change it? Ah, such are the musings of an old scientist.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("Legacy isn't about fame. It's about the impact you leave behind. I hope my discoveries will change the world for the better.");
            }
            else if (speech.Contains("attend"))
            {
                Say("Ah! A keen mind! Attend one of my lectures and you might just gain insights beyond your wildest dreams. And perhaps, a little token of my appreciation.");
            }
            else if (speech.Contains("merchant"))
            {
                Say("The merchant is elusive, but if you find him, mention my name. He owes me a favor from a long time ago.");
            }
            else if (speech.Contains("preordained"))
            {
                Say("The concept of a preordained path is comforting to some, but I believe in forging one's own destiny with every choice made.");
            }
            else if (speech.Contains("discoveries"))
            {
                Say("My discoveries range from the minuscule to the monumental. But every piece of knowledge, big or small, has the potential to change the world.");
            }
            else if (speech.Contains("insights"))
            {
                Say("True insights don't just come from books or lectures. They come from experience, observation, and sometimes, a bit of luck.");
            }
            else if (speech.Contains("favor"))
            {
                Say("Favors are a currency in their own right. The key is to know when to ask for one and when to grant one.");
            }
            else if (speech.Contains("choice"))
            {
                Say("Every choice we make has a consequence, whether immediate or delayed. Choose wisely.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is power, but it's also a responsibility. With great knowledge comes great accountability.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience is the best teacher. But remember, not all experiences are pleasant, yet they all offer lessons.");
            }
            else if (speech.Contains("currency"))
            {
                Say("Currency can buy many things, but there are things that are priceless - like a eureka moment in an experiment.");
            }
            else if (speech.Contains("consequence"))
            {
                Say("Some fear the consequences of their actions, while others embrace them as learning opportunities.");
            }
            else if (speech.Contains("accountability"))
            {
                Say("Being accountable means owning up to your mistakes and learning from them. It's a trait I value highly.");
            }
            else if (speech.Contains("lessons"))
            {
                Say("The lessons life teaches us are invaluable. It's up to us to interpret and apply them in our journey.");
            }
            else if (speech.Contains("eureka"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the eureka moment! It's a feeling of unparalleled joy when everything suddenly makes sense. Take this!");
                    from.AddToBackpack(new FishingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ProfessorOrion(Serial serial) : base(serial) { }

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
