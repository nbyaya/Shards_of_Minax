using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Einhar")]
    public class Einhar : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Einhar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Einhar";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 70;
            Int = 50;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1001 });
            AddItem(new LeatherChest() { Hue = 1001 });
            AddItem(new Boots() { Hue = 1001 });
            AddItem(new Bow() { Name = "Einhar's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Einhar, the Beastmaster!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health!");
            }
            else if (speech.Contains("job"))
            {
                Say("I capture and study exotic creatures!");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The 8 virtues, hmmm... They are like the different aspects of the beasts I encounter in my travels. Which virtue do you seek insight into?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Ah, honesty! It is like the purity of a newborn creature, untamed by deception or deceit. Seek the truth in your actions, my friend.");
            }
            else if (speech.Contains("beastmaster"))
            {
                Say("Yes, I've devoted my life to understanding the mysteries of the beasts that inhabit our lands. Each creature has its own story.");
            }
            else if (speech.Contains("good"))
            {
                Say("Thanks for asking. Living in the wild and interacting with creatures keeps me fit and strong. Nature has its own way of healing and strengthening.");
            }
            else if (speech.Contains("exotic"))
            {
                Say("From the densest forests to the highest mountains, I've seen creatures most can only dream of. Some are peaceful, some ferocious, but all are unique. If you ever want to learn about a specific creature, just ask.");
            }
            else if (speech.Contains("story"))
            {
                Say("Each beast has its own tale. Like the majestic Griffon that once saved a village from a looming disaster or the tiny squirrel that outwitted a fox. Stories that teach us lessons about life.");
            }
            else if (speech.Contains("nature"))
            {
                Say("Nature is both a teacher and a healer. Just by observing the animals, one can learn about survival, patience, and the circle of life. And the herbs and plants, they hold remedies for many ailments.");
            }
            else if (speech.Contains("ferocious"))
            {
                Say("Ferocious creatures are often misunderstood. They act out of instinct, hunger, or fear. When you understand them, you can predict and even befriend them. I once befriended a ferocious dragon and he granted me a special token as a sign of our bond.");
            }
            else if (speech.Contains("dragon"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, dragons, the mighty rulers of the skies. If you ever cross paths with one, show respect, and you might just earn its favor. For your curiosity and respect, I'll give you this old relic I found during one of my travels. May it serve you well.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("herbs"))
            {
                Say("There's a vast variety of herbs in the wild. Some can heal, some can harm, and some can even enhance one's abilities. If you are interested, I can teach you a thing or two about them.");
            }
            else if (speech.Contains("griffon"))
            {
                Say("The Griffon is a magnificent creature, a mix of lion and eagle. Their wisdom and strength are unparalleled. They are known to be guardians of treasures and ancient secrets.");
            }

            base.OnSpeech(e);
        }

        public Einhar(Serial serial) : base(serial) { }

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
