using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cao Cao")]
    public class CaoCao : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaoCao() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cao Cao";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 80;
            Hits = 70;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1175 });
            AddItem(new LeatherChest() { Hue = 1175 });
            AddItem(new LeatherGloves() { Hue = 1175 });
            AddItem(new LeatherCap() { Hue = 1175 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new Broadsword() { Name = "Cao Cao's Sword" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Cao Cao, a humble scholar from the East.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I dedicate my life to the pursuit of wisdom and knowledge.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True virtue lies in the heart, not in the deeds alone. Do you ponder the virtues, wanderer?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, the virtues are the compass of our souls. Which virtue do you hold dearest?");
            }
            else if (speech.Contains("east"))
            {
                Say("The East is a place of ancient wisdom and mysteries. I journeyed from a distant city called Luoyang.");
            }
            else if (speech.Contains("good"))
            {
                Say("Physical wellness is but a fleeting state. It's the health of the mind and soul that truly endures. Do you meditate?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that dispels ignorance. I've spent years studying the ancient texts of my homeland.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The Eight Virtues of the Avatar guide many in their actions. Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Which resonates with you?");
            }
            else if (speech.Contains("luoyang"))
            {
                Say("Luoyang is a city of rich history and culture. Its libraries are vast, and I often miss its serenity.");
            }
            else if (speech.Contains("meditate"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Meditation brings clarity and peace. If you practice it diligently, you might find treasures within. Here, take this as a token to begin your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("texts"))
            {
                Say("These texts, often written by wise sages, tell tales of heroes, morals, and the universe's truths.");
            }
            else if (speech.Contains("eight"))
            {
                Say("To fully understand them, one must experience and embody each virtue in their life. It's a path of enlightenment.");
            }
            else if (speech.Contains("libraries"))
            {
                Say("In the libraries of Luoyang, one can lose oneself in scrolls and manuscripts that span centuries. It's a scholar's paradise.");
            }
            else if (speech.Contains("clarity"))
            {
                Say("With clarity, one can see the world without illusions and discern the true path forward.");
            }
            else if (speech.Contains("sages"))
            {
                Say("The sages of old were visionaries who saw beyond their time, leaving behind teachings for future generations.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Reaching enlightenment is not an end but a journey. Every step, a lesson; every challenge, a test.");
            }

            base.OnSpeech(e);
        }

        public CaoCao(Serial serial) : base(serial) { }

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
