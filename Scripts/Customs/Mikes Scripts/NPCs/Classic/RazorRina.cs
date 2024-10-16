using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Razor Rina")]
    public class RazorRina : BaseCreature
    {
        private DateTime lastLessonTime;
        private DateTime lastManualTime;

        [Constructable]
        public RazorRina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Razor Rina";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 90;

            // Appearance
            AddItem(new LeatherCap() { Name = "Rina's Razor Cap" });
            AddItem(new LeatherGloves() { Name = "Rina's Razor Gloves" });
            AddItem(new LeatherArms() { Name = "Rina's Razor Arms" });
            AddItem(new LeatherLegs() { Name = "Rina's Razor Legs" });
            AddItem(new LeatherChest() { Name = "Rina's Razor Chest" });
            AddItem(new Boots() { Name = "Rina's Razor Boots" });
            AddItem(new Kryss() { Name = "Rina's Razor Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the last reward times to a past time
            lastLessonTime = DateTime.MinValue;
            lastManualTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Razor Rina, the rogue.");
            }
            else if (speech.Contains("health"))
            {
                Say("I've had my fair share of close calls, but I'm still in one piece.");
            }
            else if (speech.Contains("job"))
            {
                Say("I make my living in the shadows, taking what others won't miss.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Life is a game of chance, my friend. What virtue guides your actions?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Ah, compassion. A rare trait in this world. Tell me, do you extend a helping hand to those in need?");
            }
            else if (speech.Contains("rogue"))
            {
                Say("Yes, as a rogue, I've mastered the art of stealth and subterfuge. Do you know about the 'Thieves Guild'?");
            }
            else if (speech.Contains("guild"))
            {
                Say("Ah, the Thieves Guild. A network of us rogues. We have our own code, you know. Ever heard of the 'Shadow Code'?");
            }
            else if (speech.Contains("code"))
            {
                Say("The Shadow Code is our oath, our creed. It speaks of loyalty, secrecy, and respect for the craft. Are you interested in learning the 'ways' of a rogue?");
            }
            else if (speech.Contains("ways"))
            {
                Say("The ways of a rogue are not for everyone. But if you're keen, I can teach you a trick or two. Would you like a 'lesson'?");
            }
            else if (speech.Contains("lesson"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastLessonTime < cooldown)
                {
                    Say("I have no lesson right now. Please return later.");
                }
                else
                {
                    Say("Alright then. Here's a simple lockpicking technique. And take this, it might help you practice.");
                    from.AddToBackpack(new LockpickingAugmentCrystal()); // Give the reward
                    lastLessonTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("story"))
            {
                Say("Once, I was infiltrating a noble's mansion. But little did I know, a mage was hired to set magical 'traps'. I barely escaped with my life.");
            }
            else if (speech.Contains("traps"))
            {
                Say("Magical traps can be quite deadly. Unlike physical ones, they attack the mind and soul. Always be wary in places with a strong 'aura'.");
            }
            else if (speech.Contains("aura"))
            {
                Say("An aura is the energy a place or person emits. Skilled rogues can sense it, helping them avoid danger. Would you like me to 'teach' you to feel it?");
            }
            else if (speech.Contains("teach"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastManualTime < cooldown)
                {
                    Say("I have no manual right now. Please return later.");
                }
                else
                {
                    Say("It's not something that can be taught in a day. But I can give you a token to start you off.");
                    from.AddToBackpack(new OneHandedTransformDeed()); // Give the reward
                    lastManualTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are a rogue's best friend. We move silently within them, unseen. Have you ever tried 'sneaking' around?");
            }
            else if (speech.Contains("sneaking"))
            {
                Say("It's an art, really. Requires patience and observation. Avoiding 'light' is key.");
            }
            else if (speech.Contains("light"))
            {
                Say("Bright lights reveal your position, but dim lights can hide you. It's all about positioning. If you're serious about learning, I have a 'manual' that could help.");
            }
            else if (speech.Contains("manual"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastManualTime < cooldown)
                {
                    Say("I have no manual right now. Please return later.");
                }
                else
                {
                    Say("Here it is, a guide to the basics of shadow art. May it serve you well.");
                    from.AddToBackpack(new StealthAugmentCrystal()); // Give the reward
                    lastManualTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RazorRina(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastLessonTime);
            writer.Write(lastManualTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastLessonTime = reader.ReadDateTime();
            lastManualTime = reader.ReadDateTime();
        }
    }
}
