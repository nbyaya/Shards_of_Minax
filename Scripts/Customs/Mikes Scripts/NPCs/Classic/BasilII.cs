using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Basil II")]
    public class BasilII : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BasilII() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Basil II";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 100;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new Cloak(0x46F)); // BloodRed color
            AddItem(new Boots(0x46F)); // BloodRed color
            AddItem(new Longsword() { Name = "Basil II's Sword" });

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
                Say("Greetings, traveler. I am Basil II of Byzantium.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a guardian of virtue, protecting the people of Byzantium.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("The virtue of Humility teaches us to be humble in victory and gracious in defeat. Do you follow the path of virtue?");
            }
            else if (speech.Contains("yes") && speech.Contains("virtue"))
            {
                Say("It is a noble path, traveler. May your deeds reflect the virtues you hold dear.");
            }
            else if (speech.Contains("byzantium"))
            {
                Say("Ah, Byzantium, the great empire of the east. A land filled with stories, mysteries, and a rich tapestry of history. Have you heard of the legends?");
            }
            else if (speech.Contains("legends"))
            {
                Say("There are countless tales. One speaks of the hidden relic of Saint Mark, said to grant blessings to those pure of heart. If you prove yourself virtuous, I might tell you more.");
            }
            else if (speech.Contains("relic"))
            {
                Say("The relic is rumored to be hidden deep within the catacombs of Byzantium, guarded by ancient spirits. Many have sought it, but few return. Are you brave enough to seek it out?");
            }
            else if (speech.Contains("spirits"))
            {
                Say("The spirits of the catacombs are the souls of Byzantium's most devout protectors. They watch over the relic, ensuring it does not fall into the wrong hands. Treat them with respect, and they may aid you in your quest.");
            }
            else if (speech.Contains("quest"))
            {
                Say("Ah, the quest for the relic is not one to be taken lightly. It demands courage, wisdom, and purity of heart. If you embark on this journey and prove your worth, I may reward you with something that'll aid you.");
            }
            else if (speech.Contains("prove"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("I have seen many travelers, but few possess a heart as noble as yours. Here, take this [unspecified reward]. It has served me well in the past and may it guide you on your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("path"))
            {
                Say("The path of virtue is a treacherous one, filled with trials and tribulations. Yet, it is also the most rewarding. Every choice we make defines our character. Choose wisely, traveler.");
            }

            base.OnSpeech(e);
        }

        public BasilII(Serial serial) : base(serial) { }

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
