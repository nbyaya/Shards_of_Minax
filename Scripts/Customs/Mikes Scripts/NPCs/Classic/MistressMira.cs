using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mistress Mira")]
    public class MistressMira : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MistressMira() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mistress Mira";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 80;
            Int = 55;
            Hits = 65;

            // Appearance
            AddItem(new FancyDress() { Hue = 1301 }); // Clothing item with hue 1301
            AddItem(new GoldNecklace() { Name = "Mira's Necklace" });
            AddItem(new Boots() { Hue = 1157 }); // Boots with hue 1157

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Mistress Mira, a courtesan of exquisite talents.");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care about my health? It's my heart that's truly wounded.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\" is to provide companionship and fleeting moments of pleasure to those who can afford it.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you think life in the shadows is easy? The battles we fight are not with swords but with seduction.");
            }
            else if (speech.Contains("seduction"))
            {
                Say("It's a dance, a game of wit and charm. Just as a warrior learns the weight of their blade, I've mastered the art of allure.");
            }
            else if (speech.Contains("dance"))
            {
                Say("The dance is a reflection of life, filled with twists and turns, highs and lows. Just as in life, it's the passion and emotion that makes it memorable.");
            }
            else if (speech.Contains("heart"))
            {
                Say("A tale of lost love and betrayal. A knight once promised me eternity but abandoned me for a quest of Valor. By the way, I've heard whispers that the first syllable of the mantra of Valor is TRZ.");
            }
            else if (speech.Contains("companionship"))
            {
                Say("Many seek my company to escape the burdens of their lives, if only for a moment. A whispered secret, a stolen glance, these are the currencies I trade in.");
            }
            else if (speech.Contains("talents"))
            {
                Say("I have been trained in the arts of conversation, dance, and song. It's not just physical allure, but the power of the mind that truly captivates.");
            }
            else if (speech.Contains("knight"))
            {
                Say("He was a beacon of courage and strength. But sometimes, the call of duty can make one forget the promises of the heart.");
            }
            else if (speech.Contains("secret"))
            {
                Say("Secrets are like precious gems in the world I inhabit. Those who hold them have power, and those who seek them pay dearly.");
            }
            else if (speech.Contains("game"))
            {
                Say("Every interaction is a game of chance and skill. You never truly know the outcome until the final move is played.");
            }
            else if (speech.Contains("ponder"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new Gold(1000)); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MistressMira(Serial serial) : base(serial) { }

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
