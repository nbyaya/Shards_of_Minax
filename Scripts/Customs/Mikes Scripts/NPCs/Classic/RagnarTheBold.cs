using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ragnar the Bold")]
    public class RagnarTheBold : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RagnarTheBold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ragnar the Bold";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 90;
            Int = 60;
            Hits = 90;

            // Appearance
            AddItem(new ChainChest() { Name = "Ragnar's Chain Tunic" });
            AddItem(new ChainLegs() { Name = "Ragnar's Chain Leggings" });
            AddItem(new ChainCoif() { Name = "Ragnar's Chain Coif" });
            AddItem(new PlateGloves() { Name = "Ragnar's Plate Gloves" });
            AddItem(new Boots() { Name = "Ragnar's Boots" });
            AddItem(new Crossbow() { Name = "Ragnar's Crossbow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("I am Ragnar the Bold, the master archer!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust as ever!");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an archer of great renown!");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor lies not in the strength of one's bow, but in the strength of character! Are you valiant?");
            }
            else if (speech.Contains("yes") && !speech.Contains("no")) // Responding to "yes" only
            {
                Say("Then, my friend, never let your resolve waver, even in the face of adversity!");
            }
            else if (speech.Contains("archer"))
            {
                Say("Many have sought to challenge me in archery, but few have succeeded. My skills have been honed over years of rigorous training and countless battles.");
            }
            else if (speech.Contains("robust"))
            {
                Say("Aye, I keep myself fit and ready. Every morning, I trek through the forest, practicing my shots and breathing in the fresh air. Nature keeps me strong.");
            }
            else if (speech.Contains("renown"))
            {
                Say("My reputation spreads far and wide. Travelers from distant lands come seeking my guidance in the art of archery. It's not just about shooting arrows; it's about understanding the rhythm of the bow and the dance of the wind.");
            }
            else if (speech.Contains("training"))
            {
                Say("Training for me is not just about perfecting my aim. It's a spiritual journey where I connect with my ancestors and the ancient archers of yore. They guide my hand and bless my arrows.");
            }
            else if (speech.Contains("forest"))
            {
                Say("The forest is my sanctuary. It's where I find peace and solace. The rustling leaves, the chirping birds, they all speak to me. And sometimes, when the world is quiet, I can hear the whispers of the spirits.");
            }
            else if (speech.Contains("guidance"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Many seek my guidance, and in return, I often set them on a quest to test their skills and determination. If you prove yourself worthy, I might just have a special reward for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("ancestors"))
            {
                Say("My ancestors were legendary archers, their tales passed down through generations. They fought valiantly, protecting our lands from invaders and monsters. Their spirits guide me, ensuring I uphold their legacy.");
            }
            else if (speech.Contains("spirits"))
            {
                Say("Spirits of the forest and the ancestors watch over me. They provide wisdom and insight, guiding my arrows and my path. To honor them, I often hold ceremonies, paying tribute to their eternal watch.");
            }

            base.OnSpeech(e);
        }

        public RagnarTheBold(Serial serial) : base(serial) { }

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
