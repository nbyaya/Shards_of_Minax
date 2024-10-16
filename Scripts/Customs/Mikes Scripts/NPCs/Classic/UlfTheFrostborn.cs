using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ulf the Frostborn")]
    public class UlfTheFrostborn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public UlfTheFrostborn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ulf the Frostborn";
            Body = 0x190; // Human male body

            // Stats
            Str = 140;
            Dex = 90;
            Int = 30;
            Hits = 100;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1154 });
            AddItem(new StuddedChest() { Hue = 1154 });
            AddItem(new StuddedGloves() { Hue = 1154 });
            AddItem(new VikingSword() { Name = "Ulf's Blade" });

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
                Say("Ulf the Frostborn, they call me.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Bah! A scratch!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Surviving in this wretched world!");
            }
            else if (speech.Contains("tougher"))
            {
                Say("Life here is harsh, stranger. Are you made of tougher stuff?");
            }
            else if (speech.Contains("battle"))
            {
                if (speech.Contains("greatest"))
                {
                    Say("If you're as tough as you claim, prove it! Tell me, stranger, what's your greatest battle?");
                }
                else
                {
                    Say("Impressive, stranger. For your bravery and tales, I grant you this token from my clan. May it serve you well.");
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
            }
            else if (speech.Contains("frostborn"))
            {
                Say("They named me Frostborn because I was discovered as a babe in the heart of a snowstorm, left untouched by its fury.");
            }
            else if (speech.Contains("scratch"))
            {
                Say("This scratch? It's from my last encounter with a dire wolf. Those beasts are everywhere in the icy north!");
            }
            else if (speech.Contains("surviving"))
            {
                Say("Surviving, yes, but not without purpose. I seek the ancient relics lost in the frozen wastelands to reclaim my clan's honor.");
            }
            else if (speech.Contains("tundras"))
            {
                Say("I've faced frozen tundras, wild beasts, and treacherous mountains. What have you faced, stranger?");
            }
            else if (speech.Contains("snowstorm"))
            {
                Say("That snowstorm was said to be the fiercest the north had ever seen. Many believe it was a sign from the gods.");
            }
            else if (speech.Contains("wolf"))
            {
                Say("Those dire wolves are a menace, but they also hold a deep connection to our land. Some say they guard the spirits of our ancestors.");
            }
            else if (speech.Contains("relics"))
            {
                Say("These relics are said to hold immense power. Many have tried to find them, but the icy wilderness is unforgiving.");
            }
            else if (speech.Contains("gods"))
            {
                Say("The gods of the north are old and powerful. We pay our respects to them through rituals and offerings.");
            }

            base.OnSpeech(e);
        }

        public UlfTheFrostborn(Serial serial) : base(serial) { }

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
