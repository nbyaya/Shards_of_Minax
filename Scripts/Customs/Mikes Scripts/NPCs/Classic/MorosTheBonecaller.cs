using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Moros the Bonecaller")]
    public class MorosTheBonecaller : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MorosTheBonecaller() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Moros the Bonecaller";
            Body = 0x190; // Human male body

            // Stats
            Str = 138;
            Dex = 50;
            Int = 135;
            Hits = 115;

            // Appearance
            AddItem(new Robe() { Hue = 2126 });
            AddItem(new Sandals() { Hue = 1175 });
            AddItem(new SkullCap() { Hue = 1 });

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
                Say("I am Moros the Bonecaller, master of the dark arts!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is sustained by the dark forces that bind me.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a necromancer, a practitioner of the forbidden arts.");
            }
            else if (speech.Contains("dark arts"))
            {
                Say("Power is not to be taken lightly. Do you seek knowledge of the dark arts?");
            }
            else if (speech.Contains("yes") && SpeechCooldownElapsed())
            {
                Say("Ah, you've heard of me before? My reputation precedes me, it seems. Few dare to speak my name.");
            }
            else if (speech.Contains("forces"))
            {
                Say("These forces are ancient and beyond comprehension. They gift me power, at a price. Everything has its cost.");
            }
            else if (speech.Contains("necromancer"))
            {
                Say("Yes, I harness the energy of the deceased, bending it to my will. It is an art few can master. Are you here to learn or to challenge?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is power. But in the world of necromancy, knowledge can also be a curse. Be sure of what you seek before you ask.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("I sense potential in you. If you truly wish to embrace the darkness, bring me the skull of a ghoul as proof. In return, I will reward you.");
            }
            else if (speech.Contains("heard"))
            {
                Say("Whispered tales and frightened murmurs, that's what they are. Few have seen the depths of my power and lived to tell the tale.");
            }
            else if (speech.Contains("cost"))
            {
                Say("The cost is the very essence of one's being. To harness the dark forces, one must be willing to sacrifice.");
            }
            else if (speech.Contains("learn"))
            {
                Say("To learn from me, you must first prove your worth. The path of darkness is not for the faint of heart.");
            }
            else if (speech.Contains("seek"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The more you seek, the more entangled you become with the dark arts. Are you ready to bear its weight? Take this if you are ready.");
                    from.AddToBackpack(new BloodSword()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        private bool SpeechCooldownElapsed()
        {
            return (DateTime.UtcNow - lastRewardTime).TotalMinutes >= 10;
        }

        public MorosTheBonecaller(Serial serial) : base(serial) { }

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
