using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Baba Yaga")]
    public class BabaYaga : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BabaYaga() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Baba Yaga";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 55;
            Int = 105;
            Hits = 65;

            // Appearance
            AddItem(new Skirt(1130)); // Skirt with hue 1130
            AddItem(new Shirt(1130)); // Shirt with hue 1130
            AddItem(new ThighBoots(1130)); // ThighBoots with hue 1130
            AddItem(new WizardsHat(1130)); // MageHat with hue 1130
            AddItem(new Spellbook() { Name = "Yaga's Book" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("I am Baba Yaga, the all-knowing witch!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Ha! As if that's any of your business!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I brew potions, cast spells, and generally meddle in the affairs of mortals. Happy now?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom comes from a humble heart. Are you humble?");
            }
            else if (speech.Contains("yes") && speech.Contains("humble"))
            {
                Say("Ha! Your words amuse me, mortal. Few possess the wisdom they claim.");
            }
            else if (speech.Contains("potions"))
            {
                Say("Ah, my potions. They are the results of centuries of practice and exploration. Do you wish to know more about their ingredients?");
            }
            else if (speech.Contains("ingredients"))
            {
                Say("The ingredients I use are often rare and dangerous. Ever heard of the Bloodroot Flower?");
            }
            else if (speech.Contains("bloodroot"))
            {
                Say("The Bloodroot Flower is a rare blossom that only blooms under the blood moon. Many have sought it, few have found it.");
            }
            else if (speech.Contains("spells"))
            {
                Say("I've mastered spells most mortals can only dream of. Have you ever encountered a spellbound creature?");
            }
            else if (speech.Contains("spellbound"))
            {
                Say("Spellbound creatures roam the land, cursed by powerful magics. Beware the enchanted wolf that lurks in the northern woods.");
            }
            else if (speech.Contains("meddle"))
            {
                Say("I've set many a mortal on twisted paths, sending them on quests or giving them cryptic advice. Ever sought the Mirror of Truth?");
            }
            else if (speech.Contains("mirror"))
            {
                Say("The Mirror of Truth reflects not the face, but the soul. Find it, and perhaps I might reward you.");
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
                    Say("You have impressed me, mortal. Here, take this, and may it serve you well.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward (assuming Potion is the correct type)
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BabaYaga(Serial serial) : base(serial) { }

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
