using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Morgan Le Fay")]
    public class MorganLeFay : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MorganLeFay() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Morgan Le Fay";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 50;
            Int = 110;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 1155 });
            AddItem(new Boots() { Hue = 1155 });
            AddItem(new WizardsHat() { Hue = 1155 });
            AddItem(new Spellbook() { Name = "Morgan's Grimoire" });

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
                Say("I am Morgan Le Fay, the witch of these woods.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as resilient as the forest itself.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a keeper of ancient knowledge and a practitioner of arcane arts.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues, those guiding lights, they shape our destinies, don't they?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, they are the threads weaving the tapestry of our lives.");
            }
            else if (speech.Contains("woods"))
            {
                Say("The woods are ancient and full of secrets. Many seek its knowledge, few return enlightened.");
            }
            else if (speech.Contains("resilient"))
            {
                Say("Resilience is more than mere physical strength. It's the spirit's ability to endure and grow from challenges.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("Arcane arts are not merely spells. They are a deep connection to the world's energies, a bond few truly understand.");
            }
            else if (speech.Contains("enlightened"))
            {
                Say("To be enlightened is to see beyond the mundane, to grasp the threads of fate and weave one's destiny.");
            }
            else if (speech.Contains("endure"))
            {
                Say("Endurance is a test. Many face it daily. But I have seen spirits shatter and others reborn from trials.");
            }
            else if (speech.Contains("weave"))
            {
                Say("I once tried to weave my own fate, to change my destiny. For my hubris, I was given a gift and a curse.");
            }
            else if (speech.Contains("shatter"))
            {
                Say("Not all that is shattered can be mended. Yet, for those who persevere, rewards await.");
            }
            else if (speech.Contains("consumed"))
            {
                Say("I've seen many consumed by their desires, lost in the abyss of their own making.");
            }
            else if (speech.Contains("curse"))
            {
                Say("Ah, the curse. For one who proves themselves worthy, I might share its tale and perhaps offer a token of my esteem.");
            }
            else if (speech.Contains("persevere"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To persevere is to prove oneself. For such tenacity, I shall grant you this reward. Use it wisely.");
                    from.AddToBackpack(new TinkeringAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MorganLeFay(Serial serial) : base(serial) { }

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
