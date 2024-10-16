using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rydia")]
    public class Rydia : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Rydia() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rydia";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 60;
            Int = 120;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = -1 }); // Hue should be set according to your specific requirement
            AddItem(new Cloak() { Hue = -1 });
            AddItem(new Sandals() { Hue = -1 });
            AddItem(new FeatheredHat() { Hue = -1 });
            AddItem(new Whip() { Name = "Rydia's Whip" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Rydia.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a summoner, skilled in the arts of magic.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtue of compassion guides my actions. It is the key to understanding and empathy.");
            }
            else if (speech.Contains("yes") && speech.Contains("compassion"))
            {
                Say("Do you also follow the path of compassion, traveler?");
            }
            else if (speech.Contains("rydia"))
            {
                Say("Ah, you've heard of me? I was once known far and wide for my adventures alongside the great heroes of our age.");
            }
            else if (speech.Contains("good"))
            {
                Say("I feel energized, especially when I'm close to nature. The forests are my sanctuary.");
            }
            else if (speech.Contains("summoner"))
            {
                Say("Yes, as a summoner, I call upon the spirits of the land and sky to aid me. Have you ever seen an Eidolon?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is more than just a virtue; it's a way of life. When we embrace compassion, we open our hearts to others and their experiences.");
            }
            else if (speech.Contains("heroes"))
            {
                Say("The heroes I speak of battled the greatest of evils. I was proud to fight alongside them and learn the meaning of true bravery.");
            }
            else if (speech.Contains("forests"))
            {
                Say("Deep in the forests, one can find magical creatures and ancient secrets. But be wary, for not everything that lurks there is friendly.");
            }
            else if (speech.Contains("eidolon"))
            {
                Say("Eidolons are mighty spirits, bound by ancient pacts to aid summoners. In exchange, we give them offerings and respect. If you prove trustworthy, I might gift you something special.");
            }
            else if (speech.Contains("offerings"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Offerings can be anything from rare gems to whispered secrets. Ah, since you've shown interest, here's a token of my appreciation. May it serve you well.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Rydia(Serial serial) : base(serial) { }

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
