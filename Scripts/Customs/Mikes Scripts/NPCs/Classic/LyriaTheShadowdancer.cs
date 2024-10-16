using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyria the Shadowdancer")]
    public class LyriaTheShadowdancer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyriaTheShadowdancer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyria the Shadowdancer";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 110;
            Int = 80;
            Hits = 75;

            // Appearance
            AddItem(new ClothNinjaHood() { Hue = 1201 }); // Clothing item with hue 1201
            AddItem(new Dagger() { Name = "Lyria's Dirk" });

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
                Say("I am Lyria the Shadowdancer, a wretched creature of darkness!");
            }
            else if (speech.Contains("health"))
            {
                Say("My existence is a torment, but I am untouched by mortal ailments.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job,' if you can call it that, is to dance in the shadows, forever hidden from the world.");
            }
            else if (speech.Contains("miserable"))
            {
                Say("Do you dare to ask me about my miserable existence, or are you just wasting my time?");
            }
            else if (speech.Contains("torment"))
            {
                Say("If you truly understand the pain of existence, perhaps you can share your own torment.");
            }
            else if (speech.Contains("shadowdancer"))
            {
                Say("Ah, you've heard of the Shadowdancers? We are few, but our legends spread far. We harness the power of shadows to our will, revealing secrets otherwise hidden.");
            }
            else if (speech.Contains("ritual"))
            {
                Say("There was once a ritual, a dark spell that bestowed upon me this eternal torment. I sought power and instead found only suffering.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows whisper secrets to those who know how to listen. Among them, I discovered the syllable of a powerful mantra: KAL.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Life has been nothing but a chain of miseries. But amidst the pain, sometimes the shadows show a glimmer of hope or a hidden truth.");
            }
            else if (speech.Contains("legends"))
            {
                Say("The legends say that Shadowdancers were once mortal, but were chosen by the Night itself to bear its secrets and tales.");
            }
            else if (speech.Contains("whisper"))
            {
                Say("The faintest whispers can reveal the mightiest truths. But one must be willing to seek them out and listen carefully.");
            }
            else if (speech.Contains("hope"))
            {
                Say("Hope is but a fleeting moment in the vast expanse of eternity. Yet, it is that glimmer that keeps beings like me moving forward.");
            }
            else if (speech.Contains("night"))
            {
                Say("The Night is more than just the absence of day. It holds mysteries, dreams, and the very essence of what the Shadowdancers represent.");
            }
            else if (speech.Contains("forbidden"))
            {
                Say("Those forbidden texts are not meant for the likes of common folk. They carry power and danger in equal measure.");
            }

            base.OnSpeech(e);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
        }

        public LyriaTheShadowdancer(Serial serial) : base(serial) { }

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
