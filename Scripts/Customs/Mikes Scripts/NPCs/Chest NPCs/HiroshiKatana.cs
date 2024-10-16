using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Hiroshi Katana")]
    public class HiroshiKatana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HiroshiKatana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Hiroshi Katana";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new Katana() { Name = "Samurai's Blade" });
            
            Hue = Race.RandomSkinHue(); // Skin tone
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
                Say("I am Hiroshi Katana, the keeper of ancient secrets and treasures. Tell me, do you seek wisdom?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is a path to understanding the samurai way. Do you desire strength to follow this path?");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength comes from within and is nurtured by honor. Have you heard of the code of the samurai?");
            }
            else if (speech.Contains("code"))
            {
                Say("The code of the samurai is one of discipline and loyalty. It guides us in both life and battle. Do you seek knowledge of the samurai’s legacy?");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The samurai's legacy is rich with stories of valor and wisdom. Would you like to learn more about our traditions?");
            }
            else if (speech.Contains("traditions"))
            {
                Say("Traditions are the cornerstone of our way of life. To understand them, one must first learn the virtues of honor and courage. Are you familiar with the virtues?");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues such as honor and courage are essential to the samurai. They guide our actions and decisions. Do you wish to understand how these virtues are practiced?");
            }
            else if (speech.Contains("practice"))
            {
                Say("To practice virtue is to live by it daily. This requires commitment and dedication. Are you prepared to embark on a quest for honor?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest for honor is a journey of self-discovery and challenge. Are you ready to face the trials ahead?");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials test one's resolve and character. Through these trials, one earns their place among the samurai. Do you seek a reward for your dedication?");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your journey through the samurai code has shown your commitment. For your dedication, accept this 'Samurai's Stash' as a token of my gratitude.");
                    from.AddToBackpack(new SamuraiStash()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I do not understand your query. Please ask about my name, wisdom, strength, or the code of the samurai.");
            }

            base.OnSpeech(e);
        }

        public HiroshiKatana(Serial serial) : base(serial) { }

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
