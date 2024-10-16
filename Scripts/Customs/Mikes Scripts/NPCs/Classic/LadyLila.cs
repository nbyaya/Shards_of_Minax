using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Lila")]
    public class LadyLila : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyLila() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Lila";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 60;
            Int = 80;
            Hits = 50;

            // Appearance
            AddItem(new FancyDress() { Hue = 2128 }); // Dress
            AddItem(new Boots() { Hue = 2128 }); // Boots
            AddItem(new Cap() { Hue = 2128 }); // Bonnet
            AddItem(new MortarPestle() { Name = "Lila's Mortar and Pestle" }); // Mortar and Pestle

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Lady Lila, a forsaken soul.");
            }
            else if (speech.Contains("health"))
            {
                Say("My existence is a never-ending torment.");
            }
            else if (speech.Contains("job"))
            {
                Say("I dwell in this forsaken land, bound to serve.");
            }
            else if (speech.Contains("soul"))
            {
                Say("Is your heart as heavy as mine?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your words resonate with my despair.");
            }
            else if (speech.Contains("forsaken"))
            {
                Say("Long ago, I was a noble in a grand kingdom, but now that realm is lost and I remain here, trapped in time.");
            }
            else if (speech.Contains("torment"))
            {
                Say("An old curse binds me to this place, a spell wrought by envy and malice. Unless the curse is lifted, my suffering will never cease.");
            }
            else if (speech.Contains("serve"))
            {
                Say("I serve the spirits that reside here, doing their bidding and longing for release. They hold power over me, but perhaps someone strong enough could free me.");
            }
            else if (speech.Contains("despair"))
            {
                Say("In moments of profound despair, a glimmer of hope sometimes shines through. There's an ancient relic that might hold the key to my salvation.");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("The kingdom I hailed from was known as Eldoria. It was a place of beauty and wonder, now buried beneath the sands of time.");
            }
            else if (speech.Contains("curse"))
            {
                Say("The curse was cast upon me by a rival witch, filled with jealousy. She used a dark amulet to seal my fate. Find that amulet, and you might be able to help me.");
            }
            else if (speech.Contains("spirits"))
            {
                Say("These spirits were once rulers of the land, but their greed and pride led them astray. They're bound here as I am, though they revel in their power over the lost souls like me.");
            }
            else if (speech.Contains("relic"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The relic is known as the Tear of Lila. Legend has it that if it's restored to its rightful place, it has the power to break the strongest of curses. If you can find and return it, I'll reward you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LadyLila(Serial serial) : base(serial) { }

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
