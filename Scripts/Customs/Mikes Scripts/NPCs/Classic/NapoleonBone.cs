using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Napoleon Bonaparte")]
    public class NapoleonBone : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NapoleonBone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Napoleon Bonaparte";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 80;
            Hits = 70;

            // Appearance
            AddItem(new PlateLegs() { Hue = 1150 });
            AddItem(new PlateChest() { Hue = 1150 });
            AddItem(new PlateGloves() { Hue = 1150 });
            AddItem(new PlateHelm() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });
            AddItem(new Halberd() { Name = "Napoleon's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("I am Napoleon Bonaparte, Emperor of France!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in the peak of health, just like my empire!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I am a conqueror, a statesman, and a strategist!");
            }
            else if (speech.Contains("ambition") || speech.Contains("cunning"))
            {
                Say("Do you have the ambition and cunning to achieve greatness?");
            }
            else if (speech.Contains("yes") || speech.Contains("promise"))
            {
                Say("Your response shows promise. Remember, ambition and cunning can shape destinies!");
            }
            else if (speech.Contains("emperor"))
            {
                Say("I have achieved the pinnacle of leadership. I stand tall, not just in stature but in my deeds. My legacy will echo through history.");
            }
            else if (speech.Contains("empire"))
            {
                Say("My empire stretches far and wide, from the sunny coasts of Spain to the frigid lands of Russia. But an empire is only as strong as its weakest link.");
            }
            else if (speech.Contains("strategist"))
            {
                Say("As a strategist, I have orchestrated numerous campaigns across Europe. Every move on the battlefield is a result of meticulous planning.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("My legacy is built on the principles of the Revolution, my love for France, and the sacrifices of countless soldiers. Ensure my tales are passed down.");
            }
            else if (speech.Contains("russia"))
            {
                Say("Ah, Russia... A vast land of mystery and challenge. My campaign there was one of the harshest experiences. The cold was relentless, but so was my determination.");
            }
            else if (speech.Contains("campaigns"))
            {
                Say("Each of my campaigns has its own story, challenges, and lessons. If you study them closely, you may learn the art of war and leadership. Would you like a recommendation on a book?");
            }
            else if (speech.Contains("book"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Then you must read \"The Campaigns of Napoleon\". It delves deep into my military expeditions. Here, take this. It's a rare edition. Use it wisely.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public NapoleonBone(Serial serial) : base(serial) { }

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
