using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tharn the Ember Eye")]
    public class TharnTheEmberEye : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TharnTheEmberEye() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tharn the Ember Eye";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 150;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1177 }); // Robe with hue 1177
            AddItem(new Sandals() { Hue = 1176 }); // Sandals with hue 1176
            AddItem(new Item(0x1F13) { Name = "Tharn's Oracle" }); // CrystalBall

            Hue = 1175;
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
                Say("Greetings, traveler. I am Tharn the Ember Eye, a watery enigma.");
            }
            else if (speech.Contains("health"))
            {
                Say("My essence flows with the tides, and I am untouched by the flames of affliction.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a seeker of elemental balance, a guardian of the ethereal currents.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Canst thou decipher the hidden truths that lie beneath the surface of existence?");
            }
            else if (speech.Contains("truth"))
            {
                Say("Your words resonate with truth. What knowledge do you seek, and what answers can you provide?");
            }
            else if (speech.Contains("ember"))
            {
                Say("I was named the Ember Eye for the fiery gaze I possess, a sign of my connection with the elements.");
            }
            else if (speech.Contains("tides"))
            {
                Say("The ebb and flow of the tides are like the rhythm of life. When in harmony, health and serenity flourish.");
            }
            else if (speech.Contains("elemental"))
            {
                Say("Elemental forces shape our world. My duty is to ensure they remain in balance, lest chaos ensue.");
            }
            else if (speech.Contains("enigma"))
            {
                Say("Yes, an enigma. There are tales of old that may shed light on my origins, but they remain closely guarded secrets.");
            }
            else if (speech.Contains("existence"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("True existence is not just what one sees on the surface. Dive deep, and you may find treasures of wisdom. Here, for your earnest curiosity, take this token of appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("tales"))
            {
                Say("Ancient stories passed down through generations. Each holds a lesson, a truth. If you can uncover one, the path forward may be illuminated.");
            }

            base.OnSpeech(e);
        }

        public TharnTheEmberEye(Serial serial) : base(serial) { }

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
