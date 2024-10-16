using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zorgon")]
    public class Zorgon : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Zorgon() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zorgon";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1152 }); // Alien-themed appearance
            AddItem(new Sandals() { Hue = 1152 });
            AddItem(new WizardsHat() { Hue = 1152 });
            
            // Speech Hue
            SpeechHue = 1152; // Alien-themed speech hue

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
                Say("Greetings, traveler. I am Zorgon, the alien artifact researcher.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is stable, thanks to the advanced technology I have at my disposal.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am dedicated to studying ancient alien artifacts and their mysteries.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("The artifacts I study hold great power and knowledge. They can reveal secrets beyond our understanding.");
            }
            else if (speech.Contains("power"))
            {
                Say("Indeed, the power of these artifacts is immense. But wielding it requires great responsibility.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are often hidden within the artifacts. One must solve many puzzles to uncover them.");
            }
            else if (speech.Contains("responsibility"))
            {
                Say("With power comes responsibility. The knowledge and strength from these artifacts must be used wisely.");
            }
            else if (speech.Contains("puzzle"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Return later when the time is right.");
                }
                else
                {
                    Say("You have proven yourself worthy. As a reward for your curiosity and perseverance, I give you the Alien Artifact Chest!");
                    from.AddToBackpack(new AlienArtifactChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Zorgon(Serial serial) : base(serial) { }

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
