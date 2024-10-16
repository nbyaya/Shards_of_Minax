using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Blue Ranger")]
    public class BlueRanger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BlueRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Blue Ranger";
            Body = 0x190; // Human male body

            // Stats
            Str = 105;
            Dex = 115;
            Int = 55;
            Hits = 80;

            // Appearance
            AddItem(new RingmailLegs() { Hue = 2 });
            AddItem(new RingmailChest() { Hue = 2 });
            AddItem(new CloseHelm() { Hue = 2 });
            AddItem(new RingmailGloves() { Hue = 2 });
            AddItem(new Boots() { Hue = 2 });
            AddItem(new Halberd { Name = "Blue Ranger's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            Direction = Direction.East;

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
                Say("I am the Blue Ranger. What do you want, puny mortal?");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no concern to you. Just know that I'm still standing, unlike some people.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Hah! I'm a so-called 'Power Ranger,' fighting for justice and all that nonsense. What a joke!");
            }
            else if (speech.Contains("battles"))
            {
                Say("You think battles define valor? Ha! Valor is a meaningless concept, just like my existence as a Ranger. Are you valiant?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! 'Yes,' you say? You're as deluded as the rest of them. Valor is a pointless pursuit, and so is this conversation.");
            }
            else if (speech.Contains("ranger"))
            {
                Say("Why do they call me the Blue Ranger? Because of the color of my suit? Or perhaps the melancholy that plagues my thoughts? Does it even matter?");
            }
            else if (speech.Contains("standing"))
            {
                Say("Yes, I'm still standing, but that doesn't mean I'm not haunted by the memories of past battles. Every scar on my body has a tale to tell.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice? What is justice in a world filled with chaos and despair? I once believed in it, but now? I'm not so sure. Have you seen true justice?");
            }
            else if (speech.Contains("valiant"))
            {
                Say("Valiant? If you truly consider yourself valiant, then perhaps you can prove it. Complete a task for me, and I might reward you.");
            }
            else if (speech.Contains("task"))
            {
                Say("There's a dangerous beast lurking nearby. Defeat it, and bring me proof. If you succeed, I'll give you a reward worthy of your valor.");
            }
            else if (speech.Contains("scar"))
            {
                Say("Each scar is a mark of a battle fought, a life saved, or a mistake made. They remind me of the price of being a Ranger. Do you carry such reminders?");
            }
            else if (speech.Contains("suit"))
            {
                Say("This suit is more than just armor. It's a symbol of responsibility, of the weight I carry. Have you ever felt such a burden?");
            }
            else if (speech.Contains("chaos"))
            {
                Say("Chaos is everywhere. It's in the heart of the wilderness, in the heart of man. It's a force that can't be controlled, only confronted. How do you confront chaos?");
            }

            base.OnSpeech(e);
        }

        public BlueRanger(Serial serial) : base(serial) { }

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
