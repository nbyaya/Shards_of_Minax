using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of a bartender")]
    public class AlehouseBartender : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AlehouseBartender() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alehouse Bartender";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new Boots() { Hue = 2413 });
            AddItem(new Longsword() { Hue = 2413 });
            AddItem(new BodySash() { Hue = 2413 });

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
                Say("Ah, welcome to the alehouse! I am the Alehouse Bartender.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Well, I keep the ale flowing and make sure everyone has a good time. It's a tough job, but someone's got to do it!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in tip-top shape, thanks to all the hearty ale and lively company!");
            }
            else if (speech.Contains("alehouse"))
            {
                Say("The alehouse is a place of joy and merriment. If you seek a good drink and a great time, you’ve come to the right place.");
            }
            else if (speech.Contains("drink"))
            {
                Say("A good drink always brings cheer. Have you tried the house special? It’s quite famous!");
            }
            else if (speech.Contains("special"))
            {
                Say("Ah, the house special is a concoction of fine ale and secret ingredients. Only the best for our patrons.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Oh, the alehouse chest! It's a rare find. If you can impress me with your knowledge of alehouse lore, I might just reward you with one.");
            }
            else if (speech.Contains("lore"))
            {
                Say("The lore of the alehouse is rich and storied. From tales of legendary brews to the secrets of our special recipes, it’s a wealth of knowledge.");
            }
            else if (speech.Contains("legendary"))
            {
                Say("Ah, the legendary brews. Each one has a story. Ask me about them if you’re interested.");
            }
            else if (speech.Contains("story"))
            {
                Say("Every brew has its tale. Some speak of heroes who came to our alehouse and left with newfound strength.");
            }
            else if (speech.Contains("strength"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I’ve already given out all the rewards for now. Please come back later.");
                }
                else
                {
                    Say("For your keen interest and your inquisitive nature, I present to you the alehouse chest. May it serve you well!");
                    from.AddToBackpack(new AlehouseChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public AlehouseBartender(Serial serial) : base(serial) { }

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
