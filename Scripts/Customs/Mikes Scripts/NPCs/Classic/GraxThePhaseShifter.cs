using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grax the Phase Shifter")]
    public class GraxThePhaseShifter : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GraxThePhaseShifter() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grax the Phase Shifter";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 80;
            Int = 130;
            Hits = 95;

            // Appearance
            AddItem(new Tunic() { Hue = 1192 });
            AddItem(new LongPants() { Hue = 1191 });
            AddItem(new Dagger() { Name = "Grax's Quantum Blade" });
            
            Hue = 1190;
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
                Say("Grax the Phase Shifter, at your service...");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? I'm as healthy as one can be while shifting through dimensions...");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job'? I shift phases, unlike the mundane tasks most people have...");
            }
            else if (speech.Contains("phase shifting"))
            {
                Say("Do you think you understand the complexities of phase shifting?");
            }
            else if (speech.Contains("oblivion"))
            {
                Say("Ha! You know nothing of my world. Begone, before I shift you into oblivion!");
            }
            else if (speech.Contains("grax"))
            {
                Say("Ah, the name Grax. It was given to me by the elder shifters after my first successful transition.");
            }
            else if (speech.Contains("dimensions"))
            {
                Say("These dimensions are vast and numerous. Each has its own perils and wonders. I've explored many but not all.");
            }
            else if (speech.Contains("mundane"))
            {
                Say("The mundane tasks might seem insignificant to me, but they keep the world spinning. Every role is vital.");
            }
            else if (speech.Contains("complexities"))
            {
                Say("The complexities of phase shifting? It's a dance of energies and willpower. Few have the talent, and even fewer the dedication to master it.");
            }
            else if (speech.Contains("shift"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("It's not a threat, merely a caution. I've seen many who've dared to meddle in this art and ended up lost. Yet, if you show genuine interest, I might reward your curiosity.");
                }
                else
                {
                    Say("You've shown a genuine interest, unlike many before you. Here, take this. It might help you on your own adventures.");
                    from.AddToBackpack(new InscriptionAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("elder"))
            {
                Say("The elder shifters are the pioneers of our art. They've mapped the boundaries of known dimensions and passed down their wisdom.");
            }
            else if (speech.Contains("explored"))
            {
                Say("I've journeyed to realms of pure energy, lands frozen in time, and worlds that defy logic. Each journey changes you a little.");
            }
            else if (speech.Contains("role"))
            {
                Say("Every role in this universe has its importance. From the mightiest dragon to the humblest of craftsmen, all play their part.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication in phase shifting means endless study and practice. It's a devotion few can understand unless they experience it.");
            }

            base.OnSpeech(e);
        }

        public GraxThePhaseShifter(Serial serial) : base(serial) { }

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
