using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gralk the Stonehide")]
    public class GralkTheStonehide : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GralkTheStonehide() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gralk the Stonehide";
            Body = 0x190; // Human male body

            // Stats
            Str = 160;
            Dex = 70;
            Int = 40;
            Hits = 120;

            // Appearance
            AddItem(new BoneHelm() { Hue = 1167 });
            AddItem(new RingmailLegs() { Hue = 1166 });
            AddItem(new HammerPick() { Name = "Gralk's Maul" });

            // Set up NPC appearance
            Hue = 1165;
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
                Say("I am Gralk the Stonehide, born of the earth itself!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as unyielding as the rock, my health is never in question.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am the guardian of these stone lands, my job is to protect and preserve their ancient wisdom.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues of this world are like the facets of a precious gem, each reflecting the true nature of existence.");
            }
            else if (speech.Contains("seek") && speech.Contains("wisdom") && speech.Contains("virtues"))
            {
                Say("Do you seek the wisdom of the stones, young one? What virtues do you hold dear?");
            }
            else if (speech.Contains("rock"))
            {
                Say("Yes, just like the rock that weathers storms but still stands tall. My resilience is born from a deep bond with the lands I guard.");
            }
            else if (speech.Contains("protect"))
            {
                Say("I stand against those who would harm these lands. Many have tried, and many have met the unyielding might of the stone.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Existence in itself is a dance of balance. There is wisdom in understanding the virtues and how they shape the soul.");
            }
            else if (speech.Contains("dear"))
            {
                Say("Ah, the virtues one holds dear speak volumes of their character. If you prove worthy, I might grant you a boon from the stones.");
            }
            else if (speech.Contains("cycles"))
            {
                Say("These cycles have seen civilizations rise and fall. Every eon brings new lessons and challenges.");
            }
            else if (speech.Contains("resilience"))
            {
                Say("Resilience is not just about withstanding the forces, but learning and adapting. The stones teach me this every day.");
            }
            else if (speech.Contains("unyielding"))
            {
                Say("To be unyielding is to have a purpose that nothing can deter. For me, it is the safety of these lands.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is at the core of every virtue. It is the equilibrium that ensures harmony in the world.");
            }
            else if (speech.Contains("boon"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Very well, you have shown a deep interest in the wisdom of the stones. As a token of my appreciation, accept this gift from the earth itself.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("earth"))
            {
                Say("The earth's embrace nurtured me from a mere pebble to the guardian I am today. I have witnessed countless cycles and learned from the world's rhythms.");
            }

            base.OnSpeech(e);
        }

        public GralkTheStonehide(Serial serial) : base(serial) { }

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
