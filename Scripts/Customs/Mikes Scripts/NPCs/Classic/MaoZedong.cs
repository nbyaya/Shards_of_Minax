using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mao Zedong")]
    public class MaoZedong : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MaoZedong() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mao Zedong";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 95;
            Hits = 72;

            // Appearance
            AddItem(new LongPants() { Hue = 1155 });
            AddItem(new Doublet() { Hue = 1111 });
            AddItem(new Boots() { Hue = 1111 });
            AddItem(new ShepherdsCrook() { Name = "The Long March" });

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
                Say("I am Mao Zedong, the leader of the proletariat!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is irrelevant, for the strength of the people is boundless!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to spread the ideals of communism and unite the working class!");
            }
            else if (speech.Contains("communism"))
            {
                Say("Communism is the path to true equality! Do you understand, comrade?");
            }
            else if (speech.Contains("join"))
            {
                Say("The proletariat shall prevail! Will you join us in the struggle, comrade?");
            }
            else if (speech.Contains("proletariat"))
            {
                Say("The proletariat is the working class, those who toil for their bread. They are the backbone of our great revolution.");
            }
            else if (speech.Contains("strength"))
            {
                Say("The strength of the people emerges from unity and resolve. Individual weaknesses fade in the face of collective determination.");
            }
            else if (speech.Contains("ideals"))
            {
                Say("The ideals of communism call for the equal distribution of wealth and the eradication of class distinctions. It's the beacon of hope for many.");
            }
            else if (speech.Contains("equality"))
            {
                Say("Equality means that every individual, regardless of their background, has the same rights and opportunities. It is what we strive for, day and night.");
            }
            else if (speech.Contains("struggle"))
            {
                Say("The struggle is real and ongoing. It requires commitment and sacrifice. For those who truly understand and support our cause, I have a token of appreciation.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("Our revolution is not just about changing the present, but securing a future where every voice is heard, and every hand shares in the labor and rewards.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Unity is achieved when the masses come together for a common goal. Through unity, we can overcome any obstacle.");
            }
            else if (speech.Contains("wealth"))
            {
                Say("Wealth, in the hands of a few, can lead to oppression. But when shared equally, it can uplift an entire society.");
            }
            else if (speech.Contains("rights"))
            {
                Say("Rights are the fundamental privileges that every human should enjoy, regardless of their status or wealth. We fight to ensure these rights for all.");
            }
            else if (speech.Contains("token"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("This token is a symbol of our appreciation for your support. Take it as a reminder of the cause you're helping advance.");
                    from.AddToBackpack(new EarringSlotChangeDeed()); // Replace with the appropriate reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MaoZedong(Serial serial) : base(serial) { }

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
