using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald")]
    public class SirReginald : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirReginald() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 90;
            Hits = 55;

            // Appearance
            AddItem(new LongPants() { Hue = 1906 });
            AddItem(new Tunic() { Hue = 1906 });
            AddItem(new Shoes() { Hue = 1906 });
            AddItem(new FeatheredHat() { Hue = 1906 });
            AddItem(new MortarPestle() { Name = "Reginald's Mortar and Pestle" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

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
                Say("Greetings, traveler! I am Sir Reginald.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in fair health, as knights should be!");
            }
            else if (speech.Contains("job"))
            {
                Say("I serve as a knight in these lands.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor lies not just in the sword, but in one's heart. Are you a person of courage?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then never back down when honor is at stake!");
            }
            else if (speech.Contains("reginald"))
            {
                Say("Ah, you've heard of me? I come from a long line of knights. My father was Sir Roderick, a legend in his time.");
            }
            else if (speech.Contains("fair"))
            {
                Say("Indeed, daily training and discipline keep me fit. However, sometimes I do miss the taste of Lady Emeline's famous pies.");
            }
            else if (speech.Contains("knight"))
            {
                Say("A knight's duty is not just about battles. It's about protecting the weak and upholding justice. I've taken an oath to the Order of the Silver Lance.");
            }
            else if (speech.Contains("roderick"))
            {
                Say("Ah, my father! A brave soul and a beacon of hope in dark times. He once saved our village from a fearsome dragon.");
            }
            else if (speech.Contains("emeline"))
            {
                Say("Lady Emeline is not only a brilliant cook but also a skilled herbalist. She aids the wounded and sick with her remedies. If you ever need healing, seek her out and mention my name for a reward.");
            }
            else if (speech.Contains("lance"))
            {
                Say("An ancient order of knights, we've been guardians of this realm for centuries. Our symbol, a silver lance crossed with a shield, can be seen at important landmarks.");
            }
            else if (speech.Contains("dragon"))
            {
                Say("Ah, the beast was named Drakonar. Its fiery breath and scales tougher than steel posed a great challenge. My father used his wit and skill to best it.");
            }
            else if (speech.Contains("remedies"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Indeed, Lady Emeline's potions have saved many lives. I remember when I was poisoned in a skirmish; her antidote was the only thing that saved me. Here, take this vial as a token of gratitude for listening to my tales.");
                    from.AddToBackpack(new EnergyHitAreaCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirReginald(Serial serial) : base(serial) { }

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
