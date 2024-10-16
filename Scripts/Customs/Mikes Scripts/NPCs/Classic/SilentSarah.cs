using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Silent Sarah")]
    public class SilentSarah : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SilentSarah() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Silent Sarah";
            Body = 0x191; // Human female body

            // Stats
            Str = 145;
            Dex = 70;
            Int = 30;
            Hits = 105;

            // Appearance
            AddItem(new LeatherCap() { Hue = 1120 });
            AddItem(new FemaleLeatherChest() { Hue = 1120 });
            AddItem(new Sandals() { Hue = 1170 });
            AddItem(new LeatherGloves() { Name = "Sarah's Stealthy Gloves" });

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
                Say("I am Silent Sarah, the whisperer of shadows.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is not your concern, for I am beyond the grasp of death.");
            }
            else if (speech.Contains("job"))
            {
                Say("I was once a murderer, a shadow in the night. Do you seek the darkness as well?");
            }
            else if (speech.Contains("justice"))
            {
                Say("The virtue of Justice, a fleeting concept in my world. What do you know of justice?");
            }
            else if (speech.Contains("darkness"))
            {
                Say("Is justice served when darkness consumes the light?");
            }
            else if (speech.Contains("shadow"))
            {
                Say("I once walked among the shadows, unseen and unheard, leaving only whispers in my wake. If you prove worthy, I might teach you the art of the shadow. Would you be interested?");
            }
            else if (speech.Contains("whispers"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Whispers are the voices of the departed, tales of sorrow and regret. If you listen carefully, you might just hear them. For your patience, I grant you this gift.");
                    from.AddToBackpack(new FootwearSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("murderer"))
            {
                Say("There were times when I silenced those who deserved it, and times when my blade was drawn to the innocent. Regret is a heavy burden. But do you judge me?");
            }
            else if (speech.Contains("light"))
            {
                Say("The light once shone bright in my soul, but it was smothered by my actions. Darkness can only be understood when you've been in the light. Tell me, do you fear the light or embrace it?");
            }
            else if (speech.Contains("fear"))
            {
                Say("Fear is a powerful tool, one I've wielded and been subjected to. To overcome it is to truly be free. Do you seek to overcome your fears?");
            }
            else if (speech.Contains("freedom"))
            {
                Say("True freedom is not just the absence of shackles, but the liberation of the soul. I've tasted both and still yearn for the latter. What does freedom mean to you?");
            }

            base.OnSpeech(e);
        }

        public SilentSarah(Serial serial) : base(serial) { }

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
