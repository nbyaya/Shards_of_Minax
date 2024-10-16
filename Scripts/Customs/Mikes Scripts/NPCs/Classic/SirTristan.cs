using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Tristan")]
    public class SirTristan : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirTristan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Tristan";
            Body = 0x190; // Human male body

            // Stats
            Str = 160;
            Dex = 63;
            Int = 22;
            Hits = 115;

            // Appearance
            AddItem(new ChainLegs() { Hue = 1100 });
            AddItem(new ChainChest() { Hue = 1100 });
            AddItem(new PlateHelm() { Hue = 1100 });
            AddItem(new PlateGloves() { Hue = 1100 });
            AddItem(new Boots() { Hue = 1100 });

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
                Say("I am Sir Tristan, a once noble knight...");
            }
            else if (speech.Contains("health"))
            {
                Say("These days, my health is but a shadow of its former glory...");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Ha! I was a protector of the realm, but now I'm naught but a relic...");
            }
            else if (speech.Contains("battles"))
            {
                Say("But tell me, what worth is valor in a world that has forgotten me?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you think you can succeed where I failed?");
            }
            else if (speech.Contains("hope"))
            {
                Say("Very well, prove your worth, and perhaps there's hope for this world yet...");
            }
            else if (speech.Contains("noble"))
            {
                Say("I was once a part of the King's inner circle, celebrated for my deeds and prowess in battle.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("A great beast wounded me in my final battle. Its venom courses through me, dimming my vitality each day.");
            }
            else if (speech.Contains("relic"))
            {
                Say("Though many see me as a mere vestige of the past, I once stood as a beacon of hope against the darkness that threatened the land.");
            }
            else if (speech.Contains("king"))
            {
                Say("The King and I were close allies, until the darkness cast its veil between us. He too has forgotten my sacrifices.");
            }
            else if (speech.Contains("beast"))
            {
                Say("It was the Netherwyrm, a creature of legends. Its power was immense, and even with my full strength, I barely escaped with my life.");
            }
            else if (speech.Contains("darkness"))
            {
                Say("The darkness I speak of is not merely the absence of light. It's a force, an entity that wishes to consume all. Yet, I sense a glimmer of light in you.");
            }
            else if (speech.Contains("allies"))
            {
                Say("Our bond was unbreakable, fighting side by side. But time has a way of altering perspectives. Seek the truth and perhaps you'll understand.");
            }
            else if (speech.Contains("netherwyrm"))
            {
                Say("If you seek to challenge the Netherwyrm, be prepared. I have an old weapon that might aid you in this endeavor.");
            }
            else if (speech.Contains("glimmer"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("I see potential in you. Take this pendant, a symbol of my family's honor. It might just guide you when all seems lost.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("truth"))
            {
                Say("Many tales have been spun, but few know the true events that transpired. A scroll, hidden in the old ruins, might shed light on the past.");
            }

            base.OnSpeech(e);
        }

        public SirTristan(Serial serial) : base(serial) { }

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
