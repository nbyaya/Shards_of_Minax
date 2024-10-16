using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Prof. Curie")]
    public class ProfCurie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfCurie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Prof. Curie";
            Body = 0x191; // Human female body

            // Stats
            Str = 65;
            Dex = 60;
            Int = 130;
            Hits = 55;

            // Appearance
            AddItem(new Skirt() { Hue = 1905 });
            AddItem(new Shirt() { Hue = 1904 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new Bonnet() { Hue = 1905 });
            AddItem(new MortarPestle() { Name = "Prof. Curie's Experiments" });

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
                Say("I am Prof. Curie, the brilliant scientist, forced to toil in this wretched place!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is irrelevant, but this place has left me broken.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm a brilliant scientist, cursed to waste away in obscurity.");
            }
            else if (speech.Contains("torment"))
            {
                Say("Do you think you understand the torment of a mind like mine?");
            }
            else if (speech.Contains("escape"))
            {
                Say("If you truly understand, then tell me, what would you do to escape this wretched existence?");
            }
            else if (speech.Contains("brilliant"))
            {
                Say("Ah, you recognize my genius! I have made several groundbreaking discoveries, but the world is yet to know!");
            }
            else if (speech.Contains("broken"))
            {
                Say("Not just in spirit, but also from the experiments I conducted on myself. I was on the brink of a great revelation.");
            }
            else if (speech.Contains("obscurity"))
            {
                Say("In my prime, I was the talk of the town. Now, I'm forgotten. But, there's one experiment I never completed...");
            }
            else if (speech.Contains("mind"))
            {
                Say("A mind that constantly seeks knowledge and the truth, yet is surrounded by ignorance and darkness. I just need the right tools to complete my work.");
            }
            else if (speech.Contains("existence"))
            {
                Say("If you can bring me the rare ingredients I need for my experiment, I might reward you for freeing me from this mundane existence.");
            }
            else if (speech.Contains("discoveries"))
            {
                Say("My discoveries range from the elixir of life to the power of manipulating time. But alas, they are stored in my lost journals.");
            }
            else if (speech.Contains("experiments"))
            {
                Say("I once experimented with merging magic and science. The results were... unpredictable. Yet, I believe I was close to perfection.");
            }
            else if (speech.Contains("experiment"))
            {
                Say("Ah! My ultimate experiment was to create a portal to another dimension. But, the components are scattered across the land.");
            }
            else if (speech.Contains("tools"))
            {
                Say("My most important tool was the Crystal of Foresight. It was stolen from me. Retrieve it, and I might bestow a reward upon you.");
            }
            else if (speech.Contains("ingredients"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("I need the Feather of a Phoenix, the Heart of a Gorgon, and the Tear of a Siren. Bring them to me, and you will be handsomely rewarded. A sample for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ProfCurie(Serial serial) : base(serial) { }

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
