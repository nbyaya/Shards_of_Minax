using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Arcanus the Mage")]
    public class ArcanusTheMage : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ArcanusTheMage() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Arcanus the Mage";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 50;
            Int = 120;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1265, Name = "Mystic Robe" });
            AddItem(new WizardsHat() { Hue = 1265, Name = "Mystic Hat" });
            AddItem(new Sandals() { Hue = 1265 });

            HairItemID = 8253;
            HairHue = 1150;
            FacialHairItemID = 8251;
            FacialHairHue = 1150;

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
                Say("Greetings, traveler. I am Arcanus the Mage.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am well, thank you.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the secrets of the arcane, protecting powerful artifacts.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("The arcane arts are my specialty. They hold many secrets and great power.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Artifacts of great power are hidden throughout this land. One such treasure is the Arcane Treasure Chest.");
            }
            else if (speech.Contains("hidden"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward to give at this moment. Please return later.");
                }
                else
                {
                    Say("For your keen interest and perseverance, I grant you this Arcane Treasure Chest. Use it wisely.");
                    from.AddToBackpack(new ArcaneTreasureChest());
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("secret"))
            {
                Say("Secrets are the currency of the wise. Those who seek knowledge must be patient and persistent.");
            }
            else if (speech.Contains("chest"))
            {
                Say("The Arcane Treasure Chest is a vessel of powerful items. It is bestowed upon those who prove their worth.");
            }

            base.OnSpeech(e);
        }

        public ArcanusTheMage(Serial serial) : base(serial) { }

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
