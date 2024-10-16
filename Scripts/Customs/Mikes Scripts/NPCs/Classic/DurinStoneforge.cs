using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Durin Stoneforge")]
    public class DurinStoneforge : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DurinStoneforge() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Durin Stoneforge";
            Body = 0x190; // Dwarf body
            Hue = 2406; // Default skin hue

            // Stats
            Str = 130;
            Dex = 60;
            Int = 50;
            Hits = 90;

            // Appearance
            AddItem(new RingmailLegs() { Hue = 2412 });
            AddItem(new RingmailChest() { Hue = 2412 });
            AddItem(new RingmailGloves() { Hue = 2412 });
            AddItem(new ChainCoif() { Hue = 2412 });
            AddItem(new WarHammer() { Name = "Durin's Hammer" });

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
                Say("I am Durin Stoneforge, the keeper of ancient secrets!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as resilient as the mountains!");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a craftsman of the earth, shaping stone and metal into works of art.");
            }
            else if (speech.Contains("secrets") || speech.Contains("knowledge"))
            {
                Say("But in the depths of the earth, wisdom lies hidden. Do you seek knowledge?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Then prove your worth. Answer me this riddle: 'I am not alive, but I can grow; I don't have lungs, but I need air; I don't have a mouth, but water kills me. What am I?'");
            }
            else if (speech.Contains("answer"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received your reward. Please return later.");
                }
                else
                {
                    Say("Ah, you have proven your wits. The answer is 'fire'. For your cleverness, I shall bestow upon you a gift.");
                    from.AddToBackpack(new ChestSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("durin"))
            {
                Say("Many have sought my expertise in the mysteries of the earth. Through the ages, I have been a guardian of hidden truths.");
            }
            else if (speech.Contains("mountains"))
            {
                Say("The mountains are not just my strength, but they are also my home. Their towering peaks and deep caverns hold many secrets.");
            }
            else if (speech.Contains("craftsman"))
            {
                Say("I have honed my skills for many a year, creating masterpieces that many desire. My favorite is a special pendant I crafted which is said to have mystical properties.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that banishes the shadows of ignorance. I have spent lifetimes gathering wisdom from both the living and the dead.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are more than just a lack of light. They are the embodiment of the unknown, the unexplored, and the forgotten. Many fear them, but they can also be a source of power for those who understand them.");
            }
            else if (speech.Contains("rituals"))
            {
                Say("Rituals connect us to the energies of the earth and the cosmos. Through them, we can harness power or seek answers to the mysteries of existence.");
            }

            base.OnSpeech(e);
        }

        public DurinStoneforge(Serial serial) : base(serial) { }

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
