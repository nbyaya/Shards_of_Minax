using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zhen Huan")]
    public class ZhenHuan : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public ZhenHuan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zhen Huan";
            Body = 0x190; // Male body
            Title = "the Dynasty Keeper";

            // Stats
            Str = 85;
            Dex = 70;
            Int = 95;
            Hits = 70;

            // Appearance
            AddItem(new Robe(Utility.RandomGreenHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomGreenHue()));

            // Optional: Add other equipable items to enhance the appearance
            AddItem(new GoldBracelet() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldRing() { Hue = Utility.RandomMetalHue() });

            // Customize speech hue
            SpeechHue = 0;

            // Initialize the reward state
            m_RewardGiven = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Zhen Huan, the Keeper of the Dynasty's Relics.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard and maintain the treasures and relics of the ancient dynasties.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, sustained by the wisdom of the ancients.");
            }
            else if (speech.Contains("relics"))
            {
                Say("The relics of our dynasty are precious and hold great power. Seek them wisely.");
            }
            else if (speech.Contains("dynasty"))
            {
                Say("The dynasties of old were wise and powerful. Their relics hold secrets long forgotten.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures are not just material wealth, but also the knowledge and history they carry.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from understanding the past and learning from it. It is the true treasure.");
            }
            else if (speech.Contains("treasure"))
            {
                if (!m_RewardGiven)
                {
                    // Give the reward if the player has completed the dialogue puzzle
                    from.AddToBackpack(new DynastyRelicsChest());
                    Say("You have proven yourself worthy of our heritage. Take this Dynasty's Relics chest as your reward.");
                    m_RewardGiven = true;
                }
                else
                {
                    Say("I have already given you a reward. Return another time if you wish.");
                }
            }
            else
            {
                Say("I have little to say on that topic. Ask me about the relics or the dynasty.");
            }

            base.OnSpeech(e);
        }

        public ZhenHuan(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
