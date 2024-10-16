using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Gawain")]
    public class SirGawain : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirGawain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Gawain";
            Body = 0x190; // Human male body

            // Stats
            Str = 163;
            Dex = 68;
            Int = 29;
            Hits = 118;

            // Appearance
            AddItem(new PlateChest() { Hue = 1190 });
            AddItem(new PlateLegs() { Hue = 1190 });
            AddItem(new PlateGloves() { Hue = 1190 });
            AddItem(new PlateArms() { Hue = 1190 });
            AddItem(new Bascinet() { Hue = 1190 });
            
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
                Say("I am Sir Gawain, once a proud knight of the realm.");
            }
            else if (speech.Contains("health"))
            {
                Say("My body may be whole, but my spirit is broken.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' now is to haunt these accursed lands.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor, you say? Valor is a fading memory.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you believe in chivalry, in a world where honor has no place?");
            }
            else if (speech.Contains("realm"))
            {
                Say("Ah, the realm! It once shimmered with prosperity and unity. I miss the days where the knights upheld the virtues and the land was at peace.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("My spirit was shattered when I lost my most cherished possession. It was an amulet given to me by my beloved before she passed away.");
            }
            else if (speech.Contains("haunt"))
            {
                Say("Yes, I roam these lands in search of redemption. I've lost my way and I've made mistakes that I deeply regret.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Once, valor meant everything to me. But after witnessing the treachery of those I once trusted, my faith in the code of chivalry waned.");
            }
            else if (speech.Contains("mistakes"))
            {
                Say("I was entrusted with a sacred task to protect a holy relic. But in my arrogance, I failed, and it was taken by the forces of darkness.");
            }
            else if (speech.Contains("relic"))
            {
                Say("The holy relic is known as the Chalice of Virtues. If you ever come across it, please return it to the Shrine of Honor. In gratitude, I would offer you a reward for such an act of kindness.");
            }
            else if (speech.Contains("shrine"))
            {
                Say("It's an ancient site dedicated to the principles of chivalry and honor. Legend says it holds the power to cleanse tainted souls.");
            }
            else if (speech.Contains("chivalry"))
            {
                Say("Chivalry was the code we knights lived by. Honesty, valor, compassion... But times have changed, and I fear chivalry is now a forgotten ideal.");
            }
            else if (speech.Contains("amulet"))
            {
                Say("It was a pendant with a blue gem at its center, symbolizing our undying love. Alas, it was stolen from me during a skirmish, and I've never seen it since.");
            }
            else if (speech.Contains("stolen"))
            {
                Say("It was taken by a rogue named Lancelot. Our brotherhood was betrayed by his deceit and cunning.");
            }
            else if (speech.Contains("lancelot"))
            {
                Say("He was once my closest ally and brother-in-arms. But ambition and jealousy turned him. If you seek him out, be wary. He is not to be trusted.");
            }
            else if (speech.Contains("brotherhood"))
            {
                Say("We were a band of knights, sworn to protect the realm and uphold the virtues. But like all things, our unity eventually fractured. Take this friend.");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, yes. Should you aid me in my quest, I shall grant you something from my treasure trove, a token of my gratitude.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirGawain(Serial serial) : base(serial) { }

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
