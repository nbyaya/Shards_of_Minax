using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Deckard Cain")]
    public class DeckardCain : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DeckardCain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Deckard Cain";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new Robe(1102));
            AddItem(new Sandals(1102));
            AddItem(new GnarledStaff { Name = "Deckard Cain's Staff" });

            Hue = Race.RandomSkinHue();
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
                Say("Greetings, traveler! I am Deckard Cain, the last of the Horadrim.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is unimportant, for my mind is as sharp as ever!");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a scholar and an advisor, seeking knowledge and ancient lore.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Do you seek knowledge, adventurer? The path of wisdom is not for the faint of heart.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Ah, I sense a curious spirit in you. Tell me, do you believe that knowledge is power?");
            }
            else if (speech.Contains("path"))
            {
                Say("The path of wisdom often leads to sacrifice. For those who truly pursue it, I have a token of appreciation. May this aid you in your quest for knowledge.");

                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("power"))
            {
                Say("Indeed, knowledge is power. But power can be both a gift and a curse. It depends on how one wields it.");
            }
            else if (speech.Contains("evil"))
            {
                Say("The Prime Evils were the most powerful demons of the Burning Hells. The Horadrim's duty was to imprison them using the Soulstones.");
            }
            else if (speech.Contains("tomes"))
            {
                Say("My collection of tomes and scrolls is vast. I have gathered them from all corners of the world, seeking to preserve their knowledge.");
            }
            else if (speech.Contains("heroes"))
            {
                Say("Heroes come in many forms, but the truest heroes are those who fight not for glory, but for the greater good.");
            }
            else if (speech.Contains("token"))
            {
                Say("This token is a symbol of my appreciation. It has been passed down through generations of the Horadrim. Use it wisely.");
            }
            else if (speech.Contains("wields"))
            {
                Say("Power, when wielded with wisdom and restraint, can bring about great change. But when abused, it can lead to destruction.");
            }
            else if (speech.Contains("horadrim"))
            {
                Say("The Horadrim were ancient mages, dedicated to capturing the Prime Evils. I am the last of this once great order.");
            }
            else if (speech.Contains("sharp"))
            {
                Say("Despite my physical frailty, my mind remains keen, allowing me to recall and decipher the most complex of tomes and scrolls.");
            }
            else if (speech.Contains("advisor"))
            {
                Say("Throughout my life, I have advised many heroes and leaders. I have seen the rise and fall of empires, guiding those who seek to shape the world for the better.");
            }

            base.OnSpeech(e);
        }

        public DeckardCain(Serial serial) : base(serial) { }

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
