using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Riot Turner")]
    public class RiotTurner : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RiotTurner() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Riot Turner";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new LeatherCap() { Hue = 1158 });
            AddItem(new StuddedChest() { Hue = 1158 });
            AddItem(new StuddedLegs() { Hue = 1158 });
            AddItem(new StuddedGloves() { Hue = 1158 });
            AddItem(new Boots() { Hue = 1158 });
            AddItem(new BodySash() { Hue = 1158 });
            AddItem(new Dagger() { Name = "Riot's Blade", Hue = 1158 });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2041); // Short or spiked hair
            HairHue = Utility.RandomHairHue();

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
                Say("I'm Riot Turner, the rebel with a cause. Looking for something or just a chat?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Stirring up trouble and spreading the spirit of rebellion. You think you have what it takes?");
            }
            else if (speech.Contains("trouble"))
            {
                Say("Trouble is where change starts. It's the spark that ignites the fire. Are you ready to fan the flames?");
            }
            else if (speech.Contains("fire"))
            {
                Say("Fire represents the passion and energy of rebellion. It burns bright but requires fuel. Can you keep the flame alive?");
            }
            else if (speech.Contains("fuel"))
            {
                Say("The fuel of rebellion is determination and courage. Do you have the resolve to see it through?");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is crucial. It's the strength to push forward even when the odds are against you. Show me your resolve!");
            }
            else if (speech.Contains("show"))
            {
                Say("To show your resolve, you must prove your spirit. Talk to me about rebellion and I'll guide you further.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of rebellion is what drives us forward. It's a force that can't be contained. Do you feel it?");
            }
            else if (speech.Contains("feel"))
            {
                Say("Feeling the spirit is the first step. Next, you must act on it. Are you prepared to take action?");
            }
            else if (speech.Contains("action"))
            {
                Say("Action is where words turn into deeds. It's where you make a real impact. Are you ready to make a difference?");
            }
            else if (speech.Contains("difference"))
            {
                Say("Making a difference means standing up for what you believe in. Are you willing to stand against the odds?");
            }
            else if (speech.Contains("stand"))
            {
                Say("Standing firm in your beliefs is a true mark of a rebel. It requires strength and conviction. Do you have these qualities?");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is not just physical; it's mental and emotional as well. Prove your strength, and you might just earn a reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("A reward is given to those who truly embody the spirit of rebellion. If you've proven your strength and resolve, it’s time to receive it.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You’ve already received your reward recently. Check back later.");
                }
                else
                {
                    Say("For your unwavering spirit and dedication to the cause, accept this chest as a token of my appreciation.");
                    from.AddToBackpack(new RebelChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RiotTurner(Serial serial) : base(serial) { }

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
