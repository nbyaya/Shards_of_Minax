using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Plato the Idealist")]
    public class PlatoTheIdealist : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PlatoTheIdealist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Plato the Idealist";
            Body = 0x190; // Human male body

            // Stats
            Str = 65;
            Dex = 55;
            Int = 125;
            Hits = 58;

            // Appearance
            AddItem(new Robe() { Hue = 2208 });
            AddItem(new Sandals() { Hue = 1176 });
            AddItem(new Spellbook() { Name = "Plato's Dialogues" });

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
                Say("I am Plato the Idealist, a philosopher lost in this wretched world!");
            }
            else if (speech.Contains("health"))
            {
                Say("This world has wounded me, both in body and spirit.");
            }
            else if (speech.Contains("job"))
            {
                Say("My supposed 'job' is to seek the truth and challenge the status quo.");
            }
            else if (speech.Contains("philosopher") || speech.Contains("ideas") || speech.Contains("power"))
            {
                Say("Do you understand the power of ideas, or are you just another cog in the machine?");
            }
            else if (speech.Contains("free thinker") || speech.Contains("follower") || speech.Contains("mindless"))
            {
                Say("Your response reveals much about your character. Are you a free thinker or a mindless follower?");
            }
            else if (speech.Contains("plato"))
            {
                Say("I once lived in a city of ideals, where ideas were more powerful than the mightiest of warriors. My teachings were considered radical, but they shaped minds.");
            }
            else if (speech.Contains("wounded"))
            {
                Say("Yes, I was injured by those who felt threatened by my thoughts. The pen is indeed mightier than the sword, and my ideas were the sharpest dagger to some.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is a challenging mistress. I have traveled to the farthest corners of this land, from the caverns of ignorance to the peaks of enlightenment, seeking her embrace.");
            }
            else if (speech.Contains("ideas"))
            {
                Say("Ideas have the power to change the world, and every individual possesses this potential. If you can grasp the depth of an idea, you can reshape reality.");
            }
            else if (speech.Contains("follower"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To blindly follow is to give up one's essence. I've seen many lost souls, drifting without purpose. But for those who choose to break free and question, a reward awaits.");
                    from.AddToBackpack(new LifeLeechAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public PlatoTheIdealist(Serial serial) : base(serial) { }

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
