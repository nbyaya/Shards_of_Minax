using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tempting Tina")]
    public class TemptingTina : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TemptingTina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tempting Tina";
            Body = 0x191; // Human female body

            // Stats
            Str = 88;
            Dex = 72;
            Int = 53;
            Hits = 63;

            // Appearance
            AddItem(new FancyDress() { Hue = 2962 }); // Dress with specified hue
            AddItem(new Boots() { Hue = 2963 }); // Boots with specified hue

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, darling. I am Tempting Tina, your heart's desire.");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, my dear, my health is as impeccable as my beauty.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession, you ask? I am an enchantress, weaving dreams and fantasies for those who can afford them.");
            }
            else if (speech.Contains("battles"))
            {
                Say("But tell me, do you truly believe in the power of passion, darling?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you dare to indulge in the sweetest of sins, my dear?");
            }
            else if (speech.Contains("enchantress"))
            {
                Say("Ah, the world of enchantments is a mesmerizing one. Would you like to learn about the most potent spell I've ever cast?");
            }
            else if (speech.Contains("spell"))
            {
                Say("It was a spell of love and longing, so powerful it bound two souls for an eternity. But such spells come with a price. Would you risk it all for love, darling?");
            }
            else if (speech.Contains("risk"))
            {
                Say("Love and enchantments often go hand in hand, and both require sacrifices. I once saw a brave soul lose everything for a mere moment of true love. Would you make a similar sacrifice?");
            }
            else if (speech.Contains("no"))
            {
                Say("Ah, a wise choice. Love can be both a blessing and a curse. Always weigh the consequences before leaping into the unknown.");
            }
            else if (speech.Contains("sacrifice"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the allure of love is truly powerful. For your bravery and desire, allow me to gift you something special.");
                    from.AddToBackpack(new BootsOfCommand()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("passion"))
            {
                Say("Passion fuels the soul, but it also has the power to consume it. I've danced in its fiery embrace many times. Have you ever been consumed by passion?");
            }
            else if (speech.Contains("dance"))
            {
                Say("Dancing is an expression of the soul, much like love. Would you care to dance with me under the moonlight, my dear?");
            }
            else if (speech.Contains("moonlight"))
            {
                Say("Ah, perhaps another time then. Always remember, there's magic in every step, every beat, every breath.");
            }
            else if (speech.Contains("expression"))
            {
                Say("Splendid! Let the rhythm of our hearts guide our steps. Dance is the purest form of magic, after all.");
            }

            base.OnSpeech(e);
        }

        public TemptingTina(Serial serial) : base(serial) { }

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
