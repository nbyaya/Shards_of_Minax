using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Florence Nightingale")]
    public class FlorenceNightingale : BaseCreature
    {
        [Constructable]
        public FlorenceNightingale() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Florence Nightingale";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 100;
            Hits = 50;

            // Appearance
            AddItem(new Skirt() { Hue = 1908 });
            AddItem(new FancyShirt() { Hue = 1908 });
            AddItem(new Bonnet() { Hue = 1908 });
            AddItem(new Sandals() { Hue = 1908 });
            AddItem(new Crossbow() { Name = "Florence's Remedy" });

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Florence Nightingale from the land of Canada.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, as the land itself heals my wounds.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a keeper of ancient wisdom and secrets.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom lies not in the answers, but in the questions. What queries do you bring, seeker?");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Your words dance like leaves on the wind. What say you about the mysteries of the night?");
            }
            else if (speech.Contains("canada"))
            {
                Say("Ah, Canada! A vast and wondrous land to the north. The crisp air and deep woods taught me much of nature's ways.");
            }
            else if (speech.Contains("land"))
            {
                Say("This land holds many secrets. It's alive, breathing, and watching. It's more than just the ground we walk on. If you are keen, it might reveal a secret to you.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("Ah, the ancient times! When the stars were young and tales were born. I've chronicled many such tales. Would you like to hear one?");
            }
            else if (speech.Contains("questions"))
            {
                Say("Indeed, the right question can open many doors. I once sought a question so profound, it rewarded me with a treasure. Seek and you may find.");
            }
            else if (speech.Contains("night"))
            {
                Say("The night is not just darkness; it is a canvas of dreams and memories. Among them, a forgotten story waits. Perhaps I shall share it with you.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Your interest pleases me, seeker. Here, take this. It is a small token from my own collection. Use it wisely.");
                from.AddToBackpack(new HeadSlotChangeDeed()); // Give the reward
            }

            base.OnSpeech(e);
        }

        public FlorenceNightingale(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
