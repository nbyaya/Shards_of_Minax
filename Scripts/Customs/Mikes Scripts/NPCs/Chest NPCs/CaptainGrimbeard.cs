using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Grimbeard")]
    public class CaptainGrimbeard : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainGrimbeard() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Grimbeard";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new Bandana() { Hue = 0x21 }); // Pirate hat look
            AddItem(new LeatherChest() { Hue = 0x0A }); // Pirate's attire
            AddItem(new LeatherLegs() { Hue = 0x0A });
            AddItem(new LeatherGloves() { Hue = 0x0A });
            AddItem(new Sandals() { Hue = 0x0A });

            // Set hair and beard
            HairItemID = 0x203B; // Long hair
            HairHue = 0x47; // Dark color
            FacialHairItemID = 0x203D; // Beard
            FacialHairHue = 0x47; // Dark color

            // Speech Hue
            SpeechHue = 0x0; // Default speech hue

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
                Say("Ahoy, matey! I'm Captain Grimbeard, scourge of the Seven Seas!");
            }
            else if (speech.Contains("health"))
            {
                Say("Arr, I'm as hearty as a whale and twice as stubborn!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? To plunder and sail, of course! But I've settled here for now, guarding me treasure.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Aye, the treasure chest ye seek is guarded well. Prove your worth, and it shall be yours.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, answer me this: What is the essence of a pirate's life?");
            }
            else if (speech.Contains("essence") || speech.Contains("pirate"))
            {
                Say("The essence of a pirate's life be adventure and freedom. But there's more to be learned.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Adventure brings both peril and glory. Tell me, what is the greatest peril a pirate faces?");
            }
            else if (speech.Contains("peril"))
            {
                Say("The greatest peril a pirate faces be betrayal from within the crew. Can ye avoid such treachery?");
            }
            else if (speech.Contains("betrayal"))
            {
                Say("Avoiding betrayal requires vigilance and loyalty. Now tell me, what treasure lies beyond gold?");
            }
            else if (speech.Contains("treasure") && speech.Contains("beyond") || speech.Contains("gold"))
            {
                Say("The true treasure lies in the tales and legends of the sea. Do ye seek a legend or the chest?");
            }
            else if (speech.Contains("legend"))
            {
                Say("A legend speaks of a chest that holds the essence of a pirate's soul. Find this chest, and ye find true glory.");
            }
            else if (speech.Contains("glory"))
            {
                Say("Glory comes from daring deeds and unbreakable spirit. If ye have both, the chest shall be yours.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ye have unraveled the mystery and shown true pirate spirit. Here’s a treasure chest for ye, filled with riches and mystery.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I’ve no treasure for ye right now. Come back later.");
                }
                else
                {
                    from.AddToBackpack(new RiverPiratesChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I have no tales for ye right now. Ask me about the essence of a pirate's life or a legend of the sea.");
            }

            base.OnSpeech(e);
        }

        public CaptainGrimbeard(Serial serial) : base(serial) { }

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
