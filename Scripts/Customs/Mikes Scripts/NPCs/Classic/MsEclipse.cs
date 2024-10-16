using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ms. Eclipse")]
    public class MsEclipse : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MsEclipse() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ms. Eclipse";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 70;
            Int = 90;
            Hits = 40;

            // Appearance
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Cap() { Hue = 1150 });
            AddItem(new LeatherGloves() { Hue = 1150 });
            AddItem(new Shoes() { Hue = 1150 });
            AddItem(new Item(0x1F14) { Name = "Ms. Eclipse's Chip" }); // Placeholder for the Raid Chip item

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
                Say("I am Ms. Eclipse, the brilliant scientist!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Irrelevant! I'm busy with important work!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm a scientist, but what would you understand about it?");
            }
            else if (speech.Contains("science") || speech.Contains("research"))
            {
                Say("Do you even comprehend the complexity of my research?");
            }
            else if (speech.Contains("yes") || speech.Contains("no"))
            {
                Say("Are you capable of understanding anything beyond simple conversation?");
            }
            else if (speech.Contains("brilliant"))
            {
                Say("Ah, 'brilliant'? Why thank you! It's a term I've earned through countless hours of tireless research and experimentation.");
            }
            else if (speech.Contains("important"))
            {
                Say("Ah, you're curious about my 'important work'? I'm on the brink of a groundbreaking discovery that will change the world as we know it!");
            }
            else if (speech.Contains("scientist"))
            {
                Say("Yes, as a 'scientist', I delve into the mysteries of the universe. My current project could potentially alter the very fabric of reality!");
            }
            else if (speech.Contains("complexity"))
            {
                Say("The 'complexity' of my research is vast. I've been studying the effects of lunar eclipses on arcane energies.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("These 'arcane energies' are powerful forces that, when harnessed, can grant abilities beyond imagination. But they can be volatile and unpredictable.");
            }
            else if (speech.Contains("yes"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, a confident one, aren't we? Very well, for your earnestness, I shall reward you. Take this, and use it wisely!");
                    from.AddToBackpack(new PhysicalHitAreaCrystal()); // Replace with actual reward item class
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MsEclipse(Serial serial) : base(serial) { }

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
