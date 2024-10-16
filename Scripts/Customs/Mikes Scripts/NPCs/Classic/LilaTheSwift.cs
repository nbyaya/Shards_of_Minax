using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lila the Swift")]
    public class LilaTheSwift : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilaTheSwift() : base(AIType.AI_Archer, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lila the Swift";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new LeatherChest() { Name = "Lila's Leather Tunic" });
            AddItem(new LeatherLegs() { Name = "Lila's Leather Leggings" });
            AddItem(new LeatherGloves() { Name = "Lila's Leather Gloves" });
            AddItem(new LeatherCap() { Name = "Lila's Leather Cap" });
            AddItem(new Boots() { Name = "Lila's Boots" });
            AddItem(new Bow() { Name = "Lila's Bow" });

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
                Say("Greetings, traveler. I am Lila the Swift.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is but a fleeting concern, as the wind through the trees.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an archer, skilled in the art of precision and swiftness.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor is a concept not bound by the string of a bow but by the heart's resolve. Art thou familiar with valor?");
            }
            else if (speech.Contains("yes") && (speech.Contains("valor") || speech.Contains("resolve")))
            {
                Say("Indeed, valor lies not in fleeing but in standing resolute against adversity.");
            }
            else if (speech.Contains("lila"))
            {
                Say("Ah, thou knowest of my moniker. I am known as Lila the Swift, not just for my speed, but for my quick wit as well.");
            }
            else if (speech.Contains("wind"))
            {
                Say("The wind, much like our spirits, is ever-changing. Sometimes it's calm, and sometimes it rages. Have you ever tried to harness the power of the wind?");
            }
            else if (speech.Contains("archer"))
            {
                Say("Being an archer is not just about releasing the arrow, but about understanding the journey it takes. Much like the mantras we chant, every detail is crucial. Do you know about these mantras?");
            }
            else if (speech.Contains("speed"))
            {
                Say("Speed is not only in how fast you can run or shoot an arrow, but in how quickly you can adapt to changing situations. Do you believe in adapting quickly?");
            }
            else if (speech.Contains("harness"))
            {
                Say("To harness something is to gain control over it, to use it to one's advantage. Just as we harness the wind to propel ships or turn mills, one can harness their inner spirit for greater deeds.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Mantras are powerful chants, each holding deep meaning. The third syllable of the mantra of Spirituality is LOR. Remember this, for it may aid thee in the future.");
            }
            else if (speech.Contains("adapt"))
            {
                Say("Adapting is an essential skill, not just in battles but in life itself. The world is ever-changing, and only those who can change with it will thrive.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("Our spirit is our essence, our true self. It is what guides us in our decisions and actions. Have you ever felt a deep connection with your spirit?");
            }

            base.OnSpeech(e);
        }

        public LilaTheSwift(Serial serial) : base(serial) { }

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
