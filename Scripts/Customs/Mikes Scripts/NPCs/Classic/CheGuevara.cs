using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Che Guevara")]
    public class CheGuevara : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CheGuevara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Che Guevara";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 100;
            Int = 85;
            Hits = 78;

            // Appearance
            AddItem(new LongPants() { Hue = 1911 });
            AddItem(new Bandana() { Hue = 1157 });
            AddItem(new Tunic() { Hue = 1157 });
            AddItem(new Boots() { Hue = 1103 });
            AddItem(new Cutlass() { Name = "Liberator's Sabre" });

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
                Say("I am Che Guevara, a revolutionary!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, but the people need healing.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to unite the people and promote equality for all.");
            }
            else if (speech.Contains("equality"))
            {
                Say("True justice can only be achieved through unity and solidarity. Do you believe in equality?");
            }
            else if (speech.Contains("yes") && CheckSpeechDependsOn(e.Speech, 30))
            {
                Say("Good. The path to virtue lies in compassion and the well-being of all.");
            }
            else if (speech.Contains("revolutionary"))
            {
                Say("I have fought in many battles, striving for the welfare of the common people. Have you heard of the Cuban Revolution?");
            }
            else if (speech.Contains("people"))
            {
                Say("The people have suffered under the yoke of tyranny for too long. But with resilience, we'll bring forth change. Have you seen such resilience anywhere?");
            }
            else if (speech.Contains("unite"))
            {
                Say("Unity is our strength. In unity, we find the power to overthrow oppressors and bring about true change. Do you understand the importance of unity?");
            }
            else if (speech.Contains("revolution"))
            {
                Say("Yes, the Cuban Revolution was a turning point. It wasn't just a change of government but a change in the mindset of the people. Did it inspire you as well?");
            }
            else if (speech.Contains("resilience"))
            {
                Say("Resilience is the mark of a people's will to survive and thrive despite adversities. It's the fire that keeps us going. Have you felt this fire within yourself?");
            }
            else if (speech.Contains("importance"))
            {
                Say("Understanding the importance of unity is key to any revolution. When people stand together, they can't be defeated. Will you stand with us?");
            }
            else if (speech.Contains("inspire"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Those who are inspired by the revolution are the ones who carry its spirit forward. For your understanding, take this token as a symbol of our gratitude.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("fire"))
            {
                Say("This fire, the burning desire for change and justice, is what drives a revolution. It's what has driven me all these years. Do you wish to kindle this fire in others?");
            }
            else if (speech.Contains("stand"))
            {
                Say("Standing with the people is the most honorable choice. Together, we'll write history. Do you believe in our cause?");
            }

            base.OnSpeech(e);
        }

        private bool CheckSpeechDependsOn(string speech, int entryNumber)
        {
            // Implement this method based on your specific dependencies. 
            // For example, return true if the speech is related to the entryNumber given.
            return true;
        }

        public CheGuevara(Serial serial) : base(serial) { }

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
