using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Whispering Willow")]
    public class WhisperingWillow : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public WhisperingWillow() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Whispering Willow";
            Body = 0x191; // Human female body

            // Stats
            Str = 92;
            Dex = 70;
            Int = 80;
            Hits = 92;

            // Appearance
            AddItem(new Robe() { Hue = 2968 }); // Robe with hue 2968
            AddItem(new Sandals() { Hue = 1172 }); // Sandals with hue 1172
            AddItem(new ShepherdsCrook() { Name = "Willow's Whispering Wand" });

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
                Say("I am Whispering Willow, the animal tamer.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is excellent, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I have a unique job; I tame and train animals.");
            }
            else if (speech.Contains("animals"))
            {
                Say("Compassion towards animals is a virtue, don't you think?");
            }
            else if (speech.Contains("favorite"))
            {
                if (speech.Contains("animal"))
                {
                    Say("Do you have a favorite animal?");
                }
                else
                {
                    Say("My favorite animal is the majestic wolf. Their loyalty and strength inspire me every day. Would you like a wolf's tooth as a keepsake? Consider it a reward for your interest in my life.");
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        Say("Here is a wolf's tooth for you.");
                        from.AddToBackpack(new MaxxiaScroll() { Name = "Wolf's Tooth" }); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
            }
            else if (speech.Contains("willow"))
            {
                Say("My ancestors named me after an ancient tree that stands in the heart of our village. It's believed to have magical properties.");
            }
            else if (speech.Contains("excellent"))
            {
                Say("Being around animals rejuvenates me, their energy and purity keeps me feeling vibrant and alive.");
            }
            else if (speech.Contains("train"))
            {
                Say("Training animals is not just about commands; it's about understanding their emotions and building trust.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("True compassion is understanding an animal’s needs and ensuring they are cared for. Sometimes, I rescue animals who've been mistreated.");
            }
            else if (speech.Contains("tree"))
            {
                Say("Legend says that the ancient tree was planted by the first settlers of our village. It’s said to hold the spirits of our ancestors.");
            }
            else if (speech.Contains("rejuvenates"))
            {
                Say("The bond between an animal and its caretaker is powerful. When I feel down or tired, spending time with them restores my spirit.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Earning an animal’s trust is a privilege. Once you have it, they'll follow you to the ends of the earth.");
            }
            else if (speech.Contains("rescue"))
            {
                Say("I've rescued many animals over the years. Each one has a story, and each one deserves love and a safe home.");
            }
            else if (speech.Contains("wolf"))
            {
                Say("The wolf, while wild, can form deep connections with humans. Their howl speaks to the soul, telling tales of ancient forests and moonlit hunts.");
            }

            base.OnSpeech(e);
        }

        public WhisperingWillow(Serial serial) : base(serial) { }

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
