using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lilith the Revenant")]
    public class LilithTheRevenant : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilithTheRevenant() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lilith the Revenant";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 50;
            Int = 130;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1155 });
            AddItem(new WizardsHat() { Hue = 1156 });
            AddItem(new Spellbook() { Name = "Lilith's Grimoire" });

            Hue = 1150; // Skin color
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Lilith the Revenant, bound to this realm as a ghostly specter.");
            }
            else if (speech.Contains("health"))
            {
                Say("I exist in a state beyond the living, untouched by health or ailment.");
            }
            else if (speech.Contains("job"))
            {
                Say("My existence is now dedicated to wandering these lands as a wandering spirit.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Contemplating the virtues in this ethereal existence, I ponder the nature of compassion and humility.");
            }
            else if (speech.Contains("inquiries"))
            {
                Say("Do you seek wisdom in the eight virtues, or do you have other inquiries?");
            }
            else if (speech.Contains("ghostly"))
            {
                Say("In life, I was a woman of noble pursuits, but my demise led me to this ghostly existence, forever trapped between realms.");
            }
            else if (speech.Contains("living"))
            {
                Say("In this state, I've witnessed countless souls pass through, each with their own tales and regrets. Some find peace, while others, like me, remain.");
            }
            else if (speech.Contains("wandering"))
            {
                Say("As I wander, I often come across lost artifacts. If you assist me in a task, I might reward you with something from my collection.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the understanding of others' suffering, and humility is recognizing one's own limitations. By embracing these virtues, one can find inner peace, even in death.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("The virtues are the guiding principles of life. Valor, Honesty, Justice, Compassion, Sacrifice, Honor, Spirituality, and Humility. Each has its own path and lesson.");
            }
            else if (speech.Contains("demise"))
            {
                Say("My end was tragic, a consequence of love and betrayal. But even in death, I hope for redemption and understanding.");
            }
            else if (speech.Contains("souls"))
            {
                Say("Souls I've encountered have tales of joy, sorrow, and redemption. Their stories are a testament to the human spirit's resilience.");
            }
            else if (speech.Contains("artifacts"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("These artifacts are remnants of past civilizations, holding secrets and powers. Should you prove worthy, one might be yours. A sample for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LilithTheRevenant(Serial serial) : base(serial) { }

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
