using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lorelei Rhiannon")]
    public class LoreleiRhiannon : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LoreleiRhiannon() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lorelei Rhiannon";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlainDress() { Hue = 2121 }); // Elegant dress
            AddItem(new LeatherGloves() { Hue = 0 }); // Floral accessory
            AddItem(new GoldBracelet() { Hue = 2102 }); // Jewel-themed item
            
            // Hair and appearance
            HairItemID = 0x203B; // Long hair
            HairHue = 1150; // Light color for a mystical look
            FacialHairItemID = 0; // No facial hair for female

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
                Say("Greetings, I am Lorelei Rhiannon, guardian of the Rhine's treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as tranquil as the Rhine itself.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am here to protect and share the ancient secrets of the Rhine Valley.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the Rhine's treasures are hidden deep and require wisdom to uncover.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The Rhine Valley holds many secrets. Only those who prove their worth may learn them.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must demonstrate your knowledge of the valley's lore.");
            }
            else if (speech.Contains("lore"))
            {
                Say("Do you know of the legends of the Rhine and its magical treasures?");
            }
            else if (speech.Contains("legends"))
            {
                Say("Indeed, the legends speak of ancient guardians and mystical enchantments.");
            }
            else if (speech.Contains("guardians"))
            {
                Say("These guardians were protectors of the Rhine, ensuring its magic remained pure.");
            }
            else if (speech.Contains("protectors"))
            {
                Say("Yes, they protected not just the treasures, but also the very essence of the Rhine.");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of the Rhine is its enchantment and the ancient power that flows through it.");
            }
            else if (speech.Contains("enchantment"))
            {
                Say("The enchantment of the Rhine can be felt in its flowing waters and the mysteries it holds.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Many mysteries await those who seek them with a pure heart and inquisitive mind.");
            }
            else if (speech.Contains("heart"))
            {
                Say("A pure heart is essential for uncovering the true magic of the Rhine.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is woven into every aspect of the Rhine Valley. To experience it fully, one must understand its lore.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experiencing the magic of the Rhine involves exploring its depths and uncovering its hidden treasures.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Indeed, treasures await those who prove their worth through knowledge and respect for the valley.");
            }
            else if (speech.Contains("prove") && speech.Contains("knowledge"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this time. Return when you are wiser.");
                }
                else
                {
                    Say("You have shown wisdom and respect for the Rhine's lore. Accept this Rhine Valley Chest as your reward.");
                    from.AddToBackpack(new RhineValleyChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Ah, it seems you need to delve deeper into the lore of the Rhine Valley.");
            }

            base.OnSpeech(e);
        }

        public LoreleiRhiannon(Serial serial) : base(serial) { }

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
