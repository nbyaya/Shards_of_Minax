using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Reginald Arcana")]
    public class ReginaldArcana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ReginaldArcana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Reginald Arcana";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomBlueHue() });
            AddItem(new WizardsHat() { Hue = Utility.RandomBlueHue() });
            AddItem(new Sandals() { Hue = Utility.RandomBlueHue() });
            AddItem(new Spellbook() { Name = "Arcane Grimoire" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2048, 0x203C); // Random hair styles
            HairHue = Utility.RandomHairHue();

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
                Say("Greetings, I am Reginald Arcana, keeper of arcane secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as resilient as the enchanted wards that protect me.");
            }
            else if (speech.Contains("job"))
            {
                Say("I spend my days studying ancient tomes and safeguarding rare artifacts.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("Ah, the arcane arts are both wondrous and perilous. Only those with a deep understanding may truly master them.");
            }
            else if (speech.Contains("tome"))
            {
                Say("The tomes I guard contain knowledge of the ages. They are not for the faint of heart.");
            }
            else if (speech.Contains("relic"))
            {
                Say("My collection of relics includes items of great power and mystery. If you seek such treasures, prove your worth.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must first understand the arcane and the tomes. Show me your knowledge and resolve.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge of the arcane is crucial. Have you delved into the mysteries of ancient tomes?");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The mysteries are deep and complex. The more you understand, the closer you get to the true power of the relics.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power can be a double-edged sword. It must be wielded wisely and with great responsibility.");
            }
            else if (speech.Contains("responsibility"))
            {
                Say("Responsibility in wielding power means using it for the greater good, not for personal gain.");
            }
            else if (speech.Contains("good"))
            {
                Say("The greater good involves helping others and using your abilities to make the world a better place.");
            }
            else if (speech.Contains("world"))
            {
                Say("The world is filled with wonders and dangers. Those who seek to protect it must be prepared for both.");
            }
            else if (speech.Contains("prepared"))
            {
                Say("Being prepared means having both knowledge and the right tools. Have you gathered what you need?");
            }
            else if (speech.Contains("tools"))
            {
                Say("Tools can be many things: artifacts, spells, or even wisdom itself. What have you brought to aid you?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is gained through experience and learning. It is often the key to understanding the deepest secrets.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are hidden truths waiting to be uncovered. Your journey has just begun, and many more await.");
            }
            else if (speech.Contains("journey"))
            {
                Say("A journey is both an adventure and a test. Those who persevere will find their reward at the end.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received your reward. Please return later.");
                }
                else
                {
                    Say("For your perseverance and understanding, accept this Mage's Relic Chest, a collection of my most prized artifacts.");
                    from.AddToBackpack(new MagesRelicChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ReginaldArcana(Serial serial) : base(serial) { }

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
