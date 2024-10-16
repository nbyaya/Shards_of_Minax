using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of King Arthur")]
    public class KingArthur : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KingArthur() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "King Arthur";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 70;
            Int = 100;
            Hits = 90;

            // Appearance
            AddItem(new PlateLegs() { Hue = 2213 });
            AddItem(new PlateChest() { Hue = 2213 });
            AddItem(new CloseHelm() { Hue = 2213 });
            AddItem(new PlateGloves() { Hue = 2213 });
            AddItem(new Boots() { Hue = 2213 });
            AddItem(new Longsword() { Name = "Excalibur" });

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
                Say("I am King Arthur, the noble ruler of this land.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks to the wisdom of Merlin.");
            }
            else if (speech.Contains("job"))
            {
                Say("My noble duty is to uphold the virtue of Justice in my kingdom.");
            }
            else if (speech.Contains("virtue justice"))
            {
                Say("Justice is the foundation of a fair and honorable society. Do you believe in justice?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, justice is the cornerstone of a virtuous society. It is our duty to uphold it.");
            }
            else if (speech.Contains("land"))
            {
                Say("The land I govern is vast and filled with mysteries. In our quests, we've discovered a sacred stone, known to many as Excalibur's resting place.");
            }
            else if (speech.Contains("merlin"))
            {
                Say("Ah, Merlin! The great wizard and my trusted advisor. His wisdom has not only preserved my health but has also guided our kingdom through its darkest days.");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("My kingdom stretches across the forests, mountains, and lakes. We've built the grand castle of Camelot as a testament to our strength and unity.");
            }
            else if (speech.Contains("excalibur"))
            {
                Say("Excalibur is not just a sword. It represents the heart and soul of this kingdom. Only the true ruler can wield it. If you prove your worth, perhaps I may reward you.");
            }
            else if (speech.Contains("advisor"))
            {
                Say("My advisors, led by Merlin, have been instrumental in shaping the destiny of our realm. They have provided counsel on matters of state, war, and peace. Would you like to learn about our latest challenges?");
            }
            else if (speech.Contains("camelot"))
            {
                Say("Camelot is the jewel of our land, a fortress of hope and a beacon of resilience. Within its walls lie tales of bravery, love, and betrayal. Some say the Holy Grail resides there.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your dedication to our kingdom's cause is commendable. As a token of appreciation, accept this gift from the royal treasury. Use it wisely.");
                    from.AddToBackpack(new ArmSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("challenges"))
            {
                Say("Our kingdom faces threats from both within and outside. From the treacherous Morgana to invaders from distant lands, we always remain vigilant and prepared.");
            }
            else if (speech.Contains("grail"))
            {
                Say("The Holy Grail, a relic of immense power and significance. Many have sought it, but its true location remains a mystery. Some believe it can grant eternal life.");
            }

            base.OnSpeech(e);
        }

        public KingArthur(Serial serial) : base(serial) { }

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
