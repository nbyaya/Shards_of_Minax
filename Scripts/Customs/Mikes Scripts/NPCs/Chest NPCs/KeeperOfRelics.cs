using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Keeper of Relics")]
    public class KeeperOfRelics : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KeeperOfRelics() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Keeper of Relics";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1157 });
            AddItem(new WizardsHat() { Hue = 1157 });
            AddItem(new Necklace() { Hue = 1157 });
            
            // Hair
            HairItemID = 0x203C; // Long hair
            HairHue = 1157;

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
                Say("I am the Keeper of Relics, guardian of ancient treasures and secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no concern. What matters is the knowledge I guard.");
            }
            else if (speech.Contains("job"))
            {
                Say("My task is to guard and preserve the relics and knowledge of ages past.");
            }
            else if (speech.Contains("relics"))
            {
                Say("The relics I guard hold ancient power and wisdom. But to obtain them, you must prove your worth.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the true power. Seek it with a pure heart and a sharp mind.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, answer me this: What is the key to unlocking the secrets of the ancient relics?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that guides us through the darkness of ignorance. It is earned through understanding and experience.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power without wisdom is dangerous. True power lies in the balance of knowledge and strength.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the relics are revealed to those who seek them with sincerity and patience.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("The ancient relics are hidden from those who seek them for mere gain. Only the worthy shall receive their blessing.");
            }
            else if (speech.Contains("worthy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(15);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait before seeking another reward. Return later.");
                }
                else
                {
                    Say("You have shown wisdom and patience. For your efforts, accept this ancient relic as a reward.");
                    from.AddToBackpack(new AncientRelicChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I can speak of many things, but only those who seek knowledge with intent shall find what they seek.");
            }

            base.OnSpeech(e);
        }

        public KeeperOfRelics(Serial serial) : base(serial) { }

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
