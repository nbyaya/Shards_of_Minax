using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mystic Elowen")]
    public class MysticElowen : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MysticElowen() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mystic Elowen";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 60;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1154 }); // Mysterious robe
            AddItem(new WizardsHat() { Hue = 1154 }); // Matching hat
            AddItem(new Sandals() { Hue = 1154 }); // Fitting footwear
            AddItem(new Gold() { Amount = 100 }); // Some starting gold

            // Random hair and facial features
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
                Say("I am Mystic Elowen, keeper of arcane secrets. If you seek more, ask about my 'job' or my 'health'.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to guard the secrets of the sorceress's chest. But to understand it fully, inquire about my 'health'.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the magic I wield. Now, if you wish to know more, you must ask about 'secrets'.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets are not easily revealed. To gain more insight, ask about 'knowledge'.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is power. To seek further, you must express your interest in a 'challenge'.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("A challenge lies ahead. But before we proceed, do you understand the 'reward' for those who prevail?");
            }
            else if (speech.Contains("reward"))
            {
                Say("Rewards come to those who prove their worth. If you’re ready, you must show your understanding of 'treasure'.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("The treasure is guarded by many secrets. But before you can claim it, tell me what you know about 'mysteries'.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries are the heart of all enchantments. You must solve the 'puzzle' to prove your worthiness.");
            }
            else if (speech.Contains("puzzle"))
            {
                Say("The puzzle you seek involves understanding 'enchantments'. Speak of it to continue.");
            }
            else if (speech.Contains("enchantments"))
            {
                Say("Enchantments are the key to unlocking the most profound secrets. To earn your reward, you need to show insight about 'arcane'.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("Arcane knowledge is essential for the wise. If you grasp its essence, you will be closer to earning the 'chest'.");
            }
            else if (speech.Contains("chest"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Return later.");
                }
                else
                {
                    Say("You have demonstrated great understanding and perseverance. For your efforts, accept this enchanted chest.");
                    from.AddToBackpack(new SorceressSecretsChest()); // Give the Sorceress's Secrets Chest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("arcane"))
            {
                Say("Arcane knowledge is key to unlocking the greatest secrets. Continue to explore, and you might find the path to the chest.");
            }

            base.OnSpeech(e);
        }

        public MysticElowen(Serial serial) : base(serial) { }

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
