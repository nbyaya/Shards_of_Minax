using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Griswold")]
    public class Griswold : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Griswold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Griswold";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 70;
            Int = 40;
            Hits = 120;

            // Appearance
            AddItem(new SmithHammer() { Name = "Griswold's Hammer" });
            AddItem(new FullApron() { Hue = 1109 });
            AddItem(new Shoes() { Hue = 0 });

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
                Say("I am Griswold, the blacksmith. But don't expect me to be all smiles and sunshine.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? What do you care? You're not my doctor. I've seen better days.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I forge weapons and armor for all you heroes who waltz in here like you own the place.");
            }
            else if (speech.Contains("battles"))
            {
                Say("You heroes think you're all that, don't you? You believe valor is all about swinging your fancy swords.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor isn't just about the battles you win, but also the ones you choose to avoid. Not every conflict needs a blade.");
            }
            else if (speech.Contains("heroes"))
            {
                Say("True heroes know when to fight and when to parley. The world isn't as black and white as it seems.");
            }
            else if (speech.Contains("grateful"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Here, take this token of my appreciation. You've proven yourself to be a true hero.");
                    from.AddToBackpack(new Gold(1000)); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("better days"))
            {
                Say("Those 'better days' I speak of? I used to be a warrior, not just a blacksmith. I've faced foes and tasted victory.");
            }
            else if (speech.Contains("ancestors"))
            {
                Say("My ancestors were blacksmiths too. Some say our hammers are blessed by the old gods, making our creations strong.");
            }
            else if (speech.Contains("actions"))
            {
                Say("Talk is cheap. It's actions that define a person. Show me by your deeds that you're worthy of the title 'hero'.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every quest is a chance to prove oneself. Choose wisely and remember that the journey is as important as the destination.");
            }
            else if (speech.Contains("conflict"))
            {
                Say("Confidence is good, but remember, true valor is proven in deeds, not just words.");
            }

            base.OnSpeech(e);
        }

        public Griswold(Serial serial) : base(serial) { }

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
