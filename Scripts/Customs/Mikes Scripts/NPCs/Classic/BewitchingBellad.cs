using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Bewitching Bellad")]
    public class BewitchingBellad : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BewitchingBellad() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bewitching Bellad";
            Body = 0x191; // Human female body

            // Stats
            Str = 87;
            Dex = 74;
            Int = 54;
            Hits = 62;

            // Appearance
            AddItem(new FancyDress(2971)); // Clothing item with hue 2971
            AddItem(new Boots(2972)); // Boots with hue 2972

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
                Say("Greetings, dear. I am Bewitching Bellad.");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care about my health, darling? It's not like anyone cares about me.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job,' you ask? I entertain those who can afford it. But what does it matter to you?");
            }
            else if (speech.Contains("life"))
            {
                Say("Oh, you want to know more about my life? Tell me, do you judge me for it?");
            }
            else if (speech.Contains("society"))
            {
                Say("It's amusing how society condemns me while secretly indulging in my services. Do you agree?");
            }
            else if (speech.Contains("bewitching"))
            {
                Say("That's a name many whisper in the alleys and courts. I've heard tales and songs sung about me. Some true, others mere fantasies.");
            }
            else if (speech.Contains("cares"))
            {
                Say("In this line of work, very few truly care. Most just wear a mask of concern. The mask sometimes slips, revealing their true intentions.");
            }
            else if (speech.Contains("entertain"))
            {
                Say("Indeed, I am an entertainer, but not in the way you might think. I provide solace to the lonely, the weary, and the heartbroken. I've learned many secrets in my time.");
            }
            else if (speech.Contains("judge"))
            {
                Say("I've been judged by many, but it's not the judgments that weigh me down, it's the weight of the untold stories and forgotten mantras.");
            }
            else if (speech.Contains("condemns"))
            {
                Say("It's true. They shun me by day but seek me by night. Amidst the shadows and secrets, I once overheard that the third syllable of the mantra of Spirituality is LOR.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Some tales tell of my exploits, others of my heartaches. But few know of my pilgrimage to the shrine of Spirituality.");
            }
            else if (speech.Contains("mask"))
            {
                Say("Many people in our world hide behind masks. Some to protect themselves, others to deceive. But in the end, masks always fall.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("In the hushed corners of taverns, and in the embraces of the night, secrets are whispered. Some light as air, others heavy as stone.");
            }

            base.OnSpeech(e);
        }

        public BewitchingBellad(Serial serial) : base(serial) { }

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
