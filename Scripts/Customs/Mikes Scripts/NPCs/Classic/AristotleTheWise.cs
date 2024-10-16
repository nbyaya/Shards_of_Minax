using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Aristotle the Wise")]
    public class AristotleTheWise : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AristotleTheWise() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Aristotle the Wise";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 120;
            Hits = 60;

            // Appearance
            AddItem(new Robe(2211));
            AddItem(new Sandals(1175));
            AddItem(new QuarterStaff() { Name = "Aristotle's Staff" });

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
                Say("Greetings, traveler! I am Aristotle the Wise, a seeker of knowledge.");
            }
            else if (speech.Contains("health"))
            {
                Say("My physical health is unimportant, for it is the health of the mind and spirit that truly matters.");
            }
            else if (speech.Contains("job"))
            {
                Say("My vocation, if you can call it that, is the pursuit of wisdom and the contemplation of life's mysteries.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Do you ponder the eight virtues, traveler? Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility?");
            }
            else if (speech.Contains("thoughts"))
            {
                Say("What are your thoughts on these virtues, and how do they shape your path?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the foundation of wisdom. I have spent many years collecting tales, stories, and lessons from far and wide. Would you be interested in hearing a story?");
            }
            else if (speech.Contains("mind"))
            {
                Say("The mind, when nourished and cared for, can be a beacon of light in the darkest of times. Have you ever practiced meditation to strengthen yours?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom comes from understanding oneself and the world around. There is a special tome I've been searching for that is said to hold unparalleled knowledge. If you find it, perhaps you could bring it to me?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the first step towards building trust. In my travels, I've met both honest and deceitful folk. Remember, an honest heart reveals a clear conscience.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is what binds us together. In showing kindness to even the smallest creature, we elevate our own spirit. Do you show compassion in your daily life?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is not just about bravery in battle, but also about standing up for what is right. I once knew a knight who embodied this virtue. Would you like to hear his tale?");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice ensures that balance is maintained in the world. But it's crucial to distinguish between vengeance and true justice. Have you encountered such dilemmas?");
            }
            else if (speech.Contains("sacrifice"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Sacrifice involves giving up something for the greater good. I've known many who have made such sacrifices. In fact, for those who truly understand its meaning, I have a token of appreciation.");
                    from.AddToBackpack(new MageryAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is an inner code that guides one's actions. It's not about accolades or recognition, but about doing what's right even when no one is watching. Do you live by such a code?");
            }
            else if (speech.Contains("spirituality"))
            {
                Say("Spirituality connects us to the cosmos and the divine. It is through this connection that we find purpose. Do you have a spiritual practice or belief?");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility keeps us grounded and reminds us that we are but a speck in the vast universe. It is this virtue that I admire most in individuals. Do you believe it's an important trait?");
            }
            else if (speech.Contains("tome"))
            {
                Say("Ah, the tome! It's called 'The Chronicles of the Ancients'. If you bring it to me, I will reward you for your efforts. A token for you.");
            }

            base.OnSpeech(e);
        }

        public AristotleTheWise(Serial serial) : base(serial) { }

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
