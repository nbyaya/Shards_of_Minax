using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Potionmaster Pia")]
    public class PotionmasterPia : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PotionmasterPia() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Potionmaster Pia";
            Body = 0x191; // Human female body

            // Stats
            Str = 83;
            Dex = 39;
            Int = 123;
            Hits = 74;

            // Appearance
            AddItem(new PlainDress() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 38 });
            AddItem(new LeatherGloves() { Name = "Pia's Pouch Protecting Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am Potionmaster Pia, the alchemist extraordinaire!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Oh, I'm as healthy as a potion, which is to say, quite robust!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I dabble in the mystical arts of potion-making!");
            }
            else if (speech.Contains("mystical"))
            {
                Say("Alchemy is a fine art, don't you think? One must possess the intellect to unlock its secrets. Are you an intellect?");
            }
            else if (speech.Contains("intellect"))
            {
                Say("Ha! I thought as much. Well then, show me your intellect. Answer this riddle: What has keys but can't open locks?");
            }
            else if (speech.Contains("potionmaster"))
            {
                Say("Potionmaster? Ah, not just any Potionmaster. I have traveled the lands, seeking the rarest of ingredients to make the most potent concoctions.");
            }
            else if (speech.Contains("potion"))
            {
                Say("Yes, potions! I believe that potions can not only heal the body, but also the soul. One such potion I'm working on is tied to the mantra of Spirituality.");
            }
            else if (speech.Contains("potion-making"))
            {
                Say("Potion-making is more than just mixing ingredients. It's about understanding the essence of each element and invoking the right emotions. Speaking of emotions, did you know that meditation can elevate one's emotional state?");
            }
            else if (speech.Contains("alchemy"))
            {
                Say("Alchemy and spirituality often go hand in hand. For instance, the first syllable of the mantra of Spirituality is 'OM.'");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Riddles are a test of the mind, but true intellect lies in one's ability to connect with the universe. Much like the practice of mantras in spirituality.");
            }
            else if (speech.Contains("ingredients"))
            {
                Say("Gathering ingredients takes me to far-off lands. From the driest deserts to the deepest caves, I go wherever the rarest herbs and minerals are found.");
            }
            else if (speech.Contains("spirituality"))
            {
                Say("Spirituality, to me, is the bond between one's soul and the greater cosmos. Have you ever tried meditating on the virtues?");
            }
            else if (speech.Contains("meditation"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new BeltSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("serenity"))
            {
                Say("Finding serenity amidst chaos is a virtue in itself. It's a sense of peace that eludes many, but can be found in nature's embrace.");
            }
            else if (speech.Contains("herbs"))
            {
                Say("These herbs carry the wisdom of ages. If you're keen to learn, seek the elder druid in the forest. He knows their secrets.");
            }

            base.OnSpeech(e);
        }

        public PotionmasterPia(Serial serial) : base(serial) { }

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
