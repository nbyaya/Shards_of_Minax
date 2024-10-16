using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lethal Lucy")]
    public class LethalLucy : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LethalLucy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lethal Lucy";
            Body = 0x191; // Female human body

            // Stats
            Str = 155;
            Dex = 62;
            Int = 28;
            Hits = 108;

            // Appearance
            AddItem(new PlainDress() { Hue = 1150 }); // Dress with hue 1150
            AddItem(new Sandals() { Hue = 1175 }); // Sandals with hue 1175
            AddItem(new Cloak() { Hue = 1120 }); // Cloak with hue 1120

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
                Say("I am Lethal Lucy, the one they whisper about in the dark.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? It's as twisted as my soul.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm a harvester of souls, a reaper of hope.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("The truest virtue is Humility. In my darkness, it's all I have left. Do you understand?");
            }
            else if (speech.Contains("yes") && speech.Contains("virtue"))
            {
                Say("Humility teaches that even in darkness, one can find a glimmer of light. Can you find that glimmer in me?");
            }
            else if (speech.Contains("whisper"))
            {
                Say("Whispers? Ah, they've been my companion for ages. Secrets lost in the shadows, tales of my past.");
            }
            else if (speech.Contains("twisted"))
            {
                Say("My soul was twisted by a grave betrayal. Someone I once held dear turned against me.");
            }
            else if (speech.Contains("harvester"))
            {
                Say("Harvesting souls isn't a choice, it's a curse. One that was bestowed upon me due to circumstances beyond my control.");
            }
            else if (speech.Contains("past"))
            {
                Say("Long ago, I was not the Lethal Lucy you see. I was but a simple maiden, caught in the webs of fate.");
            }
            else if (speech.Contains("betrayal"))
            {
                Say("The one who betrayed me was none other than my sister. She coveted power and in her pursuit, sacrificed our bond.");
            }
            else if (speech.Contains("curse"))
            {
                Say("The curse was placed upon me by a powerful sorcerer. I crossed paths with him and paid the price. But for one who proves themselves worthy, I might bestow a reward to aid them.");
            }
            else if (speech.Contains("sister"))
            {
                Say("My sister, now known as the Enchantress Evelyn, resides in a hidden lair. She's the key to breaking my curse, though I've lost hope of reconciliation.");
            }
            else if (speech.Contains("sorcerer"))
            {
                Say("Ah, the sorcerer, a being of immense power and cruelty. His name is Malakar, and his magic knows no bounds. Beware if you ever cross his path.");
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
                    Say("Ah, you've proven yourself attentive. Very well. Take this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LethalLucy(Serial serial) : base(serial) { }

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
