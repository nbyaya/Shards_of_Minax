using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kensuke Takahashi")]
    public class KensukeTakahashi : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KensukeTakahashi() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kensuke Takahashi";
            Body = 0x191; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1157 });
            AddItem(new LongPants() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1157 });
            AddItem(new Cap() { Hue = 1157 });

            // Random Hair and Facial Hair
            HairItemID = Utility.RandomList(0x204E, 0x204F, 0x204D); // Various hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = -1; // No facial hair for this NPC

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
                Say("Greetings, traveler. I am Kensuke Takahashi, a humble keeper of treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, as one should be when guarding such precious gifts.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to ensure that the Geisha's Gift is given to those who prove their worth.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Proving one's worth involves more than just strength. It requires wisdom and understanding.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from experience and reflection. Seek knowledge in all things.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding is the key to unlocking many secrets. Tell me, do you seek a reward?");
            }
            else if (speech.Contains("seek"))
            {
                Say("Very well. If you are truly worthy, you must answer a riddle. Solve it, and you shall receive a treasure.");
            }
            else if (speech.Contains("riddle"))
            {
                Say("What has roots as nobody sees, Is taller than trees, Up, up it goes, And yet never grows? Answer me, and you shall be rewarded.");
            }
            else if (speech.Contains("mountain"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You have answered correctly! Here is the Geisha's Gift, a token of your wisdom and perseverance.");
                    from.AddToBackpack(new GeishasGift()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("gift"))
            {
                Say("The Geisha's Gift is a treasure that reflects the grace and beauty of its namesake. It is given only to those who are deserving.");
            }
            else if (speech.Contains("deserving"))
            {
                Say("To be deserving, one must show respect and diligence in their quest. Have you shown such qualities?");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect is earned through actions and deeds. Have you demonstrated such respect in your journey?");
            }
            else if (speech.Contains("actions"))
            {
                Say("Actions speak louder than words. Reflect on your actions, and they will reveal your true nature.");
            }
            else if (speech.Contains("deeds"))
            {
                Say("Deeds are the proof of one's character. Have your deeds been honorable and just?");
            }
            else if (speech.Contains("honorable"))
            {
                Say("Honor is a virtue that guides one's path. Do you follow the path of honor in your adventures?");
            }
            else if (speech.Contains("adventures"))
            {
                Say("Adventures are the trials that shape us. Through them, we discover our true selves. What have your adventures taught you?");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials are challenges that test our resolve. Each trial overcome makes us stronger. What trials have you faced?");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the strength to persevere. It is tested in moments of hardship. Have you proven your resolve?");
            }
            else if (speech.Contains("hardship"))
            {
                Say("Hardship is a teacher of resilience. It builds character and determination. How have you faced your hardships?");
            }
            else if (speech.Contains("resilience"))
            {
                Say("Resilience is the ability to recover quickly from difficulties. It is a key trait of the worthy. Show me your resilience.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("Being worthy means you have proven yourself through your actions, deeds, and character. Are you ready for your reward?");
            }
            else if (speech.Contains("reward"))
            {
                Say("A reward is a token of acknowledgment for your efforts. You have shown great understanding and perseverance.");
            }
            else if (speech.Contains("acknowledgment"))
            {
                Say("Acknowledgment comes when one's efforts are recognized. Your journey has been noted, and now you must prove your worth.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Your journey reflects your growth and learning. Each step taken is a testament to your character. What have you learned?");
            }
            else if (speech.Contains("learning"))
            {
                Say("Learning is a continuous process. Each experience adds to your wisdom. Share with me your newfound knowledge.");
            }

            base.OnSpeech(e);
        }

        public KensukeTakahashi(Serial serial) : base(serial) { }

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
