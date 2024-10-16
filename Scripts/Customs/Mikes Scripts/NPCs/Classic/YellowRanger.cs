using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Yellow Ranger")]
    public class YellowRanger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public YellowRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Yellow Ranger";
            Body = 0x190; // Human body
            Hue = 54; // Armor color

            // Stats
            Str = 102;
            Dex = 108;
            Int = 65;
            Hits = 78;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 54 });
            AddItem(new StuddedChest() { Hue = 54 });
            AddItem(new ChainCoif() { Hue = 54 });
            AddItem(new StuddedGloves() { Hue = 54 });
            AddItem(new Boots() { Hue = 54 });
            AddItem(new Spear() { Name = "Yellow Ranger's Spear" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am the Yellow Ranger, once a mighty hero...");
            }
            else if (speech.Contains("health"))
            {
                Say("My body aches, but I endure...");
            }
            else if (speech.Contains("job"))
            {
                Say("Now, I guard this wretched place...");
            }
            else if (speech.Contains("duty"))
            {
                Say("But do you truly understand the weight of duty?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! Words mean nothing...");
            }
            else if (speech.Contains("hero"))
            {
                Say("In my prime, I traveled the lands, righting wrongs and protecting the weak. It's how I earned the moniker of Ranger.");
            }
            else if (speech.Contains("endure"))
            {
                Say("The battles I've faced have scarred me. Not just physically, but also mentally. Justice, however, keeps me standing.");
            }
            else if (speech.Contains("wretched"))
            {
                Say("This location holds dark secrets and ancient relics. Many tried to uncover them, but were met with their doom.");
            }
            else if (speech.Contains("lands"))
            {
                Say("The lands I traversed were both beautiful and treacherous. The Shrine of Justice was one of the sanctuaries I often visited.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Whispered legends speak of hidden chambers and forgotten knowledge. But beware, for not all secrets should be unearthed.");
            }
            else if (speech.Contains("shrine"))
            {
                Say("The Shrine of Justice is a beacon for those who seek righteousness. To unlock its power, one must chant its mantra. The first syllable is BEH.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("A mantra is more than words; it's a connection to the virtues. The mantra of Justice, for instance, has always been close to my heart.");
            }
            else if (speech.Contains("legends"))
            {
                Say("The legends are old, passed down through generations. Some are mere tales, but others hide truths waiting to be discovered.");
            }

            base.OnSpeech(e);
        }

        public YellowRanger(Serial serial) : base(serial) { }

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
