using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Demetrius")]
    public class CaptainDemetrius : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainDemetrius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Demetrius";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 100;
            Int = 70;
            Hits = 80;

            // Appearance
            AddItem(new LongPants() { Hue = 1152 });
            AddItem(new FancyShirt() { Hue = 1152 });
            AddItem(new TricorneHat() { Hue = 1152 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Cutlass() { Name = "Demetrius' Cutlass" });

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
                Say("What do you want, stranger?");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care? My health is none of your business.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Hah! I'm a glorified babysitter for a bunch of incompetent soldiers.");
            }
            else if (speech.Contains("leadership"))
            {
                Say("Do you know anything about real leadership?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Real leaders don't waste their time talking to strangers like you.");
            }
            else if (speech.Contains("simple adventurer"))
            {
                Say("Of course, you wouldn't understand. You're just a simple adventurer.");
            }
            else if (speech.Contains("demetrius"))
            {
                Say("Captain Demetrius is the name. Many have heard of me, but few know my true story.");
            }
            else if (speech.Contains("care"))
            {
                Say("While you may think I'm just grumbling, the life of a Captain isn't as glorious as it seems. The scars run deeper than the skin.");
            }
            else if (speech.Contains("babysitter"))
            {
                Say("It's not just about handling soldiers. It's about protecting this city, its people, and its secrets.");
            }
            else if (speech.Contains("true"))
            {
                Say("I once was a mere soldier myself, climbing up the ranks. Then, a fateful event at Frostpeak changed everything.");
            }
            else if (speech.Contains("scars"))
            {
                Say("These scars are reminders of the battles I've fought, both external and internal. Some wounds, you see, never truly heal.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, curious are you? The city has many tales, but there's one artifact, the Emerald Compass, that very few speak of.");
            }
            else if (speech.Contains("frostpeak"))
            {
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    Say("For my bravery at Frostpeak, I was given this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("I have no reward right now. Please return later.");
                }
            }
            else if (speech.Contains("wounds"))
            {
                Say("While the physical wounds are evident, it's the emotional wounds that weigh the heaviest. I lost someone dear to me at Gloomwood.");
            }
            else if (speech.Contains("compass"))
            {
                Say("Legend says that this compass points to a treasure beyond imagination. But many have sought it and failed.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("True bravery isn't about being fearless. It's about facing your fears for the sake of others. Remember that, adventurer.");
            }

            base.OnSpeech(e);
        }

        public CaptainDemetrius(Serial serial) : base(serial) { }

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
