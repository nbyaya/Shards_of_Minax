using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Circe")]
    public class Circe : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Circe() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Circe";
            Body = 0x191; // Human female body

            // Stats
            Str = 95;
            Dex = 65;
            Int = 115;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1128 });
            AddItem(new Boots() { Hue = 1128 });
            AddItem(new WizardsHat() { Hue = 1128 });
            AddItem(new Spellbook() { Name = "Circe's Codex" });

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
                Say("Greetings, traveler. I am Circe, a witch of these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, my dear, health is but a fleeting concept in our world of magic and mayhem.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I weave the threads of fate, concoct elixirs, and dabble in the arcane arts.");
            }
            else if (speech.Contains("battles"))
            {
                Say("But do you dare to peer into the abyss of your own destiny? Tell me, traveler, what brings you here?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ah, a wanderer in search of answers. Wise indeed. I am here to help, should you seek guidance.");
            }
            else if (speech.Contains("magic"))
            {
                Say("It's a cruel world, isn't it? Magic, mystery, and mayhem await those who dare to seek them.");
            }

            base.OnSpeech(e);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional functionality can be added here if needed
        }

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

        public Circe(Serial serial) : base(serial) { }
    }
}
