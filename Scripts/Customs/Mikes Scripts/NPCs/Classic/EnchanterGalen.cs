using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Enchanter Galen")]
    public class EnchanterGalen : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EnchanterGalen() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Enchanter Galen";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 75;
            Int = 90;
            Hits = 100;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Shoes() { Hue = 1153 });
            AddItem(new FloppyHat() { Hue = 1153 });
            AddItem(new LeatherGloves() { Hue = 1153 });

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
                Say("Greetings, traveler! I am Enchanter Galen.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an enchanter, skilled in the ancient arts of magic.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic, the manifestation of virtue, can both heal and harm. Are you intrigued by the mysteries of magic?");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Indeed, magic is a reflection of the virtues. It can be used for good or ill. What path will you choose?");
            }
            else if (speech.Contains("galen"))
            {
                Say("Ah, you know my name. Not many outside of the arcane circles recognize it. I come from a long lineage of powerful enchanters.");
            }
            else if (speech.Contains("robust"))
            {
                Say("Yes, practicing magic requires one to be in optimal health. It's a delicate balance between mind, body, and the energies around us.");
            }
            else if (speech.Contains("enchanter"))
            {
                Say("Being an enchanter is more than casting spells. It is about understanding the fabric of the world and weaving your will into it. Have you ever felt the call to practice enchantments?");
            }
            else if (speech.Contains("mysteries"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The mysteries of magic are vast and deep. Only through dedication and study can one truly grasp its essence. To aid you on your journey, I would like to bestow upon you a token of magic. May it guide you well.");
                    from.AddToBackpack(new FletchingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("path"))
            {
                Say("The path of magic is intertwined with the choices we make. Some seek power, others wisdom, and some balance. What drives your quest for understanding?");
            }
            else if (speech.Contains("lineage"))
            {
                Say("Our lineage dates back to the First Age, where magic was as natural as breathing. Many secrets have been passed down through generations. Are you seeking any particular knowledge?");
            }
            else if (speech.Contains("energies"))
            {
                Say("Energies are all around us; they are the lifeforce of the world. By channeling these energies, enchanters can manifest spells and rituals. But remember, one must always respect this power.");
            }
            else if (speech.Contains("spells"))
            {
                Say("Spells are but a small facet of what enchanters do. They are the tangible results of our understanding of magic. Would you like to learn a basic spell?");
            }
            else if (speech.Contains("token"))
            {
                // This block will be handled by the previous reward logic.
            }
            else if (speech.Contains("quest"))
            {
                Say("Every individual's quest is unique. While some seek knowledge, others desire power or redemption. What you seek will shape your journey and destiny.");
            }

            base.OnSpeech(e);
        }

        public EnchanterGalen(Serial serial) : base(serial) { }

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
