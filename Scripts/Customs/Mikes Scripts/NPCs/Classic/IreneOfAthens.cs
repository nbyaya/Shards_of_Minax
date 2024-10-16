using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Irene of Athens")]
    public class IreneOfAthens : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public IreneOfAthens() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Irene of Athens";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 60;
            Int = 80;
            Hits = 70;

            // Appearance
            AddItem(new FancyDress() { Hue = 0xB3D9 }); // Snow White Hue
            AddItem(new Boots() { Hue = 0xB3D9 }); // Snow White Hue
            AddItem(new GoldRing() { Name = "Irene of Athens's Ring" });

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
                Say("I am Irene of Athens, and my days in this wretched city are filled with misery and contempt.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as poor as the state of this wretched empire.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\" in this forsaken land? I am but a pawn in the games of power, nothing more.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is but a distant memory in this city. Are you any different?");
            }
            else if (speech.Contains("yes"))
            {
                Say("If you truly believe you can make a difference, then go ahead and try. But remember, this world is filled with deception and treachery.");
            }
            else if (speech.Contains("misery"))
            {
                Say("The weight of my past haunts me every day, casting shadows upon my soul.");
            }
            else if (speech.Contains("empire"))
            {
                Say("This empire, once filled with glory and splendor, has now crumbled under the weight of its own ambitions and treacheries.");
            }
            else if (speech.Contains("pawn"))
            {
                Say("The mighty and powerful play their games, using innocent souls like me as mere pieces on a chessboard. But even pawns can change the course of the game.");
            }
            else if (speech.Contains("past"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("In my youth, I was filled with dreams and aspirations, but the harsh realities of life in this city have withered them away. But for you, brave traveler, I have a small token from those happier times. May it aid you in your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("glory"))
            {
                Say("The days of glory, when honor and valor ruled, seem like distant memories now. Those were times when heroes were celebrated, and their deeds became legends.");
            }
            else if (speech.Contains("mighty"))
            {
                Say("Those who consider themselves mighty and unyielding often forget the simple truths of life. They believe their power is eternal, but everything fades with time.");
            }

            base.OnSpeech(e);
        }

        public IreneOfAthens(Serial serial) : base(serial) { }

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
