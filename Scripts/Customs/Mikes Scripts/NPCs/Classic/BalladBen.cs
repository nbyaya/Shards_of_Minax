using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ballad Ben")]
    public class BalladBen : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BalladBen() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ballad Ben";
            Body = 0x190; // Human male body

            // Stats
            Str = 117;
            Dex = 69;
            Int = 83;
            Hits = 88;

            // Appearance
            AddItem(new Doublet(45)); // Doublet with hue 45
            AddItem(new Kilt(88)); // Kilt with hue 88
            AddItem(new Sandals(1176)); // Sandals with hue 1176
            AddItem(new LeatherGloves() { Name = "Ben's Ballad Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
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
                Say("I am Ballad Ben, the bard of bitter tunes!");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Hah! Who cares about such trivial matters!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I strum tunes of sorrow and despair, of course!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor? You jest! Art thou as bitter as I?");
            }
            else if (speech.Contains("yes") && speech.Contains("bitter tale"))
            {
                Say("Then share with me a bitter tale of woe, and we shall revel in our shared misery!");
            }
            else if (speech.Contains("ballad"))
            {
                Say("Ah, my parents hoped I'd sing songs of joy, but fate had different plans. Now I sing ballads that echo the shadows of the heart.");
            }
            else if (speech.Contains("trivial"))
            {
                Say("What's the point of good health when the world is filled with so much darkness and woe?");
            }
            else if (speech.Contains("sorrow"))
            {
                Say("Sorrow is the ink with which I write my songs. Have you ever felt its cold embrace?");
            }
            else if (speech.Contains("fate"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, fortune. It can be as fleeting as a sunset or as enduring as the mountains. But its true value is in how we use it. Do you believe in fate's hand guiding your path?");
                    from.AddToBackpack(new CapacityIncreaseDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("darkness"))
            {
                Say("Darkness isn't just the absence of light. It's the weight one feels when hope seems lost. But sometimes, in the deepest darkness, a glimmer of hope can shine the brightest.");
            }
            else if (speech.Contains("embrace"))
            {
                Say("The embrace of sorrow can be chilling. But like a blanket, it also wraps around you, becoming a familiar companion. Do you seek solace in music too?");
            }
            else if (speech.Contains("path"))
            {
                Say("Ah, fortune. It can be as fleeting as a sunset or as enduring as the mountains. But its true value is in how we use it. Do you believe in fate's hand guiding your path?");
            }
            else if (speech.Contains("hope"))
            {
                Say("Hope is the light that guides us through the darkest tunnels. Even I, in my most somber moments, cling to it. Have you found hope in unexpected places?");
            }
            else if (speech.Contains("music"))
            {
                Say("Music is the voice of the soul. Even in despair, it offers a bridge to better moments. Would you like to hear a tune?");
            }
            else if (speech.Contains("regret"))
            {
                Say("Every path we tread is paved with choices. Some lead to joy, others to sorrow. Tell me, traveler, have your choices led you to regret?");
            }

            base.OnSpeech(e);
        }

        public BalladBen(Serial serial) : base(serial) { }

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
