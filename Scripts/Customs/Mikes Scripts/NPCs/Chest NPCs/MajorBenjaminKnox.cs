using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Major Benjamin Knox")]
    public class MajorBenjaminKnox : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MajorBenjaminKnox() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Major Benjamin Knox";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomNeutralHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x2041, 0x203B); // Different beard options
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
                Say("Greetings, I am Major Benjamin Knox, an officer of the War of 1812. Ask me about the 'war' and its 'history'.");
            }
            else if (speech.Contains("war"))
            {
                Say("The War of 1812 was a fierce conflict. If you're interested, inquire about specific 'battles' or 'heroes' of that era.");
            }
            else if (speech.Contains("history"))
            {
                Say("The history of the War of 1812 is rich and complex. Perhaps you'd like to learn about specific 'battles' or 'treaties'?");
            }
            else if (speech.Contains("battles"))
            {
                Say("Battles such as the Battle of New Orleans were crucial. If you're keen on details, ask me about 'heroes' or 'victories' in those battles.");
            }
            else if (speech.Contains("heroes"))
            {
                Say("Heroes like Andrew Jackson were pivotal. For more, ask me about 'victories' or significant 'events' involving these heroes.");
            }
            else if (speech.Contains("victories"))
            {
                Say("Victory in battles was often hard-fought. For more on victories, inquire about significant 'events' or key 'treaties'.");
            }
            else if (speech.Contains("treaties"))
            {
                Say("Treaties like the Treaty of Ghent ended the war. If you're interested in their impact, ask about 'events' or 'relics' from that time.");
            }
            else if (speech.Contains("events"))
            {
                Say("Events such as the signing of treaties were critical. To understand more, ask about 'relics' or notable 'characters' from those events.");
            }
            else if (speech.Contains("relics"))
            {
                Say("Relics from the war are valuable and tell many stories. To know more, inquire about 'characters' or specific 'treasures'.");
            }
            else if (speech.Contains("characters"))
            {
                Say("Key characters like General Jackson were influential. Ask about 'treasures' or particular 'relics' associated with them.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures from the War of 1812 are hidden and rare. If you want one, you need to understand the significance of 'relics' and 'victories' first.");
            }
            else if (speech.Contains("battle of new orleans"))
            {
                Say("The Battle of New Orleans was a decisive victory. It played a significant role in the end of the war. Now, discuss 'victories' or 'relics' to proceed.");
            }
            else if (speech.Contains("victories") && speech.Contains("battle of new orleans"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your knowledge of the Battle of New Orleans is impressive. For your diligence, accept this chest containing relics from the War of 1812.");
                    from.AddToBackpack(new WarOf1812Vault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Speak to me about the War of 1812. Ask about the 'history', 'battles', 'heroes', or 'treasures' to learn more.");
            }

            base.OnSpeech(e);
        }

        public MajorBenjaminKnox(Serial serial) : base(serial) { }

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
