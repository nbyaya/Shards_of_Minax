using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Scarlet")]
    public class LadyScarlet : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyScarlet() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Scarlet";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 120;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1645 });
            AddItem(new StuddedChest() { Hue = 1645 });
            AddItem(new StuddedGorget() { Hue = 1645 });
            AddItem(new StuddedArms() { Hue = 1645 });
            AddItem(new Boots() { Hue = 1645 });
            AddItem(new Kryss() { Name = "Lady Scarlet's Dagger" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Lady Scarlet, an assassin by trade.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is none of your concern, but it's sufficient for my line of work.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I am an assassin, a master of shadows and death.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Oh, you want to know about battles, do you? Tell me, do you think one can truly find honor in killing for gold?");
            }
            else if (speech.Contains("honor"))
            {
                Say("Ha! You have a lot to learn, my naive friend. But let me ask you this: Have you ever taken a life for a purpose other than survival?");
            }
            else if (speech.Contains("scarlet"))
            {
                Say("Ah, you've heard of me then? Rumors tend to exaggerate, but there's always a grain of truth in them.");
            }
            else if (speech.Contains("sufficient"))
            {
                Say("Sufficient, yes, but it's not without its scars. Every mark on my body tells a story, a lesson learned.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows are both my ally and my cloak. They hide my presence and hold my secrets. Do you wish to know one?");
            }
            else if (speech.Contains("rumors"))
            {
                Say("They say I've killed kings and paupers alike. That I move like a ghost and strike without a sound. Some are true, some not. But they all serve a purpose.");
            }
            else if (speech.Contains("lesson"))
            {
                Say("Every lesson comes with a price. Sometimes it's pain, other times it's wisdom. But every scar has made me who I am today.");
            }
            else if (speech.Contains("secrets"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Very well, lean closer and I'll share one. But remember, knowledge can be a double-edged sword. Take this!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("kings"))
            {
                Say("Ah, kings. They live in luxury while their subjects suffer. Yet, they are just as mortal as any other when they meet my blade.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom is understanding the value of a life. Not every target needs to be killed; sometimes, there are other ways.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge has shaped my methods, guided my hand. In my line of work, knowing your enemy is half the battle.");
            }
            else if (speech.Contains("blade"))
            {
                Say("My blade has tasted the blood of many. Yet, it's not the weapon, but the hand that wields it which determines its course.");
            }
            else if (speech.Contains("target"))
            {
                Say("A target isn't just a mark to be eliminated. It's a puzzle, a challenge. And every challenge has its own solution.");
            }
            else if (speech.Contains("enemy"))
            {
                Say("Enemies are everywhere. But the most dangerous one is the one you don't see coming.");
            }

            base.OnSpeech(e);
        }

        public LadyScarlet(Serial serial) : base(serial) { }

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
