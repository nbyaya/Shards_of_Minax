using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Necro Nina")]
    public class NecroNina : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NecroNina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Necro Nina";
            Body = 0x190; // Human female body

            // Stats
            Str = 130;
            Dex = 70;
            Int = 100;
            Hits = 100;

            // Appearance
            AddItem(new Robe() { Hue = 38 });
            AddItem(new Sandals() { Hue = 38 });
            AddItem(new BoneGloves() { Name = "Nina's Necrotic Nails" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, mortal. I am Necro Nina, a mistress of the dark arts.");
            }
            else if (speech.Contains("health"))
            {
                Say("My body may be frail, but my power is eternal.");
            }
            else if (speech.Contains("job"))
            {
                Say("I delve into the forbidden arts of necromancy.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The eight virtues, they say... I ponder on their twisted interpretation in my dark rituals.");
            }
            else if (speech.Contains("dark arts"))
            {
                Say("Do you dare to embrace the shadows and seek power beyond mortal comprehension?");
            }
            else if (speech.Contains("necromancy"))
            {
                Say("Necromancy is not just about raising the dead. It's about understanding the balance between life and death. Those who mock it do so out of ignorance.");
            }
            else if (speech.Contains("power"))
            {
                Say("True power lies not in brute force, but in knowledge and understanding of the arcane. But beware, such knowledge comes with a price.");
            }
            else if (speech.Contains("interpretation"))
            {
                Say("The eight virtues as understood by many are limited. In my rituals, I seek a deeper, darker understanding of them. They can be tools, not just ideals.");
            }
            else if (speech.Contains("rituals"))
            {
                Say("My rituals are a bridge between the living and the dead. Through them, I can converse with spirits and seek their guidance. Would you like a demonstration?");
            }
            else if (speech.Contains("past"))
            {
                Say("I was once like any other, but my insatiable thirst for knowledge led me down this path. Now, I am bound to the shadows, forever seeking more.");
            }
            else if (speech.Contains("domain"))
            {
                Say("This realm is both my sanctuary and my prison. It's where I practice my arts and commune with the spirits. Tread carefully, for not all spirits are friendly.");
            }
            else if (speech.Contains("embrace"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, a brave soul. If you truly wish to embrace the shadows, I can grant you a token of power. Use it wisely and remember, every gift comes with a cost.");
                    from.AddToBackpack(new NecromancyAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("beware"))
            {
                Say("Yes, you should always be cautious. The path of darkness is fraught with dangers, but the rewards... they can be immense.");
            }

            base.OnSpeech(e);
        }

        public NecroNina(Serial serial) : base(serial) { }

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
