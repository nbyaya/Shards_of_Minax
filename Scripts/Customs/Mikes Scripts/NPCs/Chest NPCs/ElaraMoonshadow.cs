using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Elara Moonshadow")]
    public class ElaraMoonshadow : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ElaraMoonshadow() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Elara Moonshadow";
            Body = 0x191; // Human female body
            Hue = Utility.RandomSkinHue();

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new WizardsHat(Utility.RandomNeutralHue()));
            
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Long hair styles
            HairHue = Utility.RandomHairHue();
            SpeechHue = Utility.RandomNeutralHue();

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
                Say("Greetings, traveler. I am Elara Moonshadow, a guardian of ancient secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as steady as the moon's light. Thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to safeguard the mystical artifacts of the elven realms.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets of old are hidden within the shadows. Seek wisdom to unveil them.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("The artifacts I protect are imbued with ancient magic. Only those worthy can wield their power.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from both knowledge and experience. To earn it, one must seek and understand.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To prove your worth, you must first demonstrate your understanding of the ancient virtues.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues guide our actions and shape our destiny. Reflect upon them, and you may earn a reward.");
            }
            else if (speech.Contains("destiny"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait before receiving another reward.");
                }
                else
                {
                    Say("For your understanding and patience, take this Elven Enchantress's Chest as a token of your worth.");
                    from.AddToBackpack(new ElvenEnchantressChest());
                    lastRewardTime = DateTime.UtcNow;
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public ElaraMoonshadow(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
