using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mystic Elara")]
    public class MysticElara : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MysticElara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mystic Elara";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 75;
            Int = 150;
            Hits = 100;

            // Appearance
            AddItem(new Robe() { Hue = 1109 });
            AddItem(new Sandals() { Hue = 1905 });
            AddItem(new WizardsHat() { Hue = 1109 });
            AddItem(new LeatherGloves() { Hue = 1109 });

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
                Say("Greetings, traveler. I am Mystic Elara.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a mystic, devoted to the study of ancient knowledge and the balance of energies.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Balance is the key to understanding the virtues. Do you seek knowledge of the virtues?");
            }
            else if (speech.Contains("intrigued"))
            {
                Say("Good, for the virtues guide us all. What virtue intrigues you the most?");
            }
            else if (speech.Contains("elara"))
            {
                Say("Ah, you've heard of me? I am indeed Mystic Elara, keeper of ancient secrets and lore. But tell me, what have you heard?");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, the energies of the world keep me vibrant and well. But health is not just of the body; it's of the mind and spirit as well. Do you meditate?");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balancing the energies is a delicate task. Every action has a reaction, and the universe seeks equilibrium. Have you ever experienced the pull of opposing forces?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("The virtues are essential principles that guide our actions and decisions. They represent the best in us. Seek them, and you shall find enlightenment. Have you come across any artifacts related to them?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are but truths waiting to be uncovered. If you seek, you shall find. However, some secrets come with a price. Do you have the courage to pay it?");
            }
            else if (speech.Contains("meditate"))
            {
                Say("Meditation allows us to connect with the energies around us, to find peace and clarity. I have a special mantra that I can share with those who prove worthy. Do you seek this mantra?");
            }
            else if (speech.Contains("opposing"))
            {
                Say("Like light and dark, good and evil, life and death - opposing forces shape our reality. It's the balance between them that defines our path. Have you ever felt torn between two choices?");
            }
            else if (speech.Contains("intrigued"))
            {
                Say("There are many artifacts in this world, some imbued with the power of the virtues. If you ever come across one, treasure it. And perhaps, if you bring it to me, I might reward you for your efforts.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("You've shown great interest in the virtues. As a token of my appreciation, take this. May it guide you on your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MysticElara(Serial serial) : base(serial) { }

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
