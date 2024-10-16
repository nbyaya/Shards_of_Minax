using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Darkshade the Cursed")]
    public class DarkshadeTheCursed : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DarkshadeTheCursed() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Darkshade the Cursed";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 50;
            Int = 150;
            Hits = 110;

            // Appearance
            AddItem(new Robe(1109)); // Robe with hue 1109
            AddItem(new Sandals(1157)); // Sandals with hue 1157
            AddItem(new SkullCap(1175)); // SkullCap with hue 1175

            Hue = Race.RandomSkinHue();
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
                Say("I am Darkshade the Cursed, a necromancer of great power!");
            }
            else if (speech.Contains("health"))
            {
                Say("My dark powers keep me in perfect health!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to manipulate the dark arts and control the undead!");
            }
            else if (speech.Contains("dark arts"))
            {
                Say("Darkshade the Cursed commands the shadows and the souls of the departed. Do you dare to learn my secrets?");
            }
            else if (speech.Contains("secret"))
            {
                Say("Then prove your worth! Answer me this: What is the most potent reagent in necromancy?");
            }
            else if (speech.Contains("necromancer"))
            {
                Say("Yes, a necromancer I am. It's a path filled with secrets and power that few dare to tread. One of these secrets involves a mantra of Honesty.");
            }
            else if (speech.Contains("dark"))
            {
                Say("Ah, my dark powers come from ancient relics and forbidden rituals. One of which I came upon while searching for a hidden mantra.");
            }
            else if (speech.Contains("undead"))
            {
                Say("The undead serve me willingly, for I know the words that bind them. This knowledge also led me to a piece of the mantra of Honesty.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Ah, the mantra of Honesty. It's a powerful incantation. I know but a piece: the third syllable is FOD.");
            }
            else if (speech.Contains("relics"))
            {
                Say("These ancient relics I possess are not just instruments of power, but they also contain hidden lore of the world. Among them is a scroll that speaks of a shrine of Honesty.");
            }
            else if (speech.Contains("words"))
            {
                Say("The words that bind the undead are not just mere incantations. They are embedded with truths of the universe, much like the mantra of Honesty that speaks of integrity and truth.");
            }
            else if (speech.Contains("incantation"))
            {
                Say("Incantations are spells woven with intent and focus. Just as the mantra of Honesty requires clear intention to harness its true power.");
            }
            else if (speech.Contains("scroll"))
            {
                Say("The scroll I found speaks of the virtues and principles that govern this land. Honesty, it says, is the foundation of all virtues.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the cornerstone of Honesty. Without it, one's actions and words mean nothing. It's the binding force of every true mantra.");
            }
            else if (speech.Contains("focus"))
            {
                Say("To truly master the dark arts, one must have unwavering focus. Only then can the intricate mantras and rituals be fully understood and harnessed.");
            }

            base.OnSpeech(e);
        }

        public DarkshadeTheCursed(Serial serial) : base(serial) { }

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
