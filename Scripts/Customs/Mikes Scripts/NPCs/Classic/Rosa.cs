using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rosa")]
    public class Rosa : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Rosa() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rosa";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Cloak() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new Bonnet() { Hue = 1150 });
            AddItem(new Crossbow() { Name = "Rosa's Crossbow" });

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
                Say("I am Rosa, the sarcastic one. What do you want, hero?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Ha! I'm as healthy as you can expect in this dump.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Oh, I'm just a shining example of wasted talent.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor? In this place? You must be joking.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you think you're valiant enough for me, hero?");
            }
            else if (speech.Contains("rosa"))
            {
                Say("Ah, Rosa... a name I've grown to detest in a place like this.");
            }
            else if (speech.Contains("dump"))
            {
                Say("This 'dump' you refer to was once a beautiful village, before all the chaos descended upon us.");
            }
            else if (speech.Contains("talent"))
            {
                Say("Yes, I used to be an artist, capturing the beauty of our world on canvas. Now? Nothing seems worth painting.");
            }
            else if (speech.Contains("place"))
            {
                Say("Once a thriving town, now it's merely a shadow of its former self, thanks to the curse.");
            }
            else if (speech.Contains("chaos"))
            {
                Say("Creatures of darkness roamed, and people I once knew changed or disappeared. It's heartbreaking.");
            }
            else if (speech.Contains("artist"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("If you ever come across any inspiring sights in your adventures, bring them to me. Maybe, just maybe, I'll find the will to paint again.");
                    from.AddToBackpack(new MaxxiaScroll()); // Example reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("curse"))
            {
                Say("Many believe it's tied to the old ruins in the mountains. No one dares to venture there anymore.");
            }
            else if (speech.Contains("creatures"))
            {
                Say("Some say they are guarding a treasure, but who can say for sure? I've seen heroes go and never return.");
            }
            else if (speech.Contains("ruins"))
            {
                Say("Legend speaks of a guardian spirit that watches over those ruins. But like I said, just legends.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("They say it's an artifact with powers beyond imagination. But at what cost to retrieve it?");
            }
            else if (speech.Contains("guardian"))
            {
                Say("If you ever decide to confront this guardian, be prepared. Legends often have roots in reality.");
            }

            base.OnSpeech(e);
        }

        public Rosa(Serial serial) : base(serial) { }

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
