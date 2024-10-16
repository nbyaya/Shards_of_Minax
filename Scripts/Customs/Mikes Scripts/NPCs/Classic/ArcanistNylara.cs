using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Arcanist Nylara")]
    public class ArcanistNylara : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ArcanistNylara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Arcanist Nylara";
            Body = 0x191; // Human female body

            // Stats
            Str = 150;
            Dex = 50;
            Int = 100;
            Hits = 100;

            // Appearance
            AddItem(new Robe() { Hue = 1260 });
            AddItem(new Boots() { Hue = 1910 });
            AddItem(new TallStrawHat() { Hue = 1260 });
            AddItem(new LeatherGloves() { Hue = 1260 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, traveler. I am Arcanist Nylara.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an arcanist, dedicated to the study of ancient magic.");
            }
            else if (speech.Contains("forces"))
            {
                Say("True wisdom lies in understanding the balance of forces. Are you familiar with the concept of balance?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, balance is the foundation of magic. Only by understanding the harmony between opposing forces can one truly master the arcane arts.");
            }
            else if (speech.Contains("nylara"))
            {
                Say("Yes, I've come from a lineage of powerful mages, the Nylara clan. We've guarded the realms for generations.");
            }
            else if (speech.Contains("good"))
            {
                Say("Maintaining one's health is crucial, especially in my line of work. The arcane can be taxing on one's well-being.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("The world is filled with forgotten spells and rituals, waiting to be rediscovered. My mission is to seek them out and ensure they are used wisely.");
            }
            else if (speech.Contains("lineage"))
            {
                Say("My ancestors played crucial roles during the arcane wars, battling dark forces to keep our world safe.");
            }
            else if (speech.Contains("arcane"))
            {
                GiveReward(from);
                Say("Delving into the arcane requires focus and dedication. But with great risk comes great reward. In fact, I believe I have something that might aid you on your journey.");
            }
            else if (speech.Contains("spells"))
            {
                Say("Some spells are meant to heal, some to harm, and others to protect. Knowing which spell to use and when is the mark of a true mage.");
            }
            else if (speech.Contains("wars"))
            {
                Say("The arcane wars were a series of battles fought between mages and sorcerers over the control of powerful artifacts. Many lives were lost, and the echoes of those battles still resonate today.");
            }
            else if (speech.Contains("reward"))
            {
                Say("I hope this reward serves you well. It has been imbued with the energy of the ancients and should provide you some protection.");
            }
            else if (speech.Contains("heal"))
            {
                Say("Healing is one of the most fundamental and noble applications of magic. To mend wounds and restore health is a gift few possess.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("These artifacts were sources of immense power. Some could command the elements, while others had the power to bend time and space.");
            }

            base.OnSpeech(e);
        }

        private void GiveReward(Mobile from)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                Say("I have no reward right now. Please return later.");
            }
            else
            {
                from.AddToBackpack(new CurseAugmentCrystal());
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
        }

        public ArcanistNylara(Serial serial) : base(serial) { }

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
