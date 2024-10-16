using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kael Thunderarrow")]
    public class KaelThunderarrow : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KaelThunderarrow() : base(AIType.AI_Archer, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kael Thunderarrow";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 90;
            Int = 60;
            Hits = 120;

            // Appearance
            AddItem(new LongPants() { Hue = 1157 });
            AddItem(new Tunic() { Hue = 1108 });
            AddItem(new Boots() { Hue = 1106 });
            AddItem(new Crossbow() { Name = "Kael's Crossbow" });

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
                Say("I am Kael Thunderarrow, the archer of virtue!");
            }
            else if (speech.Contains("health"))
            {
                Say("I have always maintained good health.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to protect the virtues with my keen archery skills.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtue of Humility is often overlooked, but it's the foundation of all virtues. Do you agree?");
            }
            else if (speech.Contains("yes") && (speech.Contains("virtues") || speech.Contains("humility")))
            {
                Say("Your response is wise. Humility is the first step toward understanding the other virtues.");
            }
            else if (speech.Contains("archer"))
            {
                Say("Archery requires patience, focus, and humility. My arrows always find their mark, especially when defending the virtues.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is not merely waiting, it's about how we behave while we're waiting. It's also an essential aspect of mastering any skill. Do you value patience?");
            }
            else if (speech.Contains("yes") && speech.Contains("patience"))
            {
                Say("It's a virtue that many overlook. True patience brings peace and understanding, and can reveal the secrets of the virtues.");
            }
            else if (speech.Contains("focus"))
            {
                Say("With focus, one can aim true and never miss the target. Even beyond archery, it helps us concentrate on our goals in life. Have you ever lost focus?");
            }
            else if (speech.Contains("no") && speech.Contains("focus"))
            {
                Say("Staying true to one's path is commendable. Always hold onto that focus, for it will guide you in the darkest times.");
            }
            else if (speech.Contains("humility"))
            {
                Say("Ah, humility. It is said that the mantra of Compassion consists of three syllables. I know the third one. Do you wish to know it?");
            }
            else if (speech.Contains("mantra"))
            {
                Say("The third syllable of the mantra of Compassion is MUH. Use it wisely and with a pure heart.");
            }
            else if (speech.Contains("virtues") && speech.Contains("compassion"))
            {
                Say("The Eight Virtues are a guide to a righteous life. Among them, Compassion has always spoken to me. Would you like to hear about it?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is understanding and caring for the pain and joy of others. It's what binds us all in this vast world. Always extend a helping hand, for the ripple of one act can touch countless souls.");
            }
            else if (speech.Contains("souls"))
            {
                Say("Every soul carries the potential to embrace the virtues. It's our choices that define our path. Choose wisely, for the virtues are always watching.");
            }
            else if (speech.Contains("virtues"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public KaelThunderarrow(Serial serial) : base(serial) { }

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
