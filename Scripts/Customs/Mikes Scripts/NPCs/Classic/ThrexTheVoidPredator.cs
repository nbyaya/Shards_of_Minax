using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Threx the Void Predator")]
    public class ThrexTheVoidPredator : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ThrexTheVoidPredator() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Threx the Void Predator";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 110;
            Int = 40;
            Hits = 110;

            // Appearance
            AddItem(new BoneChest() { Hue = 1182 });
            AddItem(new BoneGloves() { Hue = 1182 });
            AddItem(new BoneHelm() { Hue = 1181 });
            AddItem(new VikingSword() { Name = "Threx's Ripper" });

            Hue = 1180;
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            
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
                Say("I am Threx the Void Predator!");
            }
            else if (speech.Contains("health"))
            {
                Say("I exist beyond the realms of flesh and bone, my health is irrelevant.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a Void Predator, a cosmic sentinel observing the ebb and flow of the universe.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Within the void, battles are but ripples in the cosmic fabric. Art thou prepared for the unknown?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then venture forth, seeker of the void's secrets, and remember, the questions are as important as the answers.");
            }
            else if (speech.Contains("threx"))
            {
                Say("Ah, so you've heard of me before? The void speaks in many tongues and whispers tales to those who listen.");
            }
            else if (speech.Contains("realms"))
            {
                Say("These realms you know are but grains of sand in the vast desert of existence. Many dimensions overlap and intertwine in ways you cannot fathom.");
            }
            else if (speech.Contains("cosmic"))
            {
                Say("The vast cosmos is my canvas, and every star, every galaxy, has a story waiting to be told. Do you wish to hear a tale from the cosmos?");
            }
            else if (speech.Contains("void"))
            {
                Say("The void is not just emptiness; it is potential, a canvas for existence. Within it lie secrets that few dare to seek. For your bravery in seeking them out, accept this token from the void itself.");
            }
            else if (speech.Contains("token"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("For your bravery in seeking out the secrets of the void, accept this token.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("dimensions"))
            {
                Say("Each dimension vibrates at its own frequency, and with the right knowledge, one can travel between them. But beware, for not all dimensions are friendly to mortals.");
            }
            else if (speech.Contains("tale"))
            {
                Say("Very well. In the early days of the cosmos, when stars were young, a great entity emerged from the void, seeking to bring balance to the raging energies. This entity's legacy is still felt across the dimensions.");
            }
            else if (speech.Contains("travel"))
            {
                Say("Traveling between dimensions requires understanding of their essence and resonance. Tools and artifacts can aid in this, but the journey is fraught with peril.");
            }
            else if (speech.Contains("entity"))
            {
                Say("This entity, known by many names across the cosmos, weaves its influence subtly, guiding the flow of energies and destinies. Some revere it as a deity, others fear its unpredictable nature.");
            }

            base.OnSpeech(e);
        }

        public ThrexTheVoidPredator(Serial serial) : base(serial) { }

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
