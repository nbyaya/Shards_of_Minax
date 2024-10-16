using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kageyoshi Matsumura")]
    public class KageyoshiMatsumura : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KageyoshiMatsumura() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kageyoshi Matsumura";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            AddItem(new Katana() { Hue = 1157, Name = "Kageyoshi's Blade" });

            Hue = Utility.RandomSkinHue(); // Random skin hue
            HairItemID = 0x2040; // Black hair
            HairHue = 0x0; // Black hair color
            FacialHairItemID = 0x203C; // Beard

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
                Say("I am Kageyoshi Matsumura, guardian of honor and tradition.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the essence of the samurai. It guides our actions and decisions.");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of honor is reflected in our daily conduct. We must live by the code of Bushido.");
            }
            else if (speech.Contains("bushido"))
            {
                Say("Bushido, the way of the warrior, emphasizes virtues such as courage, integrity, and respect.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues are the cornerstones of our character. They shape our interactions and our path in life.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is not the absence of fear, but the ability to face it. It is a key virtue in Bushido.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the quality of being honest and having strong moral principles. It is central to the samurai code.");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect is fundamental in our interactions. It is given to others and earned through our actions.");
            }
            else if (speech.Contains("actions"))
            {
                Say("Our actions reflect our true selves. Through honorable actions, we gain respect and trust.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is built through consistent and honorable actions. It is essential for meaningful relationships.");
            }
            else if (speech.Contains("relationships"))
            {
                Say("Meaningful relationships are based on mutual respect and understanding. They are a source of strength.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength comes from within and from the support of others. It is both physical and spiritual.");
            }
            else if (speech.Contains("spiritual"))
            {
                Say("The spiritual aspect of strength is nurtured through meditation and reflection on the samurai code.");
            }
            else if (speech.Contains("meditation"))
            {
                Say("Meditation helps to center the mind and body. It is a practice that enhances our understanding of honor.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding the principles of honor requires deep reflection and commitment. It is a lifelong journey.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment to the samurai code demands dedication and perseverance. It is not easily attained.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the ability to keep going despite difficulties. It is a virtue that strengthens our resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the firmness of purpose. It allows us to overcome obstacles and stay true to our path.");
            }
            else if (speech.Contains("path"))
            {
                Say("Our path is guided by the virtues of Bushido. Staying true to this path brings honor and fulfillment.");
            }
            else if (speech.Contains("fulfillment"))
            {
                Say("Fulfillment comes from living a life of honor and adhering to the principles of Bushido. It is a reward in itself.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this time. Return when the time is right.");
                }
                else
                {
                    Say("For your understanding and dedication to the samurai code, accept this Samurai's Honor chest as a token of respect.");
                    from.AddToBackpack(new SamuraiHonorChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public KageyoshiMatsumura(Serial serial) : base(serial) { }

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
