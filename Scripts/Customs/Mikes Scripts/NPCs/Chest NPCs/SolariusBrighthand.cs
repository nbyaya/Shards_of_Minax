using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Solarius Brighthand")]
    public class SolariusBrighthand : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SolariusBrighthand() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Solarius Brighthand";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1153 });
            AddItem(new PlateLegs() { Hue = 1153 });
            AddItem(new PlateArms() { Hue = 1153 });
            AddItem(new PlateHelm() { Hue = 1153 });
            AddItem(new PlateGloves() { Hue = 1153 });
            AddItem(new Boots() { Hue = 1153 });
            AddItem(new GoldShield() { Hue = 1153 });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Short hair
            HairHue = Utility.RandomHairHue();

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
                Say("Greetings, I am Solarius Brighthand, keeper of the sun's secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, radiant as the sun itself.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard the treasures of the sun and share its light with those who seek it.");
            }
            else if (speech.Contains("sun"))
            {
                Say("The sun is a beacon of hope and prosperity. Its light guides us through the darkest times.");
            }
            else if (speech.Contains("light"))
            {
                Say("Indeed, light is a powerful force. It can reveal the truth and banish the shadows.");
            }
            else if (speech.Contains("truth"))
            {
                Say("The truth is often hidden in plain sight. Seek beyond the obvious to find the deeper meaning.");
            }
            else if (speech.Contains("meaning"))
            {
                Say("The deeper meaning lies in understanding the balance of light and darkness within ourselves.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is key to harmony. Both light and darkness have their place in the world.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is achieved when we find peace with ourselves and our surroundings.");
            }
            else if (speech.Contains("peace"))
            {
                Say("Peace is a reflection of the harmony within. It allows us to face challenges with calm.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are opportunities for growth. Embrace them and you will shine brighter.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth requires effort and understanding. It is the process of becoming more than we were.");
            }
            else if (speech.Contains("effort"))
            {
                Say("Effort is the catalyst for progress. Without it, we remain stagnant in the shadows.");
            }
            else if (speech.Contains("progress"))
            {
                Say("Progress is made when we step out of our comfort zones and seek new horizons.");
            }
            else if (speech.Contains("horizon"))
            {
                Say("The horizon represents the limits of our vision. Push beyond it to discover new realms.");
            }
            else if (speech.Contains("realm"))
            {
                Say("A realm is a domain of our experiences. Expanding it can lead to great discoveries.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery comes from curiosity and exploration. It is the pursuit of knowledge and understanding.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge illuminates the path ahead. It empowers us to make informed choices.");
            }
            else if (speech.Contains("empower"))
            {
                Say("To empower oneself is to harness one's potential and drive towards one's goals.");
            }
            else if (speech.Contains("goal"))
            {
                Say("A goal is a destination that guides our journey. Set it wisely and pursue it with determination.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey is as important as the destination. It shapes who we become along the way.");
            }
            else if (speech.Contains("destination"))
            {
                Say("The destination is the culmination of our efforts. Reach it with pride and readiness for the next journey.");
            }
            else if (speech.Contains("pride"))
            {
                Say("Pride is a reflection of our achievements. Celebrate them, but remain humble in your heart.");
            }
            else if (speech.Contains("humble"))
            {
                Say("Humility is the recognition of our place in the grand scheme. It helps us remain grounded.");
            }
            else if (speech.Contains("grounded"))
            {
                Say("Being grounded means staying connected to reality. It provides stability and clarity.");
            }
            else if (speech.Contains("clarity"))
            {
                Say("Clarity of vision is essential for making wise decisions. Seek it in all that you do.");
            }
            else if (speech.Contains("decision"))
            {
                Say("Decisions shape our path. Choose wisely, for each choice leads to a new direction.");
            }
            else if (speech.Contains("direction"))
            {
                Say("Direction guides our journey. Follow it with intent and purpose.");
            }
            else if (speech.Contains("intent"))
            {
                Say("Intent is the driving force behind our actions. Ensure it is aligned with your goals.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose gives meaning to our efforts. Find yours and let it guide you through challenges.");
            }
            else if (speech.Contains("reward"))
            {
                Say("A reward is given to those who demonstrate understanding and resolve. Have you done so?");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding the essence of the sun has proven your worth. For this, you shall be rewarded.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Accept this chest of Helios’s bounty as a token of recognition for your wisdom and perseverance.");
                    from.AddToBackpack(new SpecialWoodenChestHelios()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the culmination of knowledge and experience. It guides us to make enlightened choices.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience teaches us lessons and provides context. Embrace it as part of your growth.");
            }
            else if (speech.Contains("lessons"))
            {
                Say("Lessons learned shape our future actions. Reflect on them and use them to improve.");
            }
            else if (speech.Contains("reflection"))
            {
                Say("Reflection allows us to assess our journey and adjust our course. It is a valuable practice.");
            }
            else if (speech.Contains("course"))
            {
                Say("Adjusting your course is a sign of wisdom. It shows you are attentive and adaptable.");
            }
            else if (speech.Contains("adaptable"))
            {
                Say("Being adaptable is crucial in a world of constant change. Embrace it to thrive.");
            }
            else if (speech.Contains("thrive"))
            {
                Say("To thrive is to excel and prosper. May your journey lead you to success and fulfillment.");
            }

            base.OnSpeech(e);
        }

        public SolariusBrighthand(Serial serial) : base(serial) { }

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
