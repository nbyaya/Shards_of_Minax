using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Samuel de Champlain")]
    public class SamuelDeChamplain : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SamuelDeChamplain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Samuel de Champlain";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 90;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new FancyShirt() { Hue = 1100 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new WideBrimHat() { Hue = 1100 });
            AddItem(new Cutlass() { Name = "Champlain's Blade" });

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
                Say("I am Samuel de Champlain, a traveler from the distant land of France.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is quite robust, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession, you ask? I am an explorer and a cartographer, seeking new frontiers and mapping uncharted territories.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("True adventure lies in the unknown, mon ami! Do you crave adventure?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then never let fear deter you, for the greatest treasures await those who dare to explore!");
            }
            else if (speech.Contains("france"))
            {
                Say("France, my homeland, is renowned for its rich culture, arts, and sciences. Have you ever heard tales of the Eiffel Tower?");
            }
            else if (speech.Contains("robust"))
            {
                Say("Robustness is crucial for explorers like me. The wild is unforgiving, but it also teaches resilience. Have you faced the wild before?");
            }
            else if (speech.Contains("cartographer"))
            {
                Say("Cartography is the art of map-making. Maps are invaluable tools for travelers. Would you be interested in one of my maps?");
            }
            else if (speech.Contains("unknown"))
            {
                Say("Ah, the thrill of discovering the unknown! Every corner of the world holds secrets. Are you, perhaps, searching for a particular secret?");
            }
            else if (speech.Contains("map"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, I see a fellow enthusiast! For your curiosity and sense of adventure, I'll gift you one of my prized maps. Use it wisely and it may lead you to untold treasures!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("resilience"))
            {
                Say("Resilience is not just about surviving, but learning and adapting. The wild has taught me many things. Do you wish to learn some survival tricks?");
            }
            else if (speech.Contains("secret"))
            {
                Say("Secrets are like stars; many are out there, but only a few are discovered. I've heard of a hidden cove nearby. Would you like its coordinates?");
            }

            base.OnSpeech(e);
        }

        public SamuelDeChamplain(Serial serial) : base(serial) { }

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
