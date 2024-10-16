using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Nimrod Dreamweaver")]
    public class NimrodDreamweaver : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NimrodDreamweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nimrod Dreamweaver";
            Body = 0x190; // Male body
            Hue = Utility.RandomBrightHue();

            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            AddItem(new WizardsHat(Utility.RandomBlueHue()));

            // Stats
            Str = 80;
            Dex = 70;
            Int = 100;
            Hits = 90;

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
                Say("Greetings, I am Nimrod Dreamweaver, guardian of illusions.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to weave dreams and guide those who seek the ethereal plane.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as vibrant as the dreamscape, thank you for your concern.");
            }
            else if (speech.Contains("dreams"))
            {
                Say("Dreams are the whispers of the cosmos. They reveal truths hidden from the waking world.");
            }
            else if (speech.Contains("ethereal"))
            {
                Say("The ethereal plane is a realm of pure illusion and mystery. Only the brave dare to explore its depths.");
            }
            else if (speech.Contains("reveal"))
            {
                Say("To reveal a truth, one must first see through the veil of illusion. Seek the hidden and the unseen.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait before seeking another reward.");
                }
                else
                {
                    Say("Your curiosity and perseverance have led you to this moment. Accept this Ethereal Plane Chest as your reward.");
                    from.AddToBackpack(new EtherealPlaneChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public NimrodDreamweaver(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
