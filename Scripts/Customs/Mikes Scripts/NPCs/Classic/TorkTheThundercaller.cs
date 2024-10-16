using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tork the Thundercaller")]
    public class TorkTheThundercaller : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TorkTheThundercaller() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tork the Thundercaller";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 100;
            Int = 60;
            Hits = 100;

            // Appearance
            AddItem(new ChainCoif() { Hue = 1191 });
            AddItem(new WarFork() { Name = "Tork's Spear" });

            Hue = Race.RandomSkinHue();
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
                Say("I am Tork the Thundercaller, a child of storms!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, untouched by the elements.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a Thundercaller, bound to the storms. My job is to channel their power and protect our realm.");
            }
            else if (speech.Contains("battles"))
            {
                Say("The power of thunder and lightning is a force of great valor. Can you comprehend the might of storms?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then embrace the tempest's fury, for valor is found in the heart of the storm!");
            }
            else if (speech.Contains("storms"))
            {
                Say("Ah, storms! They are my brethren, swirling tempests that dance across the skies, echoing my call.");
            }
            else if (speech.Contains("elements"))
            {
                Say("The elements have always been my allies, guiding me and strengthening my resolve. They are a force few truly understand.");
            }
            else if (speech.Contains("protect"))
            {
                Say("Yes, I am tasked with safeguarding this realm against those who would misuse the raw energy of storms. Many seek its power, but few understand the responsibility.");
            }
            else if (speech.Contains("dance"))
            {
                Say("The dance of storms is a majestic spectacle, full of fury and beauty. If you ever witness it, consider yourself fortunate. Would you like a token to remember this by?");
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
                    Say("Here, take this amulet. It carries a fragment of a storm's might. Use it wisely and let it serve as a reminder of our conversation.");
                    from.AddToBackpack(new MaceFightingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is what keeps me grounded amidst the chaos of the storm. It's a blend of discipline, dedication, and understanding the true nature of my powers.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is the bridge between mere power and true mastery. It's the countless hours spent honing skills and understanding one's purpose.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment is a lifelong journey. It's not about the destination, but the path one takes and the choices made along the way.");
            }
            else if (speech.Contains("mastery"))
            {
                Say("Mastery is the pinnacle of understanding, where power and wisdom unite. It is an elusive state, always sought but rarely achieved.");
            }

            base.OnSpeech(e);
        }

        public TorkTheThundercaller(Serial serial) : base(serial) { }

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
