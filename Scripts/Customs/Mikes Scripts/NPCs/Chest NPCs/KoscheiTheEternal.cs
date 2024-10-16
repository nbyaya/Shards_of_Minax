using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Koschei the Eternal")]
    public class KoscheiTheEternal : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KoscheiTheEternal() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Koschei the Eternal";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = 2150 });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new QuarterStaff() { Hue = 1150 });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x204B); // Dark hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203F, 0x2041); // Beard styles
            FacialHairHue = Utility.RandomHairHue();

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

            // Basic Responses
            if (speech.Contains("name"))
            {
                Say("I am Koschei the Eternal, bound by ancient magic.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is eternal, as unchanging as my curse.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard secrets and treasures beyond mortal comprehension.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are often hidden in the darkness. Seek the light to uncover them.");
            }
            else if (speech.Contains("immortality"))
            {
                Say("Immortality is both a gift and a curse, holding endless time but bound by sorrow.");
            }

            // Intermediate Keywords
            else if (speech.Contains("darkness"))
            {
                Say("In the darkness, one may find the truth or be consumed by it. What do you seek in the shadows?");
            }
            else if (speech.Contains("light"))
            {
                Say("Light is a beacon in the darkness, guiding the lost and revealing hidden truths.");
            }
            else if (speech.Contains("truth"))
            {
                Say("The truth can be elusive. Sometimes it is found by questioning everything.");
            }
            else if (speech.Contains("question"))
            {
                Say("To question is to seek understanding. But beware, for answers may not always be what you expect.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding is the key to wisdom. Only through it can one grasp the essence of existence.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is born from experience and reflection. It can illuminate even the darkest of paths.");
            }

            // Advanced Keywords
            else if (speech.Contains("experience"))
            {
                Say("Experience teaches us much, but it is often through hardship that we learn the most.");
            }
            else if (speech.Contains("hardship"))
            {
                Say("Hardship builds character and strength. Those who overcome it find their true selves.");
            }
            else if (speech.Contains("character"))
            {
                Say("Character is forged in the fires of adversity. It defines who we are and what we become.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength comes from within and is tested by trials. It is a reflection of one's spirit.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit is the essence of one's being. It guides and sustains us through all challenges.");
            }

            // Final Reward
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are opportunities in disguise. Face them with courage, and you will be rewarded.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is not the absence of fear, but the triumph over it. Show your courage, and you may earn my favor.");
            }
            else if (speech.Contains("favor"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You have proven your courage and wisdom. Accept this chest as a token of your victory over the trials.");
                    from.AddToBackpack(new KoscheisUndyingChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials are the crucible in which our true selves are revealed. Embrace them, and you shall prevail.");
            }

            base.OnSpeech(e);
        }

        public KoscheiTheEternal(Serial serial) : base(serial) { }

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
