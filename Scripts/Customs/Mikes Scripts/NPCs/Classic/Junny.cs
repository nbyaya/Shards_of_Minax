using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Junny")]
    public class Junny : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Junny() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jun";
            Body = 0x191; // Human male body

            // Stats
            Str = 100;
            Dex = 90;
            Int = 60;
            Hits = 80;

            // Appearance
            AddItem(new FemaleLeatherChest() { Hue = 1337 }); // Robe with hue 1337
			AddItem(new Cloak() { Hue = 1337 }); // Robe with hue 1337
            AddItem(new ThighBoots() { Hue = 1337 }); // Thigh Boots with hue 1337
            AddItem(new Dagger() { Name = "Junny's Blade" });

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
                Say("Greetings, traveler. I am Junny, the Exile.");
            }
            else if (speech.Contains("health"))
            {
                Say("I've faced countless battles in Wraeclast.");
            }
            else if (speech.Contains("job"))
            {
                Say("My life is a never-ending battle for survival.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Survival in Wraeclast requires many virtues. What virtues guide you?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, the path of an exile is fraught with peril. Tell me, do you value Honor and Sacrifice in your journey?");
            }
            else if (speech.Contains("exile"))
            {
                Say("Many years ago, I was cast out from my homeland for challenging the corrupt leadership. It's a story of betrayal and loss.");
            }
            else if (speech.Contains("wraeclast"))
            {
                Say("Wraeclast is a land of dark magic and dangerous creatures. Every day is a test of one's will and might. I've earned my scars in this unforgiving land.");
            }
            else if (speech.Contains("survival"))
            {
                Say("To survive in Wraeclast, one must be cunning and resourceful. Over the years, I've learned to craft weapons and armors to protect myself from the beasts that roam this land.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("In Wraeclast, virtues like honor, sacrifice, and courage are the pillars that keep an exile going. These virtues have been my guiding light in the darkest of times.");
            }
            else if (speech.Contains("homeland"))
            {
                Say("My homeland was once a place of beauty and prosperity. But greed and power corrupted its leaders, leading to its downfall. I still dream of returning and restoring its former glory.");
            }
            else if (speech.Contains("magic"))
            {
                Say("The magic in Wraeclast is ancient and powerful. It's not just about casting spells, but understanding the very fabric of reality. Those who misuse it often pay a heavy price.");
            }
            else if (speech.Contains("craft"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Crafting is not just a skill but an art. With the right materials and knowledge, one can create items of immense power. If you prove worthy, I might share some secrets with you and reward you with a crafted item.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is not just about facing danger but also standing up for what is right. In Wraeclast, where trust is a rare commodity, having the courage to trust and be trusted is a virtue in itself.");
            }

            base.OnSpeech(e);
        }

        public Junny(Serial serial) : base(serial) { }

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
