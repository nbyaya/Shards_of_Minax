using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gravemold the Shadow")]
    public class Gravemold : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Gravemold() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gravemold the Shadow";
            Body = 0x190; // Human male body

            // Stats
            Str = 135;
            Dex = 52;
            Int = 140;
            Hits = 112;

            // Appearance
            AddItem(new Robe() { Hue = 1 });
            AddItem(new ThighBoots() { Hue = 1109 });
            AddItem(new WizardsHat() { Hue = 1109 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am Gravemold the Shadow, master of the dark arts.");
            }
            else if (speech.Contains("health"))
            {
                Say("My existence is a curse, much like your mortal lives.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I manipulate the very forces of death.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Power over life and death. Do you dare to meddle with such forces?");
            }
            else if (speech.Contains("darkness"))
            {
                Say("So, you think yourself brave? Answer me this, mortal: Do you embrace the darkness or fear it?");
            }
            else if (speech.Contains("shadow"))
            {
                Say("The shadow is not just a name, but a reflection of my soul, cast in the eternal darkness.");
            }
            else if (speech.Contains("curse"))
            {
                Say("This curse was bestowed upon me after dabbling in forbidden magic. One must be wary of the costs of power.");
            }
            else if (speech.Contains("death"))
            {
                Say("Death is not the end, but a beginning. I've seen souls drift and wander, seeking purpose beyond the veil.");
            }
            else if (speech.Contains("power"))
            {
                Say("True power is not just wielding it, but understanding its essence. Many have fallen trying to grasp it without comprehension.");
            }
            else if (speech.Contains("embrace"))
            {
                Say("Those who embrace the darkness find treasures hidden from the world. Prove your worth, and I may reward you.");
            }
            else if (speech.Contains("worth"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("You have shown curiosity and courage. As a token of my acknowledgment, accept this gift from the shadows.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Gravemold(Serial serial) : base(serial) { }

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
